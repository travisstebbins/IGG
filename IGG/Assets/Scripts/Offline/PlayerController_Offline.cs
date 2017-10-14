using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController_Offline : MonoBehaviour
{
	// CLASS VARIABLES
	public int playerIndex;
	float speed = 5;
	bool m_isPredator;

	// PROPERTIES
	public bool isPredator
	{
		get
		{
			return m_isPredator;
		}
		set
		{
			m_isPredator = value;
			speed = m_isPredator ? GameManager_Offline.instance.predatorSpeed : GameManager_Offline.instance.preySpeed;
			GetComponent<SpriteRenderer> ().color = m_isPredator ? Color.red : Color.green;
		}
	}

	// CLASS FUNCTIONS

	// UNITY FUNCTIONS
	void Update()
	{
		if (playerIndex == 0)
		{
			transform.Translate (new Vector3 (Input.GetAxis ("Player1Horizontal") * Time.deltaTime * speed, Input.GetAxis ("Player1Vertical") * Time.deltaTime * speed, 0));
		}
		else
		{
			transform.Translate (new Vector3 (Input.GetAxis ("Player2Horizontal") * Time.deltaTime * speed, Input.GetAxis ("Player2Vertical") * Time.deltaTime * speed, 0));
		}
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (isPredator && coll.gameObject.GetComponent<PlayerController_Offline>() != null && !coll.gameObject.GetComponent<PlayerController_Offline>().isPredator)
		{
			Debug.Log ("Predator ate prey!");
		}
	}
}
