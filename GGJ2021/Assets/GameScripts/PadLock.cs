using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadLock : MonoBehaviour
{
	public string keyName = "";

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == keyName)
		{
			gameObject.SetActive(false);
			GetComponentInParent<Lock>().RestPad();
			other.GetComponent<CatchObject>().cursor.gameObject.SetActive(true);
			Destroy(other.gameObject);
		}
	}
}
