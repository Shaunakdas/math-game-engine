using UnityEngine;
using System.Collections;

public class UserProfile  {
	static string first_name="";
	static string last_name="";
	static string email="";
	static string number="";
	static Standard user_standard;

	public UserProfile(){

	}
	public UserProfile(string first_given, string last_given, string email_given, string number_given){
		first_name = first_given;
		last_name = last_given;
		email = email_given;
		number = number_given;
	}
	public void setStandard(Standard standard){
		user_standard = standard;
	}
	public Standard getStandard(){
		return user_standard;
	}
	public string getFirstName(){
		return first_name;
	}
	public string getLasttName(){
		return last_name;
	}
	public string getEmail(){
		return email;
	}
	public string getNumber(){
		return number;
	}
}
