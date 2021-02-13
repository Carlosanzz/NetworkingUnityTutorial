using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
namespace MirrorTutorial.GettingStarted.Clients 
{
    public class ClientInstance : NetworkBehaviour
    {

        /// <summary>
        /// Singleton reference to the client instance. Referenced value will be for LocalPlayer.
        /// </summary>  
        public static ClientInstance Instance;
        
        /// <summary>
        /// Dispatched on the owning player when a character is spawned for them.
        /// </summary>
        public static Action<GameObject> OnOwnerCharacterSpawned;
        public void InvokeCharacterSpawned(GameObject go)
        {
            Debug.Log("GOT IT!");
            OnOwnerCharacterSpawned?.Invoke(go);
        }

        /// <summary>
        /// Prefab for the Player.
        /// </summary>  
        [Tooltip("Prefab to instantiate as a player")]
        [SerializeField]
        private NetworkIdentity _playerPrefab = null;

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            Instance = this;
            CmdRequestSpawn();
        }
        
        // public override void OnStartServer()
        // {
        //     base.OnStartServer();
        //     if(isServer)
        //         NetworkSpawnPlayer();
        // }

        /// <summary>
        /// Request a spawn for character.
        /// </summary>
        [Command]
        private void CmdRequestSpawn()
        {
            NetworkSpawnPlayer();
        }

        /// <summary>
        /// Spawns a character for the player.
        /// </summary>
        [Server]
        private void NetworkSpawnPlayer() {
            GameObject go = Instantiate(_playerPrefab.gameObject, transform.position, Quaternion.identity);
            NetworkServer.Spawn(go, base.connectionToClient);
        }
        
        /// <summary>
        /// Returns the current client instance for the connection.
        /// Can be called from the server for returning instance of specific client and run whatever.
        /// Also from the clients to return the client instance.
        /// For example: 
        /// if(base.isServer)
        /// {
        ///    ClientInstance ci = ClientInstance.ReturnClientInstance(base.connectionToClient); 
        /// }
        /// </summary>
        /// <param name="conn"></param>
        // /// <returns></returns>
        public static ClientInstance ReturnClientInstance(NetworkConnection conn = null) 
        {
            if(NetworkServer.active && conn != null)
            {
                NetworkIdentity localPlayer;
                if(GettingStartedNetworkManager.LocalPlayers.TryGetValue(conn, out localPlayer))
                    return localPlayer.GetComponent<ClientInstance>();
                else
                    return null;
            }
            else
            {
                return Instance;
            }
        }

    }

}