﻿using UnityEngine;
using System.Collections;

public class RedGhost : MonoBehaviour {

	Animator animator;

	public enum RedGhostStates {Idle, Up, Down, Left, Right};

	public static RedGhostStates RedGhostState;

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
		RedGhostState = RedGhostStates.Left;
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
			ChangeDirection();
			switch (RedGhostState) {
			case RedGhostStates.Up:
				SetUp ();
				break;
			case RedGhostStates.Down:
				SetDown ();
				break;
			case RedGhostStates.Right:
				SetRight ();
				break;
			case RedGhostStates.Left:
				SetLeft ();
				break;

			}
		}
	}
	void ChangeDirection() {
		switch (RedGhostState) {
		case RedGhostStates.Left:
			if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
				if (Random.Range (0, 2) == 0) {
					RedGhostState = RedGhostStates.Left;
					break;
				}
			}
			if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1 || GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
				if (Random.Range (0, 2) == 0) {
					if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
						RedGhostState = RedGhostStates.Up;
					} else {
						RedGhostState = RedGhostStates.Down;
					}
				} else {
					if (GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
						RedGhostState = RedGhostStates.Down;
					} else {
						RedGhostState = RedGhostStates.Up;
					}
				}
			}
			break;
		case RedGhostStates.Right:
			if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1) {
				if (Random.Range (0, 2) == 0) {
					RedGhostState = RedGhostStates.Right;
					break;
				} 
			}
			if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1 || GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
				if (Random.Range (0, 2) == 0) {
					if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
						RedGhostState = RedGhostStates.Up;
					} else {
						RedGhostState = RedGhostStates.Down;
					}
				} else {
					if (GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
						RedGhostState = RedGhostStates.Down;
					} else {
						RedGhostState = RedGhostStates.Up;
					}

				}
			}
			break;
		case RedGhostStates.Up:
			if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
				if (Random.Range (0, 2) == 0) {
					RedGhostState = RedGhostStates.Up;
					break;
				}
			}
			if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 || GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
				if (Random.Range (0, 2) == 0) {
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1) {
						RedGhostState = RedGhostStates.Right;
					} else {
						RedGhostState = RedGhostStates.Left;
					}
				} else {
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
						RedGhostState = RedGhostStates.Left;
					} else {
						RedGhostState = RedGhostStates.Right;
					}
				}
			}
			break;
		case RedGhostStates.Down:
			if (GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
				if (Random.Range (0, 2) == 0) {
					RedGhostState = RedGhostStates.Down;
					break;
				}
			}
			if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 || GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
				if (Random.Range (0, 2) == 0) {
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1) {
						RedGhostState = RedGhostStates.Right;
					} else {
						RedGhostState = RedGhostStates.Left;
					}
				} else {
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
						RedGhostState = RedGhostStates.Left;
					} else {
						RedGhostState = RedGhostStates.Right;
					}
				}
			}
			break;
		}

	}
	void SetRight() {
		animator.enabled = true;
		RedGhostState = RedGhostStates.Right;
		animator.Play ("RedGhostMovesRight");
		movingDone = false;
		StartCoroutine (MoveRight ());
		colOnGrid++;

		GetComponent<SpriteRenderer> ().sprite = RightSprite;
	}
	void SetLeft() {
		animator.enabled = true;
		animator.Play ("RedGhostMovesLeft");
		RedGhostState = RedGhostStates.Left;
		movingDone = false;
		StartCoroutine (MoveLeft ());
		colOnGrid--;
		GetComponent<SpriteRenderer> ().sprite = LeftSprite;
	}
	void SetUp() {
		animator.enabled = true;
		animator.Play ("RedGhostMovesUp");
		RedGhostState = RedGhostStates.Up;
		movingDone = false;
		StartCoroutine (MoveUp ());
		rowOnGrid--;
		GetComponent<SpriteRenderer> ().sprite = UpSprite;
	}
	void SetDown() {
		animator.enabled = true;
		animator.Play ("RedGhostMovesDown");
		RedGhostState = RedGhostStates.Down;
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