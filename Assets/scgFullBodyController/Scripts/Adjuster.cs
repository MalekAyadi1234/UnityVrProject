//SlapChickenGames
//2021
//Hand and Head IK system 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace scgFullBodyController
{
    public class Adjuster : MonoBehaviour
    {
        public Transform headBone;
        public Transform holdPoint;
        public Transform handBone;
        public Vector3 headOffsetRot = new Vector3(0, 0, 0);
        public Vector3 indexFingerOffsetRot = new Vector3(0, 0, 0);
        public bool adjustIndexFinger;
        public Transform indexFinger;
        public bool isAi;
        void LateUpdate()
        {
            headBone.eulerAngles = new Vector3(headBone.eulerAngles.x + headOffsetRot.x, headBone.eulerAngles.y + headOffsetRot.y, headBone.eulerAngles.z + headOffsetRot.z);

            if (!isAi)
            {
                if (!gameObject.GetComponent<GunController>().reloading && !gameObject.GetComponent<GunController>().throwing)
                {
                    handBone.transform.position = holdPoint.transform.position;
                }
            }


            if (adjustIndexFinger)
            {
                indexFinger.localEulerAngles = new Vector3(indexFinger.localEulerAngles.x + indexFingerOffsetRot.x, indexFinger.localEulerAngles.y + indexFingerOffsetRot.y, indexFinger.localEulerAngles.z + indexFingerOffsetRot.z);
            }
        }
    }
}
