﻿using UnityEngine;
using System.Collections;

public class TitleMessageController : MonoBehaviour {

	public GameObject RedGhost;
	public GameObject ShadowText;
	public GameObject BlinkyText;
	public GameObject PinkGhost;
	public GameObject SpeedyText;
	public GameObject PinkyText;
	public GameObject BlueGhost;
	public GameObject BashfulText;
	public GameObject InkyText;
	public GameObject YellowGhost;
	public GameObject PokeyText;
	public GameObject ClydeText;
	public GameObject SmallPellet;
	public GameObject PowerPellet;
	public GameObject FirstPowerPellet;
	public GameObject TenPtsText;
	public GameObject FiftyPtsText;
	public GameObject MovePacMan;
	public GameObject MoveRedGhost;
	public GameObject MovePinkGhost;
	public GameObject MoveBlueGhost;
	public GameObject MoveYellowGhost;

	public Sprite PacManSpriteClosedLeft;
	public Sprite PacManSpriteOpenedLeft;
	public Sprite PacManSpriteOpenedWideLeft;

	public Sprite PacManSpriteClosedRight;
	public Sprite PacManSpriteOpenedRight;
	public Sprite PacManSpriteOpenedWideRight;

	public Sprite RedGhostSpriteLeft1;
	public Sprite RedGhostSpriteLeft2;
	public Sprite RedGhostSpriteRight1;
	public Sprite RedGhostSpriteRight2;

	public Sprite PinkGhostSpriteLeft1;
	public Sprite PinkGhostSpriteLeft2;
	public Sprite PinkGhostSpriteRight1;
	public Sprite PinkGhostSpriteRight2;

	public Sprite BlueGhostSpriteLeft1;
	public Sprite BlueGhostSpriteLeft2;
	public Sprite BlueGhostSpriteRight1;
	public Sprite BlueGhostSpriteRight2;

	public Sprite YellowGhostSpriteLeft1;
	public Sprite YellowGhostSpriteLeft2;
	public Sprite YellowGhostSpriteRight1;
	public Sprite YellowGhostSpriteRight2;

	public Animator Animator;

	// Use this for initialization
	void Start () {
		RedGhost.SetActive(false);
		ShadowText.SetActive(false);
		BlinkyText.SetActive(false);
		PinkGhost.SetActive(false);
		SpeedyText.SetActive(false);
		PinkyText.SetActive(false);
		BlueGhost.SetActive(false);
		BashfulText.SetActive(false);
		InkyText.SetActive(false);
		YellowGhost.SetActive(false);
		PokeyText.SetActive(false);
		ClydeText.SetActive(false);
		SmallPellet.SetActive(false);
		FirstPowerPellet.SetActive (false);
		PowerPellet.SetActive(false);
		TenPtsText.SetActive(false);
		FiftyPtsText.SetActive(false);

		StartCoroutine (StartAnimation ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator StartAnimation() {
		// Row 1
		yield return new WaitForSeconds (.5f);
		RedGhost.SetActive (true);

		yield return new WaitForSeconds (1f);
		ShadowText.SetActive (true);

		yield return new WaitForSeconds (.5f);
		BlinkyText.SetActive (true);

		// row 2
		yield return new WaitForSeconds (.5f);
		PinkGhost.SetActive (true);

		yield return new WaitForSeconds (1f);
		SpeedyText.SetActive (true);

		yield return new WaitForSeconds (.5f);
		PinkyText.SetActive (true);

		// row 3
		yield return new WaitForSeconds (.5f);
		BlueGhost.SetActive (true);

		yield return new WaitForSeconds (1f);
		BashfulText.SetActive (true);

		yield return new WaitForSeconds (.5f);
		InkyText.SetActive (true);

		// row 4
		yield return new WaitForSeconds (.5f);
		YellowGhost.SetActive (true);

		yield return new WaitForSeconds (1f);
		PokeyText.SetActive (true);

		yield return new WaitForSeconds (.5f);
		ClydeText.SetActive (true);

		// show bottom
		yield return new WaitForSeconds (1f);
		TenPtsText.SetActive(true);
		FiftyPtsText.SetActive(true);
		PowerPellet.SetActive(true);
		SmallPellet.SetActive(true);

		// show first power pellet
		FirstPowerPellet.SetActive(true);

		// start first power pellet blinking and start ghost and pac-man animation
		yield return new WaitForSeconds (1f);

		StartCoroutine (StartPelletBlinking ());
		//StartCoroutine (PacManAnimWhileMovingLeft ());
		//StartCoroutine (GhostsAnimWhileMovingLeft ());
		Animator.Play ("GhostChasingPacMan");
		Animator.Play ("PacManWalkingLeft", 1);
		Animator.Play ("RedGhostWalkingLeft", 2);
		Animator.Play ("PinkGhostWalkingLeft", 3);
		Animator.Play ("BlueGhostWalkingLeft", 4);
		Animator.Play ("YellowGhostWalkingLeft", 5);
	}
	IEnumerator StartPelletBlinking() {

		while (true) {
			FirstPowerPellet.SetActive(false);

			yield return new WaitForSeconds (.2f);

			FirstPowerPellet.SetActive (true);

			yield return new WaitForSeconds (.2f);

		}

	}


	public void OnGhostChasingPacManDone() {
		StopAllCoroutines ();
		PowerPellet.GetComponent<SpriteRenderer> ().enabled = false;

	}
}
