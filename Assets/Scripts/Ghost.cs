using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ghost : MonoBehaviour {

	public enum GhostStates
	{
		Scatter, Chase
	}
	public enum FrightenedStates
	{
		Frightened, FrightenedBlinking, NotFrightened, Eaten
	}
	public enum IndGhostStates {
		Idle, 
		IdleUpAndDown, 
		MoveOutOfBox, 
		MoveIntoBox,
		IdleDown, 
		Up, 
		Down, 
		Left, 
		Right
	}

	public GhostController GhostController;
	public PacManController PacManController;
	public Transform LeftBank;
	public Transform RightBank;

	public GameManager GameManager;

	public static GhostStates GhostState;
	public FrightenedStates FrightenedState;
	public IndGhostStates IndGhostState;
	public GameObject ScoreText;

	protected bool movingDone;
	protected int rowOnGrid;
	protected int colOnGrid;

	protected int rowOnGridStart;
	protected int colOnGridStart;

	public Sprite RightSprite;
	public Sprite LeftSprite;
	public Sprite UpSprite;
	public Sprite DownSprite;

	public float DistanceToTravel = .32f;
	public float TimeStep = .01f;
	protected float timeStep;

	protected Animator animator;

	private float modeTimer;
	private float frightenedTimer;

	class GhostMode
	{
		public GhostStates state;
		public float time;
	}

	Queue<GhostMode> ghostModes;

	// Use this for initialization
	protected virtual void Start () {
		timeStep = TimeStep;
		GhostState = GhostStates.Scatter;
		animator = GetComponent<Animator> ();
		movingDone = true;
		FrightenedState = FrightenedStates.NotFrightened;

		ghostModes = new Queue<GhostMode> ();

		GhostController.PacManController.ChangeGhostToFrightenedState += ChangeGhostToFrightenedState;
		PacManController.LevelWon += LevelWon;
		PacManController.PacManDead += PacManDead;

		SetUpInitial ();

	}
	public void SetTimeStep(float newTime) {
		timeStep = newTime;
	}
	void PacManDead ()
	{
		frightenedTimer = 0;
		timeStep = TimeStep;
		FrightenedState = FrightenedStates.NotFrightened;
	}

	void LevelWon ()
	{
		SetUpInitial ();
	}
	public void SetUpInitial(bool newLevel = true) {
		Ghost.GhostState = Ghost.GhostStates.Scatter;
		frightenedTimer = 0;
		timeStep = TimeStep;

		if (newLevel == true) {
			modeTimer = 0;


			ghostModes.Clear ();

			GhostMode mode = new GhostMode ();
			mode.state = Ghost.GhostStates.Scatter;
			mode.time = 7f; // start right away
			ghostModes.Enqueue (mode);

			GhostMode mode2 = new GhostMode ();
			mode2.state = Ghost.GhostStates.Chase;
			mode2.time = 20f;
			ghostModes.Enqueue (mode2);

			GhostMode mode3 = new GhostMode ();
			mode3.state = Ghost.GhostStates.Scatter;
			mode3.time = 7f;
			ghostModes.Enqueue (mode3);

			GhostMode mode4 = new GhostMode ();
			mode4.state = Ghost.GhostStates.Chase;
			mode4.time = 20f;
			ghostModes.Enqueue (mode4);

			GhostMode mode5 = new GhostMode ();
			mode5.state = Ghost.GhostStates.Scatter;
			mode5.time = 5f;
			ghostModes.Enqueue (mode5);
		}

	}
	void ChangeGhostToFrightenedState ()
	{
		frightenedTimer = 0;
		if (FrightenedState != FrightenedStates.Eaten) {
			animator.enabled = true;
			animator.Play ("GhostFrightened");
			timeStep = GhostController.GhostFrightenedTimeStep;
			FrightenedState = FrightenedStates.Frightened;

			// change direction
			GhostStateHasChanged ();
		}
	}
	void GhostStateHasChanged ()
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
	public virtual void Update () {
		// TODO clear and re-add ghost modes. 

		if (GameManager.state == GameManager.States.Play && FrightenedState != FrightenedStates.Frightened && FrightenedState != FrightenedStates.FrightenedBlinking) {
			modeTimer += Time.deltaTime;
			if (ghostModes.Count > 0) {
				if (modeTimer >= ghostModes.Peek ().time) {
					ghostModes.Dequeue ();
					GhostStateHasChanged ();
					if (ghostModes.Count > 0) {
						GhostState = ghostModes.Peek ().state;
					} else {
						GhostState = GhostStates.Chase;
					}

					modeTimer = 0;
				}
			} else {
				GhostState = GhostStates.Chase;
				modeTimer = 0;
			}
		}
		if (FrightenedState == FrightenedStates.Frightened && GameManager.state == GameManager.States.Play) {
			frightenedTimer += Time.deltaTime;
			if (frightenedTimer >= 6) {
				frightenedTimer = 0;
				FrightenedState = FrightenedStates.FrightenedBlinking;
				//FrightenedBlinking ();
				animator.Play ("FrightenedBlinking");
			}
		}
		if (FrightenedState == FrightenedStates.FrightenedBlinking && GameManager.state == GameManager.States.Play) {
			frightenedTimer += Time.deltaTime;
			if (frightenedTimer >= 4) {
				frightenedTimer = 0;
				if (ghostModes.Count > 0) {
					GhostState = ghostModes.Peek ().state;
				} else {
					GhostState = GhostStates.Chase;
				}
				//GhostLeftFrightenedState ();
				timeStep = TimeStep;
				FrightenedState = FrightenedStates.NotFrightened;
			}

		}	
	}
	public void SetRight() {
		animator.enabled = true;
		IndGhostState = IndGhostStates.Right;
		if (FrightenedState == Ghost.FrightenedStates.Frightened) {
			animator.Play ("GhostFrightened");
		} else if (FrightenedState == Ghost.FrightenedStates.FrightenedBlinking) { 
			animator.Play ("FrightenedBlinking");
		} else if (FrightenedState == FrightenedStates.Eaten) {
			animator.Play ("EyesMovesRight");
		} else {
			animator.Play ("MoveRight");
		}

		movingDone = false;
		StartCoroutine (MoveRight ());
		colOnGrid++;

		GetComponent<SpriteRenderer> ().sprite = RightSprite;
	}
	public void SetLeft() {
		animator.enabled = true;
		IndGhostState = IndGhostStates.Left;
		if (FrightenedState == Ghost.FrightenedStates.Frightened) {
			animator.Play ("GhostFrightened");
		} else if (FrightenedState == Ghost.FrightenedStates.FrightenedBlinking) { 
			animator.Play ("FrightenedBlinking");
		} else if (FrightenedState == FrightenedStates.Eaten) {
			animator.Play ("EyesMovesLeft");
		} else {
			animator.Play ("MoveLeft");
		}
		movingDone = false;
		StartCoroutine (MoveLeft ());
		colOnGrid--;
		GetComponent<SpriteRenderer> ().sprite = LeftSprite;
	}
	public void SetUp() {
		animator.enabled = true;

		IndGhostState = IndGhostStates.Up;
		if (FrightenedState == Ghost.FrightenedStates.Frightened) {
			animator.Play ("GhostFrightened");
		} else if (FrightenedState == Ghost.FrightenedStates.FrightenedBlinking) { 
			animator.Play ("FrightenedBlinking");
		} else if (FrightenedState == FrightenedStates.Eaten) {
			animator.Play ("EyesMovesUp");
		} else {
			animator.Play ("MoveUp");
		}
		movingDone = false;
		StartCoroutine (MoveUp ());
		rowOnGrid--;
		GetComponent<SpriteRenderer> ().sprite = UpSprite;
	}
	public void SetDown() {
		animator.enabled = true;

		IndGhostState = IndGhostStates.Down;
		if (FrightenedState == Ghost.FrightenedStates.Frightened) {
			animator.Play ("GhostFrightened");
		} else if (FrightenedState == Ghost.FrightenedStates.FrightenedBlinking) { 
			animator.Play ("FrightenedBlinking");
		} else if (FrightenedState == FrightenedStates.Eaten) {
			animator.Play ("EyesMovesDown");
		} else {
			animator.Play ("MoveDown");
		}
		movingDone = false;
		StartCoroutine (MoveDown ());
		rowOnGrid++;
		GetComponent<SpriteRenderer> ().sprite = DownSprite;
	}
	public void StartIdleUpAndDownSequence(float timeToStayInBox) {
		CancelInvoke ();
		StopAllCoroutines ();
		rowOnGrid = rowOnGridStart;
		colOnGrid = colOnGridStart;
		IndGhostState = IndGhostStates.IdleUpAndDown;
		movingDone = true;
		Invoke ("StartMovingOutOfBox", timeToStayInBox);
	}
	void StartMovingOutOfBox() {
		IndGhostState = IndGhostStates.MoveOutOfBox;
	}
	public IEnumerator MoveIntoBox() {	
		SetTimeStep (.025f);

		float timeMulti = 2f;

		animator.enabled = true;

		animator.Play ("EyesMovesDown");

		float distanceTraveled = transform.position.y;
		float endPosition = transform.position.y + (DistanceToTravel * -3f); 

		transform.position = new Vector2 (transform.position.x + .15f, transform.position.y);

		while (distanceTraveled > endPosition) {
			distanceTraveled -= .08f;
			transform.position = new Vector2(transform.position.x, distanceTraveled);

			yield return new WaitForSeconds (timeStep * timeMulti);
		}
		GetComponent<SpriteRenderer> ().enabled = true;
		GetComponent<BoxCollider2D> ().enabled = true;

		FrightenedState = FrightenedStates.NotFrightened;

		StartCoroutine (MoveUpAndOutOfBox ());

	}
	public IEnumerator MoveUpAndOutOfBox() {	

		float timeMulti = 2f;

		animator.enabled = true;

		if (FrightenedState == Ghost.FrightenedStates.Frightened) {
			animator.Play ("GhostFrightened");
		} else if (FrightenedState == Ghost.FrightenedStates.FrightenedBlinking) { 
			animator.Play ("FrightenedBlinking");
		} else if (FrightenedState == FrightenedStates.Eaten) {
			animator.Play ("EyesMovesUp");
		} else {
			animator.Play ("MoveUp");
		}


		float distanceTraveled = transform.position.y;
		float endPosition = transform.position.y + (DistanceToTravel * 3f); 

		while (distanceTraveled < endPosition) {
			distanceTraveled += .08f;
			transform.position = new Vector2(transform.position.x, distanceTraveled);

			yield return new WaitForSeconds (timeStep * timeMulti);
		}
		IndGhostState = IndGhostStates.Left;
		GetComponent<Transform> ().position = GhostController.GhostStartLocation.position;
		movingDone = true;

	}
	public IEnumerator MoveOutOfBox(bool rightAndUp = true) {
		
		float distanceTraveled = transform.position.x;
		float endPosition;
		float timeMulti = 2f;

		if (rightAndUp) {
			endPosition = transform.position.x + (DistanceToTravel * 2f); 




			animator.enabled = true;
			if (FrightenedState == Ghost.FrightenedStates.Frightened) {
				animator.Play ("GhostFrightened");
			} else if (FrightenedState == Ghost.FrightenedStates.FrightenedBlinking) { 
				animator.Play ("FrightenedBlinking");
			} else if (FrightenedState == FrightenedStates.Eaten) {
				animator.Play ("EyesMovesRight");
			} else {
				animator.Play ("MoveRight");
			}


			while (distanceTraveled < endPosition) {
				distanceTraveled += .08f;
				transform.position = new Vector2 (distanceTraveled, transform.position.y);

				yield return new WaitForSeconds (timeStep * timeMulti);
			}
		
		} else {
			endPosition = transform.position.x + (DistanceToTravel * -2f); 

			animator.enabled = true;
			if (FrightenedState == Ghost.FrightenedStates.Frightened) {
				animator.Play ("GhostFrightened");
			} else if (FrightenedState == Ghost.FrightenedStates.FrightenedBlinking) { 
				animator.Play ("FrightenedBlinking");
			} else if (FrightenedState == FrightenedStates.Eaten) {
				animator.Play ("EyesMovesLeft");
			} else {
				animator.Play ("MoveLeft");
			}


			while (distanceTraveled > endPosition) {
				distanceTraveled -= .08f;
				transform.position = new Vector2 (distanceTraveled, transform.position.y);

				yield return new WaitForSeconds (timeStep * timeMulti);
			}

		}
		if (FrightenedState == Ghost.FrightenedStates.Frightened) {
			animator.Play ("GhostFrightened");
		} else if (FrightenedState == Ghost.FrightenedStates.FrightenedBlinking) { 
			animator.Play ("FrightenedBlinking");
		} else if (FrightenedState == FrightenedStates.Eaten) {
			animator.Play ("EyesMovesUp");
		} else {
			animator.Play ("MoveUp");
		}

		distanceTraveled = transform.position.y;
		endPosition = transform.position.y + (DistanceToTravel * 3f); 

		while (distanceTraveled < endPosition) {
			distanceTraveled += .08f;
			transform.position = new Vector2(transform.position.x, distanceTraveled);

			yield return new WaitForSeconds (timeStep * timeMulti);
		}
		IndGhostState = IndGhostStates.Left;
		GetComponent<Transform> ().position = GhostController.GhostStartLocation.position;
		movingDone = true;
	}
	public IEnumerator MoveUpAndDownInBox() {
		float distanceTraveled = transform.position.y;
		float endPosition = transform.position.y + (DistanceToTravel / 2f);

		float timeMulti = 3f;

		animator.enabled = true;
		if (FrightenedState == Ghost.FrightenedStates.Frightened) {
			animator.Play ("GhostFrightened");
		} else if (FrightenedState == Ghost.FrightenedStates.FrightenedBlinking) { 
			animator.Play ("FrightenedBlinking");
		} else if (FrightenedState == FrightenedStates.Eaten) {
			animator.Play ("EyesMovesUp");
		} else {
			animator.Play ("MoveUp");
		}


		while (distanceTraveled < endPosition) {
			distanceTraveled += .08f;
			transform.position = new Vector2(transform.position.x, distanceTraveled);

			yield return new WaitForSeconds (timeStep * timeMulti);
		}
		endPosition = transform.position.y - DistanceToTravel;

		animator.enabled = true;
		if (FrightenedState == Ghost.FrightenedStates.Frightened) {
			animator.Play ("GhostFrightened");
		} else if (FrightenedState == Ghost.FrightenedStates.FrightenedBlinking) { 
			animator.Play ("FrightenedBlinking");
		} else if (FrightenedState == FrightenedStates.Eaten) {
			animator.Play ("EyesMovesDown");
		} else {
			animator.Play ("MoveDown");
		}


		while (distanceTraveled > endPosition) {
			distanceTraveled -= .08f;
			transform.position = new Vector2(transform.position.x, distanceTraveled);

			yield return new WaitForSeconds (timeStep * timeMulti);
		}
		endPosition = transform.position.y + (DistanceToTravel / 2f);

		animator.enabled = true;
		if (FrightenedState == Ghost.FrightenedStates.Frightened) {
			animator.Play ("GhostFrightened");
		} else if (FrightenedState == Ghost.FrightenedStates.FrightenedBlinking) { 
			animator.Play ("FrightenedBlinking");
		} else if (FrightenedState == FrightenedStates.Eaten) {
			animator.Play ("EyesMovesUp");
		} else {
			animator.Play ("MoveUp");
		}


		while (distanceTraveled < endPosition) {
			distanceTraveled += .08f;
			transform.position = new Vector2(transform.position.x, distanceTraveled);

			yield return new WaitForSeconds (timeStep * timeMulti);
		}

		movingDone = true;

	}

	public IEnumerator MoveRight () {
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
	public IEnumerator MoveLeft () {
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
	public IEnumerator MoveUp () {
		float distanceTraveled = transform.position.y;
		float endPosition = transform.position.y + DistanceToTravel; 

		while (distanceTraveled < endPosition) {
			distanceTraveled += .08f;
			transform.position = new Vector2(transform.position.x, distanceTraveled);

			yield return new WaitForSeconds (timeStep);
		}
		movingDone = true;
	}
	public IEnumerator MoveDown () {
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
