using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
namespace MirrorTutorial.GettingStarted.Clients {
        public class ClientInstance : NetworkBehaviour
    {
        [Tooltip("Prefab to instantiate as a player")]
        [SerializeField]
        private NetworkIdentity _playerPrefab = null;

        public override void OnStartServer()
        {
            base.OnStartServer();
            if(isServer)
                NetworkSpawnPlayer();
        }

    [Server]
        private void NetworkSpawnPlayer() {
            GameObject go = Instantiate(_playerPrefab.gameObject, transform.position, Quaternion.identity);
            NetworkServer.Spawn(go, base.connectionToClient);
        }

    }

}