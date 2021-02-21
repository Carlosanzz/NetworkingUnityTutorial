using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DestroyBullet : NetworkBehaviour
{
	[Server]
	private void OnTriggerEnter(Collider other) {
		if(other.tag == "Bullet")
		{
			Debug.Log("HITWALL");
			NetworkServer.Destroy(other.gameObject);
		}
	}
}
