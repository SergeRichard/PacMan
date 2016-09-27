using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour {

	public AudioClip IntroMusic;
	public AudioClip SirenSound;
	public AudioClip PacManDeathSound;
	public AudioClip EatGhostSound;
	public AudioSource WakaSoundAudioSource;
	public AudioSource GhostEatenAudioSource;

	protected AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void StopAllSounds() {
		audioSource.Stop ();
		WakaSoundAudioSource.Stop ();

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
	public void PlayDeathSound() {
		audioSource.clip = PacManDeathSound;
		audioSource.loop = false;
		audioSource.Play ();
	}
	public void PlayGhostEatenSound() {
		GhostEatenAudioSource.loop = false;
		GhostEatenAudioSource.Play ();

	}
}
