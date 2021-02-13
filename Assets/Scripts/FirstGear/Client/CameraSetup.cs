using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace MirrorTutorial.GettingStarted.Clients
{

    public class CameraSetup : NetworkBehaviour
    {
        [Tooltip("Camera object within the child of this transform")] // The script must be in the root object
        [SerializeField]
        private Transform _cameraObject;

        // public override void OnStartAuthority()
        // {
        //     base.OnStartAuthority();
        //     _cameraObject.gameObject.SetActive(true);
        // }
    }
}
