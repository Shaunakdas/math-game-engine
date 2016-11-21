using System;
using System.Collections;
using System;
public class DifficultyWiseAllottment{
	int difficulty_level;
	int allotted_ques,attempted_ques;

	public DifficultyWiseAllottment (int diff_level,int allotted,int attempted){
		difficulty_level = diff_level;
		allotted_ques = allotted;
		attempted_ques = attempted;
	}
	public void registerAttempt(){
		attempted_ques++;
	}
	public int getLevel(){
		return difficulty_level;
	}
	public int getAllotted(){
		return allotted_ques;
	}
	public int getAttempted(){
		return attempted_ques;
	}
	public void changeAllottment(int allotted,int attempted){
		allotted_ques = allotted;
		attempted_ques = attempted;
	}
}


