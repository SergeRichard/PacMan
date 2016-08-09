using UnityEngine;
using System.Collections;

public class GhostController : MonoBehaviour {

	public enum Modes { Scatter, Chase };
	public Modes Mode;

	public enum Actions
	{
		MoveRight,
		MoveLeft,
		MoveDown,
		MoveUp
	};

	// Use this for initialization
	void Start () {
		Mode = Modes.Scatter;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
