using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FruitController : MonoBehaviour {

	public GameManager GameManager;

	public enum FruitTypes
	{
		Cherry,
		Strawberry,
		Orange,
		Apple,
		Grapes,
		Ship,
		Bell,
		Key
	}

	public List<SpriteRenderer> FruitPositions;
	public List<Sprite> Sprites;

	public Dictionary<FruitTypes, Sprite> SpritesMap;

	private List<FruitTypes> FruitsByLevel;

	// Use this for initialization
	void Start () {
		SpritesMap = new Dictionary<FruitTypes, Sprite> () { 
			{ FruitTypes.Cherry, Sprites [0] }, 
			{FruitTypes.Strawberry, Sprites [1]},
			{FruitTypes.Orange, Sprites [2]},
			{FruitTypes.Apple, Sprites [3]},
			{FruitTypes.Grapes, Sprites [4]},
			{FruitTypes.Ship, Sprites [5]},
			{FruitTypes.Bell, Sprites [6]},
			{FruitTypes.Key, Sprites [7]}
		};
		FruitsByLevel = new List<FruitTypes> () {
			FruitTypes.Cherry,
			FruitTypes.Strawberry,
			FruitTypes.Orange,
			FruitTypes.Orange,
			FruitTypes.Apple,
			FruitTypes.Apple,
			FruitTypes.Grapes,
			FruitTypes.Grapes,
			FruitTypes.Ship,
			FruitTypes.Ship,
			FruitTypes.Bell,
			FruitTypes.Bell,
			FruitTypes.Key,
			FruitTypes.Key,
		};

		DisplayFruitsOnCounter ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void DisplayFruitsOnCounter() {
		DisablePositions ();

		int startPosition = (GameManager.Level - 7) < 0 ? 0 : (GameManager.Level - 7);

		int counter = 0;

		for (int i = startPosition; i < GameManager.Level; i++) {
			int fruitsByLevel = i;

			if (i > FruitsByLevel.Count - 1)
				fruitsByLevel = FruitsByLevel.Count - 1;				

			FruitPositions [counter].enabled = true;
			FruitPositions [counter].sprite = SpritesMap [FruitsByLevel[fruitsByLevel]];

			counter++;
		}

	}
	void DisablePositions() {
		foreach (var FruitPosition in FruitPositions) {
			FruitPosition.enabled = false;
		}

	}
}
