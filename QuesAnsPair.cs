using UnityEngine;
using System.Collections;
using System;
public class QuesAnsPair: IComparable<QuesAnsPair>{

	string ques_text="",hint_text="",ans_desc="";
	int index;
	Answer answer;

	//Difficulty Flag = 0-Easy,1-Medium,2-Hard
	int difficulty_flag=0;

	//Solved Flag = 0-Not displayed, 1-Displayed and Skipped, 2-solved incorrectly, 3-solved correctly
	int solved_flag=0;

	int maxScore=0, currentScore=0;
	int maxTimeAllotted=0,timeTaken=0;

	public string getQuesText(){
		return ques_text;
	}
	public Answer getAnswer(){
		return answer;
	}
	public string getHintText(){
		return hint_text;
	}
	public string getAnsDesc(){
		return ans_desc;
	}
	public int getIndex(){
		return index;
	}
	public int getDifficultyflag(){
		return difficulty_flag;
	}
	public void setQuesText(string text){
		ques_text = text;
	}
	public void setAnswer(Answer trialAnswer){
		answer = trialAnswer;
	}
	public void setHintText(string text){
		hint_text = text;
	}
	public void setAnsDesc(string text){
		ans_desc = text;
	}
	public QuesAnsPair(string question,Answer givenAnswer,string hint, string answer_description, int listIndex){
		ques_text = question;
		answer = givenAnswer;
		hint_text = hint;
		ans_desc = answer_description;
		index = listIndex;
	}
	public int CompareTo(QuesAnsPair other)
	{
		if(other == null)
		{
			return 1;
		}

		//Return the difference in power.
		return other.getIndex() - getIndex() ;
	}

}
