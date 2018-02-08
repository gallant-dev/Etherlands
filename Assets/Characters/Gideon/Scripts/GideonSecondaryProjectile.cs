using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GideonSecondaryProjectile : NetworkBehaviour {

    public Player_Controller controller;

    public bool activated = false;

    public float rateOfExpansion = 0.001f;
    public float maximumExpansion = 5f;

    public float oppositeForceApplied = -1.2f;

    void Update()
    {
        if(activated == true)
        {
            transform.localScale += Vector3.Lerp(Vector3.one, transform.localScale * maximumExpansion, rateOfExpansion * Time.deltaTime);
            if(transform.localScale.x >= maximumExpansion)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player" && activated == false)
        {
            activated = true;
            Player_Controller hitController = col.GetComponent<Player_Controller>();
            hitController.verticalSpeed *= oppositeForceApplied;
            hitController.moveDirection *= oppositeForceApplied;
            Debug.Log(hitController.moveDirection);
            hitController.isControllable = false;
            hitController.knockedBack = true;


            if (isServer)
            {
                controller.ApplyDamage(transform.position, 1, false);
            }
        }
        //controller.ApplyDamage(col.transform.position, 1);
    }
}
