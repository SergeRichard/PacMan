﻿using UnityEngine;
using System.Collections;

public class YellowGhost : MonoBehaviour {

	Animator animator;

	public enum YellowGhostStates {Idle, Up, Down, Left, Right};

	public static YellowGhostStates YellowGhostState;

//	public Transform LeftLocation;
//	public Transform RightLocation;

	public float DistanceToTravel = .32f;
	public float TimeStep = .01f;

	public Transform LeftBank;
	public Transform RightBank;

	public GameManager GameManager;
	public Sprite RightSprite;
	public Sprite LeftSprite;
	public Sprite UpSprite;
	public Sprite DownSprite;

	//	private int rowOnGrid = 23;
	//	private int colOnGrid = 13;
	private int rowOnGrid = 11;
	private int colOnGrid = 14;

	private bool movingDone;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		YellowGhostState = YellowGhostStates.Left;
		movingDone = true;
		//transform.position = LeftLocation.transform.position;
	}

	// Update is called once per frame
	void Update () {
//		if (GameManager.state == GameManager.States.Intro) {
//			if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
//				transform.position = RightLocation.transform.position;
//				colOnGrid = 15;
//				pacManStates = PacManStates.Right;
//			}
//			if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
//				transform.position = LeftLocation.transform.position;
//				colOnGrid = 14;
//				pacManStates = PacManStates.Left;
//			}
//		}
		if (movingDone && GameManager.state == GameManager.States.Play) {
			if (YellowGhostState == YellowGhostStates.Right && GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1) {
				SetRight ();
			} else if (YellowGhostState == YellowGhostStates.Left && GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
				SetLeft ();
			} else if (YellowGhostState == YellowGhostStates.Up && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
				SetUp ();
			} else if (YellowGhostState == YellowGhostStates.Down && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
				SetDown ();
			} else {
				animator.enabled = false;
				//GameManager.MusicController.StopWakaSound ();
			}
		}


	}

	void SetRight() {
		animator.enabled = true;
		YellowGhostState = YellowGhostStates.Right;
		animator.Play ("PacManMovesRight");
		movingDone = false;
		StartCoroutine (MoveRight ());
		colOnGrid++;

		GetComponent<SpriteRenderer> ().sprite = RightSprite;
	}
	void SetLeft() {
		animator.enabled = true;
		animator.Play ("PacManMovesLeft");
		YellowGhostState = YellowGhostStates.Left;
		movingDone = false;
		StartCoroutine (MoveLeft ());
		colOnGrid--;
		GetComponent<SpriteRenderer> ().sprite = LeftSprite;
	}
	void SetUp() {
		animator.enabled = true;
		animator.Play ("PacManMovesUp");
		YellowGhostState = YellowGhostStates.Up;
		movingDone = false;
		StartCoroutine (MoveUp ());
		rowOnGrid--;
		GetComponent<SpriteRenderer> ().sprite = UpSprite;
	}
	void SetDown() {
		animator.enabled = true;
		animator.Play ("PacManMovesDown");
		YellowGhostState = YellowGhostStates.Down;
		movingDone = false;
		StartCoroutine (MoveDown ());
		rowOnGrid++;
		GetComponent<SpriteRenderer> ().sprite = DownSprite;
	}


	IEnumerator MoveRight () {
		float distanceTraveled = transform.position.x;
		float endPosition = transform.position.x + DistanceToTravel; 
		while (distanceTraveled < endPosition) {
			distanceTraveled += .08f;
			transform.position = new Vector2(distanceTraveled,transform.position.y);

			yield return new WaitForSeconds (TimeStep);
		}
		movingDone = true;
		if (GameManager.GridMap [rowOnGrid, colOnGrid] == 2) {
			transform.position = LeftBank.position;
			rowOnGrid = 14;
			colOnGrid = 1;
		}

	}
	IEnumerator MoveLeft () {
		float distanceTraveled = transform.position.x;
		float endPosition = transform.position.x - DistanceToTravel; 

		while (distanceTraveled > endPosition) {
			distanceTraveled -= .08f;
			transform.position = new Vector2(distanceTraveled,transform.position.y);

			yield return new WaitForSeconds (TimeStep);
		}
		movingDone = true;
		if (GameManager.GridMap [rowOnGrid, colOnGrid] == 2) {
			transform.position = RightBank.position;
			rowOnGrid = 14;
			colOnGrid = 28;
		}
	}
	IEnumerator MoveUp () {
		float distanceTraveled = transform.position.y;
		float endPosition = transform.position.y + DistanceToTravel; 

		while (distanceTraveled < endPosition) {
			distanceTraveled += .08f;
			transform.position = new Vector2(transform.position.x, distanceTraveled);

			yield return new WaitForSeconds (TimeStep);
		}
		movingDone = true;
	}
	IEnumerator MoveDown () {
		float distanceTraveled = transform.position.y;
		float endPosition = transform.position.y - DistanceToTravel; 

		while (distanceTraveled > endPosition) {
			distanceTraveled -= .08f;
			transform.position = new Vector2(transform.position.x, distanceTraveled);

			yield return new WaitForSeconds (TimeStep);
		}
		movingDone = true;
	}
}
