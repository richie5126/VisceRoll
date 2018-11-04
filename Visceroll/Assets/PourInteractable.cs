using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourInteractable : DragInteractable {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	private float timer = 0.0f;
	private float pourtime = 0.1f;
	[SerializeField] private PouredInteractable pouredAsset;
	void Update ()
	{
		timer -= Time.deltaTime;
		if(timer <= 0)
		{
			timer = pourtime;
			if (Vector3.Angle(Vector3.up, transform.up) > 90.0f)
			{
				var g = Instantiate(pouredAsset, transform.position, transform.rotation);
				g.Owner = this;
			}
		
			
		}
	}
}
