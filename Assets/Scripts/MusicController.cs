using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour {

	public AudioClip IntroMusic;
	public AudioClip SirenSound;
	public AudioSource WakaSoundAudioSource;

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
		audioSource.loop = false;
		audioSource.Play ();

	}
	public void PlaySirenSound() {		
		audioSource.clip = SirenSound;
		audioSource.loop = true;
		audioSource.Play ();
	}
	public void PlayWakaSound() {
		if (!WakaSoundAudioSource.isPlaying) {
			WakaSoundAudioSource.loop = true;
			WakaSoundAudioSource.Play ();
		}

	}
	public void StopWakaSound() {
		if (WakaSoundAudioSource.isPlaying)
			WakaSoundAudioSource.Stop ();
	}
}
