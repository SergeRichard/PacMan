using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RedGhost : Ghost {

//	Animator animator;
//
//	public enum RedGhostStates {Idle, Up, Down, Left, Right};
//
//	public static RedGhostStates RedGhostState;
//
//
//	public GhostController GhostController;

	//	public Transform LeftLocation;
	//	public Transform RightLocation;

//	public float DistanceToTravel = .32f;
//	public float TimeStep = .025f;
//
//	public Transform LeftBank;
//	public Transform RightBank;
//
//	public GameManager GameManager;
//	public Sprite RightSprite;
//	public Sprite LeftSprite;
//	public Sprite UpSprite;
//	public Sprite DownSprite;

	//	private int rowOnGrid = 23;
	//	private int colOnGrid = 13;
//	private int rowOnGrid = 11;
//	private int colOnGrid = 14;
//
//	private int rowOnGridStart = 11;
//	private int colOnGridStart = 14;
//
//	private bool movingDone;
//	private float timeStep;

	public int RowOnGrid 
	{
		get { return rowOnGrid; }
		set { rowOnGrid = value; }
	}
	public int ColOnGrid
	{
		get { return colOnGrid; }
		set { colOnGrid = value; }
	}

	// Use this for initialization
	protected override void Start () {
		base.Start ();

		GhostController.GhostStateHasChanged += RedGhost_GhostStateHasChanged;
		GhostController.PacManController.ChangeGhostToFrightenedState += ChangeGhostToFrightenedState;
		GhostController.GhostLeftFrightenedState += GhostLeftFrightenedState;
		GhostController.FrightenedBlinking += FrightenedBlinking;
		//transform.position = LeftLocation.transform.position;
	}

	void FrightenedBlinking ()
	{
		animator.Play ("FrightenedBlinking");
	}

	void GhostLeftFrightenedState ()
	{
		timeStep = TimeStep;
		RedGhost_GhostStateHasChanged ();
	}

	void ChangeGhostToFrightenedState ()
	{
		animator.enabled = true;
		animator.Play ("RedGhostFrightened");
		timeStep = GhostController.GhostFrightenedTimeStep;
		// change direction
		RedGhost_GhostStateHasChanged ();
	}


	void RedGhost_GhostStateHasChanged ()
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
		if (GameManager.state == GameManager.States.PacManDead) {
			animator.enabled = false;
		}
		if (movingDone && GameManager.state == GameManager.States.Play) {
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
	public void StartMoving() {
		CancelInvoke ();
		StopAllCoroutines ();
		rowOnGrid = rowOnGridStart;
		colOnGrid = colOnGridStart;
		movingDone = true;
		IndGhostState = IndGhostStates.Left;

	}
	void ChangeDirection() {
		int homeX = 27;
		int homeY = 1;

		string pacVerticalLocation = "same";
		string pacHorizontalLocation = "same";

		int pacManX = GameManager.PacManController.ColOnGrid;
		int pacManY = GameManager.PacManController.RowOnGrid;

		int ghostX = colOnGrid;
		int ghostY = rowOnGrid;

		List<IndGhostStates> possibleStates = new List<IndGhostStates>();

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
//		IndGhostState = RedGhostStates.Right;
//		if (GhostController.GhostState == GhostController.GhostStates.Freightened) {
//			animator.Play ("RedGhostFrightened");
//		} else if (GhostController.GhostState == GhostController.GhostStates.FrightenedBlinking) {
//			animator.Play ("FrightenedBlinking");
//		} else {
//			animator.Play ("RedGhostMovesRight");
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
//
//		if (GhostController.GhostState == GhostController.GhostStates.Freightened) {
//			animator.Play ("RedGhostFrightened");
//		} else if (GhostController.GhostState == GhostController.GhostStates.FrightenedBlinking) {
//			animator.Play ("FrightenedBlinking");
//		} else {
//			animator.Play ("RedGhostMovesLeft");
//		}
//		RedGhostState = RedGhostStates.Left;
//		movingDone = false;
//		StartCoroutine (MoveLeft ());
//		colOnGrid--;
//		GetComponent<SpriteRenderer> ().sprite = LeftSprite;
//	}
//	void SetUp() {
//		animator.enabled = true;
//
//		if (GhostController.GhostState == GhostController.GhostStates.Freightened) {
//			animator.Play ("RedGhostFrightened");
//		} else if (GhostController.GhostState == GhostController.GhostStates.FrightenedBlinking) {
//			animator.Play ("FrightenedBlinking");
//		} else {
//			animator.Play ("RedGhostMovesUp");
//		}
//		RedGhostState = RedGhostStates.Up;
//		movingDone = false;
//		StartCoroutine (MoveUp ());
//		rowOnGrid--;
//		GetComponent<SpriteRenderer> ().sprite = UpSprite;
//	}
//	void SetDown() {
//		animator.enabled = true;
//
//		if (GhostController.GhostState == GhostController.GhostStates.Freightened) {
//			animator.Play ("RedGhostFrightened");
//		} else if (GhostController.GhostState == GhostController.GhostStates.FrightenedBlinking) {
//			animator.Play ("FrightenedBlinking");
//		} else {
//			animator.Play ("RedGhostMovesDown");
//		}
//		RedGhostState = RedGhostStates.Down;
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
