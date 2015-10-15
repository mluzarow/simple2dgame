using UnityEngine;
using System.Collections;

public class menuRebind : MonoBehaviour {
	Rect rebindLeftSize;
	Rect rebindRightSize;
	Rect rebindUpSize;
	Rect rebindDownSize;
	Rect rebindJumpSize;

	void Start() {
		drawRects ();
		//Load from player prefs
	}

	void OnGUI() {
		if (GUI.Button (rebindLeftSize, "LEFT")) {

		}
		if (GUI.Button (rebindRightSize, "RIGHT")) {

		}
		if (GUI.Button (rebindUpSize, "UP")) {

		}
		if (GUI.Button (rebindUpSize, "DOWN")) {
			
		}
		if (GUI.Button (rebindUpSize, "JUMP")) {
			
		}
	}

	//Redraw all Rects on resolution change
	void drawRects() {
		rebindLeftSize = new Rect (Screen.width * 0.738f, Screen.height * 0.242f, 
		                           Screen.width * 0.200f, Screen.height * 0.052f);
		rebindRightSize = new Rect (Screen.width * 0.738f, Screen.height * 0.3f, 
		                             Screen.width * 0.200f, Screen.height * 0.052f);
		rebindUpSize = new Rect (Screen.width * 0.738f, Screen.height * 0.358f, 
		                           Screen.width * 0.200f, Screen.height * 0.052f);
		
		rebindDownSize = new Rect (Screen.width * 0.738f, Screen.height * 0.416f, 
		                              Screen.width * 0.200f, Screen.height * 0.052f);
		rebindJumpSize = new Rect (Screen.width * 0.738f, Screen.height * 0.528f, 
		                                   Screen.width * 0.200f, Screen.height * 0.052f);
	}
}
