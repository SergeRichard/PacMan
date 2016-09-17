using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public int[,] GridMap;
	public int[,] GridMapInitial;

	public enum States {Intro, Play, PacManDead, WonLevel};

	public static States state;

	public GameObject LevelBackgroundWhite;
	public MusicController MusicController;
	public GameObject PacMan;
	public GameObject PacManIntro;
	public MessageController MessageController;
	public GhostController GhostController;
	public PacManController PacManController;
	public LevelManager LevelManager;
	public int Pellets;
	public PelletController PelletController;

	public Transform PacManStartLocation;

	public int Lives;

	[HideInInspector]
	public int Score;

	[HideInInspector]
	public int HighScore;

	// Use this for initialization
	void Start () {
		state = States.Intro;

		Score = 0;
		HighScore = 0;
		Lives = 5;
		Pellets = 0;

		MessageController.HighScoreValue.text = HighScore.ToString ();
		MessageController.ScoreValue.text = Score.ToString ();
		MessageController.LivesController.ShowLives (Lives);
		MessageController.GameOverText.gameObject.SetActive(false);

		GhostController.DisableGhost ();
		GhostController.GhostYellow.GetComponent<YellowGhost> ().SetOutOfBoxTimer ();

		GridMapInitial = new int[,]{
			{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
			{1,1,3,3,3,3,3,3,3,3,3,3,3,3,1,1,3,3,3,3,3,3,3,3,3,3,3,3,1,1},
			{1,1,3,1,1,1,1,3,1,1,1,1,1,3,1,1,3,1,1,1,1,1,3,1,1,1,1,3,1,1},
			{1,1,4,1,1,1,1,3,1,1,1,1,1,3,1,1,3,1,1,1,1,1,3,1,1,1,1,4,1,1},
			{1,1,3,1,1,1,1,3,1,1,1,1,1,3,1,1,3,1,1,1,1,1,3,1,1,1,1,3,1,1},
			{1,1,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,1,1},
			{1,1,3,1,1,1,1,3,1,1,3,1,1,1,1,1,1,1,1,3,1,1,3,1,1,1,1,3,1,1},
			{1,1,3,1,1,1,1,3,1,1,3,1,1,1,1,1,1,1,1,3,1,1,3,1,1,1,1,3,1,1},
			{1,1,3,3,3,3,3,3,1,1,3,3,3,3,1,1,3,3,3,3,1,1,3,3,3,3,3,3,1,1},
			{1,1,1,1,1,1,1,3,1,1,1,1,1,0,1,1,0,1,1,1,1,1,3,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,3,1,1,1,1,1,0,1,1,0,1,1,1,1,1,3,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,3,1,1,0,0,0,0,0,0,0,0,0,0,1,1,3,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,3,1,1,0,1,1,1,1,1,1,1,1,0,1,1,3,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,3,1,1,0,1,1,1,1,1,1,1,1,0,1,1,3,1,1,1,1,1,1,1},
			{2,0,0,0,0,0,0,3,0,0,0,1,1,1,1,1,1,1,1,0,0,0,3,0,0,0,0,0,0,2},
			{1,1,1,1,1,1,1,3,1,1,0,1,1,1,1,1,1,1,1,0,1,1,3,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,3,1,1,0,1,1,1,1,1,1,1,1,0,1,1,3,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,3,1,1,0,0,0,0,0,0,0,0,0,0,1,1,3,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,3,1,1,0,1,1,1,1,1,1,1,1,0,1,1,3,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,3,1,1,0,1,1,1,1,1,1,1,1,0,1,1,3,1,1,1,1,1,1,1},
			{1,1,3,3,3,3,3,3,3,3,3,3,3,3,1,1,3,3,3,3,3,3,3,3,3,3,3,3,1,1},
			{1,1,3,1,1,1,1,3,1,1,1,1,1,3,1,1,3,1,1,1,1,1,3,1,1,1,1,3,1,1},
			{1,1,3,1,1,1,1,3,1,1,1,1,1,3,1,1,3,1,1,1,1,1,3,1,1,1,1,3,1,1},
			{1,1,4,3,3,1,1,3,3,3,3,3,3,3,0,0,3,3,3,3,3,3,3,1,1,3,3,4,1,1},
			{1,1,1,1,3,1,1,3,1,1,3,1,1,1,1,1,1,1,1,3,1,1,3,1,1,3,1,1,1,1},
			{1,1,1,1,3,1,1,3,1,1,3,1,1,1,1,1,1,1,1,3,1,1,3,1,1,3,1,1,1,1},
			{1,1,3,3,3,3,3,3,1,1,3,3,3,3,1,1,3,3,3,3,1,1,3,3,3,3,3,3,1,1},
			{1,1,3,1,1,1,1,1,1,1,1,1,1,3,1,1,3,1,1,1,1,1,1,1,1,1,1,3,1,1},
			{1,1,3,1,1,1,1,1,1,1,1,1,1,3,1,1,3,1,1,1,1,1,1,1,1,1,1,3,1,1},
			{1,1,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,1,1},
			{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
		};

		GridMap = new int[,]{
			{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
			{1,1,3,3,3,3,3,3,3,3,3,3,3,3,1,1,3,3,3,3,3,3,3,3,3,3,3,3,1,1},
			{1,1,3,1,1,1,1,3,1,1,1,1,1,3,1,1,3,1,1,1,1,1,3,1,1,1,1,3,1,1},
			{1,1,4,1,1,1,1,3,1,1,1,1,1,3,1,1,3,1,1,1,1,1,3,1,1,1,1,4,1,1},
			{1,1,3,1,1,1,1,3,1,1,1,1,1,3,1,1,3,1,1,1,1,1,3,1,1,1,1,3,1,1},
			{1,1,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,1,1},
			{1,1,3,1,1,1,1,3,1,1,3,1,1,1,1,1,1,1,1,3,1,1,3,1,1,1,1,3,1,1},
			{1,1,3,1,1,1,1,3,1,1,3,1,1,1,1,1,1,1,1,3,1,1,3,1,1,1,1,3,1,1},
			{1,1,3,3,3,3,3,3,1,1,3,3,3,3,1,1,3,3,3,3,1,1,3,3,3,3,3,3,1,1},
			{1,1,1,1,1,1,1,3,1,1,1,1,1,0,1,1,0,1,1,1,1,1,3,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,3,1,1,1,1,1,0,1,1,0,1,1,1,1,1,3,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,3,1,1,0,0,0,0,0,0,0,0,0,0,1,1,3,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,3,1,1,0,1,1,1,1,1,1,1,1,0,1,1,3,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,3,1,1,0,1,1,1,1,1,1,1,1,0,1,1,3,1,1,1,1,1,1,1},
			{2,0,0,0,0,0,0,3,0,0,0,1,1,1,1,1,1,1,1,0,0,0,3,0,0,0,0,0,0,2},
			{1,1,1,1,1,1,1,3,1,1,0,1,1,1,1,1,1,1,1,0,1,1,3,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,3,1,1,0,1,1,1,1,1,1,1,1,0,1,1,3,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,3,1,1,0,0,0,0,0,0,0,0,0,0,1,1,3,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,3,1,1,0,1,1,1,1,1,1,1,1,0,1,1,3,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,3,1,1,0,1,1,1,1,1,1,1,1,0,1,1,3,1,1,1,1,1,1,1},
			{1,1,3,3,3,3,3,3,3,3,3,3,3,3,1,1,3,3,3,3,3,3,3,3,3,3,3,3,1,1},
			{1,1,3,1,1,1,1,3,1,1,1,1,1,3,1,1,3,1,1,1,1,1,3,1,1,1,1,3,1,1},
			{1,1,3,1,1,1,1,3,1,1,1,1,1,3,1,1,3,1,1,1,1,1,3,1,1,1,1,3,1,1},
			{1,1,4,3,3,1,1,3,3,3,3,3,3,3,0,0,3,3,3,3,3,3,3,1,1,3,3,4,1,1},
			{1,1,1,1,3,1,1,3,1,1,3,1,1,1,1,1,1,1,1,3,1,1,3,1,1,3,1,1,1,1},
			{1,1,1,1,3,1,1,3,1,1,3,1,1,1,1,1,1,1,1,3,1,1,3,1,1,3,1,1,1,1},
			{1,1,3,3,3,3,3,3,1,1,3,3,3,3,1,1,3,3,3,3,1,1,3,3,3,3,3,3,1,1},
			{1,1,3,1,1,1,1,1,1,1,1,1,1,3,1,1,3,1,1,1,1,1,1,1,1,1,1,3,1,1},
			{1,1,3,1,1,1,1,1,1,1,1,1,1,3,1,1,3,1,1,1,1,1,1,1,1,1,1,3,1,1},
			{1,1,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,1,1},
			{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
		};
		PlayIntroMusic ();
	}
	void PlayIntroMusic() {
		CancelInvoke ();
		PacManIntro.GetComponent<SpriteRenderer> ().enabled = true;
		PacMan.GetComponent<SpriteRenderer> ().enabled = false;
		MusicController.PlayIntro ();
		Invoke ("IntroDone", MusicController.IntroMusic.length);
		Invoke ("HidePlayerText", 1.75f);
	}
	void HidePlayerText() {
		MessageController.PlayerText.SetActive (false);
		GhostController.EnableGhost ();
	}
	void IntroDone() {
		GhostController.StartTimer ();

		PacMan.GetComponent<SpriteRenderer> ().enabled = true;
		PacManIntro.GetComponent<SpriteRenderer> ().enabled = false;
		state = States.Play;
		MessageController.GetReadyText.SetActive (false);
		MusicController.PlaySirenSound ();
	}
	// Update is called once per frame
	void Update () {
	
	}
	public void PacManDead() {
		CancelInvoke ();
		MusicController.StopAllSounds ();
		GhostController.SetUpInitial (false);
		Invoke ("PlayDeathSequence", 1f);
	}
	public void PlayDeathSequence() {
		GhostController.DisableGhost ();
		PacManController.PlayDeathSequence ();
		MusicController.PlayDeathSound ();
	}
	public void ResetLevel() {
		CancelInvoke ();
		Invoke ("StartingPoint", 1f);
	}
	void PauseBeforeTitleScreen() {
		MessageController.GameOverText.gameObject.SetActive(true);
		Invoke ("GoToTitleScreen", 2f);
	}
	void GoToTitleScreen() {
		LevelManager.LoadLevel ("TitleScene");

	}
	public void LevelWon() {
		state = States.WonLevel;
		GhostController.DisableGhost ();
		MusicController.StopAllSounds ();
		PacManController.StopAllCoroutines ();
		PacManController.GetComponent<Animator> ().enabled = false;
		GhostController.SetUpInitial ();
		Invoke ("BlinkScreen", 2f);
	}
	public void BlinkScreen() {
		StartCoroutine (StartBlinkingScreen ());

	}

	IEnumerator StartBlinkingScreen() {

		for (int t = 0; t < 4; t++) {
			LevelBackgroundWhite.GetComponent<SpriteRenderer> ().enabled = false;
			yield return new WaitForSeconds (.25f);
			LevelBackgroundWhite.GetComponent<SpriteRenderer> ().enabled = true;
			yield return new WaitForSeconds (.25f);
		}
		LevelBackgroundWhite.GetComponent<SpriteRenderer> ().enabled = false;


		ResetToStartingPoint (true);
	}
	private void ResetPelletsOnGrid() {
		for (int row = 0; row < 31; row++) {
			for (int col = 0; col < 30; col++) {
				GridMap [row, col] = GridMapInitial [row, col];
			}
		}
		Pellets = 0;
	}
	public void StartingPoint() {
		ResetToStartingPoint (false);
	}
	public void ResetToStartingPoint(bool wonLevel) {

		CancelInvoke ();
		if (!wonLevel) {
			Lives--;
			MessageController.LivesController.ShowLives (Lives);
		}
		if (Lives == 0) {
			PauseBeforeTitleScreen ();
		} else {
			MessageController.GetReadyText.gameObject.SetActive (true);
			PacManIntro.GetComponent<SpriteRenderer> ().enabled = true;
			PacMan.GetComponent<SpriteRenderer> ().enabled = false;
			PacMan.GetComponent<Animator> ().enabled = false;
			GhostController.GhostBlue.GetComponent<Transform> ().position = GhostController.GhostBlueStart.position;
			GhostController.GhostRed.GetComponent<Transform> ().position = GhostController.GhostRedStart.position;
			GhostController.GhostYellow.GetComponent<Transform> ().position = GhostController.GhostYellowStart.position;
			GhostController.GhostPink.GetComponent<Transform> ().position = GhostController.GhostPinkStart.position;

			GhostController.EnableGhost ();
			GhostController.GhostBlue.GetComponent<Animator> ().enabled = true;
			GhostController.GhostRed.GetComponent<Animator> ().enabled = true;
			GhostController.GhostYellow.GetComponent<Animator> ().enabled = true;
			GhostController.GhostPink.GetComponent<Animator> ().enabled = true;

			GhostController.GhostRed.GetComponent<Animator> ().Play ("Idle");
			GhostController.GhostYellow.GetComponent<Animator> ().Play ("Idle");
			GhostController.GhostPink.GetComponent<Animator> ().Play ("Idle");
			GhostController.GhostBlue.GetComponent<Animator> ().Play ("Idle");


			PacManController.ResetLocation ();

			if (wonLevel) {
				PelletController.ResetPellet ();
				ResetPelletsOnGrid ();
			}

			Invoke ("StartGhostSequence", 2f);
		}
	}
	void StartGhostSequence() {
		GhostController.StopAllCoroutines ();
		GhostController.StartTimer ();

		MessageController.GetReadyText.gameObject.SetActive (false);
		MusicController.PlaySirenSound ();
		state = States.Play;
		PacManIntro.GetComponent<SpriteRenderer> ().enabled = false;
		PacMan.GetComponent<SpriteRenderer> ().enabled = true;
		PacMan.GetComponent<Animator> ().enabled = true;
		GhostController.GhostBlue.GetComponent<BlueGhost> ().StartIdleUpAndDownSequence (10f);
		GhostController.GhostYellow.GetComponent<YellowGhost> ().StartIdleUpAndDownSequence (15f);

		GhostController.GhostPink.GetComponent<PinkGhost> ().StartIdleUpAndDownSequence (5f);
		GhostController.GhostRed.GetComponent<RedGhost> ().StartMoving ();

	}
}
