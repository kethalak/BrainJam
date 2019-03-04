using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreScreen : MonoBehaviour {

	GameObject player;
	public GameObject[] scorePictures;
	List<GameObject> takenPictures = new List<GameObject>();
	public TextMeshPro pointSumText;
	public TextMeshPro picturesTakenText;

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		player.transform.position = Vector3.zero;
		takenPictures = player.transform.GetComponentInChildren<HandheldCamera>().takenPictures;

		for (int i = 0; i < takenPictures.Count; i++)
		{
			scorePictures[i].SetActive(true);
			scorePictures[i].GetComponent<Renderer>().material = takenPictures[i].GetComponent<Renderer>().material;
			///calculate score
		}

		pointSumText.text = player.transform.GetComponentInChildren<HandheldCamera>().pointSum.ToString();
		picturesTakenText.text = takenPictures.Count.ToString();
	}
	
	void Update () {
		
	}
}
