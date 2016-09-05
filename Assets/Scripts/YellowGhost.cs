using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class YellowGhost : Ghost {

	Animator animator;

	public enum YellowGhostStates {Idle,IdleUpAndDown, MoveOutOfBox, Up, Down, Left, Right};

	public static YellowGhostStates YellowGhostState;

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
	protected override void Start () {
		base.Start ();
		animator = GetComponent<Animator> ();
		YellowGhostState = YellowGhostStates.IdleUpAndDown;
		movingDone = true;
		Invoke ("StartMovingOutOfBox", 16f);
		//transform.position = LeftLocation.transform.position;
		GhostController.GhostStateHasChanged += YellowGhost_GhostStateHasChanged;
	}

	void YellowGhost_GhostStateHasChanged ()
	{
		if (YellowGhostState == YellowGhostStates.Left) {
			YellowGhostState = YellowGhostStates.Right;
		} else if (YellowGhostState == YellowGhostStates.Right) {
			YellowGhostState = YellowGhostStates.Left;
		} else if (YellowGhostState == YellowGhostStates.Up) {
			YellowGhostState = YellowGhostStates.Down;
		} else if (YellowGhostState == YellowGhostStates.Down) {
			YellowGhostState = YellowGhostStates.Up;
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
		if (movingDone && GameManager.state == GameManager.States.Play && YellowGhostState == YellowGhostStates.IdleUpAndDown) {
			movingDone = false;
			StartCoroutine(MoveUpAndDownInBox ());
		}
		if (movingDone && GameManager.state == GameManager.States.Play && YellowGhostState == YellowGhostStates.MoveOutOfBox) {
			movingDone = false;
			StartCoroutine(MoveOutOfBox ());
		}
		if (GameManager.state == GameManager.States.PacManDead) {
			animator.enabled = false;
		}
		if (movingDone && GameManager.state == GameManager.States.Play && YellowGhostState != YellowGhostStates.MoveOutOfBox) {
			ChangeDirection();
			switch (YellowGhostState) {
			case YellowGhostStates.Up:
				SetUp ();
				break;
			case YellowGhostStates.Down:
				SetDown ();
				break;
			case YellowGhostStates.Right:
				SetRight ();
				break;
			case YellowGhostStates.Left:
				SetLeft ();
				break;

			}
		}


	}
	public void SetOutOfBoxTimer() {
		Invoke ("StartMovingOutOfBox", 16f);
	}
	public void StartIdleUpAndDownSequence(float timeToStayInBox) {
		CancelInvoke ();
		StopAllCoroutines ();
		rowOnGrid = rowOnGridStart;
		colOnGrid = colOnGridStart;
		YellowGhostState = YellowGhostStates.IdleUpAndDown;
		movingDone = true;
		Invoke ("StartMovingOutOfBox", timeToStayInBox);
	}
	void StartMovingOutOfBox() {
		YellowGhostState = YellowGhostStates.MoveOutOfBox;
	}
	IEnumerator MoveOutOfBox() {
		float distanceTraveled = transform.position.x;
		float endPosition = transform.position.x - (DistanceToTravel * 2f); 

		float timeMulti = 2f;

		animator.enabled = true;
		animator.Play ("MoveLeft");

		while (distanceTraveled > endPosition) {
			distanceTraveled -= .08f;
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
		YellowGhostState = YellowGhostStates.Left;
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
		int homeX = 2;
		int homeY = 29;

		string pacVerticalLocation = "same";
		string pacHorizontalLocation = "same";

		int pacManX = GameManager.PacManController.ColOnGrid;
		int pacManY = GameManager.PacManController.RowOnGrid;

		int ghostX = colOnGrid;
		int ghostY = rowOnGrid;

		List<YellowGhostStates> possibleStates = new List<YellowGhostStates>();

		int xDistance = Mathf.Abs (ghostX - pacManX);
		int yDistance = Mathf.Abs (ghostY - pacManY);

		// if pac man is closer than 8 squares away to pac-man, go back to safe space (home) until pac-man back to 8 squares away, than chase again
		if (xDistance <= 8 && yDistance <= 8) {
			pacManX = homeX;
			pacManY = homeY;

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



		switch (YellowGhostState) {
		case YellowGhostStates.Left:
			// make sure that direction is at the very least set to a possible direction to move
			if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
				possibleStates.Add (YellowGhostStates.Left);
				//RedGhostState = RedGhostStates.Left;
			}
			if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
				possibleStates.Add (YellowGhostStates.Up);
				//RedGhostState = RedGhostStates.Up;
			}
			if (GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
				possibleStates.Add (YellowGhostStates.Down);
				//RedGhostState = RedGhostStates.Down;
			}
			YellowGhostState = possibleStates [Random.Range (0, possibleStates.Count)];

			if ((pacHorizontalLocation == "left" || pacHorizontalLocation == "same") && (pacVerticalLocation == "up" || pacVerticalLocation == "same")) {
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 1) {
					YellowGhostState = YellowGhostStates.Left;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
					YellowGhostState = YellowGhostStates.Up;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
					if (xDistance > yDistance) {
						YellowGhostState = YellowGhostStates.Left;
					}
					if (xDistance < yDistance) {
						YellowGhostState = YellowGhostStates.Up;
					}
					if (xDistance == yDistance) {
						if (Random.Range (0, 2) == 0) {
							YellowGhostState = YellowGhostStates.Up;
						} else {
							YellowGhostState = YellowGhostStates.Left;
						}
					}
				}
			}
			if ((pacHorizontalLocation == "left" || pacHorizontalLocation == "same") && (pacVerticalLocation == "down" || pacVerticalLocation == "same")) {
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 1) {
					YellowGhostState = YellowGhostStates.Left;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					YellowGhostState = YellowGhostStates.Down;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					if (xDistance > yDistance) {
						YellowGhostState = YellowGhostStates.Left;
					}
					if (xDistance < yDistance) {
						YellowGhostState = YellowGhostStates.Down;
					}
					if (xDistance == yDistance) {
						if (Random.Range (0, 2) == 0) {
							YellowGhostState = YellowGhostStates.Down;
						} else {
							YellowGhostState = YellowGhostStates.Left;
						}
					}
				}
			}
			// pac-man is in the opposite direction. Ghost has to try to circle back towards pac-man.
			if ((pacHorizontalLocation == "right" || pacHorizontalLocation == "same") && (pacVerticalLocation == "up" || pacVerticalLocation == "same")) {
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 1) {
					YellowGhostState = YellowGhostStates.Left;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
					YellowGhostState = YellowGhostStates.Up;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {					

					YellowGhostState = YellowGhostStates.Up;

				}
			}
			if ((pacHorizontalLocation == "right" || pacHorizontalLocation == "same") && (pacVerticalLocation == "down" || pacVerticalLocation == "same")) {
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 1) {
					YellowGhostState = YellowGhostStates.Left;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					YellowGhostState = YellowGhostStates.Down;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {

					YellowGhostState = YellowGhostStates.Down;

				}
			}
			break;
		case YellowGhostStates.Right:
			// make sure that direction is at the very least set to a possible direction to move
			if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1) {
				possibleStates.Add (YellowGhostStates.Right);
				//RedGhostState = RedGhostStates.Left;
			}
			if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
				possibleStates.Add (YellowGhostStates.Up);
				//RedGhostState = RedGhostStates.Up;
			}
			if (GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
				possibleStates.Add (YellowGhostStates.Down);
				//RedGhostState = RedGhostStates.Down;
			}
			YellowGhostState = possibleStates [Random.Range (0, possibleStates.Count)];

			if ((pacHorizontalLocation == "right" || pacHorizontalLocation == "same") && (pacVerticalLocation == "up" || pacVerticalLocation == "same")) {
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 1) {
					YellowGhostState = YellowGhostStates.Right;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
					YellowGhostState = YellowGhostStates.Up;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
					if (xDistance > yDistance) {
						YellowGhostState = YellowGhostStates.Right;
					}
					if (xDistance < yDistance) {
						YellowGhostState = YellowGhostStates.Up;
					}
					if (xDistance == yDistance) {
						if (Random.Range (0, 2) == 0) {
							YellowGhostState = YellowGhostStates.Up;
						} else {
							YellowGhostState = YellowGhostStates.Right;
						}
					}
				}
			}
			if ((pacHorizontalLocation == "right" || pacHorizontalLocation == "same") && (pacVerticalLocation == "down" || pacVerticalLocation == "same")) {
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 1) {
					YellowGhostState = YellowGhostStates.Right;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					YellowGhostState = YellowGhostStates.Down;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					if (xDistance > yDistance) {
						YellowGhostState = YellowGhostStates.Right;
					}
					if (xDistance < yDistance) {
						YellowGhostState = YellowGhostStates.Down;
					}
					if (xDistance == yDistance) {
						if (Random.Range (0, 2) == 0) {
							YellowGhostState = YellowGhostStates.Down;
						} else {
							YellowGhostState = YellowGhostStates.Right;
						}
					}
				}
			}
			// pac-man is in the opposite direction. Ghost has to try to circle back towards pac-man.
			if ((pacHorizontalLocation == "left" || pacHorizontalLocation == "same") && (pacVerticalLocation == "up" || pacVerticalLocation == "same")) {
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 1) {
					YellowGhostState = YellowGhostStates.Right;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
					YellowGhostState = YellowGhostStates.Up;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {

					YellowGhostState = YellowGhostStates.Up;

				}
			}
			if ((pacHorizontalLocation == "left" || pacHorizontalLocation == "same") && (pacVerticalLocation == "down" || pacVerticalLocation == "same")) {
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 1) {
					YellowGhostState = YellowGhostStates.Right;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					YellowGhostState = YellowGhostStates.Down;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {	
					YellowGhostState = YellowGhostStates.Down;					
				}
			}
			break;
		case YellowGhostStates.Up:
			// make sure that direction is at the very least set to a possible direction to move
			if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1) {
				possibleStates.Add (YellowGhostStates.Right);
				//RedGhostState = RedGhostStates.Left;
			}
			if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
				possibleStates.Add (YellowGhostStates.Up);
				//RedGhostState = RedGhostStates.Up;
			}
			if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
				possibleStates.Add (YellowGhostStates.Left);
				//RedGhostState = RedGhostStates.Down;
			}
			YellowGhostState = possibleStates [Random.Range (0, possibleStates.Count)];

			if ((pacHorizontalLocation == "right" || pacHorizontalLocation == "same") && (pacVerticalLocation == "up" || pacVerticalLocation == "same")) {
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 1) {
					YellowGhostState = YellowGhostStates.Right;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
					YellowGhostState = YellowGhostStates.Up;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
					if (xDistance > yDistance) {
						YellowGhostState = YellowGhostStates.Right;
					}
					if (xDistance < yDistance) {
						YellowGhostState = YellowGhostStates.Up;
					}
					if (xDistance == yDistance) {
						if (Random.Range (0, 2) == 0) {
							YellowGhostState = YellowGhostStates.Up;
						} else {
							YellowGhostState = YellowGhostStates.Right;
						}
					}
				}
			}
			if ((pacHorizontalLocation == "left" || pacHorizontalLocation == "same") && (pacVerticalLocation == "up" || pacVerticalLocation == "same")) {
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 1) {
					YellowGhostState = YellowGhostStates.Left;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
					YellowGhostState = YellowGhostStates.Up;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
					if (xDistance > yDistance) {
						YellowGhostState = YellowGhostStates.Left;
					}
					if (xDistance < yDistance) {
						YellowGhostState = YellowGhostStates.Up;
					}
					if (xDistance == yDistance) {
						if (Random.Range (0, 2) == 0) {
							YellowGhostState = YellowGhostStates.Up;
						} else {
							YellowGhostState = YellowGhostStates.Left;
						}
					}
				}
			}
			// pac-man is in the opposite direction. Ghost has to try to circle back towards pac-man.
			if ((pacHorizontalLocation == "right" || pacHorizontalLocation == "same") && (pacVerticalLocation == "down" || pacVerticalLocation == "same")) {
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 1) {
					YellowGhostState = YellowGhostStates.Right;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
					YellowGhostState = YellowGhostStates.Up;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {

					YellowGhostState = YellowGhostStates.Right;
				}
			}
			if ((pacHorizontalLocation == "left" || pacHorizontalLocation == "same") && (pacVerticalLocation == "down" || pacVerticalLocation == "same")) {
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 1) {
					YellowGhostState = YellowGhostStates.Left;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
					YellowGhostState = YellowGhostStates.Up;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {

					YellowGhostState = YellowGhostStates.Left;

				}
			}
			break;
		case YellowGhostStates.Down:
			// make sure that direction is at the very least set to a possible direction to move
			if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1) {
				possibleStates.Add (YellowGhostStates.Right);
				//RedGhostState = RedGhostStates.Left;
			}
			if (GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
				possibleStates.Add (YellowGhostStates.Down);
				//RedGhostState = RedGhostStates.Up;
			}
			if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
				possibleStates.Add (YellowGhostStates.Left);
				//RedGhostState = RedGhostStates.Down;
			}
			YellowGhostState = possibleStates [Random.Range (0, possibleStates.Count)];

			if ((pacHorizontalLocation == "right" || pacHorizontalLocation == "same") && (pacVerticalLocation == "down" || pacVerticalLocation == "same")) {
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 1) {
					YellowGhostState = YellowGhostStates.Right;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					YellowGhostState = YellowGhostStates.Down;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					if (xDistance > yDistance) {
						YellowGhostState = YellowGhostStates.Right;
					}
					if (xDistance < yDistance) {
						YellowGhostState = YellowGhostStates.Down;
					}
					if (xDistance == yDistance) {
						if (Random.Range (0, 2) == 0) {
							YellowGhostState = YellowGhostStates.Down;
						} else {
							YellowGhostState = YellowGhostStates.Right;
						}
					}
				}
			}
			if ((pacHorizontalLocation == "left" || pacHorizontalLocation == "same") && (pacVerticalLocation == "down" || pacVerticalLocation == "same")) {
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 1) {
					YellowGhostState = YellowGhostStates.Left;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					YellowGhostState = YellowGhostStates.Down;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					if (xDistance > yDistance) {
						YellowGhostState = YellowGhostStates.Left;
					}
					if (xDistance < yDistance) {
						YellowGhostState = YellowGhostStates.Down;
					}
					if (xDistance == yDistance) {
						if (Random.Range (0, 2) == 0) {
							YellowGhostState = YellowGhostStates.Down;
						} else {
							YellowGhostState = YellowGhostStates.Left;
						}
					}
				}
			}
			// pac-man is in the opposite direction. Ghost has to try to circle back towards pac-man.
			if ((pacHorizontalLocation == "right" || pacHorizontalLocation == "same") && (pacVerticalLocation == "up" || pacVerticalLocation == "same")) {
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 1) {
					YellowGhostState = YellowGhostStates.Right;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					YellowGhostState = YellowGhostStates.Down;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {

					YellowGhostState = YellowGhostStates.Right;
				}
			}
			if ((pacHorizontalLocation == "left" || pacHorizontalLocation == "same") && (pacVerticalLocation == "up" || pacVerticalLocation == "same")) {
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 1) {
					YellowGhostState = YellowGhostStates.Left;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					YellowGhostState = YellowGhostStates.Down;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {

					YellowGhostState = YellowGhostStates.Left;
				}
			}
			break;
//		case YellowGhostStates.Left:
//			if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
//				if (Random.Range (0, 2) == 0) {
//					YellowGhostState = YellowGhostStates.Left;
//					break;
//				}
//			}
//			if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1 || GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
//				if (Random.Range (0, 2) == 0) {
//					if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
//						YellowGhostState = YellowGhostStates.Up;
//					} else {
//						YellowGhostState = YellowGhostStates.Down;
//					}
//				} else {
//					if (GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
//						YellowGhostState = YellowGhostStates.Down;
//					} else {
//						YellowGhostState = YellowGhostStates.Up;
//					}
//				}
//			}
//			break;
//		case YellowGhostStates.Right:
//			if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1) {
//				if (Random.Range (0, 2) == 0) {
//					YellowGhostState = YellowGhostStates.Right;
//					break;
//				} 
//			}
//			if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1 || GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
//				if (Random.Range (0, 2) == 0) {
//					if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
//						YellowGhostState = YellowGhostStates.Up;
//					} else {
//						YellowGhostState = YellowGhostStates.Down;
//					}
//				} else {
//					if (GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
//						YellowGhostState = YellowGhostStates.Down;
//					} else {
//						YellowGhostState = YellowGhostStates.Up;
//					}
//
//				}
//			}
//			break;
//		case YellowGhostStates.Up:
//			if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
//				if (Random.Range (0, 2) == 0) {
//					YellowGhostState = YellowGhostStates.Up;
//					break;
//				}
//			}
//			if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 || GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
//				if (Random.Range (0, 2) == 0) {
//					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1) {
//						YellowGhostState = YellowGhostStates.Right;
//					} else {
//						YellowGhostState = YellowGhostStates.Left;
//					}
//				} else {
//					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
//						YellowGhostState = YellowGhostStates.Left;
//					} else {
//						YellowGhostState = YellowGhostStates.Right;
//					}
//				}
//			}
//			break;
//		case YellowGhostStates.Down:
//			if (GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
//				if (Random.Range (0, 2) == 0) {
//					YellowGhostState = YellowGhostStates.Down;
//					break;
//				}
//			}
//			if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 || GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
//				if (Random.Range (0, 2) == 0) {
//					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1) {
//						YellowGhostState = YellowGhostStates.Right;
//					} else {
//						YellowGhostState = YellowGhostStates.Left;
//					}
//				} else {
//					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
//						YellowGhostState = YellowGhostStates.Left;
//					} else {
//						YellowGhostState = YellowGhostStates.Right;
//					}
//				}
//			}
//			break;
		}

	}
	void SetRight() {
		animator.enabled = true;
		YellowGhostState = YellowGhostStates.Right;
		animator.Play ("MoveRight");
		movingDone = false;
		StartCoroutine (MoveRight ());
		colOnGrid++;

		GetComponent<SpriteRenderer> ().sprite = RightSprite;
	}
	void SetLeft() {
		animator.enabled = true;
		animator.Play ("MoveLeft");
		YellowGhostState = YellowGhostStates.Left;
		movingDone = false;
		StartCoroutine (MoveLeft ());
		colOnGrid--;
		GetComponent<SpriteRenderer> ().sprite = LeftSprite;
	}
	void SetUp() {
		animator.enabled = true;
		animator.Play ("MoveUp");
		YellowGhostState = YellowGhostStates.Up;
		movingDone = false;
		StartCoroutine (MoveUp ());
		rowOnGrid--;
		GetComponent<SpriteRenderer> ().sprite = UpSprite;
	}
	void SetDown() {
		animator.enabled = true;
		animator.Play ("MoveDown");
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
