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

	public void CompleteTask()
	{
		_taskCompleted = true;
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
			while (!_taskCompleted)
			{
				TypeTextToTextbox(t.DuringTaskText[val], t.AutoNextSpeed);
				yield return new WaitUntil(() => _textFinished || _taskCompleted);
				val = (val + 1) % t.DuringTaskText.Count;
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

		if (Input.GetMouseButtonDown(0))
		{
			CompleteTask();
		}
	}
}
