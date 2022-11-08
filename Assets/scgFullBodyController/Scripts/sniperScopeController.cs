//SlapChickenGames
//2021
//Sniper scope controller

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

namespace scgFullBodyController
{
    public class sniperScopeController : MonoBehaviour
    {
        public CameraController camControl;
        public PostProcessProfile pprocessing;
        public float sniperAimSensitivty;
        float originalCamSensitivity;
        DepthOfField dofComponent;
        public Animator blackLensAnim;
        // Start is called before the first frame update
        void Start()
        {
            originalCamSensitivity = camControl.Sensitivity;

            DepthOfField tmp;
            if (pprocessing.TryGetSettings<DepthOfField>(out tmp))
            {
                dofComponent = tmp;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (gameObject.GetComponent<GunController>().aiming)
            {
                camControl.Sensitivity = sniperAimSensitivty;
                dofComponent.active = true;
                blackLensAnim.SetBool("aiming", true);
            }
            else
            {
                camControl.Sensitivity = originalCamSensitivity;
                dofComponent.active = false;
                blackLensAnim.SetBool("aiming", false);
            }
        }
    }
}
