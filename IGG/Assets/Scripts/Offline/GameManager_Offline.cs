using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel.Design;
using UnityEngine.SceneManagement;
using System.Net.Configuration;

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
	[SerializeField] float moduleWidth;
	[SerializeField] float moduleHeight;
	[SerializeField] bool rotateModules;
	[SerializeField] int numRows;
	[SerializeField] int numColumns;
	[SerializeField] float timeLimit;

	// CLASS VARIABLES
	public readonly int playerLimit = 2;
	public readonly float predatorSpeed = 7;
	public readonly float preySpeed = 10;
	public readonly float predatorAbilityMultiplier = 3;
	public readonly float predatorAbilityDuration = 0.5f;
	public readonly float predatorAbilityCooldown = 2;
	PlayerController_Offline player1;
	PlayerController_Offline player2;
	int m_predatorIndex;
	Collectable_Offline collectable;
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
					player1.canUseAbility = true;
					player2.isPredator = false;
					player2.canUseAbility = false;
					break;
				case 1:
					player1.isPredator = false;
					player1.canUseAbility = false;
					player2.isPredator = true;
					player2.canUseAbility = true;
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

	void generateMap()
	{
		for (int i = 0; i < numRows; ++i)
		{
			for (int j = 0; j < numColumns; ++j)
			{
				float rotation = 0;
				if (rotateModules)
				{
					rotation = 90 * UnityEngine.Random.Range (0, 4);
				}
				Module_Offline module = Instantiate (modules [UnityEngine.Random.Range (0, modules.Count)], new Vector3(moduleWidth * j + moduleWidth / 2.0f, moduleHeight * i + moduleHeight / 2.0f, 0), Quaternion.Euler(new Vector3(0, 0, rotation))).GetComponent<Module_Offline>();
				module.gates.transform.localRotation = Quaternion.Euler (new Vector3 (0, 0, -rotation));
				if (i == 0)
				{
					module.bottomGateEnabled = true;
				}
				else if (i == numRows - 1)
				{
					module.topGateEnabled = true;
				}
				if (j == 0)
				{
					module.leftGateEnabled = true;
				}
				else if (j == numColumns - 1)
				{
					module.rightGateEnabled = true;
				}
			}
		}
	}

	public void spawnCollectable()
	{
		float x = UnityEngine.Random.Range (0, numColumns * moduleWidth);
		float y = UnityEngine.Random.Range (0, numRows * moduleHeight);
		collectable = Instantiate(collectablePrefab, new Vector3(x, y, 0), Quaternion.identity).GetComponent<Collectable_Offline>();
		while (Physics2D.BoxCastAll (collectable.transform.position, collectable.GetComponent<BoxCollider2D> ().size, 0, Vector3.forward).Length > 1)
		{
			x = UnityEngine.Random.Range (0, numColumns * moduleWidth);
			y = UnityEngine.Random.Range (0, numRows * moduleHeight);
			collectable.transform.position = new Vector2 (x, y);
		}
	}

	public void spawnPlayers()
	{
		float x = UnityEngine.Random.Range (0, numColumns * moduleWidth);
		float y = UnityEngine.Random.Range (0, numRows * moduleHeight);
		player1 = GameObject.Instantiate (playerPrefab, new Vector3 (x, y, 0), Quaternion.identity).GetComponent<PlayerController_Offline> ();
		while (Physics2D.BoxCastAll (player1.transform.position, player1.GetComponent<BoxCollider2D> ().size, 0, Vector3.forward).Length > 1)
		{
			x = UnityEngine.Random.Range (0, numColumns * moduleWidth);
			y = UnityEngine.Random.Range (0, numRows * moduleHeight);
			player1.transform.position = new Vector2 (x, y);
		}
		player1.playerIndex = 0;
		x = UnityEngine.Random.Range (0, numColumns * moduleWidth);
		y = UnityEngine.Random.Range (0, numRows * moduleHeight);
		player2 = GameObject.Instantiate (playerPrefab, new Vector3 (x, y, 0), Quaternion.identity).GetComponent<PlayerController_Offline> ();
		while (Physics2D.BoxCastAll (player2.transform.position, player2.GetComponent<BoxCollider2D> ().size, 0, Vector3.forward).Length > 1)
		{
			x = UnityEngine.Random.Range (0, numColumns * moduleWidth);
			y = UnityEngine.Random.Range (0, numRows * moduleHeight);
			player2.transform.position = new Vector2 (x, y);
		}
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

	public float collectableDistance(Vector3 point)
	{
		return Vector3.Distance (point, collectable.transform.position);
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
		generateMap ();
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