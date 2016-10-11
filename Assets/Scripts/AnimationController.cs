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
	public void OnChasingFourGhostsDone() {
		TitleMessageController.OnChasingFourGhostsDone ();
	}
	public void OnChasingThreeGhostDone() {
		TitleMessageController.OnChasingThreeGhostDone ();

	}
	public void OnChasingTwoGhostDone() {
		TitleMessageController.OnChasingTwoGhostDone ();

	}
	public void OnChasingOneGhostDone() {
		TitleMessageController.OnChasingOneGhostDone ();

	}
}
