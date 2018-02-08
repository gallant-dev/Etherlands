using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestructibleObject {
	public GameObject respawnable;
	public float respawnTime;
	public float deathTime;

	public DestructibleObject(GameObject newRespawnable, float newRespawnTime, float newDeathTime){
		respawnable = newRespawnable;
		respawnTime = newRespawnTime;
		deathTime = newDeathTime;
	}
}

public class DestructibleRespawner : MonoBehaviour {
	
	List<DestructibleObject>  destructibleList= new List<DestructibleObject>();
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate(){
		CheckToRespawn ();
	}

	void CheckToRespawn(){
		foreach (DestructibleObject obj in destructibleList) {
			if (obj.respawnTime <= (Time.time -  obj.deathTime)) {
				obj.respawnable.SetActive (true);
			}
		}
	}

	public void RespawnTimer(GameObject rspwnble, float rspwnTm, float dthTm){
		destructibleList.Add (new DestructibleObject(rspwnble, rspwnTm, dthTm));
		Debug.Log (destructibleList.ToString ());
	}
}
