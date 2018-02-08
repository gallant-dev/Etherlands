using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class GideonTertiaryProjectile : NetworkBehaviour {

    public Player_Controller controller;

    [SyncVar]
    public GameObject parent;

    ParticleSystem[] particleSystems;

    bool alreadyTriggered = false;

    void Start()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
    }

    void OnTriggerEnter()
    {
        if (alreadyTriggered == false)
        {
            //Play primary particle system.
            GetComponent<ParticleSystem>().Play();

            //Play particle systems on children. 
            foreach (ParticleSystem ps in particleSystems)
            {
                ps.Play();
            }

            ApplyDamageAndEffects();

            Destroy(gameObject, 0.6f);

            alreadyTriggered = true;
        }
    }

    void ApplyDamageAndEffects()
    {
        if (isServer)
        {
            controller.ApplyDamage(transform.position, 2, true);
        }

        if(isServer && controller.playerStats.syncThirdDamageTalentLevel >= 1)
        {
            Collider[] stunTargets = Physics.OverlapSphere(transform.position, controller.charStats.GetStat("tertiaryAttackOutterRadiusBase") * controller.charStats.GetStat("tertiaryAttackOutterRadiusMultiplier"));
            foreach (Collider hit in stunTargets)
            {
                if (hit.gameObject.tag == "Player" && hit.GetComponent<Player_Controller>().localGamePlayer.team != controller.localGamePlayer.team)
                {
                    controller.ApplyEffect(hit.gameObject, "stn", 0f, controller.charStats.GetStat("tertiaryAttackStunDurationBase") * controller.charStats.GetStat("tertiaryAttackStunDurationMultiplier"));
                }
            }
        }
    }
}
