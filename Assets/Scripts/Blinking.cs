using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Blinking : MonoBehaviour {

	public float seconds = .5f;

	void Start() {
		if (GetComponent<SpriteRenderer> () != null) {
			StartCoroutine (BlinkSprite());
		}
		if (GetComponent<Text> () != null) {
			StartCoroutine (BlinkText ());
		}

	}

	IEnumerator BlinkSprite() {
		while (true) {
			GetComponent<SpriteRenderer> ().enabled = false;
			yield return new WaitForSeconds (seconds);
			GetComponent<SpriteRenderer> ().enabled = true;
			yield return new WaitForSeconds (seconds);
		}

	}
	IEnumerator BlinkText() {
		while (true) {
			GetComponent<Text> ().enabled = false;
			yield return new WaitForSeconds (seconds);
			GetComponent<Text> ().enabled = true;
			yield return new WaitForSeconds (seconds);
		}

	}
}
