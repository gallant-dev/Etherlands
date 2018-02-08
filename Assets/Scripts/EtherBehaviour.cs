using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;
using System.Collections;

public class EtherBehaviour : MonoBehaviour {

    public float etherDamage = 5.0f;

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Player_Stats stats = col.GetComponent<Player_Stats>();
            Player_Controller controller = col.GetComponent<Player_Controller>();

            if (stats.health > 0f)
            {
                stats.TakeDamage(etherDamage);
            }

            if (controller.etherDamageParticles.isStopped)
            {
                //controller.etherDamageParticles.Play();
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            Player_Controller controller = col.GetComponent<Player_Controller>();

            if (controller.etherDamageParticles.isPlaying)
            {
                //controller.etherDamageParticles.Stop();
            }
        }
    }

}
