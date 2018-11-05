using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public delegate void CollisionDetected(Interactable item1, Interactable item2, Interactable item3 = null);

	public CollisionDetected OnCollisionEntered;

	protected void OnCollisionEnter(Collision other)
	{
		if (GetComponent<AudioSource>()) GetComponent<AudioSource>().Play();
		if (other.transform.GetComponent<Interactable>())
		{
			if (OnCollisionEntered != null)
				OnCollisionEntered.Invoke(this, other.transform.GetComponent<Interactable>());
		}
	}
}
