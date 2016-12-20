using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.UI;
public class GetClassOptions : MonoBehaviour {
	List<Standard> standard_list = new List<Standard>();
	UserProfile user;
	public GameObject diagnosticTestGO;
	DiagnosticTestController diagnosticTestObject;
	// Update is called once per frame
	void Update () {
	
	}


	void Start () {
		StartCoroutine(GetStandard());
		diagnosticTestObject = (DiagnosticTestController) diagnosticTestGO.GetComponent(typeof(DiagnosticTestController));
	}


	IEnumerator GetStandard() {
		UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get ( "localhost:3000/"+"api/standards/get_standards");
		yield return www.Send ();

		if (www.isError) {
			Debug.Log (www.error);
		} else {
			Debug.Log( www.downloadHandler.text);
			var the_JSON_string = www.downloadHandler.text; 
			var N = JSON.Parse(the_JSON_string);
			var versionString = N["standards"][0]["subjects"][0]["full_name"].Value;   
			Debug.Log(versionString);
			var standardCount = N ["standards"].Count;
			//for loop on standardCount
			//add standard from API
			for (int i = 0; i < standardCount; i = i + 1) {
				int standard_number = int.Parse(N ["standards"] [i] ["standard_number"].Value);
				int standard_id = int.Parse(N ["standards"] [i] ["subjects"] [0] ["subject_id"].Value);
				int subject_id = int.Parse(N ["standards"] [i] ["subjects"] [0] ["standard_id"].Value);
				string subject_name = N ["standards"] [i] ["subjects"] [0] ["full_name"].Value;
				standard_list.Add (new Standard (standard_id, subject_id, standard_number, subject_name));
				Debug.Log ("subject name is" + subject_name);
			}
			standard_list.Sort ();
			//standardList.sort



			//Add standard to Dropdown
			var dropdownGO = GameObject.Find("Dropdown");
			var dropdown = dropdownGO.GetComponent<Dropdown>();

			dropdown.options.Clear();
			foreach (Standard option in standard_list)
			{
				Debug.Log("Adding standard option to dropdown"+option.getStandardName());
				dropdown.options.Add(new Dropdown.OptionData(option.getStandardName()));
			}
			diagnosticTestObject.updateAPIStatus ("GetStandard",true);
			//Make dropdown visible


		}
	}
	public void selectDropdownOption(){
		//Event to be called when dropdown selected option is changed
	}
	public void submitStandard(){
		//Event to be called when standard is submitted
		string first_name = GameObject.Find("FirstNameInput").GetComponent<InputField>().text;
		string last_name = GameObject.Find("LastNameInput").GetComponent<InputField>().text;
		string email = GameObject.Find("EMailInput").GetComponent<InputField>().text;
		string number = GameObject.Find("NumberInput").GetComponent<InputField>().text;
		user = new UserProfile (first_name, last_name, email, number);

		var dropdown = GameObject.Find("Dropdown").GetComponent<Dropdown>();
		Debug.Log("Dropdown value is "+dropdown.value);
	}
	public void updateButtonContent(string gameObjectName,string text, bool interact){
		//Set Button Text
		var beginbutton = GameObject.Find(gameObjectName).GetComponent<Button> ();
		beginbutton.GetComponentInChildren<Text>().text  = text;
		beginbutton.interactable = interact;
	}
}
