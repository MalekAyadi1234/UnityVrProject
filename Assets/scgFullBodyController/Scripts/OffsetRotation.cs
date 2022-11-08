//SlapChickenGames
//2021
//Spine orientation control

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace scgFullBodyController
{
    public class OffsetRotation : MonoBehaviour
    {
        public float offsetRotationRifle;
        public float offsetRotationPistol;
        public bool rifle;
        public bool pistol;
        void LateUpdate()
        {
            if (rifle)
                transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y + offsetRotationRifle, 0);

            if (pistol)
                transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y + offsetRotationPistol, 0);
        }
    }
}
