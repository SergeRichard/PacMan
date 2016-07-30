using UnityEngine;
using System.Collections;

public class PacManController : MonoBehaviour {

	Animator animator;

	public enum PacManStates {Idle, Up, Down, Left, Right};

	public static PacManStates pacManStates;

	public float DistanceToTravel = .32f;
	public float TimeStep = .05f;
	public GameManager GameManager;

	private int rowOnGrid = 23;
	private int colOnGrid = 13;

	private bool movingDone;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		pacManStates = PacManStates.Idle;
		movingDone = true;

	}
	
	// Update is called once per frame
	void Update () {
		if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && pacManStates != PacManStates.Right) {			
			pacManStates = PacManStates.Right;
			//animator.Play ("PacManMovesRight");
		}
		if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && pacManStates != PacManStates.Left) {
			//animator.Play ("PacManMovesLeft");
			pacManStates = PacManStates.Left;
		}
		if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && pacManStates != PacManStates.Up) {
			//animator.Play ("PacManMovesUp");
			pacManStates = PacManStates.Up;
		}
		if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && pacManStates != PacManStates.Down) {
			//animator.Play ("PacManMovesDown");
			pacManStates = PacManStates.Down;
		}
		Debug.Log (GameManager.GridMap [rowOnGrid, colOnGrid]);
		if (movingDone) {			
			
			CheckState();
		}
	}
	void CheckState() {
		switch (pacManStates) {
		case PacManStates.Up:
			if (GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 0) {
				rowOnGrid--;
				movingDone = false;
				StartCoroutine (MoveUp ());
				animator.Play ("PacManMovesUp");
			}
			break;
		case PacManStates.Down:
			if (GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 0) {
				rowOnGrid++;
				movingDone = false;
				StartCoroutine (MoveDown ());
				animator.Play ("PacManMovesDown");
			}
			break;
		case PacManStates.Left:
			if (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 0) {
				colOnGrid--;
				movingDone = false;
				StartCoroutine (MoveLeft ());
				animator.Play ("PacManMovesLeft");
			}
			break;
		case PacManStates.Right:
			if (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 0) {
				colOnGrid++;
				movingDone = false;
				StartCoroutine (MoveRight ());
				animator.Play ("PacManMovesRight");
			}
			break;
		}

	}


	IEnumerator MoveRight () {
		float distanceTraveled = transform.position.x;
		float endPosition = transform.position.x + DistanceToTravel; 
		while (distanceTraveled < endPosition) {
			distanceTraveled += .1f;
			transform.position = new Vector2(distanceTraveled,transform.position.y);

			yield return new WaitForSeconds (TimeStep);
		}
		movingDone = true;
	}
	IEnumerator MoveLeft () {
		float distanceTraveled = transform.position.x;
		float distanceToTravel = transform.position.x - DistanceToTravel; 
		while (distanceTraveled < distanceToTravel) {
			distanceTraveled -= .1f;
			transform.position = new Vector2(distanceTraveled,transform.position.y);

			yield return new WaitForSeconds (TimeStep);
		}
		movingDone = true;
	}
	IEnumerator MoveUp () {
		float distanceTraveled = transform.position.y;
		float distanceToTravel = transform.position.y + DistanceToTravel; 
		while (distanceTraveled < distanceToTravel) {
			distanceTraveled += .1f;
			transform.position = new Vector2(transform.position.x, distanceTraveled);

			yield return new WaitForSeconds (TimeStep);
		}
		movingDone = true;
	}
	IEnumerator MoveDown () {
		float distanceTraveled = transform.position.y;
		float distanceToTravel = transform.position.y - DistanceToTravel; 
		while (distanceTraveled > distanceToTravel) {
			distanceTraveled -= .1f;
			transform.position = new Vector2(transform.position.x, distanceTraveled);

			yield return new WaitForSeconds (TimeStep);
		}
		movingDone = true;
	}
}
