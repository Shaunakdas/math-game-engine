using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DiagnosticTestController : MonoBehaviour {
	public GameObject getClassOptionsGO;
	public GameObject beginGO;
	public GameObject qAGO;
	public GameObject userResultGO;
	public static readonly string domain = "localhost:3000/";
	GetClassOptions getClassOptionsObject;
	BeginPanelHandler beginObject;
	DiagQAViewController qAObject;
	Animator anim;
	int beginPanelHash,duringPanelHash, waitingPanelHash, explainerPanelHash, openResultPanelHash;
	// Use this for initialization
	void Start () {
		//Initiate animation components
		anim = GetComponent<Animator>();
		beginPanelHash = Animator.StringToHash("beginTrigger");
		duringPanelHash = Animator.StringToHash("duringTrigger");
		waitingPanelHash = Animator.StringToHash("waitingTrigger");
		explainerPanelHash = Animator.StringToHash("explainerTrigger");
		openResultPanelHash = Animator.StringToHash("openResultTrigger");

		//Initiate script components of child gameobjects
		getClassOptionsObject = (GetClassOptions) getClassOptionsGO.GetComponent(typeof(GetClassOptions));
		beginObject = (BeginPanelHandler) beginGO.GetComponent(typeof(BeginPanelHandler));
		qAObject = (DiagQAViewController) qAGO.GetComponent(typeof(DiagQAViewController));
		//GetClassOptions userResultObject = (GetClassOptions) userResultGO.GetComponent(typeof(GetClassOptions));
	}
	public string getDomainAddress(){
		return domain;
	}
	//Calling GetClassOptionsPanel
	public void openGetClassOptionsPanel (){
		
	}
	//Calling BeginPanel
	public void openBeginPanel (){
		//Called when button is pressed in GetClassOptionsPanel
		anim.SetTrigger (beginPanelHash);
		//Calling API call for qAObject
		qAObject.getQAListAPI();

	}

	//Calling DuringPanel
	public void openDuringPanel (){
		//Called when button is pressed in BeginPanel
		anim.SetTrigger (duringPanelHash);
	}

	//Calling WaitingPanel
	public void openWaitingPanel (){
		//Called when final answer button is pressed in DuringPanel
		anim.SetTrigger (waitingPanelHash);
	}

	//Calling ResultFormatExplainerPanel
	public void openResultFormatExplainerPanel (){
		//Called when button is pressed in openWaitingPanel
		anim.SetTrigger (explainerPanelHash);

	}

	//Calling UserResultPanel
	public void openResultPanel (){
		//Called when button is pressed in ResultFormatExplainerPanel
		anim.SetTrigger (openResultPanelHash);
	}

	//Calling New Streamwise Scene
	public void openStreamwiseScene (){
		//Called when button is pressed in UserResultPanel
		SceneManager.LoadScene("StreamwiseScreen");
	}




	// Update is called once per frame
	void Update () {
	
	}
	public void updateAPIStatus (string APIMethodName,bool success){
		switch (APIMethodName)
		{
		case "GetStandard":
			updateButtonContent ("GCOPBeginBtn", "Get Started", success);
			break;
		case "GetQAList":
			updateButtonContent ("BPBeginBtn", "Lets start the test!", success);
			break;
		case "PostQuestionAttempt":
			updateButtonContent ("RFEPBeginBtn", "Get Results", success);
			break;
		
		}
	}
	public void updateButtonContent(string gameObjectName,string text, bool interact){
		//Set Button Text
		var beginbutton = GameObject.Find(gameObjectName).GetComponent<Button> ();
		beginbutton.GetComponentInChildren<Text>().text  = text;
		beginbutton.interactable = interact;

	}
}
