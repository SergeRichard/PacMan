using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void GhostStateHasChangedEventHandler();
public delegate void GhostLeftFrightenedStateEventHandler();
public delegate void FrightenedBlinkingEventHandler();

public class GhostController : Ghost {

	class GhostMode
	{
		public GhostStates state;
		public float time;
	}

	Queue<GhostMode> ghostModes;

	public event GhostStateHasChangedEventHandler GhostStateHasChanged;
	public event GhostLeftFrightenedStateEventHandler GhostLeftFrightenedState;
	public event FrightenedBlinkingEventHandler FrightenedBlinking;

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

	private float modeTimer;
	private float frightenedTimer;

	// Use this for initialization
	protected override void Start () {
		PacManController.ChangeGhostToFrightenedState += ChangeGhostToFrightenedState;

		GhostState = GhostStates.Scatter;

		modeTimer = 0;
		frightenedTimer = 0;

		ghostModes = new Queue<GhostMode> ();

		GhostMode mode = new GhostMode ();
		mode.state = GhostStates.Scatter;
		mode.time = 7f; // start right away
		ghostModes.Enqueue (mode);

		GhostMode mode2 = new GhostMode ();
		mode2.state = GhostStates.Chase;
		mode2.time = 20f;
		ghostModes.Enqueue (mode2);

		GhostMode mode3 = new GhostMode ();
		mode3.state = GhostStates.Scatter;
		mode3.time = 7f;
		ghostModes.Enqueue (mode3);

		GhostMode mode4 = new GhostMode ();
		mode4.state = GhostStates.Chase;
		mode4.time = 20f;
		ghostModes.Enqueue (mode4);

		GhostMode mode5 = new GhostMode ();
		mode5.state = GhostStates.Scatter;
		mode5.time = 5f;
		ghostModes.Enqueue (mode5);

	}
	// Update is called once per frame
	void Update () {
		// TODO clear and re-add ghost modes. 

		if (GameManager.state == GameManager.States.Play && GhostState != GhostStates.Freightened) {
			modeTimer += Time.deltaTime;
			if (ghostModes.Count > 0) {
				if (modeTimer >= ghostModes.Peek ().time) {					
					ghostModes.Dequeue ();
					GhostStateHasChanged ();
					if (ghostModes.Count > 0) {
						GhostState = ghostModes.Peek ().state;
					} else {
						GhostState = GhostStates.Chase;
					}
					modeTimer = 0;
				}
			}
		}
		if (GhostState != GhostStates.Freightened) {
			frightenedTimer += Time.deltaTime;

		}
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


	void ChangeGhostToFrightenedState ()
	{
		//StopAllCoroutines ();
	}
	

	public void StartTimer() {
		//StartCoroutine (StartStateTimer ());
	}

	IEnumerator StartStateTimer() {
		GhostState = GhostStates.Scatter;

		for (int i = 0; i < 7f; i++) {
			if (GhostState == GhostStates.Freightened) {
				yield return new WaitForSeconds (6f);
				GhostState = GhostStates.FrightenedBlinking;
				FrightenedBlinking ();
				yield return new WaitForSeconds (4f);
				GhostLeftFrightenedState ();
				GhostState = GhostStates.Scatter;
			}
			yield return new WaitForSeconds (1f);
		}

		GhostState = GhostStates.Chase;
		GhostStateHasChanged ();
		for (int i = 0; i < 20f; i++) {
			if (GhostState == GhostStates.Freightened) {
				yield return new WaitForSeconds (6f);
				GhostState = GhostStates.FrightenedBlinking;
				FrightenedBlinking ();
				yield return new WaitForSeconds (4f);
				GhostLeftFrightenedState ();
				GhostState = GhostStates.Chase;
			}
			yield return new WaitForSeconds (1f);
		}

		GhostState = GhostStates.Scatter;
		GhostStateHasChanged ();
		for (int i = 0; i < 7f; i++) {
			if (GhostState == GhostStates.Freightened) {
				yield return new WaitForSeconds (6f);
				GhostState = GhostStates.FrightenedBlinking;
				FrightenedBlinking ();
				yield return new WaitForSeconds (4f);
				GhostLeftFrightenedState ();
				GhostState = GhostStates.Scatter;
			}
			yield return new WaitForSeconds (1f);
		}

		GhostState = GhostStates.Chase;
		GhostStateHasChanged ();
		for (int i = 0; i < 20f; i++) {
			if (GhostState == GhostStates.Freightened) {
				yield return new WaitForSeconds (6f);
				GhostState = GhostStates.FrightenedBlinking;
				FrightenedBlinking ();
				yield return new WaitForSeconds (4f);
				GhostLeftFrightenedState ();
				GhostState = GhostStates.Chase;
			}
			yield return new WaitForSeconds (1f);
		}

		GhostState = GhostStates.Scatter;
		GhostStateHasChanged ();
		for (int i = 0; i < 5f; i++) {
			if (GhostState == GhostStates.Freightened) {
				yield return new WaitForSeconds (6f);
				GhostState = GhostStates.FrightenedBlinking;
				FrightenedBlinking ();
				yield return new WaitForSeconds (4f);
				GhostLeftFrightenedState ();
				GhostState = GhostStates.Scatter;
			}
			yield return new WaitForSeconds (1f);
		}

		GhostState = GhostStates.Chase;
		GhostStateHasChanged ();
		for (int i = 0; i < 7f; i++) {
			if (GhostState == GhostStates.Freightened) {
				yield return new WaitForSeconds (6f);
				GhostState = GhostStates.FrightenedBlinking;
				FrightenedBlinking ();
				yield return new WaitForSeconds (4f);
				GhostLeftFrightenedState ();
				GhostState = GhostStates.Chase;
			}
			yield return new WaitForSeconds (1f);
		}

		GhostState = GhostStates.Scatter;
		GhostStateHasChanged ();
		for (int i = 0; i < 7f; i++) {
			if (GhostState == GhostStates.Freightened) {
				yield return new WaitForSeconds (6f);
				GhostState = GhostStates.FrightenedBlinking;
				FrightenedBlinking ();
				yield return new WaitForSeconds (4f);
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
					yield return new WaitForSeconds (6f);
					GhostState = GhostStates.FrightenedBlinking;
					FrightenedBlinking ();
					yield return new WaitForSeconds (4f);
					GhostLeftFrightenedState ();
					GhostState = GhostStates.Chase;
				}
				yield return new WaitForSeconds (1f);
			}
			yield return new WaitForSeconds (1f);
		}
	}
}
