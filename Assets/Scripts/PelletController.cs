using UnityEngine;
using System.Collections;

public class PelletController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	public void ResetPellet() {		
		gameObject.SetActiveRecursively (true);
		SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer> ();
		foreach (SpriteRenderer sr in srs) {
			sr.enabled = true;

		}
	}
}
