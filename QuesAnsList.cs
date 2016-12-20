using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class QuesAnsList{

	//Display
	int max_lives=0,user_lives=0;
	int user_score=0;
	int max_total_time=0,user_total_time=0;
	int max_index =0;static int user_current_index=0;
	public List<QuesAnsPair> QAList{get; set;}

	//Behind the scene
	public int DisplayId{get; set;}
	DifficultyWiseAllottmentList difficultyAllottmentList;
	AttemptedQuesInfoList attemptedQuesList;

	public QuesAnsList (){
		QAList = new List<QuesAnsPair>();
	}
	//Adding question to QuesAnsList
	public void add(string question,Answer givenAnswer,string hint, string answer_description, string question_image, int listIndex,  int backendId){
		QuesAnsPair quesAnsPair = new QuesAnsPair (question,givenAnswer, hint, answer_description, question_image, listIndex, backendId);
		QAList.Add(quesAnsPair);
	}

	//Getting current QuesAnsPair
	public QuesAnsPair getCurrentQuesAnsPair(){
		return QAList[user_current_index];
	}
	public int getLives(){
		return user_lives;
	}
	public int getTotalScore(){
		return user_score;
	}
	public int getMaxIndex(){
		return QAList.Count;
	}
	public int getUserIndex(){
		return user_current_index;
	}
	public int getMaxTotalTime(){
		return max_total_time;
	}

	//Setting user_current_index
	public void setUserIndex(int count){
		user_current_index = count;
	}
	public void setUserTotalTime(int time){
		user_total_time = time;
	}
	public void setMaxParams (int maxLives, int maxTotalTime){
		max_lives = maxLives;
		max_total_time = maxTotalTime;
	}
	//Setting Lives
	public int decrementLives(int solved){
		if (solved == 2) {
			if (user_lives > 0) {
				user_lives--;
			}
		}
		return user_lives;
	}

	//Setting TotalScore
	public int addTotalScore(int solved, int timeElapsed){
		user_score += calcScore(solved,timeElapsed);
		return user_score;
	}


	//After question is attempted
	public void postQuestionCalculations(int solved, int timeElapsed){
		QuesAnsPair quesAnsPair = getCurrentQuesAnsPair ();
		quesAnsPair.setSolvedFlag (solved);
		int currentScore = calcScore (solved, timeElapsed);
		quesAnsPair.setCurrentScore(currentScore);

		//Hints and total Score 


		//Next processes
		nextQuestionCalculations (solved, timeElapsed, currentScore);
		recordKeeping (solved, timeElapsed, currentScore);
	}
	public void setUserTimeTaken( int timeElapsed){
		QuesAnsPair quesAnsPair = getCurrentQuesAnsPair ();
		quesAnsPair.setTimeTaken (timeElapsed);
	}

	//Calculations for current Score
	public int calcScore(int solved, int timeElapsed){
		Debug.Log("calcScore timeElapsed"+timeElapsed);
		int maxTimeAllotted = getCurrentQuesAnsPair ().getMaxTimeAllotted ();
		int maxScore = getCurrentQuesAnsPair ().getMaxScore ();
		int score = maxScore;
		if (solved == 3) {
			score = (int)(29 * maxTimeAllotted - 27 * timeElapsed) * (maxScore) / (2 * timeElapsed);
			//score = (int)(29 * maxTimeAllotted - 27 * timeElapsed) * (maxScore) / (20 * timeElapsed);
		}
		return score;
	}
	//Calculations for next Question
	public void nextQuestionCalculations(int solved, int timeElapsed, int currentScore){
	}

	//Record Keeping work
	public void recordKeeping(int solved, int timeElapsed, int currentScore){
	}
}


