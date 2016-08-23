using UnityEngine;
using System.Collections;

public class GhostController : MonoBehaviour {

	public enum Modes { Scatter, Chase };
	public Modes Mode;

	public Transform GhostStartLocation;

	public GameObject GhostYellow;
	public GameObject GhostPink;
	public GameObject GhostBlue;
	public GameObject GhostRed;

	public Transform GhostYellowStart;
	public Transform GhostPinkStart;
	public Transform GhostBlueStart;
	public Transform GhostRedStart;

	public enum Actions
	{
		MoveRight,
		MoveLeft,
		MoveDown,
		MoveUp
	};

	public void DisableGhostRenderer() {
		GhostYellow.GetComponent<SpriteRenderer> ().enabled = false;
		GhostPink.GetComponent<SpriteRenderer> ().enabled = false;
		GhostBlue.GetComponent<SpriteRenderer> ().enabled = false;
		GhostRed.GetComponent<SpriteRenderer> ().enabled = false;

	}
	public void EnableGhostRenderer() {
		GhostYellow.GetComponent<SpriteRenderer> ().enabled = true;
		GhostPink.GetComponent<SpriteRenderer> ().enabled = true;
		GhostBlue.GetComponent<SpriteRenderer> ().enabled = true;
		GhostRed.GetComponent<SpriteRenderer> ().enabled = true;

	}
	// Use this for initialization
	void Start () {
		Mode = Modes.Scatter;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
