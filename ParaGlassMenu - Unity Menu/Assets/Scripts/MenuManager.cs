using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using NRKernal;
using Music;
using FaceTracking;


namespace MenuController {
	public class MenuManager : MonoBehaviour
	{
		private StateMachine stateMachine;
		public GameObject room;
		public GameObject livingRoom;
		public GameObject kitchen;
		public GameObject light;
		public GameObject fan;
		public GameObject music;
		public GameObject musicList;
		public GameObject tea;
		public Timer timer;
		public StateMachine.ProcessState state;
		private StateMachine.ProcessState last_state;
		public Text KeyCodeText;
		public Text playerSongText;
		public static string selectedSong;
		public HelloClient client;

	    // Start is called before the first frame update
	    void Start()
	    {
	    	stateMachine = new StateMachine();
	    	stateMachine.Process();
	    	room.SetActive(false);
	    	livingRoom.SetActive(false);
	    	kitchen.SetActive(false);
	    	light.SetActive(false);
	    	fan.SetActive(false);
	    	music.SetActive(false);
	    	musicList.SetActive(false);
	    	tea.SetActive(false);
	    	// timer.StopTimer();
	    }


	    // Update is called once per frame
	    void Update()
	    {
	    	state = stateMachine.MoveNext();

	    	// Debug.Log(state);

	    	if (Input.anyKey) {
	    		if (state == StateMachine.ProcessState.Init) 
		    	{
			    	room.SetActive(false);
			    	livingRoom.SetActive(false);
			    	kitchen.SetActive(false);
			    	light.SetActive(false);
			    	fan.SetActive(false);
			    	music.SetActive(false);
			    	musicList.SetActive(false);
			    	tea.SetActive(false);
		    	}
		    	else if (state == StateMachine.ProcessState.Room) 
		    	{
			    	room.SetActive(true);
			    	livingRoom.SetActive(false);
			    	kitchen.SetActive(false);
			    	light.SetActive(false);
			    	fan.SetActive(false);
			    	music.SetActive(false);
			    	musicList.SetActive(false);
			    	tea.SetActive(false);
		    	}
		    	else if (state == StateMachine.ProcessState.LivingRoom)
		    	{
			    	room.SetActive(false);
			    	livingRoom.SetActive(true);
			    	kitchen.SetActive(false);
			    	light.SetActive(false);
			    	fan.SetActive(false);
			    	music.SetActive(false);
			    	musicList.SetActive(false);
			    	tea.SetActive(false);
		    	}
		    	else if (state == StateMachine.ProcessState.Kitchen)
		    	{
			    	room.SetActive(false);
			    	livingRoom.SetActive(false);
			    	kitchen.SetActive(true);
			    	light.SetActive(false);
			    	fan.SetActive(false);
			    	music.SetActive(false);
			    	musicList.SetActive(false);
			    	tea.SetActive(false);
		    	} 
		    	else if (state == StateMachine.ProcessState.Light)
		    	{
			    	room.SetActive(false);
			    	livingRoom.SetActive(false);
			    	kitchen.SetActive(false);
			    	light.SetActive(true);
			    	fan.SetActive(false);
			    	music.SetActive(false);
			    	musicList.SetActive(false);
			    	tea.SetActive(false);
		    	}
		    	else if (state == StateMachine.ProcessState.Fan)
		    	{
			    	room.SetActive(false);
			    	livingRoom.SetActive(false);
			    	kitchen.SetActive(false);
			    	light.SetActive(false);
			    	fan.SetActive(true);
			    	music.SetActive(false);
			    	musicList.SetActive(false);
			    	tea.SetActive(false);
		    	}
		    	else if (state == StateMachine.ProcessState.Music)
		    	{
			    	room.SetActive(false);
			    	livingRoom.SetActive(false);
			    	kitchen.SetActive(false);
			    	light.SetActive(false);
			    	fan.SetActive(false);
			    	music.SetActive(true);
			    	musicList.SetActive(false);
			    	tea.SetActive(false);
			    	if (StateMachine.ParseCommand() == Command.Right){
			    		selectedSong = MusicController.GetCurrentSelectedSong();
			    		if (selectedSong != "" && selectedSong != null) 
				    	{
				    		playerSongText.text = selectedSong;
							client.SendMessage("Music,Start," + selectedSong);
							PlayerController.StartPlayer();
				    	}
			    	}
			    	

		    	}
		    	else if (state == StateMachine.ProcessState.MusicList)
		    	{
			    	room.SetActive(false);
			    	livingRoom.SetActive(false);
			    	kitchen.SetActive(false);
			    	light.SetActive(false);
			    	fan.SetActive(false);
			    	music.SetActive(false);
			    	musicList.SetActive(true);
			    	tea.SetActive(false);
		    	}
		    	else if (state == StateMachine.ProcessState.Tea)
		    	{
			    	room.SetActive(false);
			    	livingRoom.SetActive(false);
			    	kitchen.SetActive(false);
			    	light.SetActive(false);
			    	fan.SetActive(false);
			    	music.SetActive(false);
			    	musicList.SetActive(false);
			    	tea.SetActive(true);
			    	Command currentCommand = StateMachine.ParseCommand();
			    	if (last_state == state && currentCommand == Command.Right)
			    	{
				    	timer.SwitchTimer();
			    	}
		    	}


		    	last_state = state;    

		    	}

			if (NRKernal.NRInput.GetButtonDown(NRKernal.ControllerButton.TRIGGER))
			{
				FaceTracker.SwitchDebuggingInfoStatus();
			}

	    }

