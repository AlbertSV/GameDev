using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Arkanoid
{
    public class MoveControl : MonoBehaviour
    {

        private MoveContrl playerMoves;
        private PlayerSide.PlayerSidePick playerSide;
        private Vector2 moveInput;
        private Rigidbody rigidbody;
        private GameObject ball;
        private bool needToMove = false;
        private Vector3 lastVelocity;
        private bool needChangeDirection = false;
        private bool needParent = true;
        private bool collisionEnter = false;
        private Vector3 direction;

        public bool NeedToMove
        {
            get { return needToMove; }
            set { needToMove = value; }
        }

        public bool NeedChangeDirection
        {
            get { return needChangeDirection; }
            set { needChangeDirection = value; }
        }


        private void Awake()
        {
            playerMoves = new MoveContrl();

            if (GetComponent<PlayerSide>() != null)
            {
                playerSide = GetComponent<PlayerSide>().GetNumber;
            }
            rigidbody = GetComponent<Rigidbody>();
            if (rigidbody == null)
                Debug.Log("RigidBody is null");

        }
        private void Start()
        {
            ball = GameControl.Manager.ball;
        }

        private void Update()
        {
            lastVelocity = ball.GetComponent<Rigidbody>().velocity;
            Debug.Log(collisionEnter);
            Debug.Log(NeedToMove);
            SetBallParent();
            SetBallMove(direction);

        }

        private void OnEnable()
        {
            playerMoves.PlayerController.Enable();

            if (playerSide == PlayerSide.PlayerSidePick.SecondPlayer)
            {
                playerMoves.PlayerController.PlayerSecondShoot.performed += OnShoot;
            }
            else
            {
                playerMoves.PlayerController.PlayerFirstShoot.performed += OnShoot;
            }

        }

        private void FixedUpdate()
        {

            if (playerSide == PlayerSide.PlayerSidePick.FirstPlayer)
            {
                moveInput = playerMoves.PlayerController.PlayerFirstMove.ReadValue<Vector2>();

                Move();
            }
            else
            {
                moveInput = playerMoves.PlayerController.PlayerSecondMove.ReadValue<Vector2>();

                Move();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            collisionEnter = true;
            SetBallBounce(collision);
        }

        private void OnDisable()
        {
            playerMoves.PlayerController.Disable();
            playerMoves.PlayerController.PlayerFirstShoot.performed -= OnShoot;
            playerMoves.PlayerController.PlayerSecondShoot.performed -= OnShoot;
        }
        private void OnShoot(CallbackContext context)
        {
           BallShoot();
        }

        private void Move()
        {
            rigidbody.AddForce(moveInput.x * GameControl.Manager.playerSpeed * transform.right);
            rigidbody.AddForce(moveInput.y * GameControl.Manager.playerSpeed * transform.up);
        }

        private void BallShoot()
        {
            if (ball != null && (GameControl.Manager.firstBallHolder != null || GameControl.Manager.secondBallHolder != null))
            {
                if (ball.transform.position == GameControl.Manager.firstBallHolder.position)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        ball.GetComponent<Rigidbody>().isKinematic = false;
                        ball.transform.SetParent(null);
                        NeedToMove = true;
                    }
                }
                else if (ball.transform.position == GameControl.Manager.secondBallHolder.position)
                {
                    if (Input.GetKeyDown(KeyCode.RightShift))
                    {
                        ball.GetComponent<Rigidbody>().isKinematic = false;
                        ball.transform.SetParent(null);
                        NeedToMove = true;
                    }
                }
            }
        }

        private void SetBallMove(Vector3 direction)
        {
            if (collisionEnter)
            {
                NeedToMove = false;
                Debug.Log(NeedToMove);
            }

            if (NeedToMove)
            {
                Debug.Log("1");
                ball.transform.position += (GameControl.Manager.ballSpeed * transform.forward * Time.deltaTime);
            }


            //if(needToMove && needChangeDirection == true)
            //{
            //    Debug.Log("2");

            //    if (direction != null)
            //    {
            //        Debug.Log("sda");
            //        ball.transform.position += (direction * GameControl.Manager.ballSpeed);
            //    }
            //}

        }

        private void SetBallParent()
        {
            if (ball.transform.position == GameControl.Manager.firstBallHolder.position && needParent)
            {
                ball.GetComponent<Rigidbody>().isKinematic = true;
                ball.transform.parent = GameControl.Manager.firstBallHolder;
                if(ball.transform.parent != null)
                {
                    needParent = false;
                } 
            }
            else if (ball.transform.position == GameControl.Manager.secondBallHolder.position && needParent)
            {
                ball.GetComponent<Rigidbody>().isKinematic = true;
                ball.transform.SetParent(GameControl.Manager.secondBallHolder);
                if (ball.transform.parent != null)
                {
                    needParent = false;
                }
            }
            else if (ball.transform.position != GameControl.Manager.secondBallHolder.position || ball.transform.position != GameControl.Manager.firstBallHolder.position)
            {
                needParent = true;
            }


        }

        private void SetBallBounce(Collision coll)
        {
            //needChangeDirection = true;
            //direction = coll.contacts[0].normal;
            //var speedMagn = lastVelocity.magnitude;

            //var direction = Vector3.Reflect(transform.forward.normalized, coll.contacts[0].normal);
            NeedToMove = false;
            ball.transform.position += (coll.contacts[0].normal * GameControl.Manager.ballSpeed);

        }
    }
}