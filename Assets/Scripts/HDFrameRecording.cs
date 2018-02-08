using UnityEngine;
using System.Collections;
using System.IO;

public class HDFrameRecording : MonoBehaviour {

	public bool HDScreenshotsOn;

	void Update(){
		if(Input.GetKey(KeyCode.K) && HDScreenshotsOn){
			ScreenCapture.CaptureScreenshot (Application.dataPath + "/Screenshots/" + "shot" + shotCount + ".png");
			shotCount += 1;
		}
	}

//	public int maxFrames; //amount of frames you want to record before closing the game
//
	int shotCount = 0;

	void Awake () {
//		Application.targetFrameRate = 1; //forces frame rate to 1
		if (!System.IO.Directory.Exists("Screenshots")) //check if "Screenshots" folder exists
		{
			System.IO.Directory.CreateDirectory(Application.dataPath + "/Screenshots");
		}
	}
//
//	void Update () {
//		if (Input.GetKeyDown (KeyCode.K)) {
//			if (shotCount <= maxFrames) { //we don't want to include the first frame since it's a mess
//				Application.CaptureScreenshot (Application.dataPath + "/Screenshots/" + "shot" + shotCount + ".png");
//				shotCount += 1;
//			} 
////			else { //keep making screenshots until it reaches the max frame amount
////				StopRecording (); //quit game
////			}
//		}
//	}
//
//	public void StopRecording() //you can call this function for different reasons (e.g camera animation stops)
//	{
//		Application.Quit();
//	}
}