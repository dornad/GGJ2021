using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchObject : MonoBehaviour
{
	public string weightValue = "12.00";

	public bool isSolutionToPuzzle = false;
	private Transform takeObject;
	private bool catched = false;
	public Transform cursor;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Cursor") FindObjectOfType<UIController>().ShowExamLabel(true);
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Cursor") FindObjectOfType<UIController>().ShowExamLabel(false);
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "Cursor")
		{
			if (Input.GetMouseButton(0) && !catched)
			{
				takeObject = GameObject.FindGameObjectWithTag("TakeObject").transform;
				GetComponent<Rigidbody>().isKinematic = true;
				cursor = other.transform;
				cursor.gameObject.SetActive(false);
				FindObjectOfType<UIController>().ShowExamLabel(false);
				catched = true;
			}
		}
	}

	private void LateUpdate()
	{
		if (catched && takeObject)
		{
			transform.position = takeObject.position;

			if (Input.GetMouseButtonUp(0))
			{
				takeObject = null;
				GetComponent<Rigidbody>().isKinematic = false;

				StartCoroutine(ReactiveCursor());
			}
		}
	}

	IEnumerator ReactiveCursor()
	{
		yield return new WaitForSeconds(0.5f);
		cursor.gameObject.SetActive(true);
		catched = false;
	}
}
