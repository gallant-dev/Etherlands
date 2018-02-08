using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowGravityPath : MonoBehaviour {

    public float gravityFactor;

    void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Player")
        {
            Player_Controller controller = collider.GetComponent<Player_Controller>();

            if (controller.localGamePlayerBool == true)
            {
                if (controller.localGamePlayer.team == 0 && transform.IsChildOf(controller.blueSide))
                {
                    controller.gravity *= gravityFactor;
                    controller.verticalSpeed *= gravityFactor;
                }
                else if (controller.localGamePlayer.team == 1 && transform.IsChildOf(controller.redSide))
                {
                    controller.gravity *= gravityFactor;
                    controller.verticalSpeed *= gravityFactor;
                }
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            Player_Controller controller = collider.GetComponent<Player_Controller>();

            if (controller.localGamePlayerBool == true)
            {
                controller.gravity = 20f;
            }
        }
    }
}
