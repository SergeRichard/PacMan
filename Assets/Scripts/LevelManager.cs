using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public void LoadLevel(string name)
	{
		//Application.LoadLevel (name);
		SceneManager.LoadScene (name);
	}
	public void QuitGame()
	{
		Application.Quit ();
	}
}
