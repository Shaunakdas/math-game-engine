﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System;
using SimpleJSON;

public class FillBlankQAViewController : QuesAnsViewController {
	//Display Variables
	int totalOptionCount = 4;
	float currentTime=0,totalTime=0,maxCurrentTime = 90;
	QuesAnsList quesAnsList ;
	FillBlankQANetworkController commonQANetworkObject;
	List<GameObject> ImageGOList,AnsOpGOList;

	//Behind the scene

	//GameObject Reference
	public GameObject questionTextGO,qaPanelGO;

	//Prefabs
	public GameObject ansOption, imagePrefab;

	//GameObject textureList to hold reference to all textures downloaded
	List<Texture2D> textureList;

	//Game specific GameObject
	public GameObject blankGO,questionBlockGO,skipBtnGO,submitBtnGO,deleteBtnGO;

	//Game specific Prefabs

	//To hold blank Text generated by user after ever user click
	string blankText;
	//To hold answer Option Index of each character that user adds to blank text
	List<int> blankTextRefList; 

	// Use this for initialization
	public override void Start () {
		quesAnsList = new QuesAnsList();
		textureList = new List<Texture2D> ();
		setQAList ();
	}
	public override void setQAList(){
		Debug.Log ("getQAListAPI started");
		commonQANetworkObject = (FillBlankQANetworkController) gameObject.GetComponent(typeof(FillBlankQANetworkController));
		quesAnsList = new QuesAnsList();
		commonQANetworkObject.setQAListJSON (quesAnsList);

	}
	public override void getQAListCallFinished(){
		//Get QA List API finished. Now Display work can start.
		setQuesAnsBasedOnIndex (0);
	}
	public override string postQAAttempt(){
		Debug.Log ("postQAAttempt started");
		commonQANetworkObject = (FillBlankQANetworkController) gameObject.GetComponent(typeof(FillBlankQANetworkController));

		return commonQANetworkObject.getQAAttemptJSON ( quesAnsList);
	}
	public QuesAnsList getQAList(){
		return quesAnsList;
	}

	//Setting up views
	public override void setQuesAnsBasedOnIndex(int index){
		quesAnsList.setUserIndex(index);
		QuesAnsPair currQuesAnsPair = quesAnsList.getCurrentQuesAnsPair ();
		setQuesView (currQuesAnsPair);
		setAnsOpView (currQuesAnsPair);
		entryAnim();
	}

	//Setting Question Views
	public override  void setQuesView(QuesAnsPair currQuesAnsPair){
		//Setting Question Text
		questionTextGO.GetComponent<TEXDrawNGUI>().text  =  base.getQuestionText(currQuesAnsPair);
		ImageGOList = new List<GameObject> ();
		//Setting Question Image
		if (currQuesAnsPair.getQuesImage ().Length > 0) {
			StartCoroutine (LoadImage (@currQuesAnsPair.getQuesImage (), qaPanelGO));
		} 
		questionEntryAnim (questionTextGO);
		setBlankView ("");
	}
	public void setBlankView(string answer){
		blankText = "";blankTextRefList = new List<int> ();
		setBlankText(answer);
		questionEntryAnim (blankGO);
	}
	public void setBlankText(string answer){
		blankGO.GetComponent<TEXDrawNGUI>().text  =answer;
	}
	//Setting Answer Views
	public override  void setAnsOpView(QuesAnsPair currQuesAnsPair){
		//Breaking answerText into specific characters
		List<string> ansOptionList = currQuesAnsPair.ansOptionList[0].optionText.ToCharArray().Select( c => c.ToString()).ToArray();
		ansOptionList = populateAnsOptionList (ansOptionList);
		if (ansOptionList.Count == 0) {
			changeQuestionIndex (1,-1);
		} else {
			AnsOpGOList = new List<GameObject> ();
			for (int j = 0; j < ansOptionList.Count; j = j + 1) {
				GameObject ansOpObject = (GameObject) InstantiateNGUIGO (ansOption,qaPanelGO.transform,"AnsOp");
				//Setting Answer Option Text
				ansOpObject.GetComponentInChildren<TEXDrawNGUI> ().text = ansOptionList[j];

				//Setting Answer option Image


				//Setting Button onClickListener
				UIButton answerButton = ansOpObject.GetComponent<UIButton> ();
				int tempInt = j;
				EventDelegate.Set(answerButton.onClick, delegate() { AnswerSelected(tempInt); });

				//Keeping reference to current ansOpObject
				AnsOpGOList.Add (ansOpObject);
				answerOptionEntryAnim (ansOpObject);
			}
		}
	}
	public List<string> populateAnsOptionList(List<string> ansOpList){
		if (ansOpList.Count < 10) {
			int remaining = 10 - ansOpList.Count;
			for (int i = 0; i < remaining; i++){
				ansOpList.Add (GetRandomLetter ().ToString ());
			}

		}
		return ansOpList.OrderBy(a => Guid.NewGuid()).ToList();
	}
	
