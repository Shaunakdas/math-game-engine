using UnityEngine;
using System.Collections;
using System;
public class Answer  {
	int type;

	bool correctbool; string correctFillText; int correctOption=0;

	string[] ansOptions; int optionCount=0;

	public Answer(string[] ansOps, int opCount, int correctOp){
		//SCQ type questions
		type = 0;
		ansOptions = new string[opCount];
		ansOptions = ansOps;
		optionCount = opCount;
		correctOp = correctOption;
	}
	public Answer(bool corrBool){
		//True False type questions
		correctbool = corrBool;
	}
	public Answer(string fillText){
		//Fill in the  blanks type questions
		correctFillText = fillText;
	}


	//Getter methods
	public int getType(){
		return type;
	}
	public string[] getAnsOptions(){
		return ansOptions;
	}
	public bool getCorrectBool(){
		return correctbool;
	}
	public string getCorrectFillText(){
		return correctFillText;
	}
	public int getCorrectOption(){
		return correctOption;
	}
}
