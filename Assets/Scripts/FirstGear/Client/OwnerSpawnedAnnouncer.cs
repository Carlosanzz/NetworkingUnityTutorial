using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace MirrorTutorial.GettingStarted.Clients
{
    public class OwnerSpawnedAnnouncer : NetworkBehaviour
    {
        public override void OnStartAuthority()
        {
            base.OnStartAuthority();
            AnnounceSpawned();
        }

        private void AnnounceSpawned()
        {
            ClientInstance ci = ClientInstance.ReturnClientInstance();
            ci.InvokeCharacterSpawned(gameObject);
        }

        
    }
}

