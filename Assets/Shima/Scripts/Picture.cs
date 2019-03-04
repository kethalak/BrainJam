using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picture : MonoBehaviour {

	private Material mat;
	float despawnTimer = 1.5f;
	void Awake()
	{
		mat = GetComponent<Renderer>().material;
	}

	void Update () 
	{
		despawnTimer -= Time.deltaTime;
		Color tempcolor = mat.color;
		tempcolor.a = Mathf.MoveTowards(tempcolor.a, 1, Time.deltaTime);
		mat.color = tempcolor;

		if(despawnTimer <= 0)
		{
			StorePhoto();
		}

	}

	void StorePhoto()
	{
		gameObject.SetActive(false);
	}
}
