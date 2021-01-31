using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnHolaCountChanged))] //SYNCRONIZED VARIABLE, OBJECT IN SERVER HAS AUTHORITY AND CALLS HOOK ON CLIENTS WHEN CHANGED
    int holaCount = 0;
    void HandleMovement()
    {
        if (isLocalPlayer)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(moveHorizontal*0.1f, moveVertical*0.1f, 0);
            transform.position += movement;
        }

    }

    private void Update() {
        HandleMovement();
        if(isLocalPlayer && Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("Sending Hola to Server!");
            CmdHola();
        }

        if(isServer && transform.position.y > 10){ // Bad practice 
            RpcTooHigh();
        }
    }

    [Command]
    void CmdHola() { // COMMAND CALLED IN CLIENT OBJECT AND EXECUTED IN SERVER OBJECT.
        Debug.Log("Received Hola from Client!");
        holaCount++;
        TargetReplyHola();
    }

    [ClientRpc]  
    void RpcTooHigh() { // REMOTE PROCEDURE CALL CALLED IN SERVER OBJECT AND EXECUTED IN ALL CLIENTS OBJECT (CAN BE MODIFIED WITH NETWORK VISIBILITY).
        Debug.Log("Your client is too high!");
    }

    [TargetRpc]
    void TargetReplyHola(){ // TARGET REMOTE PROCEDURE CALL, CALLED IN SERVER OBJECT AND EXECUTED IN SPECIFIC CLIENT OBJECT (CAN BE IMPLICIT LIKE HERE, BUT CAN EXPLICITLY SAY CLIENT CONNECTION)
        Debug.Log("Received Hola from Server!");
    }

    void OnHolaCountChanged(int oldCount, int newCount) { // CALLBACK N CHANGE 
        Debug.Log($"Player with NetID {netId} had sent {oldCount} holas, but now he has sent {newCount} holas.");
    }

}
