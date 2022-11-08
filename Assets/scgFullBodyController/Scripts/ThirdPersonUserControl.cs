//SlapChickenGames
//2021
//Modified user control code also from unity standard assets

using System;
using System.Collections.Generic;
using UnityEngine;

namespace scgFullBodyController
{
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        //IMPORTANT, this script needs to be on the root transform

        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
        public float sprintSpeed;
        public float walkSpeed;
        public float crouchSpeed;
        [HideInInspector] public bool slide;
        public float slideTime;
        bool crouchToggle = false;
        bool proneToggle = false;
        bool crouch = false;
        bool prone = false;
        bool sprint = false;
        bool canVault = false;
        bool vaulting = false;
        bool strafe;
        bool forwards;
        bool backwards;
        bool right;
        bool left;
        public float vaultCancelTime;
        float horizontalInput;
        float verticalInput;
        public float sensitivity;
        public GameObject cameraController;
        GameObject collidingObj;
        private void Start()
        {
            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<ThirdPersonCharacter>();
        }

        void OnCollisionEnter(Collision col)
        {
            if (col.transform.tag == "vaultObject")
            {
                collidingObj = col.gameObject;
                canVault = true;
            }
            else
            {
                canVault = false;
            }
        }

        void OnCollisionExit(Collision col)
        {
            canVault = false;
        }

        private void Update()
        {
            //Input sensing

            verticalInput = Input.GetAxis("Vertical");
            horizontalInput = Input.GetAxis("Horizontal");

            if (!m_Jump)
            {
                m_Jump = Input.GetButtonDown("Jump");
            }

            if (m_Jump && canVault)
            {
                collidingObj.GetComponent<Collider>().enabled = false;
                m_Jump = false;
                vaulting = true;
                Invoke("vaultCancel", vaultCancelTime);
            }
            if (vaulting)
            {
                m_Jump = false;
            }
            if (Input.GetButtonDown("Crouch"))
            {
                crouchToggle = !crouchToggle;
                if (crouchToggle)
                {
                    crouch = true;
                }
                else
                {
                    crouch = false;
                }
            }

            if (Input.GetButtonDown("Crouch") && prone)
            {
                proneToggle = !proneToggle;
                crouch = true;
                prone = false;
            }

            if (Input.GetButtonDown("Prone") && crouch)
            {
                crouchToggle = !crouchToggle;
                crouch = false;
                prone = true;
            }

            if (Input.GetButtonDown("Prone"))
            {
                proneToggle = !proneToggle;
                if (proneToggle)
                {
                    prone = true;
                }
                else
                {
                    prone = false;
                }
            }

            if (Input.GetButton("Sprint") && Input.GetButtonDown("Crouch") && m_Character.m_IsGrounded)
            {
                slide = true;
                crouch = false;
                Invoke("slideCancel", slideTime);
            }

            if (Input.GetButton("Sprint"))
            {
                sprint = true;
            }
            else
            {
                sprint = false;
            }

            if (Input.GetButtonDown("Melee"))
            {
                m_Character.kick();
            }

            if (Input.GetAxis("Vertical") < 0)
            {
                backwards = true;
            }
            else if (Input.GetAxis("Vertical") > 0)
            {
                forwards = true;
            }
            else if (Input.GetAxis("Vertical") == 0)
            {
                backwards = false;
                forwards = false;
            }

            if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0)
            {
                strafe = true;
            }
            else
            {
                strafe = false;
            }

            m_Character.updateLate(m_Move, crouch, prone, vaulting, forwards, backwards, strafe, horizontalInput, verticalInput);
        }

        void vaultCancel()
        {
            vaulting = false;
            canVault = false;
            collidingObj.GetComponent<Collider>().enabled = true;
        }
        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // calculate camera relative direction to move:
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = verticalInput * m_CamForward * walkSpeed;

            if (sprint) m_Move *= sprintSpeed;

            // pass all parameters to the character control script
            m_Character.Move(m_Move, crouch, m_Jump, slide, vaulting);

            m_Character.HandleGroundMovement(crouch, m_Jump, slide);
            m_Jump = false;
        }

        void slideCancel()
        {
            slide = false;
        }

    }
}
