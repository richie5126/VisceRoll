using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements;
using UnityEngine;

public class PouredInteractable : DragInteractable {

	// Use this for initialization
	public PourInteractable Owner;
	void Start () {
	}
	
	void Update ()
	{
	}

	private new void OnCollisionEnter(Collision other)
	{
		if(Owner != null) GameNarrativeManager.Instance.CompleteTask(Owner, other.transform.GetComponent<Interactable>());
		else if(!other.transform.GetComponent<Interactable>()) Destroy(gameObject);
	}
}
