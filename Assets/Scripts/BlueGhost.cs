﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlueGhost : Ghost {

	Animator animator;

	public enum BlueGhostStates {Idle, IdleUpAndDown, MoveOutOfBox, IdleDown, Up, Down, Left, Right};

	public static BlueGhostStates BlueGhostState;

	public GhostController GhostController;

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

	private int rowOnGridStart = 11;
	private int colOnGridStart = 14;

	private bool movingDone;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		BlueGhostState = BlueGhostStates.IdleUpAndDown;
		movingDone = true;
		Invoke ("StartMovingOutOfBox", 8f);
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
		if (movingDone && GameManager.state == GameManager.States.Play && BlueGhostState == BlueGhostStates.IdleUpAndDown) {
			movingDone = false;
			StartCoroutine(MoveUpAndDownInBox ());
		}
		if (movingDone && GameManager.state == GameManager.States.Play && BlueGhostState == BlueGhostStates.MoveOutOfBox) {
			movingDone = false;
			StartCoroutine(MoveOutOfBox ());
		}
		if (GameManager.state == GameManager.States.PacManDead) {
			animator.enabled = false;
		}
		if (movingDone && GameManager.state == GameManager.States.Play && BlueGhostState != BlueGhostStates.MoveOutOfBox) {
			ChangeDirection();
			switch (BlueGhostState) {
			case BlueGhostStates.Up:
				SetUp ();
				break;
			case BlueGhostStates.Down:
				SetDown ();
				break;
			case BlueGhostStates.Right:
				SetRight ();
				break;
			case BlueGhostStates.Left:
				SetLeft ();
				break;

			}
		}
	}
	public void StartIdleUpAndDownSequence(float timeToStayInBox) {
		CancelInvoke ();
		StopAllCoroutines ();
		rowOnGrid = rowOnGridStart;
		colOnGrid = colOnGridStart;
		BlueGhostState = BlueGhostStates.IdleUpAndDown;
		movingDone = true;
		Invoke ("StartMovingOutOfBox", timeToStayInBox);
	}
	void StartMovingOutOfBox() {
		BlueGhostState = BlueGhostStates.MoveOutOfBox;
	}
	IEnumerator MoveOutOfBox() {
		float distanceTraveled = transform.position.x;
		float endPosition = transform.position.x + (DistanceToTravel * 2f); 

		float timeMulti = 2f;

		animator.enabled = true;
		animator.Play ("MoveRight");

		while (distanceTraveled < endPosition) {
			distanceTraveled += .08f;
			transform.position = new Vector2(distanceTraveled,transform.position.y);

			yield return new WaitForSeconds (TimeStep * timeMulti);
		}
		animator.Play ("MoveUp");
		distanceTraveled = transform.position.y;
		endPosition = transform.position.y + (DistanceToTravel * 3f); 

		while (distanceTraveled < endPosition) {
			distanceTraveled += .08f;
			transform.position = new Vector2(transform.position.x, distanceTraveled);

			yield return new WaitForSeconds (TimeStep * timeMulti);
		}
		BlueGhostState = BlueGhostStates.Left;
		GetComponent<Transform> ().position = GhostController.GhostStartLocation.position;
		movingDone = true;
	}
	IEnumerator MoveUpAndDownInBox() {
		float distanceTraveled = transform.position.y;
		float endPosition = transform.position.y + (DistanceToTravel / 2f);

		float timeMulti = 3f;

		animator.enabled = true;
		animator.Play ("MoveUp");

		while (distanceTraveled < endPosition) {
			distanceTraveled += .08f;
			transform.position = new Vector2(transform.position.x, distanceTraveled);

			yield return new WaitForSeconds (TimeStep * timeMulti);
		}
		endPosition = transform.position.y - DistanceToTravel;

		animator.enabled = true;
		animator.Play ("MoveDown");

		while (distanceTraveled > endPosition) {
			distanceTraveled -= .08f;
			transform.position = new Vector2(transform.position.x, distanceTraveled);

			yield return new WaitForSeconds (TimeStep * timeMulti);
		}
		endPosition = transform.position.y + (DistanceToTravel / 2f);

		animator.enabled = true;
		animator.Play ("MoveUp");

		while (distanceTraveled < endPosition) {
			distanceTraveled += .08f;
			transform.position = new Vector2(transform.position.x, distanceTraveled);

			yield return new WaitForSeconds (TimeStep * timeMulti);
		}

		movingDone = true;

	}
	void ChangeDirection() {
		string pacVerticalLocation = "same";
		string pacHorizontalLocation = "same";

		int pacManX = GameManager.PacManController.ColOnGrid;
		int pacManY = GameManager.PacManController.RowOnGrid;

		int ghostX = colOnGrid;
		int ghostY = rowOnGrid;

		List<BlueGhostStates> possibleStates = new List<BlueGhostStates>();

		if (PacManController.pacManStates == PacManController.PacManStates.Left) {
			pacManX -= 2;
		}
		if (PacManController.pacManStates == PacManController.PacManStates.Right) {
			pacManX += 2;
		}
		if (PacManController.pacManStates == PacManController.PacManStates.Down) {
			pacManY += 2;
		}
		if (PacManController.pacManStates == PacManController.PacManStates.Up) {
			pacManY -= 2;
		}

		int redGhostX = GameManager.GhostController.GhostRed.GetComponent<RedGhost> ().ColOnGrid;
		int redGhostY = GameManager.GhostController.GhostRed.GetComponent<RedGhost> ().RowOnGrid;

		pacManX = pacManX + redGhostX;
		pacManY = pacManY + redGhostY;

		// where is pac-man located relative to ghost? To the left or right?
		if (ghostX - pacManX > 0)
			pacHorizontalLocation = "left";
		else if (ghostX - pacManX < 0)
			pacHorizontalLocation = "right";		

		// where is pac-man located relative to ghost? To the up(north) or down(south)?
		if (ghostY - pacManY > 0) {
			pacVerticalLocation = "up";
		} else if (ghostY - pacManY < 0) {
			pacVerticalLocation = "down";
		} 

		int xDistance = Mathf.Abs (ghostX - pacManX);
		int yDistance = Mathf.Abs (ghostY - pacManY);

		switch (BlueGhostState) {
		case BlueGhostStates.Left:
			// make sure that direction is at the very least set to a possible direction to move
			if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
				possibleStates.Add (BlueGhostStates.Left);
				//RedGhostState = RedGhostStates.Left;
			}
			if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
				possibleStates.Add (BlueGhostStates.Up);
				//RedGhostState = RedGhostStates.Up;
			}
			if (GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
				possibleStates.Add (BlueGhostStates.Down);
				//RedGhostState = RedGhostStates.Down;
			}
			BlueGhostState = possibleStates [Random.Range (0, possibleStates.Count)];

			if ((pacHorizontalLocation == "left" || pacHorizontalLocation == "same") && (pacVerticalLocation == "up" || pacVerticalLocation == "same")) {
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 1) {
					BlueGhostState = BlueGhostStates.Left;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
					BlueGhostState = BlueGhostStates.Up;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
					if (xDistance > yDistance) {
						BlueGhostState = BlueGhostStates.Left;
					}
					if (xDistance < yDistance) {
						BlueGhostState = BlueGhostStates.Up;
					}
					if (xDistance == yDistance) {
						if (Random.Range (0, 2) == 0) {
							BlueGhostState = BlueGhostStates.Up;
						} else {
							BlueGhostState = BlueGhostStates.Left;
						}
					}
				}
			}
			if ((pacHorizontalLocation == "left" || pacHorizontalLocation == "same") && (pacVerticalLocation == "down" || pacVerticalLocation == "same")) {
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 1) {
					BlueGhostState = BlueGhostStates.Left;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					BlueGhostState = BlueGhostStates.Down;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					if (xDistance > yDistance) {
						BlueGhostState = BlueGhostStates.Left;
					}
					if (xDistance < yDistance) {
						BlueGhostState = BlueGhostStates.Down;
					}
					if (xDistance == yDistance) {
						if (Random.Range (0, 2) == 0) {
							BlueGhostState = BlueGhostStates.Down;
						} else {
							BlueGhostState = BlueGhostStates.Left;
						}
					}
				}
			}
			// pac-man is in the opposite direction. Ghost has to try to circle back towards pac-man.
			if ((pacHorizontalLocation == "right" || pacHorizontalLocation == "same") && (pacVerticalLocation == "up" || pacVerticalLocation == "same")) {
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 1) {
					BlueGhostState = BlueGhostStates.Left;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
					BlueGhostState = BlueGhostStates.Up;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {					

					BlueGhostState = BlueGhostStates.Up;

				}
			}
			if ((pacHorizontalLocation == "right" || pacHorizontalLocation == "same") && (pacVerticalLocation == "down" || pacVerticalLocation == "same")) {
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 1) {
					BlueGhostState = BlueGhostStates.Left;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					BlueGhostState = BlueGhostStates.Down;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {

					BlueGhostState = BlueGhostStates.Down;

				}
			}
			break;
		case BlueGhostStates.Right:
			// make sure that direction is at the very least set to a possible direction to move
			if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1) {
				possibleStates.Add (BlueGhostStates.Right);
				//RedGhostState = RedGhostStates.Left;
			}
			if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
				possibleStates.Add (BlueGhostStates.Up);
				//RedGhostState = RedGhostStates.Up;
			}
			if (GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
				possibleStates.Add (BlueGhostStates.Down);
				//RedGhostState = RedGhostStates.Down;
			}
			BlueGhostState = possibleStates [Random.Range (0, possibleStates.Count)];

			if ((pacHorizontalLocation == "right" || pacHorizontalLocation == "same") && (pacVerticalLocation == "up" || pacVerticalLocation == "same")) {
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 1) {
					BlueGhostState = BlueGhostStates.Right;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
					BlueGhostState = BlueGhostStates.Up;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
					if (xDistance > yDistance) {
						BlueGhostState = BlueGhostStates.Right;
					}
					if (xDistance < yDistance) {
						BlueGhostState = BlueGhostStates.Up;
					}
					if (xDistance == yDistance) {
						if (Random.Range (0, 2) == 0) {
							BlueGhostState = BlueGhostStates.Up;
						} else {
							BlueGhostState = BlueGhostStates.Right;
						}
					}
				}
			}
			if ((pacHorizontalLocation == "right" || pacHorizontalLocation == "same") && (pacVerticalLocation == "down" || pacVerticalLocation == "same")) {
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 1) {
					BlueGhostState = BlueGhostStates.Right;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					BlueGhostState = BlueGhostStates.Down;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					if (xDistance > yDistance) {
						BlueGhostState = BlueGhostStates.Right;
					}
					if (xDistance < yDistance) {
						BlueGhostState = BlueGhostStates.Down;
					}
					if (xDistance == yDistance) {
						if (Random.Range (0, 2) == 0) {
							BlueGhostState = BlueGhostStates.Down;
						} else {
							BlueGhostState = BlueGhostStates.Right;
						}
					}
				}
			}
			// pac-man is in the opposite direction. Ghost has to try to circle back towards pac-man.
			if ((pacHorizontalLocation == "left" || pacHorizontalLocation == "same") && (pacVerticalLocation == "up" || pacVerticalLocation == "same")) {
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 1) {
					BlueGhostState = BlueGhostStates.Right;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
					BlueGhostState = BlueGhostStates.Up;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {

					BlueGhostState = BlueGhostStates.Up;

				}
			}
			if ((pacHorizontalLocation == "left" || pacHorizontalLocation == "same") && (pacVerticalLocation == "down" || pacVerticalLocation == "same")) {
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 1) {
					BlueGhostState = BlueGhostStates.Right;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					BlueGhostState = BlueGhostStates.Down;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {	
					BlueGhostState = BlueGhostStates.Down;					
				}
			}
			break;
		case BlueGhostStates.Up:
			// make sure that direction is at the very least set to a possible direction to move
			if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1) {
				possibleStates.Add (BlueGhostStates.Right);
				//RedGhostState = RedGhostStates.Left;
			}
			if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
				possibleStates.Add (BlueGhostStates.Up);
				//RedGhostState = RedGhostStates.Up;
			}
			if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
				possibleStates.Add (BlueGhostStates.Left);
				//RedGhostState = RedGhostStates.Down;
			}
			BlueGhostState = possibleStates [Random.Range (0, possibleStates.Count)];

			if ((pacHorizontalLocation == "right" || pacHorizontalLocation == "same") && (pacVerticalLocation == "up" || pacVerticalLocation == "same")) {
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 1) {
					BlueGhostState = BlueGhostStates.Right;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
					BlueGhostState = BlueGhostStates.Up;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
					if (xDistance > yDistance) {
						BlueGhostState = BlueGhostStates.Right;
					}
					if (xDistance < yDistance) {
						BlueGhostState = BlueGhostStates.Up;
					}
					if (xDistance == yDistance) {
						if (Random.Range (0, 2) == 0) {
							BlueGhostState = BlueGhostStates.Up;
						} else {
							BlueGhostState = BlueGhostStates.Right;
						}
					}
				}
			}
			if ((pacHorizontalLocation == "left" || pacHorizontalLocation == "same") && (pacVerticalLocation == "up" || pacVerticalLocation == "same")) {
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 1) {
					BlueGhostState = BlueGhostStates.Left;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
					BlueGhostState = BlueGhostStates.Up;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
					if (xDistance > yDistance) {
						BlueGhostState = BlueGhostStates.Left;
					}
					if (xDistance < yDistance) {
						BlueGhostState = BlueGhostStates.Up;
					}
					if (xDistance == yDistance) {
						if (Random.Range (0, 2) == 0) {
							BlueGhostState = BlueGhostStates.Up;
						} else {
							BlueGhostState = BlueGhostStates.Left;
						}
					}
				}
			}
			// pac-man is in the opposite direction. Ghost has to try to circle back towards pac-man.
			if ((pacHorizontalLocation == "right" || pacHorizontalLocation == "same") && (pacVerticalLocation == "down" || pacVerticalLocation == "same")) {
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 1) {
					BlueGhostState = BlueGhostStates.Right;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
					BlueGhostState = BlueGhostStates.Up;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {

					BlueGhostState = BlueGhostStates.Right;
				}
			}
			if ((pacHorizontalLocation == "left" || pacHorizontalLocation == "same") && (pacVerticalLocation == "down" || pacVerticalLocation == "same")) {
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 1) {
					BlueGhostState = BlueGhostStates.Left;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
					BlueGhostState = BlueGhostStates.Up;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {

					BlueGhostState = BlueGhostStates.Left;

				}
			}
			break;
		case BlueGhostStates.Down:
			// make sure that direction is at the very least set to a possible direction to move
			if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1) {
				possibleStates.Add (BlueGhostStates.Right);
				//RedGhostState = RedGhostStates.Left;
			}
			if (GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
				possibleStates.Add (BlueGhostStates.Down);
				//RedGhostState = RedGhostStates.Up;
			}
			if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
				possibleStates.Add (BlueGhostStates.Left);
				//RedGhostState = RedGhostStates.Down;
			}
			BlueGhostState = possibleStates [Random.Range (0, possibleStates.Count)];

			if ((pacHorizontalLocation == "right" || pacHorizontalLocation == "same") && (pacVerticalLocation == "down" || pacVerticalLocation == "same")) {
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 1) {
					BlueGhostState = BlueGhostStates.Right;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					BlueGhostState = BlueGhostStates.Down;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					if (xDistance > yDistance) {
						BlueGhostState = BlueGhostStates.Right;
					}
					if (xDistance < yDistance) {
						BlueGhostState = BlueGhostStates.Down;
					}
					if (xDistance == yDistance) {
						if (Random.Range (0, 2) == 0) {
							BlueGhostState = BlueGhostStates.Down;
						} else {
							BlueGhostState = BlueGhostStates.Right;
						}
					}
				}
			}
			if ((pacHorizontalLocation == "left" || pacHorizontalLocation == "same") && (pacVerticalLocation == "down" || pacVerticalLocation == "same")) {
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 1) {
					BlueGhostState = BlueGhostStates.Left;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					BlueGhostState = BlueGhostStates.Down;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					if (xDistance > yDistance) {
						BlueGhostState = BlueGhostStates.Left;
					}
					if (xDistance < yDistance) {
						BlueGhostState = BlueGhostStates.Down;
					}
					if (xDistance == yDistance) {
						if (Random.Range (0, 2) == 0) {
							BlueGhostState = BlueGhostStates.Down;
						} else {
							BlueGhostState = BlueGhostStates.Left;
						}
					}
				}
			}
			// pac-man is in the opposite direction. Ghost has to try to circle back towards pac-man.
			if ((pacHorizontalLocation == "right" || pacHorizontalLocation == "same") && (pacVerticalLocation == "up" || pacVerticalLocation == "same")) {
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 1) {
					BlueGhostState = BlueGhostStates.Right;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					BlueGhostState = BlueGhostStates.Down;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {

					BlueGhostState = BlueGhostStates.Right;
				}
			}
			if ((pacHorizontalLocation == "left" || pacHorizontalLocation == "same") && (pacVerticalLocation == "up" || pacVerticalLocation == "same")) {
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 1) {
					BlueGhostState = BlueGhostStates.Left;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					BlueGhostState = BlueGhostStates.Down;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {

					BlueGhostState = BlueGhostStates.Left;
				}
			}
			break;
