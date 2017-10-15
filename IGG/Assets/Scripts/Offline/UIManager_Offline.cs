using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager_Offline : MonoBehaviour
{
	// INSTANCE
	static UIManager_Offline m_instance;
	public static UIManager_Offline instance
	{
		get
		{
			if (m_instance == null)
			{
				m_instance = FindObjectOfType<UIManager_Offline>();
			}
			return m_instance;
		}
	}

	// SERIALIZE FIELD VARIABLES
	[SerializeField] Text timer;
	[SerializeField] GameObject pauseScreen;
	[SerializeField] GameObject endGameScreen;
	[SerializeField] Text winText;
	[SerializeField] Text timeText;
	[SerializeField] Button resumeButton;
	[SerializeField] Button endGameScreenQuitButton;
	[SerializeField] Button pauseScreenQuitButton;

	// CLASS VARIABLES
	public bool isPaused { get; set; }

	// CLASS FUNCTIONS
	public void updateTimer(float timeRemaining)
	{
		int minutes = (int)timeRemaining / 60;
		int seconds = (int)((timeRemaining - (60 * minutes)) % 60);
		timer.text = minutes + ":";
		if (seconds < 10)
		{
			timer.text += "0" + seconds;
		}
		else
		{
			timer.text += seconds;
		}
	}

	public void showEndGameScreen(int winningPlayerIndex)
	{
		winText.text = "Player " + (winningPlayerIndex + 1) + " Wins!";
		int minutes = (int)Time.timeSinceLevelLoad / 60;
		int seconds = (int)((Time.timeSinceLevelLoad - (60 * minutes)) % 60);
		string formattedTime = minutes + ":";
		if (seconds < 10)
		{
			formattedTime += "0" + seconds;
		}
		else
		{
			formattedTime += seconds;
		}
		timeText.text = formattedTime;
		endGameScreen.SetActive (true);
		endGameScreenQuitButton.Select ();
		Time.timeScale = 0;
	}

	// EVENT HANDLERS
	public void showPauseScreen()
	{
		pauseScreen.SetActive (true);
		resumeButton.Select ();
		Time.timeScale = 0;
		isPaused = true;
	}

	public void OnResumeButtonClick()
	{
		pauseScreen.SetActive (false);
		Time.timeScale = 1;
		isPaused = false;
		GameManager_Offline.instance.isPaused = false;
	}

	public void OnQuitButtonClick()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene ("MainMenu_Offline");
	}

	// UNITY FUNCTIONS
	void Awake()
	{
		if (instance != this)
		{
			Destroy (this.gameObject);
		}
	}

	void Start()
	{
		isPaused = false;
	}
}
