using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//For reading/writing file
using System.IO;

public class menuRebind : MonoBehaviour {
	Rect rebindLeftSize;
	Rect rebindRightSize;
	Rect rebindUpSize;
	Rect rebindDownSize;
	Rect rebindJumpSize;

	Rect labelLeftSize;
	Rect labelRightSize;
	Rect labelUpSize;
	Rect labelDownSize;
	Rect labelJumpSize;

	Rect rebindingLabelSize;

	bool lookingForKey = false;
	string keyToChange = "";

	//Dictionary holding all key bindings
	public Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

	void Start() {
		//Set up GUI
		drawRects ();

		keys.Add ("LEFT", KeyCode.A);
		keys.Add ("RIGHT", KeyCode.D);
		keys.Add ("UP", KeyCode.W);
		keys.Add ("DOWN", KeyCode.S);
		keys.Add ("JUMP", KeyCode.J);
		/*
		foreach (KeyValuePair<string, KeyCode> entry in keys) {
			print (entry.Key + " is present in the dictionary.");
		}*/

		//Load from player prefs
		loadPrefs ("playerPrefs.ini");
	}

	void OnGUI() {
		GUI.Label (labelLeftSize, "LEFT");
		GUI.Label (labelRightSize, "RIGHT");
		GUI.Label (labelUpSize, "UP");
		GUI.Label (labelDownSize, "DOWN");
		GUI.Label (labelJumpSize, "JUMP");

		//Rebind using the key that the user presses
		if (lookingForKey) {
			GUI.Label (rebindingLabelSize, "PRESS A BUTTON TO USE FOR INPUT: " + keyToChange);

			Event e = Event.current;

			if (e.isKey) {
				keys[keyToChange] = e.keyCode;
				lookingForKey = false;

				List<string> keysToNull = new List<string>();

				//Check to see if the keyCode is used anywhere else
				foreach (KeyValuePair<string, KeyCode> entry in keys) {
					if (entry.Value == e.keyCode && entry.Key != keyToChange) {
						keysToNull.Add (entry.Key);
					}
				}

				//None each matching key's value
				foreach (string entry in keysToNull) {
					keys[entry] = KeyCode.None;
				}

				//Save key bind change to file
				savePrefs("playerPrefs.ini");
			}
		//Show menu of current binds
		} else {
			if (GUI.Button (rebindLeftSize, keys["LEFT"].ToString())) {
				lookingForKey = true;
				keyToChange = "LEFT";
			}
			if (GUI.Button (rebindRightSize, keys["RIGHT"].ToString())) {
				lookingForKey = true;
				keyToChange = "RIGHT";
			}
			if (GUI.Button (rebindUpSize, keys["UP"].ToString())) {
				lookingForKey = true;
				keyToChange = "UP";
			}
			if (GUI.Button (rebindDownSize, keys["DOWN"].ToString())) {
				lookingForKey = true;
				keyToChange = "DOWN";
			}
			if (GUI.Button (rebindJumpSize, keys["JUMP"].ToString())) {
				lookingForKey = true;
				keyToChange = "JUMP";
			}
		}
	}

	//Write key binds to playerPrefs.ini
	bool savePrefs(string filename) {
		try {
			StreamWriter f = new StreamWriter(Application.dataPath + "/Files/" + filename);

			using (f) {
				foreach (KeyValuePair<string, KeyCode> entry in keys) {
					f.WriteLine (entry.Key + ":" + entry.Value.ToString());
				}
			}
		} catch (IOException e) {
			print (e);
			print (true);
		}
		return(false);
	}

	//Read key binds from playerPrefs.ini
	bool loadPrefs(string filename) {
		//Begin load block
		try {
			//Data will hold single key value pair "string:keycode"
			string data;
			//Key:Value pair split into [0] key [1] value
			string[] currentKey = new string[2];

			//Start filestream
			StreamReader f = new StreamReader(Application.dataPath + "/Files/" + filename);

			//Begin reading
			using(f) {
				//Check for a line; is a line, read it and check for another
				do {
					//Read a line
					data = f.ReadLine();
					print ("Read line: " + data);

					//If line had data
					if (data != null) {
						//Split data line into key string and value keycode
						currentKey = data.Split(':');
						print ("Attenmpting to load " + currentKey[0] + " : " + currentKey[1] + " into dictionary.");

						//Parse letter as KeyCode and put in dictionary
						keys [currentKey [0]] = (KeyCode) System.Enum.Parse(typeof(KeyCode), currentKey [1]);
						print ("Entered into dictionary");
					}
				} while (data != null);

				//Close the file stream
				f.Close ();
				//Return 0
				return(false);
			}
		} catch (IOException e) {
			print(e);
			return (true);
		}
	}

	//Redraw all Rects on resolution change
	void drawRects() {
		//left top width height
		rebindLeftSize =  new Rect (Screen.width * 0.438f, Screen.height * 0.242f, Screen.width * 0.200f, Screen.height * 0.052f);
		rebindRightSize = new Rect (Screen.width * 0.438f, Screen.height * 0.300f, Screen.width * 0.200f, Screen.height * 0.052f);
		rebindUpSize =    new Rect (Screen.width * 0.438f, Screen.height * 0.358f, Screen.width * 0.200f, Screen.height * 0.052f);
		rebindDownSize =  new Rect (Screen.width * 0.438f, Screen.height * 0.416f, Screen.width * 0.200f, Screen.height * 0.052f);
		rebindJumpSize =  new Rect (Screen.width * 0.438f, Screen.height * 0.472f, Screen.width * 0.200f, Screen.height * 0.052f);

		labelLeftSize =  new Rect (Screen.width * 0.338f, Screen.height * 0.242f, Screen.width * 0.200f, Screen.height * 0.052f);
		labelRightSize = new Rect (Screen.width * 0.338f, Screen.height * 0.300f, Screen.width * 0.200f, Screen.height * 0.052f);
		labelUpSize =    new Rect (Screen.width * 0.338f, Screen.height * 0.358f, Screen.width * 0.200f, Screen.height * 0.052f);
		labelDownSize =  new Rect (Screen.width * 0.338f, Screen.height * 0.416f, Screen.width * 0.200f, Screen.height * 0.052f);
		labelJumpSize =  new Rect (Screen.width * 0.338f, Screen.height * 0.472f, Screen.width * 0.200f, Screen.height * 0.052f);
	
		rebindingLabelSize = new Rect (Screen.width * 0.500f, Screen.height * 0.500f, Screen.width * 0.500f, Screen.height * 0.500f);
	}
}
