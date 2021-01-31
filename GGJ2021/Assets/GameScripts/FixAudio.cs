using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixAudio : MonoBehaviour
{
	private void Start()
	{
		StartCoroutine(FixAudioS());
	}

	IEnumerator FixAudioS()
	{
		yield return new WaitForSeconds(5f);
		if (GetComponentInChildren<AudioSource>())
		{
			GetComponentInChildren<AudioSource>().spatialBlend = 0;
		}
	}
}
