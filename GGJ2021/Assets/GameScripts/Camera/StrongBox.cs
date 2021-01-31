using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class StrongBox : MonoBehaviour
{
	public string currentKey = "";
	public int maxCharacters = 0;
	public string key = "";
	public string[] options;
	public int[] indexes;
	public TextMesh[] displays;
	private int maxLenght;
	public Animator door;
	public Transform keyModel;

	private void Awake()
	{
		for (int i = 0; i < maxCharacters; i++ )
		{
			currentKey += "A";
		}
		indexes = new int[maxCharacters];
		maxLenght = options.Length;
	}

	public void PressButton(int index, bool up)
	{
		if (!up)
		{
			if (indexes[index] + 1 == maxLenght) indexes[index] = 0;
			else indexes[index]++;
		}
		else
		{
			if (indexes[index] - 1 < 0) indexes[index] = maxLenght - 1;
			else indexes[index]--;
		}
		displays[index].text = options[indexes[index]];

		StringBuilder sb = new StringBuilder(currentKey);
		sb[index] = Convert.ToChar(options[indexes[index]]);
		currentKey = sb.ToString();
	}

	public void ResetCombination()
	{
		ResetKeys();
		indexes = new int[maxCharacters];
	}

	public void ActionBox()
	{
		if (currentKey == key)
		{
			Debug.Log("Success");
			door.Play("StrongBox");
			keyModel.gameObject.SetActive(true);
		}
		else
		{
			Debug.Log("Fail");
		}
	}

	private void ResetKeys()
	{
		currentKey = "";
		for (int i = 0; i < maxCharacters; i++)
		{
			currentKey += "A";
			displays[i].text = "A";
		}
	}
}
