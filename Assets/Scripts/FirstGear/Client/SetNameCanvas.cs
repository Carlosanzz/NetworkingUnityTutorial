using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

namespace MirrorTutorial.GettingStarted.Clients
{
    public class SetNameCanvas : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField _input;

        private string _lastValue = string.Empty;

        private void Awake() {
            ClientInstance.OnOwnerCharacterSpawned += SetInitialName;
        }
        void Update()
        {
            CheckSetName();
        }

        private void SetInitialName(GameObject go) 
        {
            ClientInstance.Instance.SetPlayerName(_input.text);
        }
        
        private void CheckSetName()
        {
            if(!NetworkClient.active)
                return;
            ClientInstance ci = ClientInstance.ReturnClientInstance();
            if(ci == null)
                return;
            if(_input.text != _lastValue)
            {
                _lastValue = _input.text;
                ci.SetPlayerName(_input.text);
            }
            
        }
    }
}
