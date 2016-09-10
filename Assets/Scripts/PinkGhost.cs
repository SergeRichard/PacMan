using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PinkGhost : Ghost {

	Animator animator;

	public enum PinkGhostStates {Idle, IdleUpAndDown, MoveOutOfBox, Up, Down, Left, Right};

	public static PinkGhostStates PinkGhostState;
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

	const int rowOnGridStart = 11;
	const int colOnGridStart = 14;

	private bool movingDone;

	private float timeStep;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		timeStep = TimeStep;
		animator = GetComponent<Animator> ();
		PinkGhostState = PinkGhostStates.MoveOutOfBox;
		movingDone = true;
		//transform.position = LeftLocation.transform.position;
		GhostController.GhostStateHasChanged += PinkGhost_GhostStateHasChanged;
		GhostController.PacManController.ChangeGhostToFrightenedState += ChangeGhostToFrightenedState;
	}

	void ChangeGhostToFrightenedState ()
	{
		animator.enabled = true;
		animator.Play ("PinkGhostFrightened");
		timeStep = GhostController.GhostFrightenedTimeStep;
		PinkGhost_GhostStateHasChanged ();
	}

	void PinkGhost_GhostStateHasChanged ()
	{
		if (PinkGhostState == PinkGhostStates.Left) {
			PinkGhostState = PinkGhostStates.Right;
		} else if (PinkGhostState == PinkGhostStates.Right) {
			PinkGhostState = PinkGhostStates.Left;
		} else if (PinkGhostState == PinkGhostStates.Up) {
			PinkGhostState = PinkGhostStates.Down;
		} else if (PinkGhostState == PinkGhostStates.Down) {
			PinkGhostState = PinkGhostStates.Up;
		}
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
		if (movingDone && GameManager.state == GameManager.States.Play && PinkGhostState == PinkGhostStates.IdleUpAndDown) {
			movingDone = false;
			StartCoroutine(MoveUpAndDownInBox ());
		}
		if (movingDone && GameManager.state == GameManager.States.Play && PinkGhostState == PinkGhostStates.MoveOutOfBox) {
			movingDone = false;
			StartCoroutine (MoveOutOfBox ());
		}
		if (GameManager.state == GameManager.States.PacManDead) {
			animator.enabled = false;
		}
		if (movingDone && GameManager.state == GameManager.States.Play && PinkGhostState != PinkGhostStates.MoveOutOfBox) {
			ChangeDirection();
			switch (PinkGhostState) {
			case PinkGhostStates.Up:
				SetUp ();
				break;
			case PinkGhostStates.Down:
				SetDown ();
				break;
			case PinkGhostStates.Right:
				SetRight ();
				break;
			case PinkGhostStates.Left:
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
		PinkGhostState = PinkGhostStates.IdleUpAndDown;
		movingDone = true;
		Invoke ("StartMovingOutOfBox", timeToStayInBox);
	}
	void StartMovingOutOfBox() {
		PinkGhostState = PinkGhostStates.MoveOutOfBox;
	}
	IEnumerator MoveOutOfBox() {
		float distanceTraveled = transform.position.y;
		float endPosition = transform.position.y + (DistanceToTravel * 3f);

		float timeMulti = 2f;

		animator.enabled = true;

		if (GhostController.GhostState == GhostController.GhostStates.Freightened) {
			animator.Play ("PinkGhostFrightened");
		} else {
			animator.Play ("MoveUp");
		}

		while (distanceTraveled < endPosition) {
			distanceTraveled += .08f;
			transform.position = new Vector2(transform.position.x, distanceTraveled);

			yield return new WaitForSeconds (timeStep * timeMulti);
		}
		PinkGhostState = PinkGhostStates.Left;
		GetComponent<Transform> ().position = GhostController.GhostStartLocation.position;
		movingDone = true;
	}
	IEnumerator MoveUpAndDownInBox() {
		float distanceTraveled = transform.position.y;
		float endPosition = transform.position.y - (DistanceToTravel / 2f);

		float timeMulti = 3f;

		animator.enabled = true;

		if (GhostController.GhostState == GhostController.GhostStates.Freightened) {
			animator.Play ("PinkGhostFrightened");
		} else {
			animator.Play ("MoveDown");
		}

		while (distanceTraveled > endPosition) {
			distanceTraveled -= .08f;
			transform.position = new Vector2(transform.position.x, distanceTraveled);

			yield return new WaitForSeconds (timeStep * timeMulti);
		}
		endPosition = transform.position.y + DistanceToTravel;

		animator.enabled = true;

		if (GhostController.GhostState == GhostController.GhostStates.Freightened) {
			animator.Play ("PinkGhostFrightened");
		} else {
			animator.Play ("MoveUp");
		}

		while (distanceTraveled < endPosition) {
			distanceTraveled += .08f;
			transform.position = new Vector2(transform.position.x, distanceTraveled);

			yield return new WaitForSeconds (timeStep * timeMulti);
		}
		endPosition = transform.position.y - (DistanceToTravel / 2f);

		animator.enabled = true;

		if (GhostController.GhostState == GhostController.GhostStates.Freightened) {
			animator.Play ("PinkGhostFrightened");
		} else {
			animator.Play ("MoveDown");
		}

		while (distanceTraveled > endPosition) {
			distanceTraveled -= .08f;
			transform.position = new Vector2(transform.position.x, distanceTraveled);

			yield return new WaitForSeconds (timeStep * timeMulti);
		}

		movingDone = true;

	}
	void SetRight() {
		animator.enabled = true;
		PinkGhostState = PinkGhostStates.Right;

		if (GhostController.GhostState == GhostController.GhostStates.Freightened) {
			animator.Play ("PinkGhostFrightened");
		} else {
			animator.Play ("MoveRight");
		}
		movingDone = false;
		StartCoroutine (MoveRight ());
		colOnGrid++;

		GetComponent<SpriteRenderer> ().sprite = RightSprite;
	}
	void SetLeft() {
		animator.enabled = true;

		if (GhostController.GhostState == GhostController.GhostStates.Freightened) {
			animator.Play ("PinkGhostFrightened");
		} else {
			animator.Play ("MoveLeft");
		}
		PinkGhostState = PinkGhostStates.Left;
		movingDone = false;
		StartCoroutine (MoveLeft ());
		colOnGrid--;
		GetComponent<SpriteRenderer> ().sprite = LeftSprite;
	}
	void SetUp() {
		animator.enabled = true;

		if (GhostController.GhostState == GhostController.GhostStates.Freightened) {
			animator.Play ("PinkGhostFrightened");
		} else {
			animator.Play ("MoveUp");
		}
		PinkGhostState = PinkGhostStates.Up;
		movingDone = false;
		StartCoroutine (MoveUp ());
		rowOnGrid--;
		GetComponent<SpriteRenderer> ().sprite = UpSprite;
	}
	void SetDown() {
		animator.enabled = true;

		if (GhostController.GhostState == GhostController.GhostStates.Freightened) {
			animator.Play ("PinkGhostFrightened");
		} else {
			animator.Play ("MoveDown");
		}
		PinkGhostState = PinkGhostStates.Down;
		movingDone = false;
		StartCoroutine (MoveDown ());
		rowOnGrid++;
		GetComponent<SpriteRenderer> ().sprite = DownSprite;
	}
	void ChangeDirection() {
		int homeX = 2;
		int homeY = 1;

		string pacVerticalLocation = "same";
		string pacHorizontalLocation = "same";

		int pacManX = GameManager.PacManController.ColOnGrid;
		int pacManY = GameManager.PacManController.RowOnGrid;

		int ghostX = colOnGrid;
		int ghostY = rowOnGrid;

		List<PinkGhostStates> possibleStates = new List<PinkGhostStates>();

		if (PacManController.pacManStates == PacManController.PacManStates.Left) {
			pacManX -= 4;
		}
		if (PacManController.pacManStates == PacManController.PacManStates.Right) {
			pacManX += 4;
		}
		if (PacManController.pacManStates == PacManController.PacManStates.Down) {
			pacManY += 4;
		}
		if (PacManController.pacManStates == PacManController.PacManStates.Up) {
			pacManY -= 4;
		}

		if (GhostState == GhostStates.Scatter) {
			pacManX = homeX;
			pacManY = homeY;

		}

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

		if (GhostController.GhostState != GhostController.GhostStates.Freightened) {
			switch (PinkGhostState) {
			case PinkGhostStates.Left:
			// make sure that direction is at the very least set to a possible direction to move
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
					possibleStates.Add (PinkGhostStates.Left);
					//RedGhostState = RedGhostStates.Left;
				}
				if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
					possibleStates.Add (PinkGhostStates.Up);
					//RedGhostState = RedGhostStates.Up;
				}
				if (GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					possibleStates.Add (PinkGhostStates.Down);
					//RedGhostState = RedGhostStates.Down;
				}
				PinkGhostState = possibleStates [Random.Range (0, possibleStates.Count)];

				if ((pacHorizontalLocation == "left" || pacHorizontalLocation == "same") && (pacVerticalLocation == "up" || pacVerticalLocation == "same")) {
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 1) {
						PinkGhostState = PinkGhostStates.Left;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
						PinkGhostState = PinkGhostStates.Up;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
						if (xDistance > yDistance) {
							PinkGhostState = PinkGhostStates.Left;
						}
						if (xDistance < yDistance) {
							PinkGhostState = PinkGhostStates.Up;
						}
						if (xDistance == yDistance) {
							if (Random.Range (0, 2) == 0) {
								PinkGhostState = PinkGhostStates.Up;
							} else {
								PinkGhostState = PinkGhostStates.Left;
							}
						}
					}
				}
				if ((pacHorizontalLocation == "left" || pacHorizontalLocation == "same") && (pacVerticalLocation == "down" || pacVerticalLocation == "same")) {
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 1) {
						PinkGhostState = PinkGhostStates.Left;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
						PinkGhostState = PinkGhostStates.Down;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
						if (xDistance > yDistance) {
							PinkGhostState = PinkGhostStates.Left;
						}
						if (xDistance < yDistance) {
							PinkGhostState = PinkGhostStates.Down;
						}
						if (xDistance == yDistance) {
							if (Random.Range (0, 2) == 0) {
								PinkGhostState = PinkGhostStates.Down;
							} else {
								PinkGhostState = PinkGhostStates.Left;
							}
						}
					}
				}
			// pac-man is in the opposite direction. Ghost has to try to circle back towards pac-man.
				if ((pacHorizontalLocation == "right" || pacHorizontalLocation == "same") && (pacVerticalLocation == "up" || pacVerticalLocation == "same")) {
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 1) {
						PinkGhostState = PinkGhostStates.Left;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
						PinkGhostState = PinkGhostStates.Up;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {					

						PinkGhostState = PinkGhostStates.Up;

					}
				}
				if ((pacHorizontalLocation == "right" || pacHorizontalLocation == "same") && (pacVerticalLocation == "down" || pacVerticalLocation == "same")) {
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 1) {
						PinkGhostState = PinkGhostStates.Left;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
						PinkGhostState = PinkGhostStates.Down;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {

						PinkGhostState = PinkGhostStates.Down;

					}
				}
				break;
			case PinkGhostStates.Right:
			// make sure that direction is at the very least set to a possible direction to move
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1) {
					possibleStates.Add (PinkGhostStates.Right);
					//RedGhostState = RedGhostStates.Left;
				}
				if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
					possibleStates.Add (PinkGhostStates.Up);
					//RedGhostState = RedGhostStates.Up;
				}
				if (GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					possibleStates.Add (PinkGhostStates.Down);
					//RedGhostState = RedGhostStates.Down;
				}
				PinkGhostState = possibleStates [Random.Range (0, possibleStates.Count)];

				if ((pacHorizontalLocation == "right" || pacHorizontalLocation == "same") && (pacVerticalLocation == "up" || pacVerticalLocation == "same")) {
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 1) {
						PinkGhostState = PinkGhostStates.Right;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
						PinkGhostState = PinkGhostStates.Up;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
						if (xDistance > yDistance) {
							PinkGhostState = PinkGhostStates.Right;
						}
						if (xDistance < yDistance) {
							PinkGhostState = PinkGhostStates.Up;
						}
						if (xDistance == yDistance) {
							if (Random.Range (0, 2) == 0) {
								PinkGhostState = PinkGhostStates.Up;
							} else {
								PinkGhostState = PinkGhostStates.Right;
							}
						}
					}
				}
				if ((pacHorizontalLocation == "right" || pacHorizontalLocation == "same") && (pacVerticalLocation == "down" || pacVerticalLocation == "same")) {
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 1) {
						PinkGhostState = PinkGhostStates.Right;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
						PinkGhostState = PinkGhostStates.Down;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
						if (xDistance > yDistance) {
							PinkGhostState = PinkGhostStates.Right;
						}
						if (xDistance < yDistance) {
							PinkGhostState = PinkGhostStates.Down;
						}
						if (xDistance == yDistance) {
							if (Random.Range (0, 2) == 0) {
								PinkGhostState = PinkGhostStates.Down;
							} else {
								PinkGhostState = PinkGhostStates.Right;
							}
						}
					}
				}
			// pac-man is in the opposite direction. Ghost has to try to circle back towards pac-man.
				if ((pacHorizontalLocation == "left" || pacHorizontalLocation == "same") && (pacVerticalLocation == "up" || pacVerticalLocation == "same")) {
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 1) {
						PinkGhostState = PinkGhostStates.Right;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
						PinkGhostState = PinkGhostStates.Up;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {

						PinkGhostState = PinkGhostStates.Up;

					}
				}
				if ((pacHorizontalLocation == "left" || pacHorizontalLocation == "same") && (pacVerticalLocation == "down" || pacVerticalLocation == "same")) {
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 1) {
						PinkGhostState = PinkGhostStates.Right;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
						PinkGhostState = PinkGhostStates.Down;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {	
						PinkGhostState = PinkGhostStates.Down;					
					}
				}
				break;
			case PinkGhostStates.Up:
			// make sure that direction is at the very least set to a possible direction to move
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1) {
					possibleStates.Add (PinkGhostStates.Right);
					//RedGhostState = RedGhostStates.Left;
				}
				if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
					possibleStates.Add (PinkGhostStates.Up);
					//RedGhostState = RedGhostStates.Up;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
					possibleStates.Add (PinkGhostStates.Left);
					//RedGhostState = RedGhostStates.Down;
				}
				PinkGhostState = possibleStates [Random.Range (0, possibleStates.Count)];

				if ((pacHorizontalLocation == "right" || pacHorizontalLocation == "same") && (pacVerticalLocation == "up" || pacVerticalLocation == "same")) {
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 1) {
						PinkGhostState = PinkGhostStates.Right;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
						PinkGhostState = PinkGhostStates.Up;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
						if (xDistance > yDistance) {
							PinkGhostState = PinkGhostStates.Right;
						}
						if (xDistance < yDistance) {
							PinkGhostState = PinkGhostStates.Up;
						}
						if (xDistance == yDistance) {
							if (Random.Range (0, 2) == 0) {
								PinkGhostState = PinkGhostStates.Up;
							} else {
								PinkGhostState = PinkGhostStates.Right;
							}
						}
					}
				}
				if ((pacHorizontalLocation == "left" || pacHorizontalLocation == "same") && (pacVerticalLocation == "up" || pacVerticalLocation == "same")) {
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 1) {
						PinkGhostState = PinkGhostStates.Left;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
						PinkGhostState = PinkGhostStates.Up;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
						if (xDistance > yDistance) {
							PinkGhostState = PinkGhostStates.Left;
						}
						if (xDistance < yDistance) {
							PinkGhostState = PinkGhostStates.Up;
						}
						if (xDistance == yDistance) {
							if (Random.Range (0, 2) == 0) {
								PinkGhostState = PinkGhostStates.Up;
							} else {
								PinkGhostState = PinkGhostStates.Left;
							}
						}
					}
				}
			// pac-man is in the opposite direction. Ghost has to try to circle back towards pac-man.
				if ((pacHorizontalLocation == "right" || pacHorizontalLocation == "same") && (pacVerticalLocation == "down" || pacVerticalLocation == "same")) {
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 1) {
						PinkGhostState = PinkGhostStates.Right;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
						PinkGhostState = PinkGhostStates.Up;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {

						PinkGhostState = PinkGhostStates.Right;
					}
				}
				if ((pacHorizontalLocation == "left" || pacHorizontalLocation == "same") && (pacVerticalLocation == "down" || pacVerticalLocation == "same")) {
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 1) {
						PinkGhostState = PinkGhostStates.Left;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
						PinkGhostState = PinkGhostStates.Up;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {

						PinkGhostState = PinkGhostStates.Left;

					}
				}
				break;
			case PinkGhostStates.Down:
			// make sure that direction is at the very least set to a possible direction to move
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1) {
					possibleStates.Add (PinkGhostStates.Right);
					//RedGhostState = RedGhostStates.Left;
				}
				if (GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					possibleStates.Add (PinkGhostStates.Down);
					//RedGhostState = RedGhostStates.Up;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
					possibleStates.Add (PinkGhostStates.Left);
					//RedGhostState = RedGhostStates.Down;
				}
				PinkGhostState = possibleStates [Random.Range (0, possibleStates.Count)];

				if ((pacHorizontalLocation == "right" || pacHorizontalLocation == "same") && (pacVerticalLocation == "down" || pacVerticalLocation == "same")) {
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 1) {
						PinkGhostState = PinkGhostStates.Right;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
						PinkGhostState = PinkGhostStates.Down;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
						if (xDistance > yDistance) {
							PinkGhostState = PinkGhostStates.Right;
						}
						if (xDistance < yDistance) {
							PinkGhostState = PinkGhostStates.Down;
						}
						if (xDistance == yDistance) {
							if (Random.Range (0, 2) == 0) {
								PinkGhostState = PinkGhostStates.Down;
							} else {
								PinkGhostState = PinkGhostStates.Right;
							}
						}
					}
				}
				if ((pacHorizontalLocation == "left" || pacHorizontalLocation == "same") && (pacVerticalLocation == "down" || pacVerticalLocation == "same")) {
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 1) {
						PinkGhostState = PinkGhostStates.Left;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
						PinkGhostState = PinkGhostStates.Down;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
						if (xDistance > yDistance) {
							PinkGhostState = PinkGhostStates.Left;
						}
						if (xDistance < yDistance) {
							PinkGhostState = PinkGhostStates.Down;
						}
						if (xDistance == yDistance) {
							if (Random.Range (0, 2) == 0) {
								PinkGhostState = PinkGhostStates.Down;
							} else {
								PinkGhostState = PinkGhostStates.Left;
							}
						}
					}
				}
			// pac-man is in the opposite direction. Ghost has to try to circle back towards pac-man.
				if ((pacHorizontalLocation == "right" || pacHorizontalLocation == "same") && (pacVerticalLocation == "up" || pacVerticalLocation == "same")) {
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 1) {
						PinkGhostState = PinkGhostStates.Right;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
						PinkGhostState = PinkGhostStates.Down;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {

						PinkGhostState = PinkGhostStates.Right;
					}
				}
				if ((pacHorizontalLocation == "left" || pacHorizontalLocation == "same") && (pacVerticalLocation == "up" || pacVerticalLocation == "same")) {
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 1) {
						PinkGhostState = PinkGhostStates.Left;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
						PinkGhostState = PinkGhostStates.Down;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {

						PinkGhostState = PinkGhostStates.Left;
					}
				}
				break;

			}
		} else {
			switch (PinkGhostState) {
			case PinkGhostStates.Left:
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
					if (Random.Range (0, 2) == 0) {
						PinkGhostState = PinkGhostStates.Left;
						break;
					}
				}
				if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1 || GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					if (Random.Range (0, 2) == 0) {
						if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
							PinkGhostState = PinkGhostStates.Up;
						} else {
							PinkGhostState = PinkGhostStates.Down;
						}
					} else {
						if (GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
							PinkGhostState = PinkGhostStates.Down;
						} else {
							PinkGhostState = PinkGhostStates.Up;
						}
					}
				}
				break;
			case PinkGhostStates.Right:
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1) {
					if (Random.Range (0, 2) == 0) {
						PinkGhostState = PinkGhostStates.Right;
						break;
					} 
				}
				if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1 || GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					if (Random.Range (0, 2) == 0) {
						if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
							PinkGhostState = PinkGhostStates.Up;
						} else {
							PinkGhostState = PinkGhostStates.Down;
						}
					} else {
						if (GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
							PinkGhostState = PinkGhostStates.Down;
						} else {
							PinkGhostState = PinkGhostStates.Up;
						}
	
					}
				}
				break;
			case PinkGhostStates.Up:
				if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
					if (Random.Range (0, 2) == 0) {
						PinkGhostState = PinkGhostStates.Up;
						break;
					}
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 || GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
					if (Random.Range (0, 2) == 0) {
						if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1) {
							PinkGhostState = PinkGhostStates.Right;
						} else {
							PinkGhostState = PinkGhostStates.Left;
						}
					} else {
						if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
							PinkGhostState = PinkGhostStates.Left;
						} else {
							PinkGhostState = PinkGhostStates.Right;
						}
					}
				}
				break;
			case PinkGhostStates.Down:
				if (GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					if (Random.Range (0, 2) == 0) {
						PinkGhostState = PinkGhostStates.Down;
						break;
					}
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 || GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
					if (Random.Range (0, 2) == 0) {
						if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1) {
							PinkGhostState = PinkGhostStates.Right;
						} else {
							PinkGhostState = PinkGhostStates.Left;
						}
					} else {
						if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
							PinkGhostState = PinkGhostStates.Left;
						} else {
							PinkGhostState = PinkGhostStates.Right;
						}
					}
				}
				break;


			}

		}
	}

	IEnumerator MoveRight () {
		float distanceTraveled = transform.position.x;
		float endPosition = transform.position.x + DistanceToTravel; 
		while (distanceTraveled < endPosition) {
			distanceTraveled += .08f;
			transform.position = new Vector2(distanceTraveled,transform.position.y);

			yield return new WaitForSeconds (timeStep);
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

			yield return new WaitForSeconds (timeStep);
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

			yield return new WaitForSeconds (timeStep);
		}
		movingDone = true;
	}
	IEnumerator MoveDown () {
		float distanceTraveled = transform.position.y;
		float endPosition = transform.position.y - DistanceToTravel; 

		while (distanceTraveled > endPosition) {
			distanceTraveled -= .08f;
			transform.position = new Vector2(transform.position.x, distanceTraveled);

			yield return new WaitForSeconds (timeStep);
		}
		movingDone = true;
	}
}
