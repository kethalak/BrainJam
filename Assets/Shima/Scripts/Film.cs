using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Film : MonoBehaviour {

	TextMeshPro tmp;
	HandheldCamera handCam;
	void Awake () {
		tmp = GetComponent<TextMeshPro>();
		handCam = GetComponentInParent<HandheldCamera>();
	}
	
	void Update () {
		tmp.text = handCam.FilmAmount.ToString();
	}
}
