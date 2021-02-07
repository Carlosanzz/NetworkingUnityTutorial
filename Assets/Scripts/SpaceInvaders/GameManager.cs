using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class GameManager : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnScoreChanged))]
    public int score = 0;
    public Text scoreText;
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("The GameManager is NULL");
            }
            return _instance;
        }
    }

    private void Awake() {
        _instance = this; 
        scoreText = this.transform.GetChild(1).GetComponent<Text>();
        scoreText.text = score.ToString();
    }
    
    public void UpdateScore()
    {
        score++;
//        RpcUpdateScoreInClients();
    }

    // void Update() {
    //     if(Input.GetKeyDown(KeyCode.S))
    //     {
    //         score++;
    //     }
    // }

    public void OnScoreChanged(int oldScore, int newScore){
        scoreText.text = newScore.ToString();
    }


    // [ClientRpc]
    // public void RpcUpdateScoreInClients()
    // {
    //     scoreText.text = score.ToString();
    // }
}
