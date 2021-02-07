using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyPlayerManager : NetworkBehaviour
{

    public GameObject CheckersBoard;

    public Piece[,] pieces = new Piece[8,8];

    public override void OnStartClient()
    {
        base.OnStartClient();

        CheckersBoard = GameObject.Find("CheckersBoard");
    }

    [Server]
    public override void OnStartServer()
    {
        base.OnStartServer();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
