using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace MirrorTutorial.GettingStarted.Units
{
    public class Motor : NetworkBehaviour
    {
        /*
        * How quickly to move
        */
        [Tooltip("How quickly to move.")]
        [SerializeField]
        private float _moveRate = 3f;

        /*
        * How quickly to rotate
        */
        [Tooltip("How quickly to rotate.")]
        [SerializeField]
        private float _rotateRate = 90f;

        /*
        * Character controller reference
        */
        private CharacterController _characterController = null;
        private void Awake() {
            _characterController = GetComponent<CharacterController>();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            _characterController.enabled = base.hasAuthority;
        }

        private void Update() {
            if(base.hasAuthority) {
                Move(); 
            }
        }

        private void Move() {
            float forward = Input.GetAxisRaw("Vertical");
            float rotation = Input.GetAxisRaw("Horizontal");

            /*
            * X: left/right, Y: up/down, Z: fw/bw. 
            */
            Vector3 next = new Vector3(0f, 0f, forward * Time.deltaTime * _moveRate);
            next += Physics.gravity * Time.deltaTime;
            transform.Rotate(new Vector3(0f, rotation * Time.deltaTime * _rotateRate, 0f));
            _characterController.Move(transform.TransformDirection(next));
            // _characterController.Move(next);
        }
    }
}