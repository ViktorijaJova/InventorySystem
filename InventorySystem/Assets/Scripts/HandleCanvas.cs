
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HandleCanvas : MonoBehaviour {

	private CanvasScaler scaler;

	
	void Start () {
		scaler = GetComponent<CanvasScaler> ();
		scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
	}

}
