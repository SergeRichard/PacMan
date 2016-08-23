using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public int[,] GridMap;

	public enum States {Intro, Play, PacManDead};

	public static States state;
	public MusicController MusicController;
	public GameObject PacMan;
	public GameObject PacManIntro;
	public MessageController MessageController;
	public GhostController GhostController;
	public PacManController PacManController;
	public Transform RedGhostStart;
	public Transform BlueGhostStart;
	public Transform PinkGhostStart;
	public Transform YellowGhostStart;

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

		MessageController.HighScoreValue.text = HighScore.ToString ();
		MessageController.ScoreValue.text = Score.ToString ();
		MessageController.LivesController.ShowLives (Lives);

		GhostController.DisableGhostRenderer ();

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
		PacManIntro.GetComponent<SpriteRenderer> ().enabled = true;
		PacMan.GetComponent<SpriteRenderer> ().enabled = false;
		MusicController.PlayIntro ();
		Invoke ("IntroDone", MusicController.IntroMusic.length);
		Invoke ("HidePlayerText", 1.75f);
	}
	void HidePlayerText() {
		MessageController.PlayerText.SetActive (false);
		GhostController.EnableGhostRenderer ();
	}
	void IntroDone() {
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
		MusicController.StopAllSounds ();
		Invoke ("PlayDeathSequence", 1f);
	}
	public void PlayDeathSequence() {
		GhostController.DisableGhostRenderer ();
		PacManController.PlayDeathSequence ();
		MusicController.PlayDeathSound ();
	}
	public void ResetLevel() {
		Invoke ("ResetToStartingPoint", 1f);
	}
	public void ResetToStartingPoint() {
		PacManIntro.GetComponent<SpriteRenderer> ().enabled = true;
		GhostController.GhostBlue.GetComponent<Transform>().position = GhostController.GhostBlueStart.position;
		GhostController.GhostRed.GetComponent<Transform>().position = GhostController.GhostRedStart.position;
		GhostController.GhostYellow.GetComponent<Transform>().position = GhostController.GhostYellowStart.position;
		GhostController.GhostPink.GetComponent<Transform>().position = GhostController.GhostPinkStart.position;

		GhostController.EnableGhostRenderer ();
		GhostController.GhostBlue.GetComponent<Animator> ().enabled = true;
		GhostController.GhostRed.GetComponent<Animator> ().enabled = true;
		GhostController.GhostYellow.GetComponent<Animator> ().enabled = true;
		GhostController.GhostPink.GetComponent<Animator> ().enabled = true;

		GhostController.GhostRed.GetComponent<Animator> ().Play ("Idle");
		GhostController.GhostYellow.GetComponent<Animator> ().Play ("Idle");
		GhostController.GhostPink.GetComponent<Animator> ().Play ("Idle");
		GhostController.GhostBlue.GetComponent<Animator> ().Play ("Idle");
	}
}
