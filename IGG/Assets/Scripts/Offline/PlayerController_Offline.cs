using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController_Offline : MonoBehaviour
{
	// CLASS VARIABLES
	public int playerIndex;
	float speed = 5000000;
	bool m_isPredator;
	public bool canUseAbility;
	bool pausePressed;
		// variables for calculating light properties
	readonly float minDistance = 5;
	readonly float maxDistance = 40;
	readonly float minZ = -0.2f;
	readonly float maxZ = -1f;
	readonly float minR = 5;
	readonly float maxR = 25;
	readonly float minI = 0.8f;
	readonly float maxI = 1.5f;

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
		// handle input
		if (playerIndex == 0)
		{
			checkPause ();
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (Input.GetAxis ("Player1Horizontal") * speed, Input.GetAxis ("Player1Vertical") * speed);
			if (Input.GetAxis("Player1Ability") != 0 && isPredator && canUseAbility && !GameManager_Offline.instance.isPaused)
			{
				StartCoroutine (useAbility ());
			}
		}
		else
		{
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (Input.GetAxis ("Player2Horizontal") * speed, Input.GetAxis ("Player2Vertical") * speed);
			if (Input.GetAxis("Player2Ability") != 0 && isPredator && canUseAbility && !GameManager_Offline.instance.isPaused)
			{
				StartCoroutine (useAbility ());
			}
		}
		// adjust light qualities
		Light l = GetComponentInChildren<Light> ();
		float slope = 1f / (maxDistance - minDistance);
		float intercept = -slope * minDistance;
		float t = Mathf.Clamp01(slope * GameManager_Offline.instance.playerDistance () + intercept);
		l.transform.localPosition = new Vector3 (0, 0, Mathf.Lerp (minZ, maxZ, t));
		l.range = Mathf.Lerp (minR, maxR, t);
		l.intensity = Mathf.Lerp (minI, maxI, t);
		t = Mathf.Clamp01 (slope * GameManager_Offline.instance.collectableDistance (transform.position) + intercept);
		l.color = Color.Lerp (new Color(0.7f, 0.7f, 1), Color.yellow, t);
	}

	void checkPause()
	{
		if (Input.GetAxisRaw("Pause") != 0 && !pausePressed)
		{
			GameManager_Offline.instance.togglePause ();
			pausePressed = true;
		}
		if (Input.GetAxisRaw("Pause") == 0 && pausePressed)
		{
			pausePressed = false;
		}
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (isPredator && coll.gameObject.GetComponent<PlayerController_Offline>() != null && !coll.gameObject.GetComponent<PlayerController_Offline>().isPredator)
		{
			GameManager_Offline.instance.endGame (playerIndex);
		}
	}

	// COROUTINE FUNCTIONS
	IEnumerator useAbility()
	{
		Debug.Log ("player " + (playerIndex + 1) + " used ability");
		canUseAbility = false;
		float originalSpeed = speed;
		speed *= GameManager_Offline.instance.predatorAbilityMultiplier;
		yield return new WaitForSeconds (GameManager_Offline.instance.predatorAbilityDuration);
		speed = originalSpeed;
		yield return new WaitForSeconds (GameManager_Offline.instance.predatorAbilityCooldown);
		canUseAbility = true;
	}
}
