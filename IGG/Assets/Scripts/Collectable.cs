using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public enum CollectableType
{
	Reverser
}

public class Collectable : NetworkBehaviour
{
	// PROPERTIES
	public CollectableType type { get; private set;}

	// CLASS FUNCTIONS
	[Command]
	public void CmdUseCollectable(bool playerIsPredator)
	{
		Debug.Log ("useCollectable called");
		if (type == CollectableType.Reverser)
		{
			if (!playerIsPredator)
			{
				GameManager.instance.swapPredatorPrey ();
			}
		}
		GameManager.instance.spawnCollectable ();
		NetworkServer.Destroy (gameObject);
	}

	// UNITY FUNCTIONS
	void Start()
	{
		int index = UnityEngine.Random.Range(0, Enum.GetNames(typeof(CollectableType)).Length);
		type = (CollectableType)index;
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.GetComponent<PlayerController>() != null)
		{
			CmdUseCollectable (coll.gameObject.GetComponent<PlayerController>().isPredator);
		}
	}
}