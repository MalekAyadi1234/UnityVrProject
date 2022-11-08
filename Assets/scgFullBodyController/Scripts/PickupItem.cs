using scgFullBodyController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PickupItem : MonoBehaviour
{
    [HideInInspector] HealthController healthh;
    public Transform PlayerTransform;
    public GameObject Gun;
    public GameObject Player;
    public float range = 20f;
    public float open = 100f;
    public float max;

    // Start is called before the first frame update
    void Start()
    {
        Gun.GetComponent<Rigidbody>().isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("m"))
        {
            // UnequipObject();
            //Shoot()
            Pick();
           // healthh.health += 0.05f;
        }
    }

    void Pick()
    {
        RaycastHit hit;
        if (Physics.Raycast(Player.transform.position, Player.transform.forward, out hit, range))
        {
            //Debug.Log(hit.transform.name);
              Destroy(Gun);
           // healthh.health = 10f;
          
        }
    }

   
}
