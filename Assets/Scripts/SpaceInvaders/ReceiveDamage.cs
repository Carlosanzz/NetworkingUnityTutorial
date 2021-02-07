using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ReceiveDamage : NetworkBehaviour
{
		
	[SerializeField]
	private int maxHealth = 10;

	[SyncVar][SerializeField]
	private int currentHealth;

	[SerializeField]
	private string enemyTag;

	[SerializeField]
	private bool destroyOnDeath;

	private Vector2 initialPosition;

	// Use this for initialization
	void Start ()
    {
		this.currentHealth = this.maxHealth;
		this.initialPosition = this.transform.position;
		this.transform.GetChild(0).GetComponent<TextMesh>().text = this.maxHealth.ToString();
	}

	void OnTriggerEnter2D (Collider2D collider)
    {
		if(collider.tag == this.enemyTag)
        {
			this.TakeDamage(1);
			Destroy(collider.gameObject);
		}
	}

	void TakeDamage (int amount)
    {
		if(this.isServer)
        {
			this.currentHealth -= amount;
			this.transform.GetChild(0).GetComponent<TextMesh>().text = this.currentHealth.ToString();
			if(this.currentHealth <= 0)
            {
				if(this.destroyOnDeath)
                {
					if(this.tag == "Enemy" ){
						GameManager.Instance.UpdateScore();
						//CmdUpdateScore();
					}
					Destroy(this.gameObject);
				}
                else
                {
					this.currentHealth = this.maxHealth;
					RpcRespawn();
				}
			}
		}
	}

	[ClientRpc]
	void RpcRespawn ()
    {
		this.transform.position = this.initialPosition;
	}
	// [Command]
	// // Command on player object
	// void CmdUpdateScore()
	// {
	// 	GameManager.Instance.transform.GetComponent<NetworkIdentity>().AssignClientAuthority(this.GetComponent<NetworkIdentity>().connectionToClient);
	// 	GameManager.Instance.CmdUpdateScore(); 
	// 	GameManager.Instance.transform.GetComponent<NetworkIdentity>().RemoveClientAuthority();
	// }
}