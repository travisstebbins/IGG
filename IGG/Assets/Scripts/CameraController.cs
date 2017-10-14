using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraController : NetworkBehaviour
{
	PlayerController myPlayer;

	public override void OnStartLocalPlayer()
	{
		foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player"))
		{
			if (p.GetComponent<PlayerController>()!= null && p.GetComponent<PlayerController>().isLocalPlayer)
			{
				myPlayer = p.GetComponent<PlayerController>();
				break;
			}
		}
	}

	void Update()
	{
		transform.position = myPlayer.transform.position;
	}
}
