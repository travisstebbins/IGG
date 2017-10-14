using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
	// SERIALIZE FIELD VARIABLES
	[SerializeField] GameObject collectablePrefab;

	// CLASS VARIABLES
	bool m_isPredator;
	float speed = 5;
	GameObject collectable;

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
			speed = m_isPredator ? GameManager.instance.predatorSpeed : GameManager.instance.preySpeed;
			GetComponent<SpriteRenderer> ().color = m_isPredator ? Color.red : Color.green;
		}
	}

	// UNITY FUNCTIONS
	public override void OnStartLocalPlayer()
	{
		GetComponent<SpriteRenderer> ().color = Color.blue;
	}

	void Update()
	{
		if (!isLocalPlayer)
		{
			return;
		}
		transform.Translate (new Vector3 (Input.GetAxis ("Horizontal") * Time.deltaTime * speed, Input.GetAxis ("Vertical") * Time.deltaTime * speed, 0));
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		
	}
}
