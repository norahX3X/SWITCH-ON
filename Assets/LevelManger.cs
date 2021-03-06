﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManger : MonoBehaviour {
	int levelpassed ;
	int leveltopscore;
	string leveltime;
	int leveltoptime;

	/// <summary>
	/// this class is countain the shared metods for all levels 
	/// </summary>
	void Start(){
		levelpassed = getlevelpassed();
	}

	public void win (int levelnum,int cscore, string ctime){
		leveltopscore = getTopScore (levelnum);
		leveltime = getTime (levelnum);

		if (cscore >= leveltopscore) {
			subTotalScore (leveltopscore);
			addTotalScore (cscore);
			setTopScore (levelnum, cscore);
			setTime (levelnum, ctime);
		} 
		if (levelnum > levelpassed) {
			setlevelpassed (levelnum);
		}
	}
	public int getlevelpassed(){
		return PlayerPrefs.GetInt ("LevelPassed");
	}
	public void setlevelpassed(int levelnum){
		PlayerPrefs.SetInt ("LevelPassed", levelnum);
	}

	public void addTotalScore(int csore)
	{
		PlayerPrefs.SetInt ("TotalScore", getTotalScreore()+ csore);

	}


	public void subTotalScore(int csore)
	{
		if (getTotalScreore() -  csore<0)
		PlayerPrefs.SetInt ("TotalScore", 0);
		else
			PlayerPrefs.SetInt ("TotalScore", getTotalScreore() -  csore);
		

	}


	//we don't need lose method  , we well not save any data in db 


	public void LoudHome(){
		Time.timeScale = 1;
		SceneManager.LoadScene ("world");

	}
	public void LoudLevel(int levelnum){//or string 
		if (levelnum != -1) {
			//Screen.orientation = ScreenOrientation.AutoRotation;
			Screen.orientation = ScreenOrientation.Portrait;
			SceneManager.LoadScene (levelnum);


		}

	}
	/// <summary>
	/// *Level(level number)Score type:int
	/*The top score of winning level 
	Ex:Level3Score,5
	*Level(level number)Time type:string as mm:ss
	The time related to top score 
	Ex:Level3Time,1:50

	*LevelPassed type:int 
	The last level passed 
	If (LevelPassed>=current level )
	Don’t change 
	Else 
	LevelPassed=current level


		*Username type:string
		Username dash 

		*TotalScore type:int 
		The total score change when top score change totalscor—=topscore 
		*CapeColor type:int or bool or any   
		*Sound type:bool*/
	/// </summary>

	public int getTopScore (int levelnum){
		//string t = levelnum.ToString ();
	//	string top = "Level" + levelnum + "Score";
		return PlayerPrefs.GetInt ("Level" + levelnum + "Score");

	}
	public string getTime (int levelnum){
		return PlayerPrefs.GetString ("Level"+levelnum+"Time","0:00");

	}
	public void setTopScore (int levelnum,int topscore){
		PlayerPrefs.SetInt ("Level"+levelnum+"Score",topscore);

	}
	public void setTime (int levelnum,string time){
		PlayerPrefs.SetString ("Level"+levelnum+"Time",time);

	}

	public void Replay(){//or could be astring 
		Time.timeScale = 1;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}

	public int getTotalScreore()
	{
		 return PlayerPrefs.GetInt ("TotalScore");
	}
}
