//SlapChickenGames
//2021
//Kick leg sensing 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace scgFullBodyController
{
    public class kickSensing : MonoBehaviour
    {
        public float playerKickforce;
        public float doorKickforce;
        public GameObject cameraObj;
        public AudioClip kickSound;
        public int kickDamage;

        void OnTriggerEnter(Collider col)
        {
            //If we hit a player, apply damage to the player transform root object's health controller
            if (col.transform.tag == "Player" && transform.root.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Kick") && col.transform.root.GetComponent<HealthController>())
            {
                col.transform.root.GetComponent<HealthController>().DamageByKick(cameraObj.transform.forward * 360, playerKickforce, kickDamage);
                gameObject.GetComponent<AudioSource>().PlayOneShot(kickSound);
            }

            //If we hit a door, add force to its rigidbody
            if (col.transform.tag == "Door" && transform.root.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Kick"))
            {
                col.GetComponent<Rigidbody>().AddForce(cameraObj.transform.forward * 360 * doorKickforce);
                gameObject.GetComponent<AudioSource>().PlayOneShot(kickSound);
            }
        }
    }
}
