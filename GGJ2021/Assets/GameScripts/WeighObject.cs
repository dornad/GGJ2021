using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeighObject: MonoBehaviour {
	
	[SerializeField]
	private bool puzzleSolved = true;

	public CatchObject debugCatchComponent = null;
	public string debugResult = "<no data>";

	private void OnTriggerEnter(Collider other) {
		
		var catchComponent = other.gameObject.GetComponent<CatchObject>();

		this.debugCatchComponent = catchComponent;

		if (catchComponent != null) {
		
			var weightToDisplay = catchComponent.weightValue;
			puzzleSolved = catchComponent.isSolutionToPuzzle;
			Debug.Log("peso = " + weightToDisplay);
			Debug.Log("puzzle solved = " + puzzleSolved);
			debugResult = "success!!!";
		}
		else {
			debugResult = "<no data>, collider = " + other;
		}
	}
}
