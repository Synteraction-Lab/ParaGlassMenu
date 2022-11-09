using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using MenuController;

public class FanSpeedController : MonoBehaviour
{
	public Image circularBar;
	public Text fanSpeedText;
	private int fanSpeed;
    public Image switchOn;
    public Image switchOFF;
    public HelloClient client;
    private int prevFanSpeed;
    // Start is called before the first frame update
    void Start()
    {
    	fanSpeed = -1;
        prevFanSpeed = -1;
    }

    // Update is called once per frame
    void Update()
    {
        Command currentCommand = StateMachine.ParseCommand();
        if (HasMouseMoved())
        {            
            Vector3 mousePos = Input.mousePosition;
            fanSpeed = (int)(GetAngle(mousePos.x - Screen.width/2, -mousePos.y+Screen.height/2) * 100 / 360);
            if (Math.Abs(fanSpeed - prevFanSpeed) > 8)
            {
                prevFanSpeed = fanSpeed;
                client.SendMessage("Fan,Speed," + fanSpeed);
            }
            if (fanSpeed > 0)
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
            SwitchFan();
            if (fanSpeed > 0)
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
            IncreaseFanSpeed();
            if (fanSpeed > 0)
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
            DecreaseFanSpeed();
            if (fanSpeed > 0)
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
        //     fanSpeed = (int)(GetAngle(mousePos.x - Screen.width/2, -mousePos.y+Screen.height/2) * 100 / 360);
        // }

        circularBar.fillAmount = (float)fanSpeed/100;
        if (fanSpeed == -1)
        {
            fanSpeedText.text = "OFF";
        }
        else
        {
            fanSpeedText.text = fanSpeed + "%";
        }
        
    }

    bool HasMouseMoved()
    {
        return (Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0);
    }

    void SwitchFan(){
    	if (fanSpeed <= 0)
    	{
            client.SendMessage("Fan,On");
    		fanSpeed = 50;
    	} 
    	else if (fanSpeed > 0)
    	{
            client.SendMessage("Fan,Off");
            fanSpeed = -1;
    		circularBar.fillAmount = 0;
    	}
    }

    void IncreaseFanSpeed(){
        if (fanSpeed < 90)
        {
            client.SendMessage("Fan,Increase");
            fanSpeed += 10;
        } 
        else
        {
            fanSpeed = 100;
        }
    }

    void DecreaseFanSpeed(){
        if (fanSpeed >10)
        {
            client.SendMessage("Fan,Decrease");
            fanSpeed -= 10;
        } 
        else
        {
            fanSpeed = 0;
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
