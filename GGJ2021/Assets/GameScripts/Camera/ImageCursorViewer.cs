using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class ImageCursorViewer : MonoBehaviour
{
	public Texture2D texture;
	private bool selected = false;
	private bool active = true;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Cursor")
		{
			FindObjectOfType<UIController>().ShowExamLabel(true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Cursor")
		{
			FindObjectOfType<UIController>().ShowExamLabel(false);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "Cursor")
		{
			if (Input.GetMouseButton(0) && active)
			{
				selected = !selected;
				if (selected)
				{
					FindObjectOfType<UIController>().ShowTexture(texture);
					FindObjectOfType<FirstPersonController>().enabled = false;
					FindObjectOfType<CharacterController>().enabled = false;
				}
				else
				{
					FindObjectOfType<UIController>().HideTexture();
					FindObjectOfType<FirstPersonController>().enabled = true;
					FindObjectOfType<CharacterController>().enabled = true;
				}
				active = false;
				StartCoroutine(ReactiveButton());
			}
		}
	}

	IEnumerator ReactiveButton()
	{
		yield return new WaitForSeconds(0.5f);
		active = true;
	}
}
