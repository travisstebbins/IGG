using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Collectable_Offline : MonoBehaviour
{
	// PROPERTIES
	public CollectableType type { get; private set;}

	// CLASS FUNCTIONS
	public void useCollectable(bool playerIsPredator)
	{
		if (type == CollectableType.Reverser)
		{
			if (!playerIsPredator)
			{
				GameManager_Offline.instance.swapPredatorPrey ();
			}
		}
		GameManager_Offline.instance.spawnCollectable ();
		Destroy (gameObject);
	}

	// UNITY FUNCTIONS
	void Start()
	{
		int index = UnityEngine.Random.Range(0, Enum.GetNames(typeof(CollectableType)).Length);
		type = (CollectableType)index;
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.GetComponent<PlayerController_Offline>() != null)
		{
			useCollectable (coll.gameObject.GetComponent<PlayerController_Offline>().isPredator);
		}
	}
}