using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using MenuController;

namespace Music {
    public class MusicController : MonoBehaviour
    {
        public Image circularBar;
        public Text item1Text;
        public Image item1_bold;
        public Text item2Text;
        public Image item2_bold;
        public Text item3Text;
        public Image item3_bold;
        public Text item4Text;
        public Image item4_bold;
        public Text item5Text;
        public Image item5_bold;
        public Text item6Text;
        public Image item6_bold;
        public Text item7Text;
        public Image item7_bold;
        public Text item8Text;
        public Image item8_bold;
        public Text playerSongText;
        private static string currentSelectedSong;
        int angle;
        public HelloClient client;

        // Start is called before the first frame update
        void Start()
        {
            item1_bold.enabled = true;
            item2_bold.enabled = false;
            item3_bold.enabled = false;
            item4_bold.enabled = false;
            item5_bold.enabled = false;
            item6_bold.enabled = false;
            item7_bold.enabled = false;
            item8_bold.enabled = false;

            currentSelectedSong = "";
            angle = 0;
        }

        // Update is called once per frame
        void Update()
        {
            if (HasMouseMoved())
            {            
                Vector3 mousePos = Input.mousePosition;
                angle = (int)(GetAngle(mousePos.x - Screen.width/2, -mousePos.y+Screen.height/2));
                if (0 <= angle && angle < 45)
                {
                    item1_bold.enabled = true;
                    item2_bold.enabled = false;
                    item3_bold.enabled = false;
                    item4_bold.enabled = false;
                    item5_bold.enabled = false;
                    item6_bold.enabled = false;
                    item7_bold.enabled = false;
                    item8_bold.enabled = false;
                    currentSelectedSong = item1Text.text;
                }
                else if (45 <= angle && angle < 90)
                {
                    item1_bold.enabled = false;
                    item2_bold.enabled = true;
                    item3_bold.enabled = false;
                    item4_bold.enabled = false;
                    item5_bold.enabled = false;
                    item6_bold.enabled = false;
                    item7_bold.enabled = false;
                    item8_bold.enabled = false;
                    currentSelectedSong = item2Text.text;
                }
                else if (90 <= angle && angle < 135)
                {
                    item1_bold.enabled = false;
                    item2_bold.enabled = false;
                    item3_bold.enabled = true;
                    item4_bold.enabled = false;
                    item5_bold.enabled = false;
                    item6_bold.enabled = false;
                    item7_bold.enabled = false;
                    item8_bold.enabled = false;
                    currentSelectedSong = item3Text.text;
                }
                else if (135 <= angle && angle < 180)
                {
                    item1_bold.enabled = false;
                    item2_bold.enabled = false;
                    item3_bold.enabled = false;
                    item4_bold.enabled = true;
                    item5_bold.enabled = false;
                    item6_bold.enabled = false;
                    item7_bold.enabled = false;
                    item8_bold.enabled = false;
                    currentSelectedSong = item4Text.text;
                }
                else if (180 <= angle && angle < 225)
                {
                    item1_bold.enabled = false;
                    item2_bold.enabled = false;
                    item3_bold.enabled = false;
                    item4_bold.enabled = false;
                    item5_bold.enabled = true;
                    item6_bold.enabled = false;
                    item7_bold.enabled = false;
                    item8_bold.enabled = false;
                    currentSelectedSong = item5Text.text;
                }
                else if (225 <= angle && angle < 270)
                {
                    item1_bold.enabled = false;
                    item2_bold.enabled = false;
                    item3_bold.enabled = false;
                    item4_bold.enabled = false;
                    item5_bold.enabled = false;
                    item6_bold.enabled = true;
                    item7_bold.enabled = false;
                    item8_bold.enabled = false;
                    currentSelectedSong = item6Text.text;
                }
                else if (270 <= angle && angle < 315)
                {
                    item1_bold.enabled = false;
                    item2_bold.enabled = false;
                    item3_bold.enabled = false;
                    item4_bold.enabled = false;
                    item5_bold.enabled = false;
                    item6_bold.enabled = false;
                    item7_bold.enabled = true;
                    item8_bold.enabled = false;
                    currentSelectedSong = item7Text.text;
                }
                else if (315 <= angle && angle < 360)
                {
                    item1_bold.enabled = false;
                    item2_bold.enabled = false;
                    item3_bold.enabled = false;
                    item4_bold.enabled = false;
                    item5_bold.enabled = false;
                    item6_bold.enabled = false;
                    item7_bold.enabled = false;
                    item8_bold.enabled = true;
                    currentSelectedSong = item8Text.text;
                }        
            }
            else if (StateMachine.ParseCommand() == Command.Right) 
            {
                if (currentSelectedSong != ""){
                    playerSongText.text = currentSelectedSong;
                    Debug.Log("Selected: " + currentSelectedSong);
                }  
            }
            circularBar.fillAmount = (float)angle / 360;
        }

        public static string GetCurrentSelectedSong()
        {
            return currentSelectedSong;
        }

        bool HasMouseMoved()
        {
            return (Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0);
        }


        int GetAngle(float x, float y)
        {
        	double angle = 0;
    	    if (x > 0)
    	    {
    	        angle = 360 * Math.Atan((y) / (x)) / (2 * Math.PI) + 90;
    	    }
    	    else if (x < 0 && y < 0)
    	    {
    	        angle = 360 * Math.Atan((y) / (x)) / (2 * Math.PI) + 270;
    	    }
    	    else if (x < 0 && y >= 0)
    	    {
    	        angle = 360 * Math.Atan((y) / (x)) / (2 * Math.PI) + 270;
    	    }
    	    else if (x == 0 && y >= 0)
    	    {
    	        angle = 180;
    	    }
    	    else if (x == 0 && y < 0)
    	    {
    	        angle = 0;
    	    }
    	    else if (x == 0)
    	    {
    	        angle = 0;
    	    }
    	    return (int)angle;
        }
    }
}
