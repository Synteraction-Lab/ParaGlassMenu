using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MenuController;


namespace Music
{
	public class PlayerController : MonoBehaviour
	{
	    // Start is called before the first frame update
	    public static bool isPlaying;
	    public Image continueImage;
	    public Image pauseImage;
		public HelloClient client;
	    void Start()
	    {
	    	isPlaying = false;
	    	continueImage.enabled = true;
	    	pauseImage.enabled = false; 
	    }

	    // Update is called once per frame
	    void Update()
	    {
	    	if ((StateMachine.ParseCommand() == Command.Up) && isValidSong())
	    	{
	    		isPlaying = !isPlaying;
				if (isPlaying)
                {
					client.SendMessage("Music,Play");
				}
				else
                {
					client.SendMessage("Music,Stop");
				}
		    	continueImage.enabled = !continueImage.enabled;
		    	pauseImage.enabled = !pauseImage.enabled; 
	    	} 
	    	else if (isPlaying)
	    	{
	    		continueImage.enabled = false;
	    		pauseImage.enabled = true; 
	    	} 
	    	else if (!isPlaying)
	    	{
	    		continueImage.enabled = true;
	    		pauseImage.enabled = false; 

	    	}
	        
	    }

	    public static void StartPlayer()
	    {
    		isPlaying = true;
	    }

	    public static void PausePlayer()
	    {
    		isPlaying = false;
	    }

	    bool isValidSong()
	    {
	    	return (MenuManager.GetSelectedSong()!=null)? true : false;
	    }
	}
}
