using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeDex : MonoBehaviour {

	[SerializeField]
	private GameObject[] creaturePics;

	private int activePic;

	void Start () {
		for (int i = 0; i < creaturePics.Length; i++)
		{
			if(i == 0)
			{
				creaturePics[i].SetActive(true);
				activePic = i;
			}
			else
				creaturePics[i].SetActive(false);
		}
	}
	
	void Update () {
		
		if(Input.GetKeyDown(KeyCode.LeftArrow) || OVRInput.GetDown(OVRInput.RawButton.X) || OVRInput.GetDown(OVRInput.RawButton.A))
		{
			if(activePic == 0)
				return;
			else
			{
				foreach(GameObject pic in creaturePics)
				{
					pic.SetActive(false);
				}
				activePic--;
				creaturePics[activePic].SetActive(true);
			}
		}
		if(Input.GetKeyDown(KeyCode.RightArrow) || OVRInput.GetDown(OVRInput.RawButton.Y) || OVRInput.GetDown(OVRInput.RawButton.B))
		{
			if(activePic == creaturePics.Length - 1)
				return;
			else
			{
				foreach(GameObject pic in creaturePics)
				{
					pic.SetActive(false);
				}
				activePic++;
				creaturePics[activePic].SetActive(true);
			}
		}

	}
}