	IEnumerator LoadImage(string @Url,GameObject QAGameObject)
	{
		Debug.Log ("LoadImage Initiated");
		GameObject itemImageGO = InstantiateNGUIGO (imagePrefab,QAGameObject.transform ) as GameObject;
		itemImageGO.transform.SetSiblingIndex (0);
		//Calling url
		WWW www = new WWW(Url);
		yield return www;
		Debug.Log ("Loaded"+Url);
		textureList.Add (www.texture);
		//Setting image Gameobjct with downloaded texture
		base.setImageTexture (itemImageGO, www.texture);
		//Adding new generated image inside 
		ImageGOList.Add (itemImageGO);
	}
	//Animation methods
	public override void entryAnim(){
		//For entry animation
	}
	public override void exitAnim(){
		//For exit animation

	}
	public virtual void questionEntryAnim(GameObject questionItemGO){
		//For entry animation
		//Pending questionStart animation with both questionItems
	}
	public virtual void questionExitAnim(){
		//For exit animation

	}
	public virtual void answerOptionEntryAnim(GameObject answerItemGO){
		//For entry animation
		//Pending answer start animation with all answerItems
	}
	public virtual void answerOptionExitAnim(){
		//For exit animation
	}
	public override void correctAnsAnim(){
		//For correct answer animation
		//Pending correct animation on questionBlackGO,blankGO,ansOpListGO and all buttons
		questionExitAnim(questionBlockGO);questionExitAnim(blankGO);
		answerOptionExitAnim ();
	}
	public override void incorrectAnsAnim(){
		//For incorrect animation
		blankText = getQAList().getCurrentQuesAnsPair().ansOptionList[0].optionText;
		setBlankText(blankText);
		//Pending incorrect animation on blankGO
		//Pending incorrect animation on questionBlackGO,ansOpListGO and all buttons
		questionExitAnim(questionBlockGO);questionExitAnim(blankGO);
		answerOptionExitAnim ();
	}


	//On Selection of answer
	public override void AnswerSelected(int buttonNo)
	{
		Debug.Log ("Button clicked = " + buttonNo+ currentTime);
		blankText += AnsOpGOList [buttonNo].GetComponent<TEXDrawNGUI> ().text;
		blankTextRefList.Add (buttonNo);
		//Pending: Set color of user selected option to light color and interactable false
		setBlankText(blankText);
	}
	public void SkipSelected(){
		int ansOpIndex = blankTextRefList.Last ();
		blankText = blankText.Substring(blankText.Length-1);
		//Pending: Set color of ansOpGoList[ansOpIndex] to dark color and interactable true
		setBlankText(blankText);
	}
	public void SubmitSelected(){
		
		quesAnsList.postQuestionCalculations (getSolutionFlag(quesAnsList,blankText), (float)(currentTime));
		if (base.getSolutionFlag (quesAnsList, blankText) == 3)
			correctAnsAnim ();
		else 
			incorrectAnsAnim ();
		changeQuestionIndex(1,0);
	}
	public  int getSolutionFlag(QuesAnsList currQuesAnsList,string answer){
		if (answerValidated(currQuesAnsList,answer)) {
			return 3;
		} else {
			return 2;
		}
	}
	public bool answerValidated(QuesAnsList currQuesAnsList,string answer){
		return (currQuesAnsList.getCurrentQuesAnsPair ().ansOptionList[0] == answer);
	}
	public override void changeQuestionIndex(int increment,int updated){
		//Destroy (quesImageGO);
		base.destroyGOList(ImageGOList);
		base.destroyGOList (AnsOpGOList);
		textureList.ForEach (itemTexture => Destroy (itemTexture));
		quesAnsList.setUserTimeTaken (currentTime);
		//If right swipe, left swipe or answer selection
		if (updated == -1) {
			if (increment > 0) {
				//Going to next question
				if (quesAnsList.getUserIndex () < quesAnsList.getMaxIndex () - 1) {
					int increment_index = quesAnsList.getUserIndex () + increment;
					setQuesAnsBasedOnIndex (increment_index);

				} else {
					//GO: Show end of quiz page
					openFinalScreen();
				}
			} else {
				//Going to previous question
				if (quesAnsList.getUserIndex () > 0) {
					int increment_index = quesAnsList.getUserIndex () + increment;
					Debug.Log ("Going to question of index " + increment_index);
					setQuesAnsBasedOnIndex (increment_index);
				}
			}
		}else {
			//If question selection
			setQuesAnsBasedOnIndex (updated);
		}
	}
	public void openFinalScreen(){
		Debug.Log ("End of quiz reached");
	}


	//During question methods
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

	}
}