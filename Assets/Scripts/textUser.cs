using UnityEngine;
using System.Collections;

public class textUser : textLibrarian {
	
	void Start () {
		getDialogue ("TEST02");

		foreach (string s in text) {
			print (s);
		}
	}
}
