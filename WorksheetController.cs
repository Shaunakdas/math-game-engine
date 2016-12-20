using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorksheetController : MonoBehaviour {
	public GameObject preWorksheetGO;
	public GameObject worksheetScoreboardGO;

	public static readonly string domain = "localhost:3000/";
	PreWorksheetController preWorksheetObject;
	WorksheetScoreBoardController worksheetScoreboardObject;

	Animator anim;
	int worksheetPanelHash, postWorksheetHash, scoreboardHash,restartWorksheetHash;
	// Use this for initialization
	void Start () {
		//Initiate animation components
		anim = GetComponent<Animator>();
		worksheetPanelHash = Animator.StringToHash("beginTrigger");
		postWorksheetHash = Animator.StringToHash("postWorksheetTrigger");
		scoreboardHash = Animator.StringToHash("scoreboardTrigger");
		restartWorksheetHash = Animator.StringToHash("restartWorksheetTrigger");

		//Initiate script components of child gameobjects
		preWorksheetObject = (PreWorksheetController) preWorksheetGO.GetComponent(typeof(PreWorksheetController));
		worksheetScoreboardObject = (WorksheetScoreBoardController) worksheetScoreboardGO.GetComponent(typeof(WorksheetScoreBoardController));
	}
	public string getDomainAddress(){
		return domain;
	}
	//Calling PreWorksheetPanel
	public void openPreWorksheetPanel (){
		//Calling API call for qAObject
		//qAObject.getQAListAPI();
	}
	//Calling WorksheetPanel
	public void openWorksheetPanel (){
		//Called when button is pressed in PreWorksheetPanel
		anim.SetTrigger (worksheetPanelHash);


	}
	//Calling PostWorksheetPanel
	public void openPostWorksheetPanel (){
		//Called when button is pressed in WorksheetPanel
		anim.SetTrigger (postWorksheetHash);
	}
	//Calling ScoreboardPanel
	public void openScoreboardPanel (){
		//Called when button is pressed in PostWorksheetPanel
		anim.SetTrigger (scoreboardHash);
	}


	//Calling New StreamwiseScene
	public void openStreamwiseScene (){
		//Called when button is pressed in UserResultPanel
		SceneManager.LoadScene("StreamwiseScene");
	}
	//Calling PrePostWorksheetPanel
	public void restartPostWorksheetPanel (){
		//Called when button is pressed in WorksheetPanel
		anim.SetTrigger (restartWorksheetHash);
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
		
		}
	}
	public void updateButtonContent(string gameObjectName,string text, bool interact){
		//Set Button Text
		var beginbutton = GameObject.Find(gameObjectName).GetComponent<Button> ();
		beginbutton.GetComponentInChildren<Text>().text  = text;
		beginbutton.interactable = interact;

	}
}
