using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pausemenu : MonoBehaviour
{
	public static bool GameIsPaused = false; //static because then we can use this verivale in other scripts

	public GameObject pausemenuGame;


	private void Start()
	{
	}
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (GameIsPaused)
			{
				Resume();
			}
			else
				Pause();
		}
	}

	public void Resume()
	{
		pausemenuGame.SetActive(false);
		Time.timeScale = 1;
		GameIsPaused = false;
	}
	void Pause()
	{
		pausemenuGame.SetActive(true);
		Time.timeScale = 0;
		GameIsPaused = true;
	}

	public void LoadMenu()
	{
		Time.timeScale = 1;

		SceneManager.LoadScene("Multiplayer_Menu");
	}

	public void Quitgame()
	{
		Application.Quit();
	}
}
