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
	readonly float minDistance = 5;
	readonly float maxDistance = 40;
	readonly float minZ = -0.4f;
	readonly float maxZ = -0.15f;
	readonly float minR = 4;
	readonly float maxR = 15;
	readonly float minI = 0.8f;
	readonly float maxI = 1.2f;

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
		Light l = GetComponentInChildren<Light> ();
		float slope = 1f / (maxDistance - minDistance);
		float intercept = -slope * minDistance;
		float t = Mathf.Clamp01(slope * GameManager_Offline.instance.playerDistance () + intercept);
		l.transform.localPosition = new Vector3 (0, 0, Mathf.Lerp (minZ, maxZ, t));
		l.range = Mathf.Lerp (minR, maxR, t);
		l.intensity = Mathf.Lerp (minI, maxI, t);
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (isPredator && coll.gameObject.GetComponent<PlayerController_Offline>() != null && !coll.gameObject.GetComponent<PlayerController_Offline>().isPredator)
		{
			GameManager_Offline.instance.endGame (playerIndex);
		}
	}
}
