using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongBoxKey : MonoBehaviour
{
	public int index = 0;
	public bool up = false;
	public bool resetKey = false;
	public bool actionKey = false;
	public bool active = true;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Cursor")
		{
			FindObjectOfType<UIController>().ShowPressLabel(true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Cursor")
		{
			FindObjectOfType<UIController>().ShowPressLabel(false);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "Cursor")
		{
			if (Input.GetMouseButton(0) && active)
			{
				active = false;
				StartCoroutine(ReactiveButton());
				PressKey();
			}
		}
	}

	public void PressKey()
	{
		if (!resetKey && !actionKey) GetComponentInParent<StrongBox>().PressButton(index, up);
		else
		{
			if(resetKey) GetComponentInParent<StrongBox>().ResetCombination();
			if(actionKey) GetComponentInParent<StrongBox>().ActionBox();
		}
	}

	IEnumerator ReactiveButton()
	{
		yield return new WaitForSeconds(0.3f);
		active = true;
	}
}
