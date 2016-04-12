﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalScript : Photon.MonoBehaviour
{
    //players info
    public int PlayerAmount = 0;
    public List<PhotonPlayer> PlayerList = new List<PhotonPlayer>();

    //match info
    public int[] TeamScore = new int[2];
    public List<int> PlayerKills = new List<int>();
    public List<int> PlayerDeaths = new List<int>();

	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        //update player amount
        PlayerAmount = PlayerList.Count;
	}

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //send players
            stream.SendNext(PlayerList.Count);
            for (int i = 0; i < PlayerList.Count; i++)
            {
                stream.SendNext(PlayerList[i]);
            }

            //send kills
            stream.SendNext(PlayerKills.Count);
            for (int i = 0; i < PlayerKills.Count; i++)
            {
                stream.SendNext(PlayerKills[i]);
            }

            //send deaths
            stream.SendNext(PlayerDeaths.Count);
            for (int i = 0; i < PlayerDeaths.Count; i++)
            {
                stream.SendNext(PlayerDeaths[i]);
            }
        }
        else
        {
            //get players
            int plLength = (int)stream.ReceiveNext();
            List<PhotonPlayer> plTemp = new List<PhotonPlayer>();
            for (int i = 0; i < plLength; i++)
            {
                plTemp.Add((PhotonPlayer)stream.ReceiveNext());
            }
            PlayerList = plTemp;

            //get kills
            int pkLength = (int)stream.ReceiveNext();
            List<int> pkTemp = new List<int>();
            for (int i = 0; i < pkLength; i++)
            {
                pkTemp.Add((int)stream.ReceiveNext());
            }
            PlayerKills = pkTemp;

            //get deaths
            int pdLength = (int)stream.ReceiveNext();
            List<int> pdTemp = new List<int>();
            for (int i = 0; i < pdLength; i++)
            {
                pdTemp.Add((int)stream.ReceiveNext());
            }
            PlayerDeaths = pdTemp;
        }
    }

    /// <summary>
    /// Returns player id or 255 for non-existing players
    /// </summary>
    /// <param name="player">Player to find</param>
    /// <returns>Player id</returns>
    public int GetPlayerId(PhotonPlayer player)
    {
        //PhotonPlayer[] playerList = PhotonNetwork.playerList;
        List<PhotonPlayer> pList = PlayerList;
        for (int i = 0; i < pList.Count; i++)
        {
            if (pList[i] == player)
                return i;
        }
        return 255;
    }
    /// <summary>
    /// Add player to global tracking
    /// </summary>
    /// <param name="player">Player to add</param>
    public void AddPlayer(PhotonPlayer player)
    {
        PlayerList.Add(player);
        PlayerKills.Add(0);
        PlayerDeaths.Add(0);
        Debug.Log("Player Added");
    }
    /// <summary>
    /// Remove player from global tracking
    /// </summary>
    /// <param name="player">Player to remove</param>
    public void RemovePlayer(PhotonPlayer player)
    {
        int id = GetPlayerId(player);
        if (id != 255)
        {
            PlayerList.RemoveAt(id);
            PlayerKills.RemoveAt(id);
            PlayerDeaths.RemoveAt(id);
        }
        else
        {
            Debug.Log("Unknown player to remove");
        }
        Debug.Log("Player Removed");
    }
}
