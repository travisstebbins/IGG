using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	// CLASS VARIABLES
	bool m_isPredator;
	float speed;

	// PROPERTIES
	public int playerIndex { get; set; }
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
	void Update()
	{
		if (playerIndex == 0)
		{
			transform.position += Vector3.right * Input.GetAxis ("Player1Horizontal") * speed * Time.deltaTime;
			transform.position += Vector3.up * Input.GetAxis ("Player1Vertical") * speed * Time.deltaTime;
		}
		else if (playerIndex == 1)
		{
			transform.position += Vector3.right * Input.GetAxis ("Player2Horizontal") * speed * Time.deltaTime;
			transform.position += Vector3.up * Input.GetAxis ("Player2Vertical") * speed * Time.deltaTime;
		}
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (isPredator && coll.gameObject.GetComponent<Player>() != null)
		{
			Debug.Log ("predator (player " + playerIndex + ") got prey");
		}
	}
}
