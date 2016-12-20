using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;


public class QuesAnsViewController : MonoBehaviour {
	int currentCount =0;
	QuesAnsList quesAnsList ;
	QuesAnsPair currQuesAnsPair;
	Animator anim;


	//Constructor
	public virtual void Start () {
		quesAnsList = new QuesAnsList();
		quesAnsList.setUserIndex(currentCount);
		currQuesAnsPair = quesAnsList.getCurrentQuesAnsPair ();
		setQuesAnsView ();
		entryAnim();
	}
	//Getter methods
	public int getCurrentCount(){
		return currentCount;
	}
	public QuesAnsList getQuesAnsList(){
		return quesAnsList;
	}
	public QuesAnsPair getCurrQuesAnsPair(){
		return currQuesAnsPair;
	}
	//Animation methods
	public virtual void entryAnim(){
		//For entry animation
	}
	public virtual void exitAnim(){
		//For exit animation
	}
	public virtual void correctAnsAnim(){
		//For correct answer animation
	}
	public virtual void incorrectAnsAnim(){
		//For incorrect animation
	}

	//Setting Question and Answer Views
	public virtual void setQuesAnsView(){
		//Setting Question and Answer View
	}
	public virtual void setQuesView(Text quesText){
		//For setting question and answer view
		quesText.text = (currQuesAnsPair.getQuesText ());
	}
	public virtual void setAnsOptions(){
		//Setting Answer options
	}


	//On Answer Submission
	public virtual bool answerValidated(){
		return false;
	}
	public virtual void submitAnswer(){
		if (answerValidated()) {
			correctAnsAnim ();
		} else {
			incorrectAnsAnim ();
		}
		exitAnim ();
		increaseCount ();
		setQuesAnsView ();
		entryAnim ();
	}
	public virtual void increaseCount(){
		currentCount++;
		quesAnsList.setUserIndex (currentCount);
		currQuesAnsPair = quesAnsList.getCurrentQuesAnsPair ();
	}


	public virtual void Update(){

	}
		
}
