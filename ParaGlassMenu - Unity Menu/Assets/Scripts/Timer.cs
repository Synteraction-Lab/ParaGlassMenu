using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
	public Text timerText;
	private float startTime;
	private int totalTime = 120;
    public bool isKettleOn;
	private float passedTime;
    public Image switchOn;
    public Image switchOFF;
    public HelloClient client;

    // Start is called before the first frame update
    void Start()
    {
    	startTime = Time.time;
    	isKettleOn = false;
    }

    // Update is called once per frame
    void Update()
    {
    	// Debug.Log(isKettleOn);
    	if (isKettleOn) {
    		UpdateTime();
            switchOn.enabled = true;
            switchOFF.enabled = false;
        } else {
            timerText.text = "OFF";
            switchOn.enabled = false;
            switchOFF.enabled = true;
        }
    }

    void UpdateTime()
    {
    	passedTime = Time.time - startTime;
    	float remainTime = totalTime - passedTime;
        if (remainTime > 0) {
            string min = ((int)remainTime / 60).ToString();
            string sec = ((int)remainTime % 60).ToString();
            timerText.text = "Remain Time: \n" + min + " : " + sec;
        }
        else {
            isKettleOn = false;
            timerText.text = "Finished";
        }
    	

    }

    public void StopTimer()
    {
        client.SendMessage("Timer,Stop");
        isKettleOn = false;
        // Debug.Log("StopTimer " + isKettleOn);
    }

    public void ResumeTimer()
    {
        isKettleOn = true;
        startTime = Time.time - passedTime;
    }
 
    public void StartTimer()
    {
        client.SendMessage("Timer,Start");
        isKettleOn = true;
        startTime = Time.time;
    }

    public void SwitchTimer()
    {
        if (isKettleOn == false)
        {
            StartTimer();
        }
        else
        {
            StopTimer();
        }
    }
}
