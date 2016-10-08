using UnityEngine;
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



	}

}
