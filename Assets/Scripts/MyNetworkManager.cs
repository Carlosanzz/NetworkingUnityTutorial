using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{

    public override void OnStartServer() // GENERAL MIRROR CALLBACKS EXECUTED IN SERVER
    {
        Debug.Log("Server started!");
    }

    public override void OnStopServer() // GENERAL MIRROR CALLBACKS EXECUTED IN SERVER
    {
        Debug.Log("Server stopped!");
        SpawnEnemies.Instance.StopSpawn();
    }

    public override void OnClientConnect(NetworkConnection conn) // GENERAL MIRROR CALLBACKS EXECUTED IN CLIENT
    {
        Debug.Log("Connected to server!");
    }
    public override void OnClientDisconnect(NetworkConnection conn) // GENERAL MIRROR CALLBACKS EXECUTED IN CLIENT
    {
        Debug.Log("Disconnected to server!");
    }

}
