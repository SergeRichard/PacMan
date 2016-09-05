using UnityEngine;
using System.Collections;



public class Ghost : MonoBehaviour {



	public enum GhostStates
	{
		Scatter, Chase, Freightened
	}
	public static GhostStates GhostState;

	// Use this for initialization
	protected virtual void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
