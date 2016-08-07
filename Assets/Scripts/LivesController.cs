using UnityEngine;
using System.Collections;

public class LivesController : MonoBehaviour {

	public GameObject[] Lives;

	// Use this for initialization
	void Start () {
		foreach (var life in Lives) {
			life.SetActive (false);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void ShowLives(int numOfLives) {
		for (int i = 1; i < numOfLives; i++) {
			Lives [i-1].SetActive (true);
		}
	}
}
