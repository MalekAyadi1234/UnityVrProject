//SlapChickenGames
//2021
//Health controller

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace scgFullBodyController
{
    public class HealthController : MonoBehaviour
    {
        //IMPORTANT, this script needs to be on the root transform

        [Header("Basics")]


        public float health;
        public GameObject dwe;
        float maxHealth;
        public GameObject ragdoll;

        public bool dontSpawnRagdoll;
        public float deadTime;
        GameObject tempdoll;
        bool meleeDeath;
        public bool isAiOrDummy;

        [Header("Sound")]
        public bool playNoiseOnHurt;
        public float percentageToPlay;
        public AudioClip hurtNoise;

        [Header("Regen")]
        public bool regen;
        public float timeBeforeRegen;
        float origTimeBeforeRegen;
        public float regenSpeed;
        bool alreadyRegenning;

        void Start()
        {
            //Get a reference to the original reset time
            origTimeBeforeRegen = timeBeforeRegen;

            //Set maxHealth to what our max is at start of the scene
            maxHealth = health;
        }

        void Update()
        {
            //If health is low enough and we are not kicked to death, then die normally
            if (health <= 0)
            {
                if (!meleeDeath)
                    Die();
            }

            //Only update HUD text if we are a player
            if (!isAiOrDummy)
            {
                if (health > 0)
                {
                    GameObject ui = GameObject.FindGameObjectWithTag("hud");
                    ui.GetComponent<hudController>().uiHealth.text = health.ToString();
                }
                else
                {
                    GameObject ui = GameObject.FindGameObjectWithTag("hud");
                    ui.GetComponent<hudController>().uiHealth.text = "0";
                }
            }


            //Check if we are done regenning and stop
            if (health == maxHealth && regen && alreadyRegenning)
            {
                alreadyRegenning = false;
                StopCoroutine("regenHealth");
            }

        }

        public void Damage(float damage)
        {
            //If we are a player, take damage, otherwise (AI), apply the hit animation and attack the player
            if (!isAiOrDummy)
            {
                health -= damage;

                if (playNoiseOnHurt)
                {
                    if (Random.value < percentageToPlay)
                    {
                        GetComponent<AudioSource>().PlayOneShot(hurtNoise);
                    }
                }
            }
            else
            {
                health -= damage;
                GetComponent<Animator>().SetTrigger("hit");

                if (gameObject.GetComponent<AiController>())
                    gameObject.GetComponent<AiController>().overrideAttack = true;

                if (playNoiseOnHurt)
                {
                    if (Random.value < percentageToPlay)
                    {
                        GetComponent<AudioSource>().PlayOneShot(hurtNoise);
                    }
                }

            }
            //GameObject Cube = GameObject.Find("CanGrab");

            //If we are allowed to regen, start gaining health
            //   if (regen && !GameObject.FindWithTag("CanGrab"))
               if (regen )

            {

                GameObject ui = GameObject.FindGameObjectWithTag("hud");
                ui.GetComponent<hudController>().uiHealth.text = "100";

                timeBeforeRegen = origTimeBeforeRegen;
                StopCoroutine("regenHealth");
                CancelInvoke();
                if (timeBeforeRegen == origTimeBeforeRegen)
                {
                    alreadyRegenning = true;
                    Invoke(nameof(regenEnumeratorStart), timeBeforeRegen);
                }
            }
        }

        void regenEnumeratorStart()
        {
            StartCoroutine("regenHealth");
        }

        IEnumerator regenHealth()
        {
            //Only regen while under max health and gain 1 health every regenSpeed seconds
            while (health < maxHealth)
            {
                health++;
                yield return new WaitForSeconds(regenSpeed);
            }
        }

        void Die()
        {
            //Only spawn ragdoll if option is selected
            if (!dontSpawnRagdoll)
            {
                //Spawn ragdoll and destroy us
                tempdoll = Instantiate(ragdoll, this.transform.position, this.transform.rotation) as GameObject;

                //Tell the ragdoll if we are a player or not so it knows to move our camera or not to the ragdoll
                tempdoll.GetComponent<ragdollCamera>().isAi = isAiOrDummy;
                Destroy(gameObject);

                //Destroy ragdoll if we are an AI after deadTime seconds
                if (isAiOrDummy)
                    Destroy(tempdoll, deadTime);
            }
            else if (isAiOrDummy)
            {
                //If we aren't spawning a ragdoll, then disable all important scripts on us and destroy after deadtime seconds
                //This feature is for AI with the ragdoll built in for a more realistic death
                if (gameObject.GetComponent<Animator>())
                    gameObject.GetComponent<Animator>().enabled = false;

                if (gameObject.GetComponent<AiController>())
                    gameObject.GetComponent<AiController>().enabled = false;

                if (gameObject.GetComponent<HealthController>())
                    gameObject.GetComponent<HealthController>().enabled = false;

                if (gameObject.GetComponent<SimpleFootsteps>())
                    gameObject.GetComponent<SimpleFootsteps>().enabled = false;

                if (gameObject.GetComponent<NavMeshAgent>())
                    gameObject.GetComponent<NavMeshAgent>().enabled = false;

                if (gameObject.GetComponentInChildren<OffsetRotation>())
                    gameObject.GetComponentInChildren<OffsetRotation>().enabled = false;

                if (gameObject.GetComponentInChildren<AiGunController>())
                    gameObject.GetComponentInChildren<AiGunController>().enabled = false;

                if (gameObject.GetComponentInChildren<Adjuster>())
                    gameObject.GetComponentInChildren<Adjuster>().enabled = false;

                Destroy(gameObject, deadTime);
            }
        }

        public void DamageByKick(Vector3 pos, float kickForce, int kickDamage)
        {
            //Subtract the damage from values passed in by kickSensing
            health -= kickDamage;

            //If kicked enough, then die
            if (health <= 0)
            {
                meleeDeath = true;
                tempdoll = Instantiate(ragdoll, this.transform.position, this.transform.rotation) as GameObject;
                tempdoll.GetComponent<ragdollCamera>().isAi = isAiOrDummy;
                Destroy(gameObject);

                foreach (Rigidbody rb in tempdoll.GetComponentsInChildren<Rigidbody>())
                {
                    rb.AddForce(pos * kickForce);
                }
            }
            else
            {
                //Dont die just play hit anim
                gameObject.GetComponent<Animator>().SetTrigger("hit");
            }
        }
    }
}