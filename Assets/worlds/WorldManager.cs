﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//using System.Collections;
using UnityEngine.SceneManagement;


public class WorldManager : MonoBehaviour {
	/// <summary>
	/// this class countain the world mithods and dialogncs 
	/// </summary>

	//if the vareble or method didn't appear on unity use [System.Serializable]
	//doors button 
	public Button level01Button,level02Button, level03Button, level04Button, level05Button, level06Button;
	//locked doors
	public Image Image02, Image03, Image04, Image05, Image06;
	//doors changes
	public Sprite win, close;
	public bool nextavi=true;
	int levelPassed;
	public Animator shaking,shaking2;
	public Text totalScore;
	//levels dialogs 
	//public
	LevelManger levelsmanger;
	int leveltopscore;
	int worldtopscore;
	string leveltime;
	public GameObject levelDetailsY, levelDetailsG,levelDetails;
	public settingManger setting;

	public Text scoreText;
	public Text timeText;
	public Text levelnumber;
	int clevel=-1;
	public Button playbtn;
	public SetGetName dialogsmanger;


	public float fakeIncrement=0f;
	public float fakeTiming=0f;

	//to change the parent 
	public Transform world2;
	public Transform BlueC;
	public Transform RedC;

	public AsyncOperation async;
	public GameObject loadingscreen;
	public Slider slider;

	public AudioSource button_sound;

	//do once in the bigining 
	void Start(){
		Screen.orientation = ScreenOrientation.Landscape;

		//ShowInstruction (true);
		//get total score 
		levelsmanger=new LevelManger();
		setting = GameObject.Find("setting manger").GetComponent<settingManger> ();
		playbtn.onClick.AddListener(levelToLoad);

		//get the last passed level from playerprefs
		levelPassed = levelsmanger.getlevelpassed();
		//to change doors images depend on last passed level
		switch (levelPassed) {
		case 6:
			level06Button.image.overrideSprite = win;
			goto case 5;
		case 5:
			Image06.enabled = false;
			level05Button.image.overrideSprite = win;
			goto case 4;
		case 4:
			Image05.enabled = false;
			level04Button.image.overrideSprite = win;
			goto case 3;
		case 3:
			Image04.enabled = false;
			level03Button.image.overrideSprite = win;
			goto case 2;
		case 2:
			//setting.charcter2.transform.localPosition = new Vector3(204f,55f,0f);//level02Button.transform.position;//change posiition of red character

			Image03.enabled = false;
			//level03Button.image.overrideSprite = current; we change it to close and win only 
			level02Button.image.overrideSprite = win;
			goto case 1;
		case 1:
			//setting.charcter2.transform.localPosition = new Vector3(-26.025f,-84f);//level02Button.transform.position;//change posiition of red character

			Image02.enabled = false;
			level01Button.image.overrideSprite = win;
			break;
		}
		changePosition (levelPassed);
		worldtopscore = levelsmanger.getTotalScreore ();
		totalScore.text = worldtopscore.ToString ();

	}

	//to made animation between the world and level we use invoke("method",sf)
	public void levelToLoad ()
	{
		//slider.gameObject.SetActive (true);


		if (clevel != -1) {
			loadingscreen.SetActive (true);

			StartCoroutine (LoudAsyncroulsly (clevel));
			//Screen.orientation = ScreenOrientation.AutoRotation;
			Screen.orientation = ScreenOrientation.Portrait;
			//SceneManager.LoadScene (levelnum);


		}
	}



	IEnumerator LoudAsyncroulsly (int levelnum){


		/*yield return new WaitForSeconds(1);

		slider.value+=fakeIncrement;
		yield return new WaitForSeconds(fakeTiming);
		levelsmanger.LoudLevel (levelnum);

			yield return null; 
*/

		//loadingscreen.SetActive ();
		async=SceneManager.LoadSceneAsync(levelnum);
		async.allowSceneActivation =false;
		while(async.isDone==false){
			slider.value = async.progress;
			if(async.progress==0.9f){
				slider.value = 1f;
				async.allowSceneActivation = true;
			}
			yield return null;
		}

	}


	//settings page animations 
	public void opensettings(Animator anim){
		if (!isDActivated ()) {
			anim.SetBool ("settings", true);
			button_sound.Play ();
		}

	}

