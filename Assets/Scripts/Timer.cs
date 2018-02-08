using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class Timer : NetworkBehaviour {
    float matchTimer;
    [SyncVar]
    float startTime;

    Text text;
	void Start () {
		text = GameObject.Find("Timer").GetComponent<Text>();
		if (isServer) {
			startTime = (float)Network.time;
		}
	}

	void OnGUI() {
		int minutes = Mathf.FloorToInt(matchTimer / 60.00f);
		int seconds = Mathf.FloorToInt(matchTimer - minutes * 60.00f);
		string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);

		text.text = niceTime;
	}

	void LateUpdate () {
            TimerCount();
	}

	void TimerCount(){
        matchTimer = (float)Network.time - startTime;
	}	
}