using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System;
using SimpleJSON;

public class WorksheetQAViewController : QuesAnsViewController {
	//Display Variables
	int totalOptionCount = 4;
	float currentTime=0,totalTime=0,maxCurrentTime = 90;
	QuesAnsList quesAnsList ;
	StringWrapper stringWrapper ;
	public GameObject worksheetGO;
	WorksheetController diagnosticTestObject;
	//Behind the scene
	UserProfile user;



	//Prefabs
	public GameObject ansOption;
	public GameObject imagePrefab;
	public GameObject tickImage;
	GameObject quesImageGO;
	Texture2D quesTexture;

	// Use this for initialization
	public override void Start () {
		//base.Start ();
		StartCoroutine(GetQAList());
		quesAnsList = new QuesAnsList();
		user = new UserProfile();
		stringWrapper = new StringWrapper ();
		diagnosticTestObject = (WorksheetController) worksheetGO.GetComponent(typeof(WorksheetController));
		SwipeManager.OnSwipeDetected += OnSwipeDetected;
	}
	public void getQAListAPI(){
		StartCoroutine(GetQAList());
		quesAnsList = new QuesAnsList();
	}
	IEnumerator GetQAList() {
		var standard_id = 4;
		//string domain = "diagnosticTestObject.getDomainAddress()";
		string domain = "http://8d161b2c.ngrok.io";
		//var standard_id = user.getStandard ().getStandardId ();
		string getQuesListUrl = domain + "/api/diagnostic_tests/get_test.json?standard_id="+standard_id+"&subject_id="+standard_id;
		UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get (getQuesListUrl);
		yield return www.Send ();

		if (www.isError) {
			Debug.Log (www.error);
		} else {
			var the_JSON_string = www.downloadHandler.text; 
			var N = JSON.Parse(the_JSON_string);
			int questionCount =( N ["questions"].Count);
			Debug.Log ("Values of diagnostic test id"+N["diagnostic_test_id"].Value);
			quesAnsList.DisplayId = int.Parse(N ["diagnostic_test_id"].Value);
			int maxLives = 4;
			int maxTotalTime = 900;
			quesAnsList.setMaxParams (maxLives, maxTotalTime);
			//for loop on standardCount
			//add standard from API
			for (int i = 0; i < questionCount; i = i + 1) {
				string question=N ["questions"][i]["question_text"].Value;
				string question_image=N ["questions"][i]["question_image"].Value;
				string hint=""; 
				string answer_description=N ["questions"][i]["answer"].Value;
				int listIndex=i;
				int backendId = int.Parse(N ["questions"][i]["short_choice_question_id"].Value);
				int opCount = (N ["questions"][i]["answers"].Count);
				string[] ansOps = new string[opCount];
				string[] ansOpImgs = new string[opCount];
				int correctOp = 0;
				for (int j = 0; j < opCount; j = j + 1) {
					ansOps [j] = N ["questions"] [i] ["answers"] [j] ["answer_text"].Value;
					ansOpImgs [j] = N ["questions"] [i] ["answers"] [j] ["image"].Value;
					
					if ((N ["questions"] [i] ["answers"] [j] ["correct"].Value)=="true") {
						correctOp = j;
					}

				}
				Answer givenAnswer= new Answer(ansOps,opCount,correctOp,ansOpImgs);

				quesAnsList.add (question, givenAnswer, hint, answer_description,question_image, listIndex, backendId);
			}
			setQuesAnsBasedOnIndex (0, true);
			//diagnosticTestObject.updateAPIStatus ("GetQAList",true);
		}

	}

	IEnumerator PostQuestionAttempt(WWW data){
		yield return data; // Wait until the download is done
		if (data.error != null){
			Debug.Log("There was an error sending request: " + data.error);
		}else{
			Debug.Log("WWW Response: " + data.text);
			diagnosticTestObject.updateAPIStatus ("PostQuestionAttempt",true);
		}
	}
	public void postQuestionAttempt(){
		//diagnosticTestObject.openWaitingPanel ();
		string postQuesAttemptUrl = diagnosticTestObject.getDomainAddress() + "api/diagnostic_tests/test_attempt";
		WWW www;

		//Creating header
		Dictionary<string,string> postHeader = new Dictionary<string,string>();
		postHeader.Add("Content-Type", "application/json");

		//CreatingJSONNODE
		JSONNode PostJson = new JSONClass(); // Start with JSONArray or JSONClass

		PostJson["user"]["first_name"] = "shaunak";
		PostJson["user"]["last_name"] = "das";
		PostJson["user"]["email"] = "shaunakdas2020@gmail.com";
		PostJson["user"]["number"] = "9740644522";
		PostJson["diagnostic_test"]["id"] = quesAnsList.DisplayId.ToString();

		for (var i = 0; i < quesAnsList.QAList.Count; i++) {
			//Console.WriteLine("Amount is {0} and type is {1}", quesAnsList[i].amount, quesAnsList[i].type);

			PostJson["diagnostic_test"]["short_choice_questions"][quesAnsList.QAList[i].BackendId.ToString()]["time_taken"] = quesAnsList.QAList[i].getUserTimeTaken().ToString();
			PostJson["diagnostic_test"]["short_choice_questions"][quesAnsList.QAList[i].BackendId.ToString()]["score"] = quesAnsList.QAList[i].getUserScore().ToString();
		}

		string json_string="";
		json_string = PostJson.ToString();
		Debug.Log ("PostQuesAttempt Json" + json_string);
		// convert json string to byte
		var formData = System.Text.Encoding.UTF8.GetBytes(json_string);

		www = new WWW(postQuesAttemptUrl, formData, postHeader);
		StartCoroutine(PostQuestionAttempt(www));
	}
	public void setQuesAnsBasedOnIndex(int index,bool createFlag){
		quesAnsList.setUserIndex(index);
		QuesAnsPair currQuesAnsPair = quesAnsList.getCurrentQuesAnsPair ();
		setQuesView (currQuesAnsPair,createFlag);
		setAnsOpView (currQuesAnsPair,createFlag);
		setOtherView (currQuesAnsPair,createFlag);
		setOtherVariable (currQuesAnsPair,createFlag);
		entryAnim();
	}
	//Animation methods
	public override void entryAnim(){
		//For entry animation
	}
	public override void exitAnim(){
		//For exit animation
	}
	public override void correctAnsAnim(){
		//For correct answer animation
	}
	public override void incorrectAnsAnim(){
		//For incorrect animation
	}


