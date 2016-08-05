using UnityEngine;
using System.Collections;

public class PacManController : MonoBehaviour {

	Animator animator;

	public enum PacManStates {Idle, Up, Down, Left, Right};

	public static PacManStates pacManStates;

	public Transform LeftLocation;
	public Transform RightLocation;

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
	private int rowOnGrid = 23;
	private int colOnGrid = 14;

	private bool movingDone;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		pacManStates = PacManStates.Left;
		movingDone = true;
		transform.position = LeftLocation.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.state == GameManager.States.Intro) {
			if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
				transform.position = RightLocation.transform.position;
				colOnGrid = 15;
				pacManStates = PacManStates.Right;
			}
			if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
				transform.position = LeftLocation.transform.position;
				colOnGrid = 14;
				pacManStates = PacManStates.Left;
			}
		}
		if (movingDone && GameManager.state == GameManager.States.Play) {
			if ((Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) && (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 0 || GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 2)) {			
				SetRight ();
			} else if ((Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) && (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 0 || GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 2)) {
				SetLeft ();
			} else if ((Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W)) && GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 0) {
				SetUp ();
			} else if ((Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.S)) && GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 0) {
				SetDown ();
			} else if (pacManStates == PacManStates.Right && (GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 0 || GameManager.GridMap [rowOnGrid, colOnGrid + 1] == 2)) {
				SetRight ();
			} else if (pacManStates == PacManStates.Left && (GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 0 || GameManager.GridMap [rowOnGrid, colOnGrid - 1] == 2)) {
				SetLeft ();
			} else if (pacManStates == PacManStates.Up && GameManager.GridMap [rowOnGrid - 1, colOnGrid] == 0) {
				SetUp ();
			} else if (pacManStates == PacManStates.Down && GameManager.GridMap [rowOnGrid + 1, colOnGrid] == 0) {
				SetDown ();
			} else {
				animator.enabled = false;
			}
		}


	}
	void SetRight() {
		animator.enabled = true;
		pacManStates = PacManStates.Right;
		animator.Play ("PacManMovesRight");
		movingDone = false;
		StartCoroutine (MoveRight ());
		colOnGrid++;
		GetComponent<SpriteRenderer> ().sprite = RightSprite;
	}
	void SetLeft() {
		animator.enabled = true;
		animator.Play ("PacManMovesLeft");
		pacManStates = PacManStates.Left;
		movingDone = false;
		StartCoroutine (MoveLeft ());
		colOnGrid--;
		GetComponent<SpriteRenderer> ().sprite = LeftSprite;
	}
	void SetUp() {
		animator.enabled = true;
		animator.Play ("PacManMovesUp");
		pacManStates = PacManStates.Up;
		movingDone = false;
		StartCoroutine (MoveUp ());
		rowOnGrid--;
		GetComponent<SpriteRenderer> ().sprite = UpSprite;
	}
	void SetDown() {
		animator.enabled = true;
		animator.Play ("PacManMovesDown");
		pacManStates = PacManStates.Down;
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

	void OnTriggerEnter2D(Collider2D other) {
		other.gameObject.SetActive (false);

	}
}
