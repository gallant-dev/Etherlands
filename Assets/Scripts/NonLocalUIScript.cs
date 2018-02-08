using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NonLocalUIScript : MonoBehaviour {

    public Slider nonLocalHealthSliderRight;
    public Slider nonLocalHealthSliderLeft;
    public Slider nonLocalEnergySliderRight;
    public Slider nonLocalEnergySliderLeft;

    public Transform playerTransform;
	
	// Update is called once per frame
	void Update () {
        transform.position = playerTransform.position;
	}
}
