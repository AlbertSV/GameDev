using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Checks
{
    public class ValidMove : MonoBehaviour
    {
        //private features
        #region
        private ColorType selectedCheckColorValidMove;
        private GameObject selectedCheck;
        private Component component;
        #endregion


        //check if the move is possible
        public bool MoveApproved(GameObject[,] checkOnBoard, int x1, int y1, int x2, int y2)
        {
            selectedCheckColorValidMove = MoveController.SelectedCheckColor;
            selectedCheck = FindObjectOfType<Selected>().gameObject;

            int deltaMoveX = Mathf.Abs(x1 - x2);
            int deltaMoveY = (y2 - y1);

            if (checkOnBoard[x2, y2] != null)
            {
                return false;
            }

            //check if the position to move is right
            if ((selectedCheckColorValidMove == ColorType.Black && MoveController.IsBlackTurn == true) && selectedCheck.GetComponent<isKing>() == null)
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

            if ((selectedCheckColorValidMove == ColorType.White && MoveController.IsBlackTurn == false) && selectedCheck.GetComponent<isKing>() == null)
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
            
            //if check is King
            if (selectedCheck.GetComponent<isKing>() != null && ((selectedCheckColorValidMove == ColorType.Black && MoveController.IsBlackTurn == true) || (selectedCheckColorValidMove == ColorType.White && MoveController.IsBlackTurn == false)))
            {
                if (deltaMoveX == 1)
                {
                    if (deltaMoveY == 1)
                        return true;
                    else if (deltaMoveY == -1)
                        return true;
                }
                else if (deltaMoveX == 2)
                {
                    if (deltaMoveY == 2 || deltaMoveY == -2)
                    {
                        GameObject checkBetweenMove = checkOnBoard[(x1 + x2) / 2, (y1 + y2) / 2];
                        ColorType checkColor = checkBetweenMove.GetComponent<ChipComponent>().GetColor;


                        if (checkBetweenMove != null && checkColor != ColorType.Black && selectedCheckColorValidMove == ColorType.Black)
                            return true;
                        else if (checkBetweenMove != null && checkColor == ColorType.Black && selectedCheckColorValidMove == ColorType.White)
                            return true;
                    }
                }
            }
            return false;
        }

        //Forcing to kill Enemy check, if it's possible
        public bool IsForcedToKill(GameObject[,] board, int x, int y)
        {

            if (board[x,y].GetComponent<ChipComponent>().GetColor == ColorType.Black)
            {
                //for top right check
                if (x <=5 && y <= 5)
                {
                    GameObject check = board[x + 1, y + 1];
                    //if there is an enemy check
                    if (check != null && check.GetComponent<ChipComponent>().GetColor == ColorType.White)
                    {
                        //if there is a space after enemy check
                        if (board[x + 2, y + 2] == null)
                            return true;
                    }
                }

                //for top left check
                if(x >= 2 && y <=5)
                {
                    GameObject check = board[x - 1, y + 1];
                    //if there is an enemy check
                    if (check != null && check.GetComponent<ChipComponent>().GetColor == ColorType.White)
                    {
                        //if there is a space after enemy check
                        if (board[x - 2, y + 2] == null)
                            return true;
                    }
                }
            }

            if (board[x, y].GetComponent<ChipComponent>().GetColor == ColorType.White)
            {
                //for bottom left check
                if (x >= 2 && y >= 2)
                {
                    GameObject check = board[x - 1, y - 1];

                    //if there is an enemy check
                    if (check != null && check.GetComponent<ChipComponent>().GetColor == ColorType.Black)
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
                    if (check != null && check.GetComponent<ChipComponent>().GetColor == ColorType.Black)
                    {
                        //if there is a space after enemy check
                        
                        if (board[x + 2, y - 2] == null)
                            return true;
                    }
                }
            }
            
            if (board[x,y].GetComponent<isKing>() != null)
            {
                //y = -y;
                //for left check

                if (x >= 2)
                {
                    GameObject checkTop;
                    GameObject checkBottom;

                    if (y != 0)
                    {
                         checkBottom = board[x - 1, y - 1];
                    }
                    else
                    {
                        checkBottom = null;
                    }
                    if (y != 7)
                    {
                         checkTop = board[x - 1, y + 1];
                    }
                    else
                    {
                        checkTop = null;
                    }

                    //if there is an enemy check in bottom left
                    if (y >= 2)
                    {
                        if (checkBottom != null && ((checkBottom.GetComponent<ChipComponent>().GetColor == ColorType.Black && board[x, y].GetComponent<ChipComponent>().GetColor == ColorType.White)
                            || (checkBottom.GetComponent<ChipComponent>().GetColor == ColorType.White && board[x, y].GetComponent<ChipComponent>().GetColor == ColorType.Black)))
                        {
                            //if there is a space after enemy check

                            if (board[x - 2, y - 2] == null)
                                return true;
                        }
                    }

                    if (y <= 5)
                    {
                        if (checkTop != null && ((checkTop.GetComponent<ChipComponent>().GetColor == ColorType.Black && board[x, y].GetComponent<ChipComponent>().GetColor == ColorType.White)
                            || (checkTop.GetComponent<ChipComponent>().GetColor == ColorType.White && board[x, y].GetComponent<ChipComponent>().GetColor == ColorType.Black)))
                        {
                            //if there is a space after enemy check

                            if (board[x - 2, y + 2] == null)
                                return true;
                        }
                    }
                }

                if (x <= 5)
                {
                    GameObject checkTop;
                    GameObject checkBottom;

                    if (y != 0)
                    {
                        checkBottom = board[x + 1, y - 1];
                    }
                    else
                    {
                        checkBottom = null;
                    }
                    if (y != 7)
                    {
                        checkTop = board[x + 1, y + 1];
                    }
                    else
                    {
                        checkTop = null;
                    }
                    

                    //if there is an enemy check in bottom left
                    if (y >= 2)
                    {
                        if (checkBottom != null && ((checkBottom.GetComponent<ChipComponent>().GetColor == ColorType.Black && board[x, y].GetComponent<ChipComponent>().GetColor == ColorType.White)
                            || (checkBottom.GetComponent<ChipComponent>().GetColor == ColorType.White && board[x, y].GetComponent<ChipComponent>().GetColor == ColorType.Black)))
                        {
                            //if there is a space after enemy check

                            if (board[x + 2, y - 2] == null)
                                return true;
                        }
                    }

                    if (y <= 5)
                    {
                        if (checkTop != null && ((checkTop.GetComponent<ChipComponent>().GetColor == ColorType.Black && board[x, y].GetComponent<ChipComponent>().GetColor == ColorType.White)
                            || (checkTop.GetComponent<ChipComponent>().GetColor == ColorType.White && board[x, y].GetComponent<ChipComponent>().GetColor == ColorType.Black)))
                        {
                            //if there is a space after enemy check

                            if (board[x + 2, y + 2] == null)
                                return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}

