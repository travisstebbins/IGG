using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
	string matchName = "room";
	uint matchSize = 4;
	bool matchAdvertise = true;
	string matchPassword = "";

	void Start()
	{
		NetworkManager.singleton.StartMatchMaker ();
	}

	public void createGameButtonOnClick()
	{
		NetworkManager.singleton.matchMaker.CreateMatch(matchName, matchSize, matchAdvertise, matchPassword, "", "", 0, 0, OnInternetMatchCreate);
	}

	public void joinGameButtonOnClick()
	{
		NetworkManager.singleton.matchMaker.ListMatches (0, 10, "", true, 0, 0, OnInternetMatchList);
	}

	void OnInternetMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
	{
		if (success)
		{
			//Debug.Log("Create match succeeded");

			MatchInfo hostInfo = matchInfo;
			NetworkServer.Listen(hostInfo, 9000);

			NetworkManager.singleton.StartHost(hostInfo);
		}
		else
		{
			Debug.LogError("Create match failed");
		}
	}

	public void OnInternetMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
	{
		if (success)
		{
			if (matches.Count != 0)
			{
				//Debug.Log("A list of matches was returned");

				//join the last server (just in case there are two...)
				NetworkManager.singleton.matchMaker.JoinMatch(matches[matches.Count - 1].networkId, "", "", "", 0, 0, OnJoinInternetMatch);
			}
			else
			{
				Debug.Log("No matches in requested room!");
			}
		}
		else
		{
			Debug.LogError("Couldn't connect to match maker");
		}
	}

	public void OnJoinInternetMatch(bool success, string extendedInfo, MatchInfo matchInfo)
	{
		if (success)
		{
			//Debug.Log("Able to join a match");

			MatchInfo hostInfo = matchInfo;
			NetworkManager.singleton.StartClient(hostInfo);
		}
		else
		{
			Debug.LogError("Join match failed");
		}
	}

	public void OnConnected(NetworkMessage msg)
	{
		Debug.Log("Connected!");
	}
}
