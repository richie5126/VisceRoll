using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameNarrativeManager : MonoBehaviour {

	// Use this for initialization
	[System.Serializable]
	public class Task
	{
		public AudioClip StartTaskClip, CompletedTaskClip;
		public List<AudioClip> DuringTaskClip;
		public string StartTaskText, CompletedText;
		public UnityEvent OnStartTask, OnCompleteTask;

	}

	public List<Task> Jobs;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
