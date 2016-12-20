using UnityEngine;
using System.Collections;
using System;
public class QuesAnsPair: IComparable<QuesAnsPair>{

	//Display variables
	string ques_text="",hint_text="",ans_desc="",ques_image = "";
	Answer answer;
	int max_score=0, user_score=0;
	int max_time_allotted=0,user_time_taken=0;


	//Behind the scene
	public int BackendId{get; set;}
	int index;
	//Difficulty Flag = 0-Easy,1-Medium,2-Hard
	int difficulty_flag=0;
	//Solved Flag = 0-Not displayed, 1-Displayed and Skipped, 2-solved incorrectly, 3-solved correctly
	int user_solved_flag=0;

	public string getQuesText(){
		return ques_text;
	}
	public string getQuesImage(){
		return ques_image;
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
	public int getMaxScore(){
		return max_score;
	}
	public int getMaxTimeAllotted(){
		return max_time_allotted;
	}
	public int getUserTimeTaken(){
		return user_time_taken;
	}
	public int getUserScore(){
		return user_score;
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
	public void setSolvedFlag(int solved){
		user_solved_flag = solved;
	}
	public void setCurrentScore(int score){
		user_score = score;
	}
	public void setTimeTaken(int time){
		Debug.Log("QuesAnsPair time taken "+user_time_taken+"ques_text"+ques_text);
		user_time_taken = time;
	}
	public QuesAnsPair(string question,Answer givenAnswer,string hint, string answer_description,string question_image, int listIndex, int backendId){
		ques_text = question;
		answer = givenAnswer;
		hint_text = hint;
		ans_desc = answer_description;
		index = listIndex;
		BackendId = backendId;
		ques_image = question_image;
	}
	public QuesAnsPair(){
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