	public void closesettings(Animator anim){
		anim.SetBool ("settings", false);
	}

	//world animations 
	public void next(Animator anim){
		if (nextavi&& !isDActivated()) {
			button_sound.Play ();
			anim.SetBool ("thereisnext", true);
			nextavi = false;
		}
	}

	//slide to previos world
	public void previos(Animator anim){
		if (!nextavi && !isDActivated() ) {
			anim.SetBool ("thereisnext", false);
			nextavi = true;
		}
	}

	public void shakeworld(int worldnum){
		if (worldnum == 1) {
			shaking.SetBool ("shake", true);
			Invoke ("stopshakeworld", 2);
		}  else {
			shaking2.SetBool ("shake2", true);
			Invoke ("stopshakeworld2", 0.5f);
		}
		//GetComponent<CameraShake>().ShakeCamera(20f, 1f);
		//Camera.main.gameObject.GetComponent<CameraShake>().ShakeCamera(20f, 1f);
		//anim.SetBool ("shake", true);
		//Originalpos=;
		//Camera.main.transform.position=Originalpos +Vector3.Scale(SmoothRandom.GetVector2(Shakespeed--),Shakerange);
	}
	public void stopshakeworld(){
		shaking.SetBool ("shake", false);

	}
	public void stopshakeworld2(){
		shaking2.SetBool ("shake2", false);

	}
	public void leveldetails(int levelnum){
		if (isDActivated()) {return;}  

		if (levelnum - 1 <= levelPassed) {

			clevel = levelnum;
			leveltopscore = levelsmanger.getTopScore (levelnum);
			leveltime = levelsmanger.getTime (levelnum);
			scoreText.text = leveltopscore.ToString();
			timeText.text = leveltime.ToString();
			levelnumber.text = levelnum.ToString ();
			levelDetails.SetActive (true);
			if (leveltopscore == 0 ) {
				//timeText.text = "0:00";
				levelDetailsG.SetActive (true);
			}//end if 

			else {
				levelDetailsY.SetActive (true);
			}
		}
		else if(levelnum>3)
			shakeworld (2);  

		else
			shakeworld (1);  

	}
	/// Closes the level details.



	public void closeLevelDetails() {
		clevel = -1;
		levelDetails.SetActive (false);
		levelDetailsY.SetActive (false); 
		levelDetailsG.SetActive (false); 

	}
	//method for test the system 
	public void reset(){
		PlayerPrefs.DeleteAll();
	}

	public bool isDActivated(){
		if (dialogsmanger.nameDialouge.activeInHierarchy ||levelDetailsG.activeInHierarchy ||levelDetailsY.activeInHierarchy || dialogsmanger.welcomeBack.activeInHierarchy || dialogsmanger.welcomeDialouge.activeInHierarchy)
			return true;
		return false;
	}

	//change chacracter position
	public void changePosition(int level){

		switch( level+1 ){

		case 2:
			setting.charcter2.transform.localPosition = new Vector3 (-26.025f, -84f);
			setting.charcter1.transform.localPosition = new Vector3 (-26.025f, -84f);
			break;
		case 3:
			setting.charcter2.transform.localPosition = new Vector3 (204f, 55f, 0f);
			setting.charcter1.transform.localPosition = new Vector3 (204f, 55f, 0f);
			break;
		case 4:
			RedC.SetParent (world2);
			BlueC.SetParent (world2);

			setting.charcter2.transform.localPosition = new Vector3 (-269.6f,49.3f, 0f);
			setting.charcter1.transform.localPosition = new Vector3 (-269.6f,49.3f, 0f);
			break;		
		case 5:
			RedC.SetParent (world2);
			BlueC.SetParent (world2);

			setting.charcter2.transform.localPosition = new Vector3 (-49.7f,-73.1f, 0f);
			setting.charcter1.transform.localPosition = new Vector3 (-49.7f,-73.1f, 0f);
			break;
		case 6:
			RedC.SetParent (world2);
			BlueC.SetParent (world2);

			setting.charcter2.transform.localPosition = new Vector3 (245f,-9f, 0f);
			setting.charcter1.transform.localPosition = new Vector3 (245f,-9f, 0f);
			break;

		}//end switch 
	}

}//end whole



