using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public delegate void ChangeGhostToFrightenedStateEventHandler();
public delegate void PacManDeadEventHandler();
public delegate void LevelWonEventHandler();

public class PacManController : MonoBehaviour {

	public event ChangeGhostToFrightenedStateEventHandler ChangeGhostToFrightenedState;
	public event PacManDeadEventHandler PacManDead;
	public event LevelWonEventHandler LevelWon;

	Animator animator;

	public enum PacManStates {Idle, Up, Down, Left, Right};

	public static PacManStates pacManStates;

	private List<int> numOfGhostEatenScore = new List<int>() {200, 400, 800, 1600};

	private int ghostScoreIndex = 0;

	public Transform LeftLocation;
	public Transform RightLocation;

	public float DistanceToTravel = .32f;
	public float TimeStep = .01f;

	public Transform LeftBank;
	public Transform RightBank;
	public Transform PacManStartLocation;

	public GameManager GameManager;
	public Sprite RightSprite;
	public Sprite LeftSprite;
	public Sprite UpSprite;
	public Sprite DownSprite;
	public GhostController GhostController;

//	private int rowOnGrid = 23;
//	private int colOnGrid = 13;
	private int rowOnGrid = 23;
	private int colOnGrid = 14;

	const int rowOnGridStart = 23;
	const int colOnGridStart = 14;

