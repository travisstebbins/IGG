using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum CollectableType
{
	Reverser
}

public class Collectable : MonoBehaviour
{
	// PROPERTIES
	public CollectableType type { get; private set;}

	// CLASS FUNCTIONS
	public void useCollectable(Player whichPlayer)
	{
		if (type == CollectableType.Reverser)
		{
			Debug.Log ("using reverser");
			if (!whichPlayer.isPredator)
			{
				GameManager.instance.swapPredatorPrey ();
			}
		}
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
		if (coll.gameObject.GetComponent<Player>() != null)
		{
			useCollectable (coll.gameObject.GetComponent<Player>());
		}
	}
}