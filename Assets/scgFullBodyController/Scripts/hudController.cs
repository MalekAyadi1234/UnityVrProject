//SlapChickenGames
//2021
//Simple hud referencer

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace scgFullBodyController
{
    public class hudController : MonoBehaviour
    {
        //Simple references to the HUD for other scripts to access and modify
        public Text uiHealth;
        public Text uiBullets;
        
        public GameObject crosshair;
        public Text uiPassHealth;
    }
}
