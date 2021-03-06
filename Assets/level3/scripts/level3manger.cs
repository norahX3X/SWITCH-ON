﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class level3manger : MonoBehaviour
{
	//array of boxes in the scean 
	public List<GameObject> boxes = new List<GameObject> ();
	//array countains random objects
	public GameObject[] random;
	//array of objects repetesion 
	private int[] randomthingsrepet;
	//game size (if there is 4 boxes =>size=2)
	public int size;
    //current tutched box
	BoxControl current;
	//current opend boxes 2 at time 
	private BoxControl[] currentboxes = new BoxControl[2];
	//number of tries 
	private int nroftries = 0;
	//time
	public Text time;
	private float timer,timeongoing;
	public float timelimitbysec;
	//score
	public Text score;
	private int scorenum=0;
	//if anyy thing goes wrong
	public GameObject debugbox;
	//public Scene scene;
	public int levelnum;
	//instruction dialog 
	public GameObject instructionpanle,warning;
	//exit + pause dialog
	public GameObject exitD,pauseslider,panleOncamera;
	GameObject hitObject;
	public boxesHit hit;
	bool paused=false;
	//win and lose objects 
	public GameObject endpanle,wining,lose;
	public Text Topscore,timetext,cscoretext;
	bool end=false,started=false;
	//sounds 
	public AudioSource pickup,mismatch,Endwin,Youwin,Endlose,background,getCloser,hurryup,noo,yay;

	//to mange level detels 
	LevelManger levelmanger=new LevelManger();

	//NetworkClient myClient;


	// Use this for initialization
	void Start ()
	{
		//Screen.orientation = ScreenOrientation.Portrait;
		//start timer depend on the complexity 
		timer = Time.time + timelimitbysec;
		randomthingsrepet = new int[random.Length];
		foreach (GameObject box in boxes) {
			placeRandomobj (box);
			//depend on random integer from 1-10 if its 7 placeBounsStar (box) 
		}
		setScore ();
		if (!instructionpanle.activeInHierarchy && !warning.activeInHierarchy)
			Showinstruction (false);

	}

	//to place objects insaid boxws 
	public void placeRandomobj (GameObject box)
	{
		int randomInt = GetRandom ();
		//to be fair check if it not od or not all have same object 
		if (randomthingsrepet [randomInt] % 2 != 0 ||(randomthingsrepet [randomInt]<2))// (randomthingsrepet [randomInt] < (size / 2))) 
		{
			Vector3 newpos = new Vector3 (box.transform.position.x, box.transform.position.y + 0.05f, box.transform.position.z);
			GameObject newObject = (GameObject)Instantiate (random [randomInt],newpos, box.transform.rotation, box.transform)as GameObject;
			if(newObject)
			newObject.transform.localScale = new Vector3 (0.007f, 0.007f, 0.007f);// (box.transform.localScale.x-1f, box.transform.localScale.y-1f, box.transform.localScale.z-1f);//(0.005f, 0.005f, 0.005f);// // change its local scale in x y z format
			
			#if UNITY_EDITOR
			UnityEditor.Selection.activeObject = newObject;
			#endif
			//newObject.AddComponent<>();
			//NetworkServer.Spawn (newObject);
			randomthingsrepet [randomInt] += 1;
			box.GetComponent<BoxControl> ().insaideobj = newObject;
		} else
			placeRandomobj (box);


	}
	public void placeBounsStar (GameObject box){
		//add 3D star above 3 boxes or 4  

	}


	int GetRandom ()
	{
		return Random.Range (0, random.Length);
	}

	void Update ()
	{//time decreasing 
		//if inst +pause+home not active 
		//+stop if win or lose 
		//if (paused) {
		//} else
		if (!paused && started) {
			Time.timeScale = 1;
		}else 
			Time.timeScale = 0;
		
		//time untile instruction closes 
		if (started && (!end)) {
			Timedecrising();
		}
	}

	void Timedecrising(){
		timeongoing= timer - Time.time;
		string min = ((int)timeongoing / 60).ToString ();
		string sec = ((int)timeongoing % 60).ToString ();
		//Debug.Log (timeongoing);
		if (timeongoing == 5f)
			hurryup.Play ();
		if (timeongoing <= 0) {//if time is up 
			timeend();
		} else//we can change it to red if its close to end by 5 or 10 sec 
			time.text = min + ":" + sec;
	}

	public void touchsomething (GameObject hitobject)//if player hit something the hit example will send the hited object to her e
	{
		//get it and openit or close it the mange will be in other method 
		current = hitobject.GetComponent<BoxControl> ();

		//check if there is more than 2 are open if yes then colse them thin opent the tutched one 
		if (current != null) {
			if (current.isOpen ()) {
				//debugbox.text= "This box is already open!";//thats not efftint 
			} else {
				current.openit ();
			}	
		} else//if its star then go to star script
			print ("not box!");//message that says tuch me again!
	}
	public void farAway(){
		getCloser.Play ();
		StartCoroutine("setDebugText", "Get closer"); 


	}

	//check if there is 2 opened before
	public void CheckBoxes (BoxControl bc)
	{
		//not executed 
		if(bc.insaideobj==null) {
			Debug.Log("null thing");
			bc.closeit();
		}
		//PrefabUtility.GetPrefabParent(gameObject) == null && PrefabUtility.GetPrefabObject(go) != null; // Is a prefab
		if (currentboxes [0] == null && bc!=currentboxes [0])
			currentboxes [0] = bc;
		else {
			currentboxes [1] = bc;
			nroftries++;
			//check if it's not the same box !
			//PrefabUtility.FindPrefabRoot get same name why not working ?
			//Debug.Log ("1 " +PrefabUtility.FindPrefabRoot(currentboxes [0].insaideobj)+"2"+PrefabUtility.FindPrefabRoot(currentboxes [1].insaideobj));
			//if (newAddedGO.name == string.Format("{0}(Clone)", myNeedPrefab.name) {};
			if (currentboxes [0].insaideobj.name ==currentboxes [1].insaideobj.name)//finally works it check the prefab name if its same 
				BoxesMatching ();
			else
				BoxesNotMatching ();

			StartCoroutine("emptyCurrentBoxes");


		}
	}

	public void BoxesMatching ()
	{
		currentboxes [0].mached ();
		currentboxes [1].mached ();
		size--;
		StartCoroutine("setDebugText", "YaaY +4 "); 
		pickup.Play();
		yay.Play ();
		addscore (4);
		if (size == 0)
			GameEnd (true);
	}

	public void BoxesNotMatching ()
	{
		//StartCoroutine("setDebugText", "OH NO -2 "); 
		//removescore (2);

		mismatch.Play ();
		noo.Play ();
		StartCoroutine("closeCurrentBoxes");
		//closeCurrentBoxes();
	}

	IEnumerator closeCurrentBoxes()
	{
		yield return new WaitForSeconds(1);
		currentboxes [0].closeit ();
		currentboxes [1].closeit ();
	}
	IEnumerator emptyCurrentBoxes()
	{
		yield return new WaitForSeconds(1);

	currentboxes [0] = null;
	currentboxes [1] = null;
	}

	public void timeend ()
	{
		
		if (size == 0)
			GameEnd (true);
		else
			GameEnd (false);

	}
	public void addscore (int addedscore)
	{
		scorenum += addedscore;
		setScore ();
	}
	public void removescore (int removedscore)
	{
		
		scorenum -= removedscore;
		if (scorenum <= 0)
			scorenum = 0;
		setScore ();
	}
	private void setScore(){
		score.text = scorenum.ToString ();
	}

	void GameEnd (bool win)
	{
		background.Stop ();
		end = !end;
		activateGray (true);
		endpanle.SetActive (true);
		timetext.text = doneTime();
		cscoretext.text = score.text;
		if (win) {
			Endwin.Play();
			Youwin.Play ();
			wining.SetActive (true);
			levelmanger.win (levelnum, scorenum, timetext.text.ToString());//doneTime()
			Topscore.text = levelmanger.getTopScore (levelnum).ToString();
				//scorenum.ToString();
			//scene = SceneManager.GetActiveScene();
			//.buildIndex ,GetHashCode()
			/*debugbox.SetActive(true);
			Text t =debugbox.GetComponentInChildren(typeof(Text))as Text;
			t.text =  "tries: " + nroftries;*/
		} else {
			Endlose.Play ();
			lose.SetActive (true);
			}
	}
	string doneTime(){
		float DoneTime =  timelimitbysec-timeongoing;
		//timer = Time.time + timelimitbysec;

		return((int)DoneTime/60).ToString ()+":"+((int)DoneTime % 60).ToString();
	}


	public void pause (bool open)
	{
		if (instructionpanle.activeInHierarchy)
			Showinstruction (false);
		pauseslider.SetActive(open);
		activateGray (open);
	}

	public void home (bool open)
	{
		exitD.SetActive(open);
		activateGray (open);
	}
	 void activateGray (bool open)
	{
		panleOncamera.SetActive (open);
		paused = open;
		//boxesHit test=hitObject.GetComponent ("boxesHit")as boxesHit;
			hit.enabled = !open;
	}
	public void closeLevel ()
	{
		home (false);
		//SceneManager.LoadScene ("world");
		levelmanger.LoudHome ();
	}
	public void ReplayLevel ()
	{
		levelmanger.Replay ();
	}
   public void Showinstruction (bool show)
	{
		instructionpanle.SetActive (show);
		activateGray (show);
		started = true;
	}
	public IEnumerator setDebugText(string textin )
	{
		debugbox.SetActive(true);
		Text t =debugbox.GetComponentInChildren(typeof(Text))as Text;
		t.text = textin;
		yield return new WaitForSeconds(1);
		t.text = "";
		debugbox.SetActive(false);


	
	}
	public void Showarning ()
	{
		warning.SetActive (false);
		Showinstruction (true);
	}


}