	//Setting Question Views
	public  void setQuesView(QuesAnsPair currQuesAnsPair,bool createFlag){
		//For setting getCurrentQuesAnsPair.getQuesText ()on view 
		var quesText = GameObject.Find("QuesText");
		string question_text = currQuesAnsPair.getQuesText ();
		question_text = stringWrapper.changeString (question_text);
		quesText.GetComponent<TEXDraw>().text  =  "";
		quesText.GetComponent<TEXDraw>().text  =  question_text;
		Debug.Log("Question text"+currQuesAnsPair.getQuesText ());
		Debug.Log("Url of question image"+currQuesAnsPair.getQuesImage ());
		if (currQuesAnsPair.getQuesImage ().Length > 0) {
			
			StartCoroutine (LoadImage (@currQuesAnsPair.getQuesImage (), "QAPanel"));
		} else{
			

		}
	}
	//Setting Answer Views
	public  void setAnsOpView(QuesAnsPair currQuesAnsPair,bool createFlag){
		Answer answer = currQuesAnsPair.getAnswer ();
		List<string> answerOption = answer.getAnsOptions ();
		List<string> answerOptionImg = answer.getAnsOptionImgs ();
		if (answerOption.Count == 0) {
			incrementIndex ();

		} else {
			if (createFlag) {
				for (int j = 0; j < answer.getAnsOptions ().Count; j = j + 1) {
					GameObject ansOpObject = Instantiate (ansOption);
					ansOpObject.name = "AnsOp";
					ansOpObject.transform.parent = GameObject.Find ("QAPanel").transform;
					answerOption [j] = stringWrapper.changeString (answerOption [j]);
					ansOpObject.GetComponentInChildren<TEXDraw> ().text = answerOption [j];

					Button answerButton = ansOpObject.GetComponent<Button> ();
					int tempInt = j;
					answerButton.onClick.AddListener (() => AnswerSelected (tempInt));

					if (j == quesAnsList.getCurrentQuesAnsPair ().getAnswer ().userSelectedOp) {
						Debug.Log ("Correct answer selected + initiating tick"+quesAnsList.getCurrentQuesAnsPair ().getAnswer ().userSelectedOp);
						GameObject tickGO = Instantiate (tickImage);
						tickGO.name = "TickImg";
						tickGO.transform.parent = ansOpObject.transform;
						tickGO.transform.position = new Vector3(370f,0f , 0f);
					} else {
						GameObject tick = getChildGameObject (ansOpObject,"TickImg");
						Destroy (tick);
					}
				}
//			} else {
//				GameObject[] ansOpTagged = GameObject.FindGameObjectsWithTag ("AnswerOption");
//
//				for (int j = 0; j < Math.Min (answer.getAnsOptions ().Count, ansOpTagged.Length); j = j + 1) {
//					GameObject ansOpObject = ansOpTagged [j];
//					answerOption [j] = stringWrapper.changeString (answerOption [j]);
//					ansOpObject.GetComponentInChildren<TEXDraw> ().text = answerOption [j]; 
//
//					Debug.Log(j+" Option text is "+answerOption [j]);
//					//GO: Set color of user selected option to light color
//					Debug.Log("User selected option is "+quesAnsList.getCurrentQuesAnsPair ().getAnswer ().userSelectedOp);
//
//					if (j == quesAnsList.getCurrentQuesAnsPair ().getAnswer ().userSelectedOp) {
//						Debug.Log ("Correct answer selected + initiating tick"+quesAnsList.getCurrentQuesAnsPair ().getAnswer ().userSelectedOp);
//						GameObject tickGO = Instantiate (tickImage);
//						tickGO.name = "TickImg";
//						tickGO.transform.parent = ansOpObject.transform;
//						tickGO.transform.position = new Vector3(370f,0f , 0f);
//					} else {
//						GameObject tick = getChildGameObject (ansOpObject,"TickImg");
//						Destroy (tick);
//					}
//				}
//				Debug.Log("Current Status "+answer.getAnsOptions ().Count + " tagged data "+ansOpTagged.Length);
//				int optionDiff = ansOpTagged.Length - answer.getAnsOptions ().Count;
//
//				if (optionDiff == 0) {
//					//No. of present answer option views is same as question options
//				
//				} else if (optionDiff > 0) {
//					//Answer Views are more than Question answer options
//					for (int j = Math.Min (answer.getAnsOptions ().Count, ansOpTagged.Length); j < Math.Max (answer.getAnsOptions ().Count, ansOpTagged.Length); j = j + 1) {
//						GameObject ansOpObject = ansOpTagged [j];
//						//GO: Destroy answerOption
//						Destroy (ansOpObject);
//					}
//				
//				} else if (optionDiff < 0) {
//					//Answer Views are less than Question answer options
//					for (int j = Math.Min (answer.getAnsOptions ().Count, ansOpTagged.Length); j < Math.Max (answer.getAnsOptions ().Count, ansOpTagged.Length); j = j + 1) {
//						//GO: Create answerOption
//						Debug.Log("Starting afresh "+answer.getAnsOptions ().Count + " tagged data "+ansOpTagged.Length);
//						GameObject ansOpObject = Instantiate (ansOption);
//						ansOpObject.name = "AnsOp";
//						ansOpObject.transform.parent = GameObject.Find ("QAPanel").transform;
//						answerOption [j] = stringWrapper.changeString (answerOption [j]);
//						ansOpObject.GetComponentInChildren<TEXDraw> ().text = answerOption [j];
//
//						Button answerButton = ansOpObject.GetComponent<Button> ();
//						int tempInt = j;
//						answerButton.onClick.AddListener (() => AnswerSelected (tempInt));
//
//						Debug.Log(j+" Option text is "+answerOption [j]);
//						//GO: Set color of user selected option to light color
//						Debug.Log("User selected option is "+quesAnsList.getCurrentQuesAnsPair ().getAnswer ().userSelectedOp);
//						if (j == quesAnsList.getCurrentQuesAnsPair ().getAnswer ().userSelectedOp) {
//							Debug.Log ("Correct answer selected + initiating tick"+quesAnsList.getCurrentQuesAnsPair ().getAnswer ().userSelectedOp);
//							GameObject tickGO = Instantiate (tickImage);
//							tickGO.name = "TickImg";
//							tickGO.transform.parent = ansOpObject.transform;
//							tickGO.transform.position = new Vector3(370f,0f , 0f);
//						} else {
//							GameObject tick = getChildGameObject (ansOpObject,"TickImg");
//							Destroy (tick);
//						}
//					}
//				}

			}

		}
	}
	static public GameObject getChildGameObject(GameObject fromGameObject, string withName) {
		//Author: Isaac Dart, June-13.
		Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
		foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
		return null;
	}
	IEnumerator LoadImage(string @Url,string ImageGameObject)
	{
		Debug.Log ("Initiated");

		quesImageGO = Instantiate (imagePrefab,GameObject.Find("QAPanel").transform ) as GameObject;
		quesImageGO.transform.SetSiblingIndex (0);
		WWW www = new WWW(Url);
		yield return www;

		Debug.Log ("Loaded");
		quesTexture = www.texture;


		Image img = quesImageGO.GetComponent<Image>();
		img.sprite = Sprite.Create (quesTexture, new Rect (0, 0, quesTexture.width, quesTexture.height),Vector2.zero);
		LayoutElement layout = quesImageGO.GetComponent<LayoutElement>();
		layout.minWidth = 1.5f*quesTexture.width;
		layout.minHeight = 1.5f*quesTexture.height;
	}
	//Setting other views
	public void setOtherView(QuesAnsPair currQuesAnsPair,bool createFlag){
		//GO: current Index = 
		currQuesAnsPair.getIndex();
		//GO: Max index =
		quesAnsList.getMaxIndex ();
	}
	//Setting other variables
	public void setOtherVariable(QuesAnsPair currQuesAnsPair,bool createFlag){
		
		currentTime = (float)currQuesAnsPair.getUserTimeTaken();
		Debug.Log ("Setting current time " + currentTime);
	}

