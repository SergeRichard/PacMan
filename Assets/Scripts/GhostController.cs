using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void GhostStateHasChangedEventHandler();
public delegate void GhostLeftFrightenedStateEventHandler();
public delegate void FrightenedBlinkingEventHandler();

public class GhostController : MonoBehaviour {

	class GhostMode
	{
		public Ghost.GhostStates state;
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
	void Start () {
		ghostModes = new Queue<GhostMode> ();
		SetUpInitial ();

	}
	public void SetUpInitial(bool newLevel = true) {
		Ghost.GhostState = Ghost.GhostStates.Scatter;
		frightenedTimer = 0;

		if (newLevel == true) {
			modeTimer = 0;


			ghostModes.Clear ();

			GhostMode mode = new GhostMode ();
			mode.state = Ghost.GhostStates.Scatter;
			mode.time = 7f; // start right away
			ghostModes.Enqueue (mode);

			GhostMode mode2 = new GhostMode ();
			mode2.state = Ghost.GhostStates.Chase;
			mode2.time = 20f;
			ghostModes.Enqueue (mode2);

			GhostMode mode3 = new GhostMode ();
			mode3.state = Ghost.GhostStates.Scatter;
			mode3.time = 7f;
			ghostModes.Enqueue (mode3);

			GhostMode mode4 = new GhostMode ();
			mode4.state = Ghost.GhostStates.Chase;
			mode4.time = 20f;
			ghostModes.Enqueue (mode4);

			GhostMode mode5 = new GhostMode ();
			mode5.state = Ghost.GhostStates.Scatter;
			mode5.time = 5f;
			ghostModes.Enqueue (mode5);
		}

	}
	// Update is called once per frame
	void Update () {
		// TODO clear and re-add ghost modes. 

//		if (GameManager.state == GameManager.States.Play && FrightenedState != FrightenedStates.Frightened && FrightenedState != FrightenedStates.FrightenedBlinking) {
//			modeTimer += Time.deltaTime;
//			if (ghostModes.Count > 0) {
//				if (modeTimer >= ghostModes.Peek ().time) {					
//					ghostModes.Dequeue ();
//					GhostStateHasChanged ();
//
//					GhostState = ghostModes.Peek ().state;
//
//				}
//			} else {
//				GhostState = GhostStates.Chase;
//				modeTimer = 0;
//			}
//		}
//		if (FrightenedState == FrightenedStates.Frightened) {
//			frightenedTimer += Time.deltaTime;
//			if (frightenedTimer >= 6) {
//				frightenedTimer = 0;
//				FrightenedState = FrightenedStates.FrightenedBlinking;
//				FrightenedBlinking ();
//			}
//		}
//		if (FrightenedState == FrightenedStates.FrightenedBlinking) {
//			frightenedTimer += Time.deltaTime;
//			if (frightenedTimer >= 4) {
//				frightenedTimer = 0;
//				if (ghostModes.Count > 0) {
//					GhostState = ghostModes.Peek ().state;
//				} else {
//					GhostState = GhostStates.Chase;
//				}
//				GhostLeftFrightenedState ();
//			}
//
//		}
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
