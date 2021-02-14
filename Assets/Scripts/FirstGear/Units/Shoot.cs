using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Shoot : NetworkBehaviour
{
	[SerializeField]
	private GameObject bulletPrefab;

	[SerializeField]
	private float bulletSpeed = 10f;

	void Update ()
    {
		if(base.hasAuthority)
        {
			if (Input.GetButtonDown("Fire2"))
			{
				this.CmdShoot(bulletSpeed);
			}
			else if (Input.GetButtonDown("Fire3"))
			{
				this.CmdShoot(bulletSpeed * 10f);
			}
			
		}
	}

	[Command]
	void CmdShoot (float speed)
    {
		GameObject bullet = Instantiate(bulletPrefab, this.transform.position + transform.forward * 2f, Quaternion.identity);
		bullet.transform.rotation = transform.rotation;
		bullet.GetComponent<Rigidbody>().velocity = transform.forward * speed;
		Debug.Log(bullet.GetComponent<Rigidbody>().velocity);
		NetworkServer.Spawn(bullet);
		Destroy(bullet, 5.0f);
	}
}