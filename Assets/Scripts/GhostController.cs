using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void GhostStateHasChangedEventHandler();
public delegate void GhostLeftFrightenedStateEventHandler();
public delegate void FrightenedBlinkingEventHandler();

public class GhostController : MonoBehaviour {

	public float GhostFrightenedTimeStep = 0.05f;

	public PacManController PacManController;

	public Transform GhostStartLocation;

	public GameObject GhostYellow;
	public GameObject GhostPink;
	public GameObject GhostBlue;
	public GameObject GhostRed;

	public Transform GhostYellowStart;
	public Transform GhostPinkStart;
	public Transform GhostBlueStart;
	public Transform GhostRedStart;



	// Use this for initialization
	void Start () {


	}

	// Update is called once per frame
	void Update () {

	}
	public void DisableGhost() {
		GhostYellow.GetComponent<YellowGhost> ().CancelInvoke ();
		GhostYellow.GetComponent<YellowGhost> ().StopAllCoroutines ();
		GhostPink.GetComponent<PinkGhost> ().CancelInvoke ();
		GhostPink.GetComponent<PinkGhost> ().StopAllCoroutines ();
		GhostBlue.GetComponent<BlueGhost> ().CancelInvoke ();
		GhostBlue.GetComponent<BlueGhost> ().StopAllCoroutines ();
		GhostRed.GetComponent<RedGhost> ().CancelInvoke ();
		GhostRed.GetComponent<RedGhost> ().StopAllCoroutines ();

		GhostYellow.GetComponent<Animator> ().enabled = false;
		GhostPink.GetComponent<Animator> ().enabled = false;
		GhostBlue.GetComponent<Animator> ().enabled = false;
		GhostRed.GetComponent<Animator> ().enabled = false;

		GhostYellow.GetComponent<SpriteRenderer> ().enabled = false;
		GhostPink.GetComponent<SpriteRenderer> ().enabled = false;
		GhostBlue.GetComponent<SpriteRenderer> ().enabled = false;
		GhostRed.GetComponent<SpriteRenderer> ().enabled = false;

	}
	public void EnableGhost() {
		GhostYellow.GetComponent<SpriteRenderer> ().enabled = true;
		GhostPink.GetComponent<SpriteRenderer> ().enabled = true;
		GhostBlue.GetComponent<SpriteRenderer> ().enabled = true;
		GhostRed.GetComponent<SpriteRenderer> ().enabled = true;

	}
}
