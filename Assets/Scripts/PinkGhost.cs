using UnityEngine;
using System.Collections;

public class PinkGhost : MonoBehaviour {

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

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		PinkGhostState = PinkGhostStates.MoveOutOfBox;
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
		animator.Play ("MoveUp");

		while (distanceTraveled < endPosition) {
			distanceTraveled += .08f;
			transform.position = new Vector2(transform.position.x, distanceTraveled);

			yield return new WaitForSeconds (TimeStep * timeMulti);
		}
		PinkGhostState = PinkGhostStates.Left;
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
	void SetRight() {
		animator.enabled = true;
		PinkGhostState = PinkGhostStates.Right;
		animator.Play ("MovesRight");
		movingDone = false;
		StartCoroutine (MoveRight ());
		colOnGrid++;

		GetComponent<SpriteRenderer> ().sprite = RightSprite;
	}
	void SetLeft() {
		animator.enabled = true;
		animator.Play ("MovesLeft");
		PinkGhostState = PinkGhostStates.Left;
		movingDone = false;
		StartCoroutine (MoveLeft ());
		colOnGrid--;
		GetComponent<SpriteRenderer> ().sprite = LeftSprite;
	}
	void SetUp() {
		animator.enabled = true;
		animator.Play ("MovesUp");
		PinkGhostState = PinkGhostStates.Up;
		movingDone = false;
		StartCoroutine (MoveUp ());
		rowOnGrid--;
		GetComponent<SpriteRenderer> ().sprite = UpSprite;
	}
	void SetDown() {
		animator.enabled = true;
		animator.Play ("MovesDown");
		PinkGhostState = PinkGhostStates.Down;
		movingDone = false;
		StartCoroutine (MoveDown ());
		rowOnGrid++;
		GetComponent<SpriteRenderer> ().sprite = DownSprite;
	}
	void ChangeDirection() {
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
