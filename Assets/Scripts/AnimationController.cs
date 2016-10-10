using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour {

	public TitleMessageController TitleMessageController; 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnGhostChasePacManDone() {
		TitleMessageController.OnGhostChasingPacManDone ();



	}
}
