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
}
