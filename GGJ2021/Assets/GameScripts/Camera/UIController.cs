using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	public Transform examLabel;
	public RawImage textureToShow;

	public void ShowExamLabel(bool show)
	{
		examLabel.gameObject.SetActive(show);
	}

	public void ShowTexture(Texture2D toShow)
	{
		textureToShow.texture = toShow;
		textureToShow.SetNativeSize();
		textureToShow.gameObject.SetActive(true);
	}

	public void HideTexture()
	{
		textureToShow.gameObject.SetActive(false);
	}
}
