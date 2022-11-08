//SlapChickenGames
//2021
//Scene controller

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace scgFullBodyController
{
    public class sceneController : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
