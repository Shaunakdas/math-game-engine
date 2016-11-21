using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;


public class TFQAViewController : QuesAnsViewController {
	
	//	QuesAnsList quesAnsList ;
	//	QuesAnsPair currQuesAnsPair;

	Text quesText;
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
		setAnsOptions();
	}
	public override void setAnsOptions(){
		//Setting True false option view
	}



	//During question methods




	//On Answer Submission
	public override bool answerValidated(){
		//Check for answer
		return true;
	}


}
