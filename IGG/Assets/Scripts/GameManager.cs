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
	int m_predatorIndex;
	public readonly float predatorSpeed = 5;
	public readonly float preySpeed = 5;

	// PROPERTIES
	public int predatorIndex
	{
		get
		{
			return m_predatorIndex;
		}
		private set
		{
			m_predatorIndex = value;
//			switch(m_predatorIndex)
//			{
//				case 0:
//					player1.isPredator = true;
//					player2.isPredator = false;
//					break;
//				case 1:
//					player1.isPredator = false;
//					player2.isPredator = true;
//					break;
//			}
		}
	}

	// CLASS FUNCTIONS
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
		
	// UNITY FUNCTIONS
	public override void OnStartServer()
	{
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

	void Start()
	{
		predatorIndex = 0;
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
