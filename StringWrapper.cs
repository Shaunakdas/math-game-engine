using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StringWrapper {

	List<string> oldString;
	List<string> newString;
	public StringWrapper(){
		oldString = new List<string> { "\\div","$$","&#160;","<span>","</span>","\\displaystyle","\\frac","\\cfrac","<br/>","<div>","</div>","\\dfarc","\\times","\\circ","&nbsp","\\delta","\\cong","\\angle","\\sqrt","\\RightArrow","\\left","\\right" };
		newString = new List<string> { "\\div",""," ","","","","\\frac","\\frac","\n","","","\\frac","\\times","\\circ"," ","\\delta","\\approxeq","\\angle","\\root","\\Rightarrow","",""};

	}
	public string changeString(string textToChange){
		for (var i = 0; i < oldString.Count; i++) {
			textToChange = textToChange.Replace (oldString [i], newString [i]);
		}
		textToChange = textToChange.Replace("\n",System.Environment.NewLine);
		//Debug.Log ("StringWrapper text output" + textToChange);
		return textToChange;
	}	

}
