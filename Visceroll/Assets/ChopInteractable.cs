using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopInteractable : DragInteractable {

	// Use this for initialization
	public Interactable Owner;
	void Start ()
	{
		if(Owner == null) Owner = transform.root.GetComponent<Interactable>();
	}
	
	// Update is called once per frame
	void Update () {
	}
	protected new void OnCollisionEnter(Collision other)
	{
		if (GetComponent<AudioSource>()) GetComponent<AudioSource>().Play();
		if (other.transform.GetComponent<Interactable>())
		{
			if (other.transform.GetComponent<Knife>())
			{
				transform.parent = null;
				if (GetComponent<Rigidbody>() != null) GetComponent<Rigidbody>().isKinematic = false;
			}
			
			GameNarrativeManager.Instance.CompleteTask(Owner, other.transform.GetComponent<Interactable>(), this);
			
		}
	}
}
