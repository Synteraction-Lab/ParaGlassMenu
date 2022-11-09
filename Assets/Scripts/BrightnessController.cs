using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using MenuController;
using System.Threading.Tasks;
using System.Linq;

public class BrightnessController : MonoBehaviour
{
	public Image circularBar;
	public Text brightnessText;
	private int brightness;
    public Image switchOn;
    public Image switchOFF;
    public HelloClient client;
    private int prevBrightness;

    void Start()
    {
    	brightness = 0;
        prevBrightness = 0;
    }



    // Update is called once per frame
    void Update()
    { 

        Command currentCommand = StateMachine.ParseCommand();

        if (HasMouseMoved())
        {            
            Vector3 mousePos = Input.mousePosition;
            brightness = (int)(GetAngle(mousePos.x - Screen.width/2, -mousePos.y+Screen.height/2) * 100 / 360);
            if (Math.Abs(brightness - prevBrightness) > 5)
            {
                prevBrightness = brightness;
                client.SendMessage("Light,Brightness," + brightness);
            }
            if (brightness > 0)
            {
                switchOn.enabled = true;
                switchOFF.enabled = false;
            }
            else
            {
                switchOn.enabled = false;
                switchOFF.enabled = true;
            }
        }
        else if (currentCommand == Command.Right)
        {
            SwitchLight();
            if (brightness > 0)
            {
                switchOn.enabled = true;
                switchOFF.enabled = false;
            }
            else
            {
                switchOn.enabled = false;
                switchOFF.enabled = true;
            }
        }
        else if (currentCommand == Command.Up) {
            IncreaseBrightness();
            if (brightness > 0)
            {
                switchOn.enabled = true;
                switchOFF.enabled = false;
            }
            else
            {
                switchOn.enabled = false;
                switchOFF.enabled = true;
            }
        }
        else if (currentCommand == Command.Down) {
            DecreaseBrightness();
            if (brightness > 0)
            {
                switchOn.enabled = true;
                switchOFF.enabled = false;
            }
            else
            {
                switchOn.enabled = false;
                switchOFF.enabled = true;
            }
        }
        // else if (HasMouseMoved())
        // {            
        //     Vector3 mousePos = Input.mousePosition;
        //     brightness = (int)(GetAngle(mousePos.x - Screen.width/2, -mousePos.y+Screen.height/2) * 100 / 360);
        // }

        circularBar.fillAmount = (float)brightness/100;
        brightnessText.text = brightness + "%";
    }

    bool HasMouseMoved()
    {
        return (Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0);
    }

    void SwitchLight(){
    	if (brightness == 0)
    	{
    		brightness = 100;
            client.SendMessage("Light,On");
        } 
    	else if (brightness > 0)
    	{
            client.SendMessage("Light,Off");
            brightness = 0;
    		circularBar.fillAmount = 0;
    	}
    }

    void IncreaseBrightness(){
        if (brightness < 90)
        {
            brightness += 10;
            client.SendMessage("Light,Increase");
        } 
        else
        {
            brightness = 100;
        }
    }

    void DecreaseBrightness(){
        if (brightness >10)
        {
            brightness -= 10;
            client.SendMessage("Light,Decrease");
        } 
        else
        {
            brightness = 0;
        }
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
