﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeText : MonoBehaviour {
	public Text timer;
	public Text finalTimer;
	public Text finalTimerWin;
	public Button resumeButton;

	//private LevelManger m;
	public float timeStamp;
	public bool usingT=false;
	//public GameObject battry;
	public GameObject gameOver;
	public AccelerationControlScript grass;
	public ThenderDisable thender;
	public BattryChange battryHide;
	public BattryShow jump;
	public GameObject back;
	public Resume_Paused resume;
	public instruction instruc;
	public GameObject chare;
	public string winT;
	public GameObject gameWin;
	public LevelManger levelmanger;
	public TimeText timeScript;
	public Scene scene;
	public AudioSource Endwin,Endlose,background;

	// Use this for initialization
	void Start () {
		//here to set how many time you want
		Screen.orientation = ScreenOrientation.Portrait;
		setTimer (40);
		gameOver.SetActive (false);
		back.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		//here to make cheak if the timer work set the tex of time
		if (usingT)
			SetUIText ();
	}
	//set timer to start
	public void setTimer(float time){
		//here if not using timer return to start
		if (usingT)
			return;
		//time stamping 
		timeStamp = Time.time + time;
		usingT = true;
	}

	//to set time to text

	public void SetUIText(){
		float timeLeft = timeStamp - Time.time;
		//if timer reaching 0 will go to another function
		if (timeLeft <= 0) {
			FinishTimer ();
			return;
		}
		float hours;
		float minu;
		float sec;
		float minisSecound;
		GetTimeValues (timeLeft, out hours, out minu, out sec,out minisSecound);
		if (hours > 0) {
			timer.text = string.Format ("{0}", hours, minu);
			finalTimer.text = string.Format ("{0}", hours, minu);
			winT=finalTimer.text;
			int tt = 40-int.Parse (winT);

			finalTimer.text="0"+" "+":"+" "+tt.ToString ();

			string ww = tt.ToString ();
			finalTimerWin.text = "0"+" "+":"+" "+ww;

		} else if (minu > 0) {
			timer.text = string.Format ("{0}", minu, sec);
			finalTimer.text = string.Format ("{0}", minu, sec);
			int tt = 40-int.Parse (winT);

			winT=finalTimer.text;
			finalTimer.text="0"+" "+":"+" "+tt.ToString ();

			string ww = tt.ToString ();
			finalTimerWin.text = "0"+" "+":"+" "+ww;
		} else {
			timer.text = string.Format ("{0}", sec, minisSecound);
			finalTimer.text=string.Format ("{0}", sec, minisSecound);
			winT=finalTimer.text;
			int tt = 40-int.Parse (winT);

			finalTimer.text="0"+" "+":"+" "+tt.ToString ();;

			string ww = tt.ToString ();
			finalTimerWin.text ="0"+" "+":"+" "+ww;
		}
		
	}
	//make calculation of timer
	public void GetTimeValues(float time,out float hours,out float minu,out float sec,out float minisSecound){
		hours = (int)(time / 3600f);
		minu=(int)((time - hours *3600)/60f);
		sec=(int)((time - hours *3600-minu*60f));
		minisSecound=(int)((time - hours *3600-minu*60-sec)*100);

	}
	//if timer finish this things will apper
	public void FinishTimer(){

		timer.text = "40";
		if (thender.score >=6) {
			thender.finalScore.text=thender.score.ToString();
			thender.scoreTextWin.text=thender.score.ToString();
			gameWin.SetActive (true);
			background.Stop ();
			Endwin.Play ();
			back.SetActive (true);
			grass.enabled = false;
			battryHide.enabled = false;
			jump.enabled = false;
			resume.enabled = false;
		//	scene = SceneManager.GetActiveScene();
			//.buildIndex ,GetHashCode()
			levelmanger.win (thender.level,thender.score ,"0: 40");
			thender.topScore.text=levelmanger.getTopScore (thender.level).ToString();
			thender.enabled = false;
			timeScript.enabled = false;
			usingT = false;
			resumeButton.interactable = false;


		}
		else{
			gameOver.SetActive (true);
			finalTimer.text ="0"+" "+":"+" 40";
			background.Stop ();
			Endlose.Play ();
		back.SetActive (true);
		chare.SetActive (false);
		resume.enabled = false;
		grass.enabled = false;
		battryHide.enabled = false;
		jump.enabled = false;
		usingT = false;
			resumeButton.interactable = false;

		}
	}
}
