using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class QuesAnsList{
	static int currentCount=0;
	int lives=0,maxLives=0;
	int totalScore;
	DifficultyWiseAllottmentList difficultyAllottmentList;
	AttemptedQuesInfoList attemptedQuesList;
	//QuesAnsPair qaPair;
	List<QuesAnsPair> qaList = new List<QuesAnsPair>();
	public QuesAnsList (){
		string hint = "";
		string desc = "";
		string question1 = "A number for whose sum of all its factors is equal to twice the number";
		string answer1 = "perfect";
		Answer answer = new Answer (answer1);
		add (question1, answer, hint, desc,0);
	}
	public void add(string question,Answer givenAnswer,string hint, string answer_description, int listIndex){
		QuesAnsPair quesAnsPair = new QuesAnsPair (question,givenAnswer, hint, answer_description, listIndex);
		qaList.Add(quesAnsPair);
	}
	public QuesAnsPair getCurrentQuesAnsPair(){
		return qaList[currentCount];
	}
	public void setCurrentCount(int count){
		currentCount = count;
	}
	public void postQuestionCalculations(int solved){
		QuesAnsPair quesAnsPair = getCurrentQuesAnsPair ();
		int difficulty_level = quesAnsPair.getDifficultyflag ();
		int index = quesAnsPair.getIndex ();
		int newLevel = 0;
		registerAttempt (index,solved,difficulty_level);

		if (solved == 2) {
			//Question solved incorrectly
			newLevel = correctQuesCalculations(difficulty_level);
		} else if (solved == 3) {
			//Question solved correctly
			newLevel = inCorrectQuesCalculations(difficulty_level);
		}

	}
	public int correctQuesCalculations(int difficulty_level){
		if (difficultyAllottmentList.differenceByLevel (difficulty_level) > 0) {
			return difficulty_level;
		} else if (difficultyAllottmentList.differenceByLevel (difficulty_level) == 0) {
			return incrementDifficultyLevel (difficulty_level);
		} else if (difficultyAllottmentList.differenceByLevel (difficulty_level) < 0) {
			if (attemptedQuesList.lastSolvedLevelRepeat ()) {
				return incrementDifficultyLevel (difficulty_level);
			} else {
				return difficulty_level;
			}
		} else {
			return difficulty_level;
		}
	}

	public int inCorrectQuesCalculations(int difficulty_level){
		if(attemptedQuesList.lastSolvedLevelRepeat()){
			return decrementDifficultyLevel(difficulty_level);
		}else{
			return difficulty_level;
		}
	}
	public int incrementDifficultyLevel(int level){
		return level;
	}
	public int decrementDifficultyLevel(int level){
		return level;
	}
	public void registerAttempt(int index, int solved, int difficlty_level){
		difficultyAllottmentList.registerAttempt (difficlty_level);
		attemptedQuesList.addAttemptedQues (index, solved, difficlty_level);
	}
}


