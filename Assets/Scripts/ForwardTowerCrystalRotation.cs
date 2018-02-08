using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardTowerCrystalRotation : MonoBehaviour {

    public float crystalRotationRate = 1f;
    public Vector3 rotationDirection;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.Rotate(rotationDirection * crystalRotationRate * Time.deltaTime);
	}
}
