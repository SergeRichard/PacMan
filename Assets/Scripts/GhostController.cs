﻿using UnityEngine;
using System.Collections;

public delegate void GhostStateHasChangedEventHandler();
public delegate void GhostLeftFrightenedStateEventHandler();

public class GhostController : Ghost {

	public event GhostStateHasChangedEventHandler GhostStateHasChanged;
	public event GhostLeftFrightenedStateEventHandler GhostLeftFrightenedState;

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

	public enum Actions
	{
		MoveRight,
		MoveLeft,
		MoveDown,
		MoveUp
	};

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
	// Use this for initialization
	protected override void Start () {
		PacManController.ChangeGhostToFrightenedState += ChangeGhostToFrightenedState;
	}

	void ChangeGhostToFrightenedState ()
	{
		//StopAllCoroutines ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void StartTimer() {
		StartCoroutine (StartStateTimer ());
	}

	IEnumerator StartStateTimer() {
		GhostState = GhostStates.Scatter;

		for (int i = 0; i < 7f; i++) {
			if (GhostState == GhostStates.Freightened) {
				yield return new WaitForSeconds (10f);
				GhostLeftFrightenedState ();
				GhostState = GhostStates.Scatter;
			}
			yield return new WaitForSeconds (1f);
		}

		GhostState = GhostStates.Chase;
		GhostStateHasChanged ();
		for (int i = 0; i < 20f; i++) {
			if (GhostState == GhostStates.Freightened) {
				yield return new WaitForSeconds (10f);
				GhostLeftFrightenedState ();
				GhostState = GhostStates.Chase;
			}
			yield return new WaitForSeconds (1f);
		}

		GhostState = GhostStates.Scatter;
		GhostStateHasChanged ();
		for (int i = 0; i < 7f; i++) {
			if (GhostState == GhostStates.Freightened) {
				yield return new WaitForSeconds (10f);
				GhostLeftFrightenedState ();
				GhostState = GhostStates.Scatter;
			}
			yield return new WaitForSeconds (1f);
		}

		GhostState = GhostStates.Chase;
		GhostStateHasChanged ();
		for (int i = 0; i < 20f; i++) {
			if (GhostState == GhostStates.Freightened) {
				yield return new WaitForSeconds (10f);
				GhostLeftFrightenedState ();
				GhostState = GhostStates.Chase;
			}
			yield return new WaitForSeconds (1f);
		}

		GhostState = GhostStates.Scatter;
		GhostStateHasChanged ();
		for (int i = 0; i < 5f; i++) {
			if (GhostState == GhostStates.Freightened) {
				yield return new WaitForSeconds (10f);
				GhostLeftFrightenedState ();
				GhostState = GhostStates.Scatter;
			}
			yield return new WaitForSeconds (1f);
		}

		GhostState = GhostStates.Chase;
		GhostStateHasChanged ();
		for (int i = 0; i < 7f; i++) {
			if (GhostState == GhostStates.Freightened) {
				yield return new WaitForSeconds (10f);
				GhostLeftFrightenedState ();
				GhostState = GhostStates.Chase;
			}
			yield return new WaitForSeconds (1f);
		}

		GhostState = GhostStates.Scatter;
		GhostStateHasChanged ();
		for (int i = 0; i < 7f; i++) {
			if (GhostState == GhostStates.Freightened) {
				yield return new WaitForSeconds (10f);
				GhostLeftFrightenedState ();
				GhostState = GhostStates.Scatter;
			}
			yield return new WaitForSeconds (1f);
		}

		// at this point, chase for the rest
		GhostState = GhostStates.Chase;
		GhostStateHasChanged ();
		while (true) {
			for (int i = 0; i < 7f; i++) {
				if (GhostState == GhostStates.Freightened) {
					yield return new WaitForSeconds (10f);
					GhostLeftFrightenedState ();
					GhostState = GhostStates.Chase;
				}
				yield return new WaitForSeconds (1f);
			}
			yield return new WaitForSeconds (1f);
		}
	}
}
