using UnityEngine;
using System.Collections;

public class LivesController : MonoBehaviour {

	public GameObject[] Lives;

	// Use this for initialization
	void Start () {
		HideAllPacman ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void ShowLives(int numOfLives) {
		HideAllPacman ();
		for (int i = 1; i < numOfLives; i++) {
			Lives [i-1].SetActive (true);
		}
	}
	private void HideAllPacman() {
		foreach (var life in Lives) {
			life.SetActive (false);
		}

	}
}