	private bool movingDone;
	private GameObject ghostEaten;

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
			if ((Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) && GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1) {			
				SetRight ();
			} else if ((Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) && GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
				SetLeft ();
			} else if ((Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W)) && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
				SetUp ();
			} else if ((Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.S)) && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
				SetDown ();
			} else if (pacManStates == PacManStates.Right && GameManager.GridMap [rowOnGrid, colOnGrid + 1] != 1) {
				SetRight ();
			} else if (pacManStates == PacManStates.Left && GameManager.GridMap [rowOnGrid, colOnGrid - 1] != 1) {
				SetLeft ();
			} else if (pacManStates == PacManStates.Up && GameManager.GridMap [rowOnGrid - 1, colOnGrid] != 1) {
				SetUp ();
			} else if (pacManStates == PacManStates.Down && GameManager.GridMap [rowOnGrid + 1, colOnGrid] != 1) {
				SetDown ();
			} else {
				animator.enabled = false;
				GameManager.MusicController.StopWakaSound ();
			}
		}


	}

	public void ResetLocation() {
		CancelInvoke ();
		StopAllCoroutines ();
		rowOnGrid = rowOnGridStart;
		colOnGrid = colOnGridStart;
		pacManStates = PacManStates.Left;
		colOnGrid = 14;
		transform.position = LeftLocation.transform.position;
		movingDone = true;
		//GetComponent<Transform> ().position = PacManStartLocation.position;
	}
	void PlayWakaIfPelletThere() {
		if (GameManager.GridMap [rowOnGrid, colOnGrid] == 3 || GameManager.GridMap [rowOnGrid, colOnGrid] == 4) {
			GameManager.MusicController.PlayWakaSound ();

			if (GameManager.GridMap [rowOnGrid, colOnGrid] == 3)
				AddToAndUpdateScore(10);
			
			if (GameManager.GridMap [rowOnGrid, colOnGrid] == 4)
				AddToAndUpdateScore(50);

			GameManager.GridMap [rowOnGrid, colOnGrid] = 0;
		} else {
			GameManager.MusicController.StopWakaSound ();
		}
	}
	private void AddToAndUpdateScore(int scoreToAdd) {
		GameManager.Score += scoreToAdd;

		if (GameManager.HighScore < GameManager.Score) {
			GameManager.HighScore = GameManager.Score;
			GameManager.MessageController.HighScoreValue.text = GameManager.HighScore.ToString ();
		}
		GameManager.MessageController.ScoreValue.text = GameManager.Score.ToString ();
	}
	public void PlayDeathSequence() {
		// reset to left for restart of level
		pacManStates = PacManStates.Left;
		animator.enabled = true;
		animator.Play ("PacManDeath");
	}
	// event triggered from animator when death animation done
	public void OnEndOfDeathSequence () {
		GetComponent<SpriteRenderer> ().enabled = false;
		GameManager.ResetLevel ();
	}
	void SetRight() {
		animator.enabled = true;
		pacManStates = PacManStates.Right;
		animator.Play ("PacManMovesRight");
		movingDone = false;
		StartCoroutine (MoveRight ());
		colOnGrid++;
		PlayWakaIfPelletThere ();

		GetComponent<SpriteRenderer> ().sprite = RightSprite;
	}
	void SetLeft() {
		animator.enabled = true;
		animator.Play ("PacManMovesLeft");
		pacManStates = PacManStates.Left;
		movingDone = false;
		StartCoroutine (MoveLeft ());
		colOnGrid--;
		PlayWakaIfPelletThere ();
		GetComponent<SpriteRenderer> ().sprite = LeftSprite;
	}
	void SetUp() {
		animator.enabled = true;
		animator.Play ("PacManMovesUp");
		pacManStates = PacManStates.Up;
		movingDone = false;
		StartCoroutine (MoveUp ());
		rowOnGrid--;
		PlayWakaIfPelletThere ();
		GetComponent<SpriteRenderer> ().sprite = UpSprite;
	}
	void SetDown() {
		animator.enabled = true;
		animator.Play ("PacManMovesDown");
		pacManStates = PacManStates.Down;
		movingDone = false;
		StartCoroutine (MoveDown ());
		rowOnGrid++;
		PlayWakaIfPelletThere ();
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
		if (other.tag == "Pellet" || other.tag == "PowerPellet" && GameManager.state != GameManager.States.PacManDead && GameManager.state != GameManager.States.WonLevel) {
			other.gameObject.SetActive (false);
			GameManager.Pellets++;
			if (GameManager.Pellets == 244) {
				GameManager.LevelWon ();
				LevelWon ();
			}

			//GameManager.LevelWon ();
		}
		if (other.tag == "PowerPellet" && GameManager.state != GameManager.States.PacManDead && GameManager.state != GameManager.States.WonLevel) {
			//Ghost.FrightenedState = Ghost.FrightenedStates.Frightened;
			ghostScoreIndex = 0;
			ChangeGhostToFrightenedState ();
		}
		if (other.tag == "Ghost" && GameManager.state != GameManager.States.PacManDead && GameManager.state != GameManager.States.WonLevel && other.gameObject.GetComponent<Ghost>().FrightenedState == Ghost.FrightenedStates.NotFrightened) {
			GameManager.state = GameManager.States.PacManDead;
			animator.enabled = false;
			GameManager.PacManDead ();
			PacManDead ();
		}
		if (other.tag == "Ghost" && GameManager.state != GameManager.States.PacManDead && GameManager.state != GameManager.States.WonLevel && other.gameObject.GetComponent<Ghost>().FrightenedState != Ghost.FrightenedStates.NotFrightened) {
			GameManager.state = GameManager.States.GhostEaten;
			animator.enabled = false;
			other.gameObject.GetComponent<SpriteRenderer> ().enabled = false;
			AddToAndUpdateScore (numOfGhostEatenScore [ghostScoreIndex]);
			other.gameObject.GetComponent<Ghost> ().ScoreText.SetActive (true);
			other.GetComponent<Ghost> ().ScoreText.GetComponent<Text>().text = numOfGhostEatenScore [ghostScoreIndex++].ToString ();

			gameObject.GetComponent<SpriteRenderer> ().enabled = false;
			ghostEaten = other.gameObject;
			ghostEaten.GetComponent<BoxCollider2D> ().enabled = false;

			Invoke ("ResumeAfterEaten", 2f);
		}
	}
	void ResumeAfterEaten() {
		
		animator.enabled = true;

		ghostEaten.GetComponent<Ghost> ().ScoreText.SetActive (false);
		gameObject.GetComponent<SpriteRenderer> ().enabled = true;
		GameManager.state = GameManager.States.Play;
		ghostEaten.GetComponent<Ghost> ().FrightenedState = Ghost.FrightenedStates.Eaten;
		ghostEaten.GetComponent<Animator> ().enabled = true;
		ghostEaten.GetComponent<Animator> ().Play ("EyesMovesLeft");
		ghostEaten.GetComponent<SpriteRenderer> ().enabled = true;
		ghostEaten.GetComponent<Ghost> ().SetTimeStep (.025f);


	}
}
