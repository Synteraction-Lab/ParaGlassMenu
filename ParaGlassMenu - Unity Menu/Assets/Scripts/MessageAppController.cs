using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MenuController;


namespace Message
{
	public class MessageAppController : MonoBehaviour
	{
	    // Start is called before the first frame update
	    public static bool isNew;

		public HelloClient client;
	    void Start()
	    {
	    	isNew = false;
	    }

	    // Update is called once per frame
	    void Update()
	    {

	        
	    }

	    public static void ReceivedNewMessage()
	    {
    		isNew = true;
	    }

	    public static void SendMessage()
	    {
    		isNew = false;
	    }

	    bool IsValidMessage()
	    {
	    	return (MenuManager.GetSelectedMessage()!=null)? true : false;
	    }
	}
}
