using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	void Awake () 
	{
        DontDestroyOnLoad(transform.gameObject);
	}
	
	void Update () {
		
	}
}