//		case BlueGhostStates.Left:
//			if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
//				if (Random.Range (0, 2) == 0) {
//					BlueGhostState = BlueGhostStates.Left;
//					break;
//				}
//			}
//			if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1 || GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
//				if (Random.Range (0, 2) == 0) {
//					if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
//						BlueGhostState = BlueGhostStates.Up;
//					} else {
//						BlueGhostState = BlueGhostStates.Down;
//					}
//				} else {
//					if (GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
//						BlueGhostState = BlueGhostStates.Down;
//					} else {
//						BlueGhostState = BlueGhostStates.Up;
//					}
//				}
//			}
//			break;
//		case BlueGhostStates.Right:
//			if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1) {
//				if (Random.Range (0, 2) == 0) {
//					BlueGhostState = BlueGhostStates.Right;
//					break;
//				} 
//			}
//			if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1 || GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
//				if (Random.Range (0, 2) == 0) {
//					if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
//						BlueGhostState = BlueGhostStates.Up;
//					} else {
//						BlueGhostState = BlueGhostStates.Down;
//					}
//				} else {
//					if (GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
//						BlueGhostState = BlueGhostStates.Down;
//					} else {
//						BlueGhostState = BlueGhostStates.Up;
//					}
//
//				}
//			}
//			break;
//		case BlueGhostStates.Up:
//			if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
//				if (Random.Range (0, 2) == 0) {
//					BlueGhostState = BlueGhostStates.Up;
//					break;
//				}
//			}
//			if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 || GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
//				if (Random.Range (0, 2) == 0) {
//					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1) {
//						BlueGhostState = BlueGhostStates.Right;
//					} else {
//						BlueGhostState = BlueGhostStates.Left;
//					}
//				} else {
//					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
//						BlueGhostState = BlueGhostStates.Left;
//					} else {
//						BlueGhostState = BlueGhostStates.Right;
//					}
//				}
//			}
//			break;
//		case BlueGhostStates.Down:
//			if (GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
//				if (Random.Range (0, 2) == 0) {
//					BlueGhostState = BlueGhostStates.Down;
//					break;
//				}
//			}
//			if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 || GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
//				if (Random.Range (0, 2) == 0) {
//					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1) {
//						BlueGhostState = BlueGhostStates.Right;
//					} else {
//						BlueGhostState = BlueGhostStates.Left;
//					}
//				} else {
//					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
//						BlueGhostState = BlueGhostStates.Left;
//					} else {
//						BlueGhostState = BlueGhostStates.Right;
//					}
//				}
//			}
//			break;
		}

	}
	void SetRight() {
		animator.enabled = true;
		BlueGhostState = BlueGhostStates.Right;
		animator.Play ("MoveRight");
		movingDone = false;
		StartCoroutine (MoveRight ());
		colOnGrid++;

		GetComponent<SpriteRenderer> ().sprite = RightSprite;
	}
	void SetLeft() {
		animator.enabled = true;
		animator.Play ("MoveLeft");
		BlueGhostState = BlueGhostStates.Left;
		movingDone = false;
		StartCoroutine (MoveLeft ());
		colOnGrid--;
		GetComponent<SpriteRenderer> ().sprite = LeftSprite;
	}
	void SetUp() {
		animator.enabled = true;
		animator.Play ("MoveUp");
		BlueGhostState = BlueGhostStates.Up;
		movingDone = false;
		StartCoroutine (MoveUp ());
		rowOnGrid--;
		GetComponent<SpriteRenderer> ().sprite = UpSprite;
	}
	void SetDown() {
		animator.enabled = true;
		animator.Play ("MoveDown");
		BlueGhostState = BlueGhostStates.Down;
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
