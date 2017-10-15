using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager_Offline : MonoBehaviour
{
	// SERIALIZE FIELD VARIABLES
	[SerializeField] Button startGameButton;
	[SerializeField] Button howToPlayButton;
	[SerializeField] GameObject howToPlayScreen;
	[SerializeField] Button backButton;

	// EVENT HANDLERS
	public void OnStartGameButtonClick()
	{
		SceneManager.LoadScene ("Main_Offline");
	}

	public void OnHowToPlayButtonClick()
	{
		howToPlayScreen.SetActive (true);
		backButton.Select ();
	}

	public void OnBackButtonClick()
	{
		howToPlayScreen.SetActive (false);
		startGameButton.Select ();
	}

	// UNITY FUNCTIONS
	void Start()
	{
		startGameButton.Select ();
	}
}