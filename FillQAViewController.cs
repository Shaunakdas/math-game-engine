using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;


public class FillQAViewController : QuesAnsViewController {
	int startCharCount = 2,totalOptionCount = 10;
//	QuesAnsList quesAnsList ;
//	QuesAnsPair currQuesAnsPair;
	string currAnsText = "",totalAnsText = "";
	Text quesText, ansText;
	Animator anim;
	// Use this for initialization
	public override void Start () {
		base.Start ();
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


	//Setting Question and Answer Views
	public override void setQuesAnsView(){
		//For setting question view 
		base.setQuesView (quesText);

		//For setting answer view
		totalAnsText = base.getCurrQuesAnsPair().getAnswer().getCorrectFillText();
		currAnsText = totalAnsText.Substring (0, startCharCount);
		ansText.text = (currAnsText);
		setAnsOptions();
	}
	public override void setAnsOptions(){
		int charReq = 0;
		char[] ansOptions = currAnsText.ToCharArray();
		char[] totalChar  = {'a','b','c','d','e','f','g','h','i','j','k'};
		//Copy from ansOptions to new list "totalOptions"
		List<char> totalOptions = new List<char>(ansOptions);

		//calculate no. of element required more
		charReq = totalOptionCount - totalOptions.Count;

		//for each element required, randomly select element from totalChar
		while (charReq > 0) {
			System.Random rnd = new System.Random ();
			int index = rnd.Next (totalChar.Length);
			totalOptions.Add (totalChar [index]);
			charReq--;
		}
		//randomise totalOptions List Order
		System.Random rand = new System.Random ();
		totalOptions = totalOptions.OrderBy(item => rand.Next()).ToList();
		//setText of all textView of "options" tag to totalOptions[i]
	}



	//During question methods
	public void addText (char c){
		currAnsText = currAnsText + c;
		ansText.text = (currAnsText);
	}
	public void deleteText (){
		currAnsText.Substring (0, currAnsText.Length - 1);
		ansText.text =  (currAnsText);
	}



	//On Answer Submission
	public override bool answerValidated(){
		//Check for answer
		return (ansText.text == totalAnsText);
	}


}
