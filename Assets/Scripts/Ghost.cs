using UnityEngine;
using System.Collections;



public class Ghost : MonoBehaviour {



	public enum GhostStates
	{
		Scatter, Chase
	}
	public enum FrightenedStates
	{
		Frightened, FrightenedBlinking, NotFrightened
	}
	public enum IndGhostStates {
		Idle, 
		IdleUpAndDown, 
		MoveOutOfBox, 
		IdleDown, 
		Up, 
		Down, 
		Left, 
		Right
	}

	public GhostController GhostController;
	public Transform LeftBank;
	public Transform RightBank;

	public GameManager GameManager;

	public GhostStates GhostState;
	public FrightenedStates FrightenedState;
	public IndGhostStates IndGhostState;

	public bool movingDone;
	public int rowOnGrid;
	public int colOnGrid;

	public int rowOnGridStart;
	public int colOnGridStart;

	public Sprite RightSprite;
	public Sprite LeftSprite;
	public Sprite UpSprite;
	public Sprite DownSprite;

	public float DistanceToTravel = .32f;
	public float TimeStep = .01f;
	public float timeStep;

	protected Animator animator;

	// Use this for initialization
	protected virtual void Start () {
		timeStep = TimeStep;
		GhostState = GhostStates.Scatter;
		animator = GetComponent<Animator> ();
		movingDone = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void SetRight() {
		animator.enabled = true;
		IndGhostState = IndGhostStates.Right;
		if (FrightenedState == Ghost.FrightenedStates.Frightened) {
			animator.Play ("GhostFrightened");
		} else if (FrightenedState == Ghost.FrightenedStates.FrightenedBlinking) { 
			animator.Play ("FrightenedBlinking");
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
	public IEnumerator MoveOutOfBox() {
		float distanceTraveled = transform.position.x;
		float endPosition = transform.position.x + (DistanceToTravel * 2f); 

		float timeMulti = 2f;

		animator.enabled = true;
		if (FrightenedState == Ghost.FrightenedStates.Frightened) {
			animator.Play ("GhostFrightened");
		} else if (FrightenedState == Ghost.FrightenedStates.FrightenedBlinking) { 
			animator.Play ("FrightenedBlinking");
		} else {
			animator.Play ("MoveRight");
		}


		while (distanceTraveled < endPosition) {
			distanceTraveled += .08f;
			transform.position = new Vector2(distanceTraveled,transform.position.y);

			yield return new WaitForSeconds (timeStep * timeMulti);
		}
		if (FrightenedState == Ghost.FrightenedStates.Frightened) {
			animator.Play ("GhostFrightened");
		} else if (FrightenedState == Ghost.FrightenedStates.FrightenedBlinking) { 
			animator.Play ("FrightenedBlinking");
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
