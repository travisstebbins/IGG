using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel.Design;
using UnityEngine.SceneManagement;

public class GameManager_Offline : MonoBehaviour
{
	// INSTANCE
	static GameManager_Offline m_instance;
	public static GameManager_Offline instance
	{
		get
		{
			if (m_instance == null)
			{
				m_instance = FindObjectOfType<GameManager_Offline>();
			}
			return m_instance;
		}
	}

	// SERIALIZE FIELD VARIABLES
	[SerializeField] GameObject playerPrefab;
	[SerializeField] GameObject collectablePrefab;
	[SerializeField] List<GameObject> modules;
	[SerializeField] int rows;
	[SerializeField] int columns;
	[SerializeField] float timeLimit;

	// CLASS VARIABLES
	public readonly int playerLimit = 2;
	public readonly float predatorSpeed = 5;
	public readonly float preySpeed = 5;
	PlayerController_Offline player1;
	PlayerController_Offline player2;
	int m_predatorIndex;
	float timeRemaining;

	public int predatorIndex
	{
		get
		{
			return m_predatorIndex;
		}
		set
		{
			m_predatorIndex = value;
			switch (m_predatorIndex)
			{
				case 0:
					player1.isPredator = true;
					player2.isPredator = false;
					break;
				case 1:
					player1.isPredator = false;
					player2.isPredator = true;
					break;
			}
		}
	}

	// CLASS FUNCTIONS
	public void initializePredator()
	{
		predatorIndex = UnityEngine.Random.Range (0, 2);
	}

	public void swapPredatorPrey()
	{
		if (predatorIndex == 0)
		{
			predatorIndex = 1;
		}
		else
		{
			predatorIndex = 0;
		}
	}

	public void spawnCollectable()
	{
		int i = UnityEngine.Random.Range (0, rows);
		int j = UnityEngine.Random.Range (0, columns);
		Instantiate(collectablePrefab, new Vector3(14 * i - 3.5f, 14 * j, 0), Quaternion.identity);
	}

	public void spawnPlayers()
	{
		int i = UnityEngine.Random.Range (0, rows);
		int j = UnityEngine.Random.Range (0, columns);
		player1 = GameObject.Instantiate (playerPrefab, new Vector3 (14 * i - 3.5f, 14 * j, 0), Quaternion.identity).GetComponent<PlayerController_Offline> ();
		player1.playerIndex = 0;
		i = UnityEngine.Random.Range (0, rows);
		j = UnityEngine.Random.Range (0, columns);
		player2 = GameObject.Instantiate (playerPrefab, new Vector3 (14 * i - 3.5f, 14 * j, 0), Quaternion.identity).GetComponent<PlayerController_Offline> ();
		player2.playerIndex = 1;
		player2.GetComponentInChildren<Camera> ().rect = new Rect (0.5f, 0, 0.5f, 1);
	}

	public void endGame(int winningPlayerIndex)
	{
		Time.timeScale = 0;
		UIManager_Offline.instance.showEndGameScreen (winningPlayerIndex);
	}

	public float playerDistance()
	{
		return Vector3.Distance (player1.transform.position, player2.transform.position);
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
		for (int i = 0; i < rows; ++i)
		{
			for (int j = 0; j < columns; ++j)
			{
				Instantiate (modules [UnityEngine.Random.Range (0, modules.Count)], new Vector3(14 * i, 14 * j, 0), Quaternion.identity);
			}
		}
		spawnCollectable ();
		spawnPlayers ();
		initializePredator ();
		StartCoroutine (timerCoroutine ());
	}

	IEnumerator timerCoroutine()
	{
		do
		{
			timeRemaining = Mathf.Ceil(timeLimit - Time.timeSinceLevelLoad);
			UIManager_Offline.instance.updateTimer (timeRemaining);
			yield return new WaitForSeconds(1);
		} while (timeRemaining > 0);
	}
}