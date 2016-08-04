using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour {

	public AudioClip IntroMusic;

	protected AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayIntro() {
		audioSource.clip = IntroMusic;
		audioSource.Play ();

	}
}
