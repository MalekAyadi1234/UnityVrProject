//SlapChickenGames
//2021
//Camera spine controller

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace scgFullBodyController
{
    public class CameraControlledIK : MonoBehaviour
    {
        public Transform spineToOrientate;

        // Update is called once per frame
        void LateUpdate()
        {
            spineToOrientate.rotation = transform.rotation;
        }
    }
}
