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
	[SerializeField] GameObject endGameScreen;
	[SerializeField] Text winText;
	[SerializeField] Text timeText;

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
	}

	public void OnQuitButtonClick()
	{
		Debug.Log ("quit button clicked");
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
}
