using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LandingScript :MonoBehaviour {


	public float landingPosition;
	public Slider slider;

	// Update is called once per frame

	void FixedUpdate () {
		landingPosition = transform.position.y;
	}
	void Update(){
		if (slider != null) {
			slider.value = landingPosition;
		}
	}
}