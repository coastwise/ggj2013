using UnityEngine;
using System.Collections;

[RequireComponent (typeof(GUITexture))]
public class FullscreenGUITexture : MonoBehaviour {

	void Start() {
		guiTexture.pixelInset = new Rect (-Screen.width/2, -Screen.height/2, Screen.width, Screen.height);
	}
}
