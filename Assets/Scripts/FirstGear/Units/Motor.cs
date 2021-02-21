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
        private float _jumpForce = 10f;
        [SerializeField]
        private int _jumping = 1;

        /// <summary>
        /// Max jumping time
        /// </summary>
        [Tooltip("How much time to jump")]
        [SerializeField]
        private const float _maxJumpTime = 2f;

        /// <summary>
        /// current jumping time
        /// </summary>
        [SerializeField]
        private float _currentJumpTime = _maxJumpTime;

        /*
        * How fast to dash
        */
        [Tooltip("How fast to dash.")]
        [SerializeField]
        private float _dashForce = 50f;

        private bool _dashing = false;

        /// <summary>
        /// Able to dash
        /// </summary>
        private bool _canDash = true;

        /// <summary>
        /// Able to dash
        /// </summary>
        [Tooltip("How much time to dash")]
        [SerializeField]
        private const float _maxDashTime = 1f;

        /// <summary>
        /// Able to dash
        /// </summary>
        private float _currentDashTime = _maxDashTime;

        private float _dashingCooldown = 1f;



        // /// <summary>
        // /// Able to jump in the air
        // /// </summary>
        // private bool _canDoubleJump = true;

        /*
        * Character controller and animator references
        */
        private CharacterController _characterController = null;
        private Animator _animator;
        private NetworkAnimator _networkAnimator;
        private void Awake() {
            _characterController = GetComponent<CharacterController>();
        }

        public override void OnStartAuthority()
        {
            base.OnStartAuthority();
            _animator = GetComponent<Animator>();
            _networkAnimator = GetComponent<NetworkAnimator>();
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
            /*
            * INPUTS
            */
            float forward  = Input.GetAxisRaw("Vertical");
            float rotation = Input.GetAxisRaw("Horizontal");
            bool  isJump  = Input.GetKeyDown(KeyCode.Space);
            bool isLeft = Input.GetKey(KeyCode.Q);
            bool isRight = Input.GetKey(KeyCode.E);
            bool isDash = Input.GetKeyDown(KeyCode.Tab);//Input.GetButtonDown("Fire2");
            
            /*
            * FWD/BWD & LATERAL
            * X: left/right, Y: up/down, Z: fw/bw. 
            */
            Vector3 next = new Vector3(0f, 0f, forward * Time.deltaTime * _moveRate);     

            if (isLeft && !isRight) {
                next += new Vector3 (-Time.deltaTime * _moveRate * 0.8f ,0f,0f);
            } else if (!isLeft && isRight){
                next += new Vector3 (Time.deltaTime * _moveRate * 0.8f ,0f,0f);
            }      


            /*
            * JUMP + DOUBLEJUMP
            */
            if(_jumping == 1 && isJump)
            {
                // _canDoubleJump = false;
                _networkAnimator.SetTrigger("JumpTrigger");
                _jumping = 2;
                _currentJumpTime = 0f;
            }
            if (isJump && _characterController.isGrounded) 
            {
                _networkAnimator.SetTrigger("JumpTrigger");
                _jumping = 1;
                _currentJumpTime = 0f;
            }
            if(_jumping > 0)
            {
                if(_currentJumpTime <_maxJumpTime && _jumping == 1 )
                {
                    next +=  new Vector3(0f, Time.deltaTime * _jumpForce, 0f);
                }
                if (_currentJumpTime <_maxJumpTime && _jumping == 2)
                {
                    next +=  new Vector3(0f, Time.deltaTime * _jumpForce, 0f);
                }
                if (_currentJumpTime >= _maxJumpTime && _jumping == 2)
                {
                    _jumping = 0;
                }
                _currentJumpTime += 0.1f;
            }   

            /*
            * DASH
            */
            if(isDash && _canDash) {
                _dashing = true;
                _currentDashTime = 0f;
                _canDash = false;
            } 
            if(_dashing)
            {
                if(_currentDashTime < _maxDashTime)
                {
                    next+= new Vector3 (0f,0f,_dashForce * Time.deltaTime);
                    _currentDashTime += 0.1f;
                }
                else
                {
                    _dashing = false;
                }
            }
            if (!_dashing && !_canDash)
            {
                if(_currentDashTime < (_maxDashTime + _dashingCooldown))
                {
                    _currentDashTime += 0.1f;
                }
                else
                {
                    _canDash = true;
                }
            }


            /*
            * GRAVITY
            */
            next += 0.4f * Physics.gravity * Time.deltaTime; 
            // More realistic option, to have parabolic vertical move (accelerated), but we sall change also jump. 
            // _characterController.SimpleMove(Physics.gravity * Time.deltaTime * 0.1f);


            /*
            * ROTATION 
            */
            transform.Rotate(new Vector3(0f, rotation * Time.deltaTime * _rotateRate, 0f));
            _characterController.Move(transform.TransformDirection(next));
            // _characterController.Move(next);

            /// <summary>
            /// Animator shit
            /// </summary>
            /// <returns></returns>
            
            _animator.SetFloat("Forward", next.z);
            _animator.SetFloat("Lateral", next.x);
            _animator.SetFloat("Jump", next.y);
            if (_characterController.isGrounded)
                _animator.SetTrigger("Grounded");
                

        }

        public bool GetDashing () {
            return _dashing;
        }

        private void OnTriggerEnter(Collider other) {
            if(other.gameObject.tag == "Character")
            {
                Debug.Log("GOTIT");
            }
        }
        // public void OnCollisionEnter(Collision other) {
        //     if(other.gameObject.tag == "Player")
        //     {
        //         Debug.Log("GOTIT");
        //     }
        // }

        // [Command]
        // public void CmdKnockBack(Vector3 direction){
        //     _characterController.Move(transform.TransformDirection(direction * Time.deltaTime * 50f));
        // }
    }
}