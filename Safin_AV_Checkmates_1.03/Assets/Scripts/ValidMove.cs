using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Checks
{
    public class ValidMove : MonoBehaviour
    {
        private bool isKing = false;

        private MoveController selectedCheck;
        private ColorType selectedCheckColorValidMove;
        private Component component;

        private void Awake()
        {

        }
        void Update()
        {

        }

        public bool MoveApproved(GameObject[,] checkOnBoard, int x1, int y1, int x2, int y2)
        {
            selectedCheckColorValidMove = FindObjectOfType<MoveController>().SelectedCheckColor;

            int deltaMoveX = Mathf.Abs(x1 - x2);
            int deltaMoveY = (y2 - y1);

            if (checkOnBoard[x2, y2] != null)
            {
                return false;
            }

            if ((selectedCheckColorValidMove == ColorType.Black && MoveController.IsBlackTurn == true) || isKing)
            {
                if (deltaMoveX == 1)
                {
                    if (deltaMoveY == 1)
                        return true;
                }
                else if (deltaMoveX == 2)
                {
                    if (deltaMoveY == 2)
                    {
                        GameObject checkBetweenMove = checkOnBoard[(x1 + x2) / 2, (y1 + y2) / 2];
                        ColorType checkColor = checkBetweenMove.GetComponent<ChipComponent>().GetColor;
                        if (checkBetweenMove != null && checkColor != ColorType.Black)
                            return true;
                    }
                }
            }

            if ((selectedCheckColorValidMove == ColorType.White && MoveController.IsBlackTurn == false) || isKing)
            {
                if (deltaMoveX == 1)
                {
                    if (deltaMoveY == -1)
                        return true;
                }
                else if (deltaMoveX == 2)
                {
                    if (deltaMoveY == -2)
                    {
                        GameObject checkBetweenMove = checkOnBoard[(x1 + x2) / 2, (y1 + y2) / 2];
                        ColorType checkColor = checkBetweenMove.GetComponent<ChipComponent>().GetColor;
                        if (checkBetweenMove != null && checkColor != ColorType.White)
                            return true;
                    }
                }
            }

            return false;
        }

        //Forcing to kill Enemy check, if it's possible
        public bool IsForcedToKill(GameObject[,] board, int x, int y)
        {
            if(selectedCheckColorValidMove == ColorType.Black)
            {
                //for top left check
                if(x >=2 && y <= 5)
                {
                    GameObject check = board[x - 1, y + 1];

                    //if there is an enemy check
                    if(check != null && selectedCheckColorValidMove != ColorType.Black)
                    {
                        //if there is a space after enemy check
                        if (board[x - 2, y + 2] == null)
                            return true;
                    }
                }

                //for top right check
                if(x <= 5 && y <=5)
                {
                    GameObject check = board[x - 1, y - 1];

                    //if there is an enemy check
                    if (check != null && selectedCheckColorValidMove != ColorType.White)
                    {
                        //if there is a space after enemy check
                        if (board[x - 2, y - 2] == null)
                            return true;
                    }
                }
            }

            if (selectedCheckColorValidMove == ColorType.White)
            {
                //for bottom left check
                if (x >= 2 && y >= 2)
                {
                    GameObject check = board[x - 1, y - 1];

                    //if there is an enemy check
                    if (check != null && selectedCheckColorValidMove != ColorType.White)
                    {
                        //if there is a space after enemy check
                        if (board[x - 2, y - 2] == null)
                            return true;
                    }
                }

                //for bottom right check
                if (x <= 5 && y >= 2)
                {
                    GameObject check = board[x + 1, y - 1];

                    //if there is an enemy check
                    if (check != null && selectedCheckColorValidMove != ColorType.White)
                    {
                        //if there is a space after enemy check
                        if (board[x + 2, y - 2] == null)
                            return true;
                    }
                }
            }

            return false;
        }
    }
}

