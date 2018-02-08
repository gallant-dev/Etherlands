using UnityEngine;
using UnityEngine.Networking;

public class Player_NetworkSetup : NetworkBehaviour {

	[SerializeField]Camera playerCamera;
	[SerializeField]AudioListener audioListener;
	[SerializeField]GameObject aimTarget;
	[SerializeField] GameObject nonLocalUI;
    Player_Controller controller;
	// Use this for initialization

	public void Start ()
	{
		if (isLocalPlayer) {
			GameObject.Find ("Main Camera").SetActive (false);
			GetComponent<Player_Controller> ().isControllable = true;
			Cursor.visible = false;
			aimTarget.SetActive (true);
			playerCamera.enabled = true;
			audioListener.enabled = true;
			nonLocalUI.SetActive (false);
		}

        if (!isLocalPlayer)
        {
            playerCamera.enabled = false;
            playerCamera.GetComponent<Player_Camera>().enabled = false;
        }
	}

   /* void SetPlayerColours()
    {
        controller = GetComponent<Player_Controller>();
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        Debug.Log(controller.localGamePlayer);
        if (controller.localGamePlayer.team == 0)
        {
            foreach (Renderer renderer in renderers)
            {
                if (renderer.material.color != null && renderer.material.color.b <= 0.1)
                {
                    renderer.material.color = new Color(0f, renderer.material.color.g, 1f);
                }
            }
        }
        else
        {
            foreach (Renderer renderer in renderers)
            {
                if (renderer.material.color != null && renderer.material.color.r <= 0.1)
                {
                    renderer.material.color = new Color(1f, renderer.material.color.g, 0f);
                }
            }
        }
    }*/
}
