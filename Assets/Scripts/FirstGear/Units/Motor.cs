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
        private float _moveRate = 4f;

        /*
        * How quickly to rotate
        */
        [Tooltip("How quickly to rotate.")]
        [SerializeField]
        private float _rotateRate = 90f;

        /*
        * How strong to jump
        */
        [Tooltip("How strong to jump.")]
        [SerializeField]
        private float _jumpForce = 100f;

        /// <summary>
        /// Able to jump in the air
        /// </summary>
        private bool _canDoubleJump = true;

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
            float forward  = Input.GetAxisRaw("Vertical");
            float rotation = Input.GetAxisRaw("Horizontal");
            bool  jumping  = Input.GetKeyDown(KeyCode.Space);
            bool isLeft = Input.GetKey(KeyCode.Q);
            bool isRight = Input.GetKey(KeyCode.E);
            /*
            * X: left/right, Y: up/down, Z: fw/bw. 
            */
            Vector3 next = new Vector3(0f, 0f, forward * Time.deltaTime * _moveRate);     

            if (isLeft && !isRight) {
                next += new Vector3 (-Time.deltaTime * _moveRate * 0.8f ,0f,0f);
            } else if (!isLeft && isRight){
                next += new Vector3 (Time.deltaTime * _moveRate * 0.8f ,0f,0f);
            }      
            if (jumping) {
                if (_characterController.isGrounded) {
                    next +=  new Vector3(0f, Time.deltaTime * _jumpForce, 0f);
                    _canDoubleJump = true;
                } else {
                    if (_canDoubleJump) {
                    _canDoubleJump = false;
                    next +=  new Vector3(0f, Time.deltaTime * _jumpForce, 0f);
                    }
                }
            }        
            next += 0.4f * Physics.gravity * Time.deltaTime; 


            transform.Rotate(new Vector3(0f, rotation * Time.deltaTime * _rotateRate, 0f));
            _characterController.Move(transform.TransformDirection(next));
            // _characterController.Move(next);
        }
    }
}