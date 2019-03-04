using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.transform.root.CompareTag("Player")) {
			Destroy(GameObject.FindGameObjectWithTag("Player"));
			SceneManager.LoadScene("Main");
		}
	}
}
