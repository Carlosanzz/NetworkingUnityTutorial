using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnEnemies : NetworkBehaviour
{

  	private static SpawnEnemies _instance;
    public static SpawnEnemies Instance
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


	[SerializeField]
	private GameObject enemyPrefab;

	[SerializeField]
	private float spawnInterval = 1.0f;

	[SerializeField]
	private float enemySpeed = 1.0f;

	[SerializeField]
	private float spawnPositionRange = 9.0f;

    private void Awake()
    {
        _instance = this;
    }

	public override void OnStartServer ()
    {
		InvokeRepeating("SpawnEnemy", this.spawnInterval, this.spawnInterval);
	}

	public void StopSpawn()
	{
		CancelInvoke("SpawnEnemy");
	}


	void SpawnEnemy ()
    {
		Vector2 spawnPosition = new Vector2(Random.Range(-spawnPositionRange, spawnPositionRange), this.transform.position.y);
		GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity) as GameObject;
		enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -this.enemySpeed);
		NetworkServer.Spawn(enemy);
		Destroy(enemy, 10);
	}
}