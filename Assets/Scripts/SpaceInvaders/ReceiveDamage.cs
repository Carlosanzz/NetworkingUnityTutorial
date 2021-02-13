using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ReceiveDamage : NetworkBehaviour
{
		
	[SerializeField]
	private int maxHealth = 10;

	[SyncVar(hook = nameof(OnHpChanged))]
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

public void OnHpChanged(int oldHp, int newHp) {
	this.transform.GetChild(0).GetComponent<TextMesh>().text = newHp.ToString();
}

	[ClientRpc]
	void RpcRespawn ()
    {
		this.transform.position = this.initialPosition;
	}

}