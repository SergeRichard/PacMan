using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlueGhost : Ghost {

	//Animator animator;

	//public GhostController GhostController;

	//	public Transform LeftLocation;
	//	public Transform RightLocation;



//	public Transform LeftBank;
//	public Transform RightBank;
//
//	public GameManager GameManager;
//	public Sprite RightSprite;
//	public Sprite LeftSprite;
//	public Sprite UpSprite;
//	public Sprite DownSprite;

//	private int rowOnGrid = 11;
//	private int colOnGrid = 14;
//
//	private int rowOnGridStart = 11;
//	private int colOnGridStart = 14;

	//private bool movingDone;

	//private float timeStep;

	// Use this for initialization
	protected override void Start () {
		base.Start ();

		rowOnGrid = 11;
		colOnGrid = 14;

		rowOnGridStart = 11;
		colOnGridStart = 14;

		IndGhostState = IndGhostStates.IdleUpAndDown;

		Invoke ("StartMovingOutOfBox", 8f);
		//transform.position = LeftLocation.transform.position;
		GhostController.GhostStateHasChanged += BlueGhost_GhostStateHasChanged;
		GhostController.PacManController.ChangeGhostToFrightenedState += ChangeGhostToFrightenedState;
		GhostController.GhostLeftFrightenedState += GhostLeftFrightenedState;
		GhostController.FrightenedBlinking += FrightenedBlinking;
	}
	void FrightenedBlinking ()
	{
		animator.Play ("FrightenedBlinking");
	}
	void GhostLeftFrightenedState ()
	{
		timeStep = TimeStep;
		BlueGhost_GhostStateHasChanged ();
	}

	void ChangeGhostToFrightenedState ()
	{
		animator.enabled = true;
		animator.Play ("GhostFrightened");
		timeStep = GhostController.GhostFrightenedTimeStep;
		BlueGhost_GhostStateHasChanged ();
	}

	void BlueGhost_GhostStateHasChanged ()
	{
		if (IndGhostState == IndGhostStates.Left) {
			IndGhostState = IndGhostStates.Right;
		} else if (IndGhostState == IndGhostStates.Right) {
			IndGhostState = IndGhostStates.Left;
		} else if (IndGhostState == IndGhostStates.Up) {
			IndGhostState = IndGhostStates.Down;
		} else if (IndGhostState == IndGhostStates.Down) {
			IndGhostState = IndGhostStates.Up;
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
		if (movingDone && GameManager.state == GameManager.States.Play && IndGhostState == IndGhostStates.IdleUpAndDown) {
			movingDone = false;
			StartCoroutine(MoveUpAndDownInBox ());
		}
		if (movingDone && GameManager.state == GameManager.States.Play && IndGhostState == IndGhostStates.MoveOutOfBox) {
			movingDone = false;
			StartCoroutine(MoveOutOfBox ());
		}
		if (GameManager.state == GameManager.States.PacManDead) {
			animator.enabled = false;
		}
		if (movingDone && GameManager.state == GameManager.States.Play && IndGhostState != IndGhostStates.MoveOutOfBox) {
			ChangeDirection();
			switch (IndGhostState) {
			case IndGhostStates.Up:
				SetUp ();
				break;
			case IndGhostStates.Down:
				SetDown ();
				break;
			case IndGhostStates.Right:
				SetRight ();
				break;
			case IndGhostStates.Left:
				SetLeft ();
				break;

			}
		}
	}
//	public void StartIdleUpAndDownSequence(float timeToStayInBox) {
//		CancelInvoke ();
//		StopAllCoroutines ();
//		rowOnGrid = rowOnGridStart;
//		colOnGrid = colOnGridStart;
//		IndGhostState = IndGhostStates.IdleUpAndDown;
//		movingDone = true;
//		Invoke ("StartMovingOutOfBox", timeToStayInBox);
//	}
//	void StartMovingOutOfBox() {
//		IndGhostState = IndGhostStates.MoveOutOfBox;
//	}
//	IEnumerator MoveOutOfBox() {
//		float distanceTraveled = transform.position.x;
//		float endPosition = transform.position.x + (DistanceToTravel * 2f); 
//
//		float timeMulti = 2f;
//
//		animator.enabled = true;
//		if (FrightenedState == Ghost.FrightenedStates.Frightened) {
//			animator.Play ("GhostFrightened");
//		} else if (FrightenedState == Ghost.FrightenedStates.FrightenedBlinking) { 
//			animator.Play ("FrightenedBlinking");
//		} else {
//			animator.Play ("MoveRight");
//		}
//
//
//		while (distanceTraveled < endPosition) {
//			distanceTraveled += .08f;
//			transform.position = new Vector2(distanceTraveled,transform.position.y);
//
//			yield return new WaitForSeconds (timeStep * timeMulti);
//		}
//		if (FrightenedState == Ghost.FrightenedStates.Frightened) {
//			animator.Play ("GhostFrightened");
//		} else if (FrightenedState == Ghost.FrightenedStates.FrightenedBlinking) { 
//			animator.Play ("FrightenedBlinking");
//		} else {
//			animator.Play ("MoveUp");
//		}
//
//
//		distanceTraveled = transform.position.y;
//		endPosition = transform.position.y + (DistanceToTravel * 3f); 
//
//		while (distanceTraveled < endPosition) {
//			distanceTraveled += .08f;
//			transform.position = new Vector2(transform.position.x, distanceTraveled);
//
//			yield return new WaitForSeconds (timeStep * timeMulti);
//		}
//		IndGhostState = IndGhostStates.Left;
//		GetComponent<Transform> ().position = GhostController.GhostStartLocation.position;
//		movingDone = true;
//	}
//	IEnumerator MoveUpAndDownInBox() {
//		float distanceTraveled = transform.position.y;
//		float endPosition = transform.position.y + (DistanceToTravel / 2f);
//
//		float timeMulti = 3f;
//
//		animator.enabled = true;
//		if (FrightenedState == Ghost.FrightenedStates.Frightened) {
//			animator.Play ("GhostFrightened");
//		} else if (FrightenedState == Ghost.FrightenedStates.FrightenedBlinking) { 
//			animator.Play ("FrightenedBlinking");
//		} else {
//			animator.Play ("MoveUp");
//		}
//
//
//		while (distanceTraveled < endPosition) {
//			distanceTraveled += .08f;
//			transform.position = new Vector2(transform.position.x, distanceTraveled);
//
//			yield return new WaitForSeconds (timeStep * timeMulti);
//		}
//		endPosition = transform.position.y - DistanceToTravel;
//
//		animator.enabled = true;
//		if (FrightenedState == Ghost.FrightenedStates.Frightened) {
//			animator.Play ("GhostFrightened");
//		} else if (FrightenedState == Ghost.FrightenedStates.FrightenedBlinking) { 
//			animator.Play ("FrightenedBlinking");
//		} else {
//			animator.Play ("MoveDown");
//		}
//
//
//		while (distanceTraveled > endPosition) {
//			distanceTraveled -= .08f;
//			transform.position = new Vector2(transform.position.x, distanceTraveled);
//
//			yield return new WaitForSeconds (timeStep * timeMulti);
//		}
//		endPosition = transform.position.y + (DistanceToTravel / 2f);
//
//		animator.enabled = true;
//		if (FrightenedState == Ghost.FrightenedStates.Frightened) {
//			animator.Play ("GhostFrightened");
//		} else if (FrightenedState == Ghost.FrightenedStates.FrightenedBlinking) { 
//			animator.Play ("FrightenedBlinking");
//		} else {
//			animator.Play ("MoveUp");
//		}
//
//
//		while (distanceTraveled < endPosition) {
//			distanceTraveled += .08f;
//			transform.position = new Vector2(transform.position.x, distanceTraveled);
//
//			yield return new WaitForSeconds (timeStep * timeMulti);
//		}
//
//		movingDone = true;
//
//	}
	void ChangeDirection() {
		int homeX = 27;
		int homeY = 29;

		string pacVerticalLocation = "same";
		string pacHorizontalLocation = "same";

		int pacManX = GameManager.PacManController.ColOnGrid;
		int pacManY = GameManager.PacManController.RowOnGrid;

		int ghostX = colOnGrid;
		int ghostY = rowOnGrid;

		List<IndGhostStates> possibleStates = new List<IndGhostStates>();

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

		if (FrightenedState != Ghost.FrightenedStates.Frightened && FrightenedState != Ghost.FrightenedStates.FrightenedBlinking) {
			switch (IndGhostState) {
			case IndGhostStates.Left:
			// make sure that direction is at the very least set to a possible direction to move
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
					possibleStates.Add (IndGhostStates.Left);
					//RedGhostState = RedGhostStates.Left;
				}
				if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
					possibleStates.Add (IndGhostStates.Up);
					//RedGhostState = RedGhostStates.Up;
				}
				if (GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					possibleStates.Add (IndGhostStates.Down);
					//RedGhostState = RedGhostStates.Down;
				}
				IndGhostState = possibleStates [Random.Range (0, possibleStates.Count)];

				if ((pacHorizontalLocation == "left" || pacHorizontalLocation == "same") && (pacVerticalLocation == "up" || pacVerticalLocation == "same")) {
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 1) {
						IndGhostState = IndGhostStates.Left;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
						IndGhostState = IndGhostStates.Up;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
						if (xDistance > yDistance) {
							IndGhostState = IndGhostStates.Left;
						}
						if (xDistance < yDistance) {
							IndGhostState = IndGhostStates.Up;
						}
						if (xDistance == yDistance) {
							if (Random.Range (0, 2) == 0) {
								IndGhostState = IndGhostStates.Up;
							} else {
								IndGhostState = IndGhostStates.Left;
							}
						}
					}
				}
				if ((pacHorizontalLocation == "left" || pacHorizontalLocation == "same") && (pacVerticalLocation == "down" || pacVerticalLocation == "same")) {
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 1) {
						IndGhostState = IndGhostStates.Left;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
						IndGhostState = IndGhostStates.Down;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
						if (xDistance > yDistance) {
							IndGhostState = IndGhostStates.Left;
						}
						if (xDistance < yDistance) {
							IndGhostState = IndGhostStates.Down;
						}
						if (xDistance == yDistance) {
							if (Random.Range (0, 2) == 0) {
								IndGhostState = IndGhostStates.Down;
							} else {
								IndGhostState = IndGhostStates.Left;
							}
						}
					}
				}
			// pac-man is in the opposite direction. Ghost has to try to circle back towards pac-man.
				if ((pacHorizontalLocation == "right" || pacHorizontalLocation == "same") && (pacVerticalLocation == "up" || pacVerticalLocation == "same")) {
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 1) {
						IndGhostState = IndGhostStates.Left;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
						IndGhostState = IndGhostStates.Up;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {					

						IndGhostState = IndGhostStates.Up;

					}
				}
				if ((pacHorizontalLocation == "right" || pacHorizontalLocation == "same") && (pacVerticalLocation == "down" || pacVerticalLocation == "same")) {
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 1) {
						IndGhostState = IndGhostStates.Left;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
						IndGhostState = IndGhostStates.Down;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {

						IndGhostState = IndGhostStates.Down;

					}
				}
				break;
			case IndGhostStates.Right:
			// make sure that direction is at the very least set to a possible direction to move
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1) {
					possibleStates.Add (IndGhostStates.Right);
					//RedGhostState = RedGhostStates.Left;
				}
				if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
					possibleStates.Add (IndGhostStates.Up);
					//RedGhostState = RedGhostStates.Up;
				}
				if (GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					possibleStates.Add (IndGhostStates.Down);
					//RedGhostState = RedGhostStates.Down;
				}
				IndGhostState = possibleStates [Random.Range (0, possibleStates.Count)];

				if ((pacHorizontalLocation == "right" || pacHorizontalLocation == "same") && (pacVerticalLocation == "up" || pacVerticalLocation == "same")) {
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 1) {
						IndGhostState = IndGhostStates.Right;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
						IndGhostState = IndGhostStates.Up;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
						if (xDistance > yDistance) {
							IndGhostState = IndGhostStates.Right;
						}
						if (xDistance < yDistance) {
							IndGhostState = IndGhostStates.Up;
						}
						if (xDistance == yDistance) {
							if (Random.Range (0, 2) == 0) {
								IndGhostState = IndGhostStates.Up;
							} else {
								IndGhostState = IndGhostStates.Right;
							}
						}
					}
				}
				if ((pacHorizontalLocation == "right" || pacHorizontalLocation == "same") && (pacVerticalLocation == "down" || pacVerticalLocation == "same")) {
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 1) {
						IndGhostState = IndGhostStates.Right;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
						IndGhostState = IndGhostStates.Down;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
						if (xDistance > yDistance) {
							IndGhostState = IndGhostStates.Right;
						}
						if (xDistance < yDistance) {
							IndGhostState = IndGhostStates.Down;
						}
						if (xDistance == yDistance) {
							if (Random.Range (0, 2) == 0) {
								IndGhostState = IndGhostStates.Down;
							} else {
								IndGhostState = IndGhostStates.Right;
							}
						}
					}
				}
			// pac-man is in the opposite direction. Ghost has to try to circle back towards pac-man.
				if ((pacHorizontalLocation == "left" || pacHorizontalLocation == "same") && (pacVerticalLocation == "up" || pacVerticalLocation == "same")) {
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 1) {
						IndGhostState = IndGhostStates.Right;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
						IndGhostState = IndGhostStates.Up;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {

						IndGhostState = IndGhostStates.Up;

					}
				}
				if ((pacHorizontalLocation == "left" || pacHorizontalLocation == "same") && (pacVerticalLocation == "down" || pacVerticalLocation == "same")) {
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 1) {
						IndGhostState = IndGhostStates.Right;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
						IndGhostState = IndGhostStates.Down;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {	
						IndGhostState = IndGhostStates.Down;					
					}
				}
				break;
			case IndGhostStates.Up:
			// make sure that direction is at the very least set to a possible direction to move
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1) {
					possibleStates.Add (IndGhostStates.Right);
					//RedGhostState = RedGhostStates.Left;
				}
				if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
					possibleStates.Add (IndGhostStates.Up);
					//RedGhostState = RedGhostStates.Up;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
					possibleStates.Add (IndGhostStates.Left);
					//RedGhostState = RedGhostStates.Down;
				}
				IndGhostState = possibleStates [Random.Range (0, possibleStates.Count)];

				if ((pacHorizontalLocation == "right" || pacHorizontalLocation == "same") && (pacVerticalLocation == "up" || pacVerticalLocation == "same")) {
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 1) {
						IndGhostState = IndGhostStates.Right;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
						IndGhostState = IndGhostStates.Up;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
						if (xDistance > yDistance) {
							IndGhostState = IndGhostStates.Right;
						}
						if (xDistance < yDistance) {
							IndGhostState = IndGhostStates.Up;
						}
						if (xDistance == yDistance) {
							if (Random.Range (0, 2) == 0) {
								IndGhostState = IndGhostStates.Up;
							} else {
								IndGhostState = IndGhostStates.Right;
							}
						}
					}
				}
				if ((pacHorizontalLocation == "left" || pacHorizontalLocation == "same") && (pacVerticalLocation == "up" || pacVerticalLocation == "same")) {
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 1) {
						IndGhostState = IndGhostStates.Left;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
						IndGhostState = IndGhostStates.Up;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
						if (xDistance > yDistance) {
							IndGhostState = IndGhostStates.Left;
						}
						if (xDistance < yDistance) {
							IndGhostState = IndGhostStates.Up;
						}
						if (xDistance == yDistance) {
							if (Random.Range (0, 2) == 0) {
								IndGhostState = IndGhostStates.Up;
							} else {
								IndGhostState = IndGhostStates.Left;
							}
						}
					}
				}
			// pac-man is in the opposite direction. Ghost has to try to circle back towards pac-man.
				if ((pacHorizontalLocation == "right" || pacHorizontalLocation == "same") && (pacVerticalLocation == "down" || pacVerticalLocation == "same")) {
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 1) {
						IndGhostState = IndGhostStates.Right;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
						IndGhostState = IndGhostStates.Up;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {

						IndGhostState = IndGhostStates.Right;
					}
				}
				if ((pacHorizontalLocation == "left" || pacHorizontalLocation == "same") && (pacVerticalLocation == "down" || pacVerticalLocation == "same")) {
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 1) {
						IndGhostState = IndGhostStates.Left;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
						IndGhostState = IndGhostStates.Up;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {

						IndGhostState = IndGhostStates.Left;

					}
				}
				break;
			case IndGhostStates.Down:
			// make sure that direction is at the very least set to a possible direction to move
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1) {
					possibleStates.Add (IndGhostStates.Right);
					//RedGhostState = RedGhostStates.Left;
				}
				if (GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					possibleStates.Add (IndGhostStates.Down);
					//RedGhostState = RedGhostStates.Up;
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
					possibleStates.Add (IndGhostStates.Left);
					//RedGhostState = RedGhostStates.Down;
				}
				IndGhostState = possibleStates [Random.Range (0, possibleStates.Count)];

				if ((pacHorizontalLocation == "right" || pacHorizontalLocation == "same") && (pacVerticalLocation == "down" || pacVerticalLocation == "same")) {
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 1) {
						IndGhostState = IndGhostStates.Right;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
						IndGhostState = IndGhostStates.Down;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
						if (xDistance > yDistance) {
							IndGhostState = IndGhostStates.Right;
						}
						if (xDistance < yDistance) {
							IndGhostState = IndGhostStates.Down;
						}
						if (xDistance == yDistance) {
							if (Random.Range (0, 2) == 0) {
								IndGhostState = IndGhostStates.Down;
							} else {
								IndGhostState = IndGhostStates.Right;
							}
						}
					}
				}
				if ((pacHorizontalLocation == "left" || pacHorizontalLocation == "same") && (pacVerticalLocation == "down" || pacVerticalLocation == "same")) {
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 1) {
						IndGhostState = IndGhostStates.Left;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
						IndGhostState = IndGhostStates.Down;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
						if (xDistance > yDistance) {
							IndGhostState = IndGhostStates.Left;
						}
						if (xDistance < yDistance) {
							IndGhostState = IndGhostStates.Down;
						}
						if (xDistance == yDistance) {
							if (Random.Range (0, 2) == 0) {
								IndGhostState = IndGhostStates.Down;
							} else {
								IndGhostState = IndGhostStates.Left;
							}
						}
					}
				}
			// pac-man is in the opposite direction. Ghost has to try to circle back towards pac-man.
				if ((pacHorizontalLocation == "right" || pacHorizontalLocation == "same") && (pacVerticalLocation == "up" || pacVerticalLocation == "same")) {
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 1) {
						IndGhostState = IndGhostStates.Right;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
						IndGhostState = IndGhostStates.Down;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {

						IndGhostState = IndGhostStates.Right;
					}
				}
				if ((pacHorizontalLocation == "left" || pacHorizontalLocation == "same") && (pacVerticalLocation == "up" || pacVerticalLocation == "same")) {
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 1) {
						IndGhostState = IndGhostStates.Left;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
						IndGhostState = IndGhostStates.Down;
					}
					if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1 && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {

						IndGhostState = IndGhostStates.Left;
					}
				}
				break;

			}
		} else {
			switch (IndGhostState) {
			case IndGhostStates.Left:
				if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
					if (Random.Range (0, 2) == 0) {
						IndGhostState = IndGhostStates.Left;
						break;
					}
				}
				if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1 || GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					if (Random.Range (0, 2) == 0) {
						if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
							IndGhostState = IndGhostStates.Up;
						} else {
							IndGhostState = IndGhostStates.Down;
						}
					} else {
						if (GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
							IndGhostState = IndGhostStates.Down;
						} else {
							IndGhostState = IndGhostStates.Up;
						}
					}
				}
				break;
			case IndGhostStates.Right:
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1) {
					if (Random.Range (0, 2) == 0) {
						IndGhostState = IndGhostStates.Right;
						break;
					} 
				}
				if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1 || GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					if (Random.Range (0, 2) == 0) {
						if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
							IndGhostState = IndGhostStates.Up;
						} else {
							IndGhostState = IndGhostStates.Down;
						}
					} else {
						if (GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
							IndGhostState = IndGhostStates.Down;
						} else {
							IndGhostState = IndGhostStates.Up;
						}
	
					}
				}
				break;
			case IndGhostStates.Up:
				if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
					if (Random.Range (0, 2) == 0) {
						IndGhostState = IndGhostStates.Up;
						break;
					}
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 || GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
					if (Random.Range (0, 2) == 0) {
						if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1) {
							IndGhostState = IndGhostStates.Right;
						} else {
							IndGhostState = IndGhostStates.Left;
						}
					} else {
						if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
							IndGhostState = IndGhostStates.Left;
						} else {
							IndGhostState = IndGhostStates.Right;
						}
					}
				}
				break;
			case IndGhostStates.Down:
				if (GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
					if (Random.Range (0, 2) == 0) {
						IndGhostState = IndGhostStates.Down;
						break;
					}
				}
				if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1 || GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
					if (Random.Range (0, 2) == 0) {
						if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1) {
							IndGhostState = IndGhostStates.Right;
						} else {
							IndGhostState = IndGhostStates.Left;
						}
					} else {
						if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
							IndGhostState = IndGhostStates.Left;
						} else {
							IndGhostState = IndGhostStates.Right;
						}
					}
				}
				break;
			}
		}

	}
//	void SetRight() {
//		animator.enabled = true;
//		IndGhostState = IndGhostStates.Right;
//		if (FrightenedState == Ghost.FrightenedStates.Frightened) {
//			animator.Play ("GhostFrightened");
//		} else if (FrightenedState == Ghost.FrightenedStates.FrightenedBlinking) { 
//			animator.Play ("FrightenedBlinking");
//		} else {
//			animator.Play ("MoveRight");
//		}
//
//		movingDone = false;
//		StartCoroutine (MoveRight ());
//		colOnGrid++;
//
//		GetComponent<SpriteRenderer> ().sprite = RightSprite;
//	}
//	void SetLeft() {
//		animator.enabled = true;
//		IndGhostState = IndGhostStates.Left;
//		if (FrightenedState == Ghost.FrightenedStates.Frightened) {
//			animator.Play ("GhostFrightened");
//		} else if (FrightenedState == Ghost.FrightenedStates.FrightenedBlinking) { 
//			animator.Play ("FrightenedBlinking");
//		} else {
//			animator.Play ("MoveLeft");
//		}
//		movingDone = false;
//		StartCoroutine (MoveLeft ());
//		colOnGrid--;
//		GetComponent<SpriteRenderer> ().sprite = LeftSprite;
//	}
//	void SetUp() {
//		animator.enabled = true;
//
//		IndGhostState = IndGhostStates.Up;
//		if (FrightenedState == Ghost.FrightenedStates.Frightened) {
//			animator.Play ("GhostFrightened");
//		} else if (FrightenedState == Ghost.FrightenedStates.FrightenedBlinking) { 
//			animator.Play ("FrightenedBlinking");
//		} else {
//			animator.Play ("MoveUp");
//		}
//		movingDone = false;
//		StartCoroutine (MoveUp ());
//		rowOnGrid--;
//		GetComponent<SpriteRenderer> ().sprite = UpSprite;
//	}
//	void SetDown() {
//		animator.enabled = true;
//
//		IndGhostState = IndGhostStates.Down;
//		if (FrightenedState == Ghost.FrightenedStates.Frightened) {
//			animator.Play ("GhostFrightened");
//		} else if (FrightenedState == Ghost.FrightenedStates.FrightenedBlinking) { 
//			animator.Play ("FrightenedBlinking");
//		} else {
//			animator.Play ("MoveDown");
//		}
//		movingDone = false;
//		StartCoroutine (MoveDown ());
//		rowOnGrid++;
//		GetComponent<SpriteRenderer> ().sprite = DownSprite;
//	}
//
//
//	IEnumerator MoveRight () {
//		float distanceTraveled = transform.position.x;
//		float endPosition = transform.position.x + DistanceToTravel; 
//		while (distanceTraveled < endPosition) {
//			distanceTraveled += .08f;
//			transform.position = new Vector2(distanceTraveled,transform.position.y);
//
//			yield return new WaitForSeconds (timeStep);
//		}
//		movingDone = true;
//		if (GameManager.GridMap [rowOnGrid, colOnGrid] == 2) {
//			transform.position = LeftBank.position;
//			rowOnGrid = 14;
//			colOnGrid = 1;
//		}
//
//	}
//	IEnumerator MoveLeft () {
//		float distanceTraveled = transform.position.x;
//		float endPosition = transform.position.x - DistanceToTravel; 
//
//		while (distanceTraveled > endPosition) {
//			distanceTraveled -= .08f;
//			transform.position = new Vector2(distanceTraveled,transform.position.y);
//
//			yield return new WaitForSeconds (timeStep);
//		}
//		movingDone = true;
//		if (GameManager.GridMap [rowOnGrid, colOnGrid] == 2) {
//			transform.position = RightBank.position;
//			rowOnGrid = 14;
//			colOnGrid = 28;
//		}
//	}
//	IEnumerator MoveUp () {
//		float distanceTraveled = transform.position.y;
//		float endPosition = transform.position.y + DistanceToTravel; 
//
//		while (distanceTraveled < endPosition) {
//			distanceTraveled += .08f;
//			transform.position = new Vector2(transform.position.x, distanceTraveled);
//
//			yield return new WaitForSeconds (timeStep);
//		}
//		movingDone = true;
//	}
//	IEnumerator MoveDown () {
//		float distanceTraveled = transform.position.y;
//		float endPosition = transform.position.y - DistanceToTravel; 
//
//		while (distanceTraveled > endPosition) {
//			distanceTraveled -= .08f;
//			transform.position = new Vector2(transform.position.x, distanceTraveled);
//
//			yield return new WaitForSeconds (timeStep);
//		}
//		movingDone = true;
//	}

}
