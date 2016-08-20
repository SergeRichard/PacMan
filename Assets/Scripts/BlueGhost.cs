using UnityEngine;
using System.Collections;

public class BlueGhost : MonoBehaviour {

	Animator animator;

	public enum BlueGhostStates {Idle, IdleUpAndDown, IdleDown, Up, Down, Left, Right};

	public static BlueGhostStates BlueGhostState;

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
		BlueGhostState = BlueGhostStates.IdleUpAndDown;
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
		if (movingDone && GameManager.state == GameManager.States.Play && BlueGhostState == BlueGhostStates.IdleUpAndDown) {
			movingDone = false;
			StartCoroutine(MoveUpAndDownInBox ());
		}
//		if (movingDone && GameManager.state == GameManager.States.Play) {
//			if (BlueGhostState == BlueGhostStates.Right && GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1) {
//				SetRight ();
//			} else if (BlueGhostState == BlueGhostStates.Left && GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
//				SetLeft ();
//			} else if (BlueGhostState == BlueGhostStates.Up && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
//				SetUp ();
//			} else if (BlueGhostState == BlueGhostStates.Down && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
//				SetDown ();
//			} else {
//				animator.enabled = false;
//				//GameManager.MusicController.StopWakaSound ();
//			}
//		}
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
		BlueGhostState = BlueGhostStates.Right;
		animator.Play ("PacManMovesRight");
		movingDone = false;
		StartCoroutine (MoveRight ());
		colOnGrid++;

		GetComponent<SpriteRenderer> ().sprite = RightSprite;
	}
	void SetLeft() {
		animator.enabled = true;
		animator.Play ("PacManMovesLeft");
		BlueGhostState = BlueGhostStates.Left;
		movingDone = false;
		StartCoroutine (MoveLeft ());
		colOnGrid--;
		GetComponent<SpriteRenderer> ().sprite = LeftSprite;
	}
	void SetUp() {
		animator.enabled = true;
		animator.Play ("PacManMovesUp");
		BlueGhostState = BlueGhostStates.Up;
		movingDone = false;
		StartCoroutine (MoveUp ());
		rowOnGrid--;
		GetComponent<SpriteRenderer> ().sprite = UpSprite;
	}
	void SetDown() {
		animator.enabled = true;
		animator.Play ("PacManMovesDown");
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
