using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
	// CLASS VARIABLES
	float speed = 5;

	// PROPERTIES
	[SyncVar(hook = "OnChangeIsPredator")]
	public bool isPredator;

	// HOOKS
	void OnChangeIsPredator(bool _isPredator)
	{
		isPredator = _isPredator;
		speed = isPredator ? GameManager.instance.predatorSpeed : GameManager.instance.preySpeed;
		GetComponent<SpriteRenderer> ().color = isPredator ? Color.red : Color.green;
		foreach (PlayerController p in GameManager.instance.getPlayers())
		{
			if (p != this)
			{
				p.GetComponent<SpriteRenderer> ().color = p.isPredator ? Color.red : Color.green;
			}
		}
	}

	// CLASS FUNCTIONS
//	[ClientRpc]
//	public void RpcSetIsPredator(bool newValue)
//	{
//		Debug.Log ("RpcSetIsPredator called");
//		isPredator = newValue;
//		speed = isPredator ? GameManager.instance.predatorSpeed : GameManager.instance.preySpeed;
//		GetComponent<SpriteRenderer> ().color = isPredator ? Color.red : Color.green;
//	}

	// UNITY FUNCTIONS
	public override void OnStartLocalPlayer()
	{
		
	}

	public override void OnStartServer()
	{
		base.OnStartServer();
		if (GameObject.FindGameObjectsWithTag ("Player").Length == GameManager.instance.playerLimit)
		{
			GameManager.instance.initializePlayers ();
		}
	}

	void Update()
	{
		if (!isLocalPlayer)
		{
			return;
		}
		transform.Translate (new Vector3 (Input.GetAxis ("Horizontal") * Time.deltaTime * speed, Input.GetAxis ("Vertical") * Time.deltaTime * speed, 0));
		Camera.main.transform.position = transform.position - Vector3.forward;
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		
	}
}