	    public static string GetSelectedSong()
	    {
	    	if (selectedSong == "" || selectedSong == null)
	    	{
	    		return null;
	    		
	    	}
	    	return selectedSong;

	    }
	}


	public enum Command
	{
		Left,
		Right,
		Up,
		Down,
		Invalid,
	}

	public class StateMachine
	{
		public enum ProcessState
		{
		    Init,
		    Room,
		    LivingRoom,
		    Kitchen,
		    Light,
		    Fan,
		    Music,
		    MusicList,
		    Tea,
		    Invalid
		}

		public static Command currentInput;


		class StateTransition
		{
		    readonly ProcessState CurrentState;
		    readonly Command Command;

		    public StateTransition(ProcessState currentState, Command command)
		    {
		        CurrentState = currentState;
		        Command = command;
		    }

		    public override int GetHashCode()
		    {
		        return 17 + 31 * CurrentState.GetHashCode() + 31 * Command.GetHashCode();
		    }

		    public override bool Equals(object obj)
		    {
		        StateTransition other = obj as StateTransition;
		        return other != null && this.CurrentState == other.CurrentState && this.Command == other.Command;
		    }
		}

		Dictionary<StateTransition, ProcessState> transitions;
		public ProcessState CurrentState { get; private set; }

		public void Process()
		{
		    CurrentState = ProcessState.Init;
		    transitions = new Dictionary<StateTransition, ProcessState>
		    {
		        { new StateTransition(ProcessState.Init, Command.Left), ProcessState.Room},
		        { new StateTransition(ProcessState.Init, Command.Right), ProcessState.Room},
		        { new StateTransition(ProcessState.Init, Command.Up), ProcessState.Room},
		        { new StateTransition(ProcessState.Init, Command.Down), ProcessState.Room},
		        { new StateTransition(ProcessState.Room, Command.Left), ProcessState.Init},
		        { new StateTransition(ProcessState.Room, Command.Right), ProcessState.LivingRoom},
		        { new StateTransition(ProcessState.Room, Command.Down), ProcessState.Kitchen},
		        { new StateTransition(ProcessState.LivingRoom, Command.Left), ProcessState.Room},
		        { new StateTransition(ProcessState.LivingRoom, Command.Right), ProcessState.Fan},
		        { new StateTransition(ProcessState.LivingRoom, Command.Up), ProcessState.Light},
		        { new StateTransition(ProcessState.LivingRoom, Command.Down), ProcessState.Music},
		        { new StateTransition(ProcessState.Light, Command.Left), ProcessState.LivingRoom},
		        { new StateTransition(ProcessState.Fan, Command.Left), ProcessState.LivingRoom},
		        { new StateTransition(ProcessState.Music, Command.Left), ProcessState.LivingRoom},
		        { new StateTransition(ProcessState.Music, Command.Right), ProcessState.MusicList},
		        { new StateTransition(ProcessState.MusicList, Command.Right), ProcessState.Music},
		        { new StateTransition(ProcessState.MusicList, Command.Left), ProcessState.Music},
		        { new StateTransition(ProcessState.Kitchen, Command.Right), ProcessState.Tea},
		        { new StateTransition(ProcessState.Tea, Command.Left), ProcessState.Kitchen},
		        { new StateTransition(ProcessState.Kitchen, Command.Left), ProcessState.Room},
		    };
		}

		public static Command ParseCommand(){

			if (Input.GetKeyDown(KeyCode.DownArrow))
	    	{
	    		return Command.Down;
	    	} 
	    	else if (Input.GetKeyDown(KeyCode.UpArrow)) 
	    	{
	    		return Command.Up;
	    	} 
	    	else if (Input.GetMouseButtonDown(0)) 
	    	{
	    		return Command.Left;
	    	}
	    	else if (Input.GetMouseButtonDown(1)) 
	    	{
	    		return Command.Right;
	    	}
	    	else 
	    	{
	    		return Command.Invalid;
	    	}

		}

		public ProcessState GetNext(Command command)
		{
		    StateTransition transition = new StateTransition(CurrentState, command);
		    ProcessState nextState;
			if (transitions.TryGetValue(transition, out nextState)) 
			{
				return nextState;
			}
			else
			{
				return ProcessState.Invalid;
			}
		    
		}

		public ProcessState MoveNext()
		{
			Command command = ParseCommand();
			ProcessState tempState;
			if (command != Command.Invalid) {
				tempState = GetNext(command);
				if (tempState != ProcessState.Invalid) {
					CurrentState = tempState;
				}
			}
			return CurrentState;

		}
	}
}