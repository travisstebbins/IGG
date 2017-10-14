using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel.Design;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{
	// INSTANCE
	static GameManager m_instance;
	public static GameManager instance
	{
		get
		{
			if (m_instance == null)
			{
				m_instance = FindObjectOfType<GameManager>();
			}
			return m_instance;
		}
	}

	// SERIALIZE FIELD VARIABLES
	[SerializeField] GameObject collectablePrefab;
	[SerializeField] List<GameObject> modules;
	[SerializeField] int rows;
	[SerializeField] int columns;

	// CLASS VARIABLES
	public readonly int playerLimit = 2;
	public readonly float predatorSpeed = 5;
	public readonly float preySpeed = 5;
	List<PlayerController> players = new List<PlayerController>();
	int m_predatorIndex;

	public int predatorIndex
	{
		get
		{
			return m_predatorIndex;
		}
		set
		{
			m_predatorIndex = value;
			for (int i = 0; i < players.Count; ++i)
			{
				if (i == m_predatorIndex)
				{
					players [i].isPredator = true;
				}
				else
				{
					players [i].isPredator = false;
				}
			}
		}
	}

	// CLASS FUNCTIONS
	public void initializePlayers()
	{
		players.Clear ();
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
		{
			if (go.GetComponent<PlayerController>() != null)
			{
				players.Add (go.GetComponent<PlayerController> ());
			}
		}
		predatorIndex = UnityEngine.Random.Range (0, players.Count);
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
		GameObject collectable = Instantiate(collectablePrefab, new Vector3(14 * i - 3.5f, 14 * j, 0), Quaternion.identity);
		NetworkServer.Spawn (collectable);
	}

	public List<PlayerController> getPlayers()
	{
		return players;
	}

	void resetGameManager()
	{
		players.Clear ();
	}
		
	// UNITY FUNCTIONS
	public override void OnStartServer()
	{
		resetGameManager ();
		Debug.Log ("#players: " + players.Count);
		for (int i = 0; i < rows; ++i)
		{
			for (int j = 0; j < columns; ++j)
			{
				GameObject module = Instantiate (modules [UnityEngine.Random.Range (0, modules.Count)], new Vector3(14 * i, 14 * j, 0), Quaternion.identity);
				NetworkServer.Spawn (module);
			}
		}
		spawnCollectable ();
	}

	void Awake()
	{
		if (instance != this)
		{
			Destroy (this.gameObject);
		}
	}

	public void generateCollectable()
	{
		if (!isServer)
		{
			return;
		}
		RpcGenerateCollectable ();
	}

	[ClientRpc]
	void RpcGenerateCollectable()
	{
		GameObject collectable = Instantiate (collectablePrefab, Vector3.zero, Quaternion.identity);
		NetworkServer.Spawn (collectable);
	}
}
