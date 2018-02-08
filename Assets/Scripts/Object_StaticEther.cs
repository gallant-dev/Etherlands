using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Object_StaticEther : NetworkBehaviour {

	// Use this for initialization
	public int charge = 1;

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Player") {
			col.gameObject.GetComponent<Player_Stats> ().AddCharge (charge);
			Destroy(gameObject);
		}
	}
}
