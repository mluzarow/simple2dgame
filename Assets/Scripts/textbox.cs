using UnityEngine;
using System.Collections;

public class textbox : MonoBehaviour {

	public string textboxContent;
	public GUIStyle textboxStyle;
	public Rect textboxSize;
	public float crawlTime = 0.5f;

	bool triggerCoroutine = true;
	bool triggerTextbox = false;
	bool triggerFinished = false;

	void Start() {
		textboxSize = new Rect (Screen.width * 0.200f, Screen.height * 0.800f, Screen.width * 0.600f, Screen.height * 0.400f);
	}

	void Update() {
		//Check if there is currently a coroutine running for the textbox
		//If trigger is true, ok to run coroutine
		if (triggerCoroutine) {
			//Check if player is triggering the textbox (Up and Action)
			if (playerInput.input.y == 1 && Input.GetKeyDown (KeyCode.K)) {
				//New coroutine is being run, disallow new coroutines to be run during this time
				triggerCoroutine = false;

				print ("Starting textCrawler coroutine");
				StartCoroutine(textCrawler());
			}
		//Coroutine is currently running OR coroutine has finished
		} else if (triggerFinished) {
			//We have player to press Action to remove the textbox
			if (Input.GetKeyDown (KeyCode.K)) {
				//Disable the textbox from displaying
				triggerTextbox = false;
				//Set finished to false
				triggerFinished = false;
				//Allow coroutine to run again
				triggerCoroutine = true;
			}
		}
	}

	//Text crawler coroutine to crawl through text of textboxContent
	IEnumerator textCrawler () {
		//Find the length of the texbox text string
		int stringLength = textboxContent.Length;
		//Save the text to a temporary variable
		string originalText = textboxContent;
		//Remove all text from the textbox
		textboxContent = "";

		//Allow textbox to be drawn on OnGUI()
		triggerTextbox = true;

		print ("Beginning to crawl text for the text: " + originalText);
		//Interate through the string (originalText)
		for (int i = 0; i < stringLength; i++) {
			print ("Passing " + originalText[i] + " to output " + textboxContent);
			//Copy the next letter from the original text to output textbox text
			textboxContent += originalText[i];
			
			//Yield coroutine by seconds and continue
			yield return new WaitForSeconds(crawlTime);
		}
		yield return 0;
	}

	void OnGUI() {
		if (triggerTextbox) {
			GUI.TextArea(textboxSize, textboxContent, textboxStyle);
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.blue; //Gizmo color
		float size = 0.5f; //Gizmo size
		
		//Draw gizmos for the start and end positions of the door teleports
		Gizmos.DrawLine ((Vector2)transform.position - Vector2.up * size, (Vector2)transform.position + Vector2.up * size);
		Gizmos.DrawLine ((Vector2)transform.position - Vector2.right * size, (Vector2)transform.position + Vector2.right * size);
	}
}
