using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace MirrorTutorial.GettingStarted.Clients
{

    public class WorldCameraSetup : MonoBehaviour
    {
        /// <summary>
        /// Offset to be from the character
        /// </summary>
        /// <returns></returns>
        [Tooltip("Offset to be from the character.")]
        [SerializeField]
        private Vector3 _positionOffset = new Vector3(0f,2f,-4f);

        /// <summary>
        /// Target to follow
        /// </summary>
        private Transform _target = null;
        private void Awake() 
        {
            ClientInstance.OnOwnerCharacterSpawned += ClientInstance_OnOwnerCharacterSpawned;
        }
        
        private void OnDestroy() 
        {
            ClientInstance.OnOwnerCharacterSpawned -= ClientInstance_OnOwnerCharacterSpawned;
        }

        private void Update()
        {
            if(_target == null)
                return;
            transform.rotation = _target.rotation;
            transform.position = _target.position + _positionOffset.z * _target.forward + new Vector3 (0f,_positionOffset.y,0f); 
        }

        private void ClientInstance_OnOwnerCharacterSpawned(GameObject go)
        {
            if(go!=null) {
                _target = go.transform;
            }
        }
    }
}
