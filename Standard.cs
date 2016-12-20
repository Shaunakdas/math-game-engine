using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class Standard: IComparable<Standard>{
	int standard_id,subject_id,standard_number;
	string subject_name;
	public Standard(int stand_id,int subj_id,int stand_num, string subj_name){
		standard_id = stand_id;
		subject_id = subj_id;
		standard_number = stand_num;
		subject_name = subj_name;
	}
	public int getStandardNumber(){
		return standard_number;
	}
	public string getStandardName(){
		return subject_name;
	}
	public int getStandardId(){
		return standard_id;
	}
	public int CompareTo(Standard other)
	{
		if(other == null)
		{
			return 1;
		}

		//Return the difference in power.
		return other.getStandardNumber() - getStandardNumber() ;
	}
}
