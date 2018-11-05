using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scener : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}	
	// Update is called once per frame
	public void NextScene()
	{

		UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
	}

	void Update () {
		
	}
}
