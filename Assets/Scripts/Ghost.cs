using UnityEngine;
using System.Collections;



public class Ghost : MonoBehaviour {



	public enum GhostStates
	{
		Scatter, Chase, Freightened, FrightenedBlinking
	}
	public static GhostStates GhostState;

	// Use this for initialization
	protected virtual void Start () {
		GhostState = GhostStates.Scatter;
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
