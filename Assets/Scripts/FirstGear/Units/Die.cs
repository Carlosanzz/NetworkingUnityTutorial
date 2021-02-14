using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Die : NetworkBehaviour
{
	private void OnTriggerEnter(Collider other) {
		if(other.tag == "Bullet")
		{
			Debug.Log("HIT");
			Destroy(other.gameObject);
			Destroy(gameObject);
		}
	}

}