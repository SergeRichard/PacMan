using UnityEngine;
using System.Collections;

public class BonusItem : MonoBehaviour {

	float time = 0f;
	public AudioClip audioClip;

	// Use this for initialization
	void Start () {
		time = 0f;

	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.state == GameManager.States.Play) {
			time += Time.deltaTime;
			if (time > 9.75f) {
				Destroy (gameObject);
			}
		}
		if (GameManager.state == GameManager.States.PacManDead || GameManager.state == GameManager.States.WonLevel) {
			Destroy (gameObject);
		}
	}
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			AudioSource.PlayClipAtPoint (audioClip, Vector3.zero,1f);
			Destroy (gameObject);
		}
	}
}
