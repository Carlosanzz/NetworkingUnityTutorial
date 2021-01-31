using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnHolaCountChanged))]
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
    void CmdHola() {
        Debug.Log("Received Hola from Client!");
        holaCount++;
        TargetReplyHola();
    }

    [ClientRpc]
    void RpcTooHigh() {
        Debug.Log("Your client is too high!");
    }

    [TargetRpc]
    void TargetReplyHola(){
        Debug.Log("Received Hola from Server!");
    }

    void OnHolaCountChanged(int oldCount, int newCount) {
        Debug.Log($"Player with NetID {netId} had sent {oldCount} holas, but now he has sent {newCount} holas.");
    }

}
