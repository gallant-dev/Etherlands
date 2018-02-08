using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Auras : MonoBehaviour {

    Player_Controller controller;

	// Use this for initialization
	void Start () {
        controller = GetComponentInParent<Player_Controller>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Player")
        {
            //Player_Controller otherController = collider.GetComponent<Player_Controller>();

            controller.UseAura(collider.gameObject);
        }
    }
}
