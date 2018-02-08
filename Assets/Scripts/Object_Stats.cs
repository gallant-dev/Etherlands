using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;

public class Object_Stats : NetworkBehaviour {

	[SyncVar][SerializeField] private float syncObjectHealth;

	[SerializeField] GameObject debris;
	[SerializeField] GameObject staticEther;

	private bool shouldDie = false;
	public bool isDead = false;

	public bool timedRespawn = false;
	public float respawnDelay = 5f;

    public int objectTeam = 0;

	public delegate void ObjectDieDelegate();
	public event ObjectDieDelegate EventDie;

	DestructibleRespawner respawner;

	void Start(){
		syncObjectHealth = 100.0f;
		EventDie += DisableObject;
		EventDie += EnableObject;

		respawner = gameObject.GetComponentInParent<DestructibleRespawner> ();

        //If gameObject is on red side, change objectTeam to 1
        if(transform.IsChildOf(GameObject.Find("Red Side").transform))
        {
            objectTeam = 1;
        }
	}

	private void OnEvent(byte eventCode, object content, int senderID){
		if (eventCode == 10) {
			syncObjectHealth -= (float)content;
		}
	}

	void OnDisable(){
		EventDie -= DisableObject;
		EventDie -= EnableObject;
	}

	void OnEnable(){
		syncObjectHealth = 100.0f;
	}

	void Update(){
		CheckCondition ();
	}
		
	void CheckCondition(){

		if(syncObjectHealth <= 0 && !shouldDie && !isDead){
			shouldDie = true;
		}

		if(syncObjectHealth <= 0  && shouldDie){
			if(EventDie != null){
				EventDie();
			}
		}

		if (shouldDie && isDead) {
			if (timedRespawn == true) {
				if (EventDie != null) {
					EventDie ();
				}
			}
		}
	}

	void DisableObject(){
		if (syncObjectHealth <= 0.0f && !isDead) {
			gameObject.SetActive(false);
			SetRespawnTimer ();
            if (debris != null)
            {
                GameObject debrisGo = (GameObject)Instantiate(debris, transform.TransformPoint(0f, -1f, 0f), transform.rotation);
                debrisGo.transform.localScale = transform.localScale;
            }
			GameObject staticEtherGo = (GameObject)Instantiate(staticEther, transform.position, transform.rotation);
			staticEtherGo.transform.localScale = transform.localScale;
		} 
		isDead = true;
	}

	void EnableObject(){
		if (syncObjectHealth > 0.0f && isDead) {
		} 
		isDead = false;
	}

	void SetRespawnTimer(){
		if (timedRespawn == true) {
			float deathTime = Time.time;
			respawner.RespawnTimer (gameObject, respawnDelay, deathTime);
		} else {
			return;
		}
	}

	public void TakeDamage(float value){
        syncObjectHealth -= value;
	}
}
