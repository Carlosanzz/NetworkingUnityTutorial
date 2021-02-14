using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

namespace MirrorTutorial.GettingStarted.Units
{

    public class PlayerName : NetworkBehaviour
    {
        [Tooltip("Player name UI.")]
        [SerializeField]
        private TextMeshProUGUI _text; 

        [SyncVar(hook = nameof(OnNameUpdated))]
        private string _synchronizedName = string.Empty;


        /// <summary>
        /// SyncVar hook for _synchronizedName 
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        private void OnNameUpdated(string oldName, string newName) {
            _text.text = newName;
        }

        /// <summary>
        /// Sets the player name for owner.
        /// </summary>
        /// <param name="name"></param>
        [Client]
        public void SetName(string name) {
            CmdSetName(name);
        }

        [Command]
        private void CmdSetName(string name) {
            _synchronizedName = name;
        }
    }

}