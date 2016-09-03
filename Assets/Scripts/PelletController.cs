using UnityEngine;
using System.Collections;

public class PelletController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	public void ResetPellet() {
		SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer> ();
		foreach (SpriteRenderer spr in spriteRenderers) {
			spr.enabled = true;

		}

	}
}