	//On Selection of answer
	void AnswerSelected(int buttonNo)
	{
		int solved = 0;
		Debug.Log ("Button clicked = " + buttonNo+ currentTime);
		GameObject[] ansOps;
		ansOps = GameObject.FindGameObjectsWithTag("AnswerOption");
		GameObject tickGO = Instantiate (tickImage);
		tickGO.name = "TickImg";
		tickGO.transform.parent = ansOps[buttonNo].transform;
		quesAnsList.getCurrentQuesAnsPair ().getAnswer ().userSelectedOp = buttonNo;
		if (quesAnsList.getCurrentQuesAnsPair ().getAnswer ().correctAnswer (buttonNo)) {
			solved = 3;
		} else {
			solved = 2;
		}
		//GO: Set color of user selected option to light color
		quesAnsList.postQuestionCalculations (solved, (int)(10*currentTime));

		//Starting next question
		incrementIndex();
	}
	void OnSwipeDetected (Swipe direction)
	{
		switch (direction) {
		case global::Swipe.Left:
			{
				incrementIndex ();
				break; 
			}
		case global::Swipe.Right:
			{
				decrementIndex ();
				break; 
			}
		}
	}
	public void incrementIndex(){
		Destroy (quesImageGO);
		Destroy (quesTexture);

		GameObject[] ansOpTagged = GameObject.FindGameObjectsWithTag ("AnswerOption");
		foreach (GameObject go in ansOpTagged) { 
			Destroy (go);
		}
		quesAnsList.setUserTimeTaken ( (int)(10*currentTime));

		if (quesAnsList.getUserIndex () < quesAnsList.getMaxIndex ()-1) {
			Debug.Log("Middle of quiz"+quesAnsList.getUserIndex ()+"<"+quesAnsList.getMaxIndex ());
			float currentIndex = (float)quesAnsList.getUserIndex ();
			float totalIndex = (float)quesAnsList.getMaxIndex ();
			setProgressBar (currentIndex, totalIndex, "ProgressFGImg");
			int increment_index = quesAnsList.getUserIndex () + 1;
			setQuesAnsBasedOnIndex (increment_index, true);

		} else {
			//GO: Show end of quiz page
			Debug.Log("End of quiz reached");
			postQuestionAttempt ();

		}
	}
	public void decrementIndex(){
		Destroy (quesImageGO);
		Destroy (quesTexture);

		GameObject[] ansOpTagged = GameObject.FindGameObjectsWithTag ("AnswerOption");
		foreach (GameObject go in ansOpTagged) { 
			Destroy (go);
		}
		quesAnsList.setUserTimeTaken ( (int)(10*currentTime));
		if (quesAnsList.getUserIndex() > 0) {
			Debug.Log("Middle of quiz"+quesAnsList.getUserIndex ()+"<"+quesAnsList.getMaxIndex ());
			float currentIndex = (float)quesAnsList.getUserIndex ();
			float totalIndex = (float)quesAnsList.getMaxIndex ();
			setProgressBar (currentIndex, totalIndex, "ProgressFGImg");
			int increment_index = quesAnsList.getUserIndex () - 1;
			setQuesAnsBasedOnIndex (increment_index, true);
		}
	}
	public void setProgressBar(float current, float total, string gameObjectName){
		//Debug.Log("updating progress of "+gameObjectName);
		GameObject progressGO = GameObject.Find (gameObjectName);
		if (progressGO != null) {
			
			Image img = progressGO.GetComponent<Image> ();
			img.fillAmount = (float)(current / total);
		}
	}
	//During question methods



	//On Answer Submission
	public override bool answerValidated(){
		//Check for answer
		return true;
	}

	public void totalTimeFinished(){
		//GO: Total time finished
	}
	public override void Update(){
		
		//Debug.Log (currentTime);
		currentTime += Time.deltaTime;
		totalTime += Time.deltaTime;
		if ((quesAnsList.getMaxTotalTime() > 0)&&(quesAnsList.getMaxTotalTime() > ((int)totalTime)-1)) {
			totalTimeFinished ();
		}
		//Debug.Log ("currentTime"+currentTime+"maxCurrentTime"+maxCurrentTime);
		setProgressBar (currentTime, maxCurrentTime, "ProgressOverlayImg");
		//GO: Set totalTime remaining
	}
	public void updateButtonContent(string gameObjectName,string text, bool interact){
		//Set Button Text
		var beginbutton = GameObject.Find(gameObjectName).GetComponent<Button> ();
		beginbutton.GetComponentInChildren<Text>().text  = text;
		beginbutton.interactable = interact;

	}

}
