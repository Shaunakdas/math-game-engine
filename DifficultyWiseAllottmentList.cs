using System;
using System.Collections;
using System.Collections.Generic;
public class DifficultyWiseAllottmentList{
	List<DifficultyWiseAllottment> allottmentList = new List<DifficultyWiseAllottment>();
	int maxLevels=0;
	public DifficultyWiseAllottmentList (){
		
	}
	public void addDifficultyLevel(int level,int ques_allotted, int ques_attempted){
		
		DifficultyWiseAllottment allottment = allottmentList.Find (e => e.getLevel () == level);
		if (allottment != null) {
			allottment.changeAllottment (ques_allotted, ques_attempted);
		} else {
			maxLevels += 1;
			allottmentList.Add (new DifficultyWiseAllottment (level, ques_allotted, ques_attempted));
		}
	}
	public void registerAttempt(int level){
		DifficultyWiseAllottment allottment = allottmentList.Find (e => e.getLevel () == level);
		if (allottment != null) {
			int ques_allotted = allottment.getAllotted();
			int ques_attempted = allottment.getAttempted();	
			if (ques_allotted > ques_attempted) {
				allottment.changeAllottment (ques_allotted, ques_attempted+1);
			}

		} else {
			
		}
	}
	public int differenceByLevel(int level){
		DifficultyWiseAllottment allottment = allottmentList.Find (e => e.getLevel () == level);
		if (allottment != null) {
			return allottment.getAllotted () - allottment.getAttempted ();

		} else {
			return 0;
		}
	}

}

