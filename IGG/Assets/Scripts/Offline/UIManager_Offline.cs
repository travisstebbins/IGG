using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

	// UNITY FUNCTIONS
	void Awake()
	{
		if (instance != this)
		{
			Destroy (this.gameObject);
		}
	}
}
