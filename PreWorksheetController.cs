using UnityEngine;
using System.Collections;

public class PreWorksheetController : MonoBehaviour {
	public GameObject worksheetGO;
	WorksheetController worksheetObject;
	QuesAnsList quesAnsList ;
	// Use this for initialization
	void Start () {
		worksheetObject = (WorksheetController) worksheetGO.GetComponent(typeof(WorksheetController));
	}
	public void getQAListAPI(){
		StartCoroutine(GetQAList());
		quesAnsList = new QuesAnsList();
	}
	IEnumerator GetQAList() {
		string getQuesListUrl = worksheetObject.getDomainAddress() + "api/diagnostic_tests/get_test.json?standard_id=";
		UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get (getQuesListUrl);
		yield return www.Send ();

		if (www.isError) {
			Debug.Log (www.error);
		} else {

			worksheetObject.updateAPIStatus ("GetQAList",true);
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
