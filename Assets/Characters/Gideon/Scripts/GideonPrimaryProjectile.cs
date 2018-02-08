using UnityEngine;
using UnityEngine.Networking;

public class GideonPrimaryProjectile : NetworkBehaviour {
	[SerializeField] GameObject debris;

    [SyncVar]
    public GameObject player;

	Player_Controller controller;

    void Start()
    {
        controller = player.GetComponent<Player_Controller>();
    }

	void OnCollisionEnter(Collision col){
        if (col.gameObject != player)
        {
            Vector3 explosionPos = transform.position;
            if (isServer && controller != null)
            {
                controller.ApplyDamage(explosionPos, 0, true);
            }
            GameObject go = (GameObject)Instantiate(debris, transform.position, transform.rotation);
            go.SetActive(true);
            Destroy(gameObject);
        }
	}
}