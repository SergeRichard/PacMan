using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BonusItem : MonoBehaviour {

	float time = 0f;
	public AudioClip audioClip;
	public Text ScoreText;

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
			ScoreText.enabled = true;
			GetComponent<SpriteRenderer> ().enabled = false;
			GetComponent<BoxCollider2D> ().enabled = false;

			Invoke ("DestroyMyself", 2f);

		}
	}
	void DestroyMyself() {

		Destroy (gameObject);
	}
}
