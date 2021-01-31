using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
	public int padlocks = 0;

	public void RestPad()
	{
		padlocks--;
		Debug.Log("Pads: " + padlocks);
		if (padlocks == 0) FindObjectOfType<UIController>().ShowScapedLabel();
	}
}
