using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager_Offline : MonoBehaviour
{
	public void OnStartGameButtonClick()
	{
		SceneManager.LoadScene ("Main_Offline");
	}
}