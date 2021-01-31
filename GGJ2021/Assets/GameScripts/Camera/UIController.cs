using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	public Transform examLabel;
	public Transform pressLabel;
	public RawImage textureToShow;
	public Transform escapedLabel;

	public void ShowExamLabel(bool show)
	{
		examLabel.gameObject.SetActive(show);
	}

	public void ShowPressLabel(bool show)
	{
		pressLabel.gameObject.SetActive(show);
	}

	public void ShowTexture(Texture2D toShow)
	{
		textureToShow.texture = toShow;
		textureToShow.SetNativeSize();
		textureToShow.gameObject.SetActive(true);
	}

	public void ShowScapedLabel()
	{
		escapedLabel.gameObject.SetActive(true);
	}

	public void HideTexture()
	{
		textureToShow.gameObject.SetActive(false);
	}
}
