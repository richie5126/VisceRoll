using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameNarrativeManager : MonoBehaviour {

	// Use this for initialization
	public static GameNarrativeManager Instance;

	void Awake()
	{
		if(Instance != null) Destroy(this);
		Instance = this;
	}
	[System.Serializable]
	public class Task
	{
		public float IntroDelay = 5.0f;
		public AudioClip StartTaskClip, CompletedTaskClip;
		public List<AudioClip> DuringTaskClip;
		public List<string> StartTaskText, DuringTaskText, CompletedTaskText;
		public UnityEvent OnStartTask, OnCompleteTask;
		public float AutoNextSpeed = 3.0f;

		public enum ActionType
		{
			SPAWN1,
			PLACE1ON2,
			COMBINE1WITH2TOMAKE3,
			CHOP1
		}

		public ActionType Action;
		public Transform SpawnPosition;
		public Interactable Item1, Item2, Item3;
		public Transform LockedTransform;

	}
	public List<Task> Jobs;
	[SerializeField] private UnityEngine.UI.Text _taskBox;

	private Coroutine _cr;
	private bool _textFinished;
	public void TypeTextToTextbox(string text, float timeToLive)
	{
		if(_cr != null) StopCoroutine(_cr);
		_cr = StartCoroutine(TypeString(text, timeToLive));
	}
	IEnumerator TypeString(string textToType, float timeToLive)
	{
		_textFinished = false;
		_taskBox.text = "";
		int i = 0;
		while (_taskBox.text.Length < textToType.Length)
		{
			_taskBox.text += textToType[i];
			yield return new WaitForSeconds(0.05f);
			++i;
		}

		timer = timeToLive;
		yield return new WaitWhile(() => timer > 0);
		_textFinished = true;
	}

	public void CompleteTask(Interactable item1, Interactable item2)
	{
		var t = Jobs[_conductor];
		if (t.Item1 == item1 && t.Item2 == item2)
		{
			_taskCompleted = true;
		}
	}

	private bool _taskCompleted;
	private int _conductor = 0;
	IEnumerator TaskUpdater()
	{
		while (true)
		{
			var t = Jobs[_conductor];
			yield return new WaitForSeconds(t.IntroDelay);
			for (int i = 0; i < t.StartTaskText.Count; ++i)
			{
				TypeTextToTextbox(t.StartTaskText[i], t.AutoNextSpeed);
				yield return new WaitUntil(() => _textFinished);
			}

			int val = 0;
			_taskCompleted = false;
			
			//start the task elegantly
			switch (t.Action)
			{
				case Task.ActionType.COMBINE1WITH2TOMAKE3:
					t.Item1.OnCollisionEntered += CompleteTask;
					break;
				case Task.ActionType.PLACE1ON2:
					t.Item1.OnCollisionEntered += CompleteTask;
					break;
				case Task.ActionType.CHOP1:
					break;
				case Task.ActionType.SPAWN1:
					t.Item1.OnCollisionEntered += CompleteTask;
					t.Item1.gameObject.SetActive(true);
					t.Item1.transform.position = t.SpawnPosition.position;
					if (t.Item1.GetComponent<Rigidbody>())
						t.Item1.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
					
					break;
				default:
					break;
			}
			while (!_taskCompleted)
			{
				TypeTextToTextbox(t.DuringTaskText[val], t.AutoNextSpeed);
				yield return new WaitUntil(() => _textFinished || _taskCompleted);
				val = (val + 1) % t.DuringTaskText.Count;
			}
			switch (t.Action)
			{
				case Task.ActionType.COMBINE1WITH2TOMAKE3:
					t.Item1.OnCollisionEntered -= CompleteTask;
					t.Item3.gameObject.SetActive(true);
					t.Item1.gameObject.SetActive(false);
					t.Item2.gameObject.SetActive(false);
					t.Item3.transform.position = t.Item2.transform.position;
					break;
					
				case Task.ActionType.PLACE1ON2:
					t.Item1.OnCollisionEntered -= CompleteTask;
					t.Item1.transform.parent = t.Item2.transform;
					if (t.Item1.GetComponent<Rigidbody>())
						t.Item1.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
					
					break;
				case Task.ActionType.CHOP1:
					break;
				case Task.ActionType.SPAWN1:
					t.Item1.OnCollisionEntered -= CompleteTask;
					break;
				default:
					break;
			}
			for (int i = 0; i < t.CompletedTaskText.Count; ++i)
			{
				TypeTextToTextbox(t.CompletedTaskText[i], t.AutoNextSpeed);
				yield return new WaitUntil(() => _textFinished);
			}

			_conductor = (_conductor + 1) % Jobs.Count;
		}
	}
	void Start ()
	{
		StartCoroutine(TaskUpdater());
	}
	
	// Update is called once per frame
	private float timer;
	void Update ()
	{
		if(timer > 0)
		timer -= Time.deltaTime;

		if (Input.GetKeyDown(KeyCode.Semicolon))
		{
			_taskCompleted = true;
		}
	}
}
