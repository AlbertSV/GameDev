using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Arkanoid
{
    public class MoveControl : MonoBehaviour
    {
        #region
        private MoveContrl playerMoves;
        private PlayerSide.PlayerSidePick playerSide;
        private Vector2 moveInput;
        #endregion


        private void Awake()
        {
            playerMoves = new MoveContrl();

            if (GetComponent<PlayerSide>() != null)
            {
                playerSide = GetComponent<PlayerSide>().GetNumber;
            }

        }


        //start Player control on enable
        private void OnEnable()
        {
            playerMoves.PlayerController.Enable();
        }

        private void FixedUpdate()
        {

            if (playerSide == PlayerSide.PlayerSidePick.FirstPlayer)
            {
                
                moveInput = playerMoves.PlayerController.PlayerFirstMove.ReadValue<Vector2>();
                Debug.Log(gameObject);
                Move(gameObject.GetComponent<Rigidbody>());
            }
            else if (playerSide == PlayerSide.PlayerSidePick.SecondPlayer)
            {
                moveInput = playerMoves.PlayerController.PlayerSecondMove.ReadValue<Vector2>();
                Move(gameObject.GetComponent<Rigidbody>());
            }
           
        }

        private void OnDisable()
        {
            playerMoves.PlayerController.Disable();
        }

        //Players move control
        private void Move(Rigidbody rigidbody)
        {
            rigidbody.AddForce(moveInput.x * GameControl.Manager.playerSpeed * transform.right);
            rigidbody.AddForce(moveInput.y * GameControl.Manager.playerSpeed * transform.up);
        }
    }
}