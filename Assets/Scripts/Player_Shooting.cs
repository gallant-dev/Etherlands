//using UnityEngine;
//using System.Collections;
//using UnityEngine.Networking;
//
//public class Player_Shooting : NetworkBehaviour {
//	private int damage = 25;
//	private float range = 200;
//	[SerializeField] private Transform camTransform;
//
//	private Transform aimTarget;
//	private RaycastHit hit;
//
//
//	// Use this for initialization1
//	void Start () {
//		aimTarget = 	
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		CheckIfShooting();
//	}
//
//	void CheckIfShooting(){
//		if(!isLocalPlayer){
//
//		}
//
//		if(Input.GetKeyDown (KeyCode.Mouse0)){
//			CheckIfShooting ();
//		}
//	}
//
//	void Shoot(){
//		if(Physics.Raycast(camTransform.TransformPoint(0, 0, 0.5f), camTransform.forward, out hit, range)){
//			Debug.Log (hit.transform.tag);
//				
//			if(hit.transform.tag == "Player"){
//				string uIdentity = hit.transform.name;
//				CmdTellServerWhoWasShot(uIdentity, damage);
//				}
//			}
//		}
//
//	[Command]
//		void CmdTellServerWhoWasShot (string uniqueID, int dmg){
//			GameObject go = GameObject.Find (uniqueID);
//			go.GetComponent<Player_Health>().DeductHealth(dmg);
//			//Apply damage to that player.
//		}
//}
