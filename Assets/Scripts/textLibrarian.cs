using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class textLibrarian : MonoBehaviour {
	private List<string> buffer = new List<string>();
	private Stack<string> reader = new Stack<string>();
	protected List<string> text = new List<string>();

	protected void getDialogue(string dialogue) {
		//Load text from file into buffer
		loadText ("Dialogue.txt", dialogue);
		
		int temp = reader.Count;
		
		for (int i = 0; i < temp; i++) {
			buffer.Add (reader.Pop ());
		}
		
		buffer.Reverse ();
		
		processDialogue ();
	}

	private void processDialogue() {
		bool found = false;

		foreach (string s in buffer) {
			if (!found) {
				if (s == "INIT") {
					found = true;
				}
			} else {
				if (s != "END DIALOGUE") {
					text.Add (s);
				} else {
					return;
				}
			}
		}
	}

	//Read requested text from Dialogue.txt
	private int loadText(string filename, string dialogue) {
		//Begin load block
		try {
			//Alter string to the entire line in the library
			dialogue = "DIALOGUE " + dialogue;
			string line;

			//Start filestream
			StreamReader f = new StreamReader(Application.dataPath + "/Files/" + filename);
			
			//Begin reading
			using(f) {
				//Find the corresponding dialogue set for the give name
				do {
					line = f.ReadLine();
				} while (line != dialogue);

				//Read diaglogue script text into buffer
				do {
					reader.Push(f.ReadLine());
				} while (reader.Peek() != "END DIALOGUE");
				
				//Close the file stream
				f.Close ();

				//Return OK
				return(0);
			}
		} catch (IOException e) {
			print(e);
			//Return error
			return (1);
		}
	}
}

