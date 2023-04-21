using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Checks
{
    public class MoveController : MonoBehaviour
    {
        //private features
        #region
        private ColorType selectedCheckColor = ColorType.Black;
        private GameObject selectedCheckMove;
        private FieldCreation fieldCreation;
        private ValidMove selectedCheckValidMove;

        private Vector2 startPosition;
        private Vector2 endPosition;
        private Vector2 mouseOver;

        private GameObject[,] checksArrayMove;

        private static bool cameraHasToMove = false;
        private static bool isBlackTurn = true;
        private bool hasKilled;

        private List<GameObject> forcedToKill;
        #endregion

        //properties
        #region
        public static bool IsBlackTurn
        {
            get { return isBlackTurn; }
            set { isBlackTurn = value; }
        }

        public static bool CameraHasToMove
        {
            get { return cameraHasToMove; }
            set { cameraHasToMove = value; }
        }

        public GameObject SelectedCheck
        {
            get { return selectedCheckMove; }
        }

        public ColorType SelectedCheckColor
        {
            get { return selectedCheckColor; }
        }
        #endregion

        private void Update()
        {
            UpdateMouseOver();

            if (Input.GetMouseButtonUp(0))
            {
                try
                {
                    FindSelected();
                }
                catch
                {
                    Debug.Log("Choose the check for the move");
                }
                endPosition.x = mouseOver.x;
                endPosition.y = mouseOver.y;

                TryMove((int)startPosition.x, (int)startPosition.y, (int)endPosition.x, (int)endPosition.y);

            }
        }


        //to catch the position of the mouse
        private void UpdateMouseOver()
        {
            RaycastHit hit;
            
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50.0f, LayerMask.GetMask("Board")))
            {
                mouseOver.x = (int)(hit.point.x + 0.5f);
                mouseOver.y = (int)(hit.point.z + 0.5f);
            }
            else
            {
                mouseOver.x = -1;
                mouseOver.y = -1;
            }
        }

        //Searching for the selected check
        private void FindSelected()
        {
            selectedCheckMove = FindObjectOfType<Selected>().gameObject;
            selectedCheckColor = selectedCheckMove.GetComponent<ChipComponent>().GetColor;
            startPosition.x = selectedCheckMove.transform.position.x + 0.5f;
            startPosition.y = selectedCheckMove.transform.position.z + 0.5f;
        }

        //Moving availability check
        private void TryMove(int x1, int y1, int x2, int y2)
        {
            if (selectedCheckMove != null)
            {
                selectedCheckValidMove = selectedCheckMove.GetComponent<ValidMove>();
            }

            checksArrayMove = new GameObject[8, 8];
            checksArrayMove = FindObjectOfType<FieldCreation>().ChecksArray;
            forcedToKill = ScanForChecksToDestroy();

            //check if we are out of bounds
            if (x2 < 0 || x2 >= 8 || y2 < 0 || y2 >= 8)
            {
                if (selectedCheckMove != null)
                {
                    MoveChecks(selectedCheckMove, x1, y1);
                    Destroy(selectedCheckMove.GetComponent<Selected>());
                    selectedCheckMove = null;
                    ChipComponent.IsClicked = false;
                    ChipComponent.CellFocusAdded = false;
                }

                startPosition = Vector2.zero;
            }
            
            
            //check if its a valid move
            else if (selectedCheckValidMove != null && selectedCheckValidMove.MoveApproved(checksArrayMove, x1, y1, x2, y2))
            {
                //Did the checked killed?
                //if this is a jump
                if (Mathf.Abs(x1 - x2) == 2)
                {
                    GameObject checkBetweenMove = checksArrayMove[(x1 + x2) / 2, (y1 + y2) / 2];
                    if (checkBetweenMove != null)
                    {
                        checksArrayMove[(x1 + x2) / 2, (y1 + y2) / 2] = null;
                        Destroy(checkBetweenMove.gameObject);
                        hasKilled = true;

                    }
                }

                //do we have to kill the checkers?
                if(forcedToKill.Count != 0 && hasKilled != true)
                {
                    MoveChecks(selectedCheckMove, x1, y1);
                    startPosition = Vector2.zero;
                    Destroy(selectedCheckMove.GetComponent<Selected>());
                    //selectedCheckMove = null;
                    ChipComponent.IsClicked = false;
                    ChipComponent.CellFocusAdded = false;
                }
                //you can move to the different cell
                else if (selectedCheckMove != null)
                {
                    MoveChecks(selectedCheckMove, x2, y2);
                    checksArrayMove[x2, y2] = selectedCheckMove;
                    checksArrayMove[x1, y1] = null;
                    Destroy(selectedCheckMove.GetComponent<Selected>());
                    //selectedCheckMove = null;
                    ChipComponent.IsClicked = false;
                    ChipComponent.CellFocusAdded = false;
                    cameraHasToMove = true;
                }
                //scan if we need to destroy enemy check
                if (ScanForChecksToDestroy(selectedCheckMove, x2, y2).Count != 0 && hasKilled)
                {
                    hasKilled = false;
                    return;
                }

                isBlackTurn = !isBlackTurn;
            }

            //if the check wasn't moved
            else if (selectedCheckMove != null && endPosition == startPosition)
            {
                {

                    MoveChecks(selectedCheckMove, x1, y1);
                    startPosition = Vector2.zero;
                    Destroy(selectedCheckMove.GetComponent<Selected>());
                    selectedCheckMove = null; 
                    ChipComponent.IsClicked = false;
                    ChipComponent.CellFocusAdded = false;
                    Debug.Log(ChipComponent.CellFocusAdded);
                }
            }

            EndTurn();
        }

        //to end the turn, clean up and check if there is King
        private void EndTurn()
        {
            forcedToKill = ScanForChecksToDestroy();

            if (selectedCheckMove != null)
            {

                int x = (int)endPosition.x;
                int y = (int)endPosition.y;

                if (selectedCheckColor == ColorType.Black && y == 7 && selectedCheckMove.GetComponent<isKing>() == null)
                {
                    selectedCheckMove.AddComponent<isKing>();
                    selectedCheckMove.transform.Rotate(Vector3.right * 180);
                }
                else if (selectedCheckColor == ColorType.White && y == 0 && selectedCheckMove.GetComponent<isKing>() == null)
                {
                    selectedCheckMove.AddComponent<isKing>();
                    selectedCheckMove.transform.Rotate(Vector3.right * 180);
                }
            }
            selectedCheckMove = null;
            hasKilled = false;
            CheckVictory();

        }

        //checking condition of victory
        private void CheckVictory()
        {
            var leftChecks = FindObjectsOfType<IsCheck>();
            bool hasWhite = false, hasBlack = false;
            bool blackWin = false;

            for (int i = 0; i < leftChecks.Length; i++)
            {
                if (leftChecks[i].GetComponent<ChipComponent>().GetColor == ColorType.Black)
                {
                    hasBlack = true;
                }
                else
                {
                    hasWhite = true;
                }
            }

            if(hasBlack == false)
            {
                blackWin = false;
                Victory(blackWin);
            }
            if(hasWhite == false)
            {
                blackWin = true;
                Victory(blackWin);
            }
        }
        //claim the victory
        private void Victory(bool color)
        {
            if (color)
            {
                Debug.Log("Player black has  won!");
            }
            else
            {
                Debug.Log("Player white has won!");
            }
        }

        //simple move of the check
        private void MoveChecks(GameObject check, int x, int y)
        {
            check.transform.position = Vector3.right * x + Vector3.forward * y + Vector3.up * 0.1f;
        }

        //scan to destroy enemy checks
        private List<GameObject> ScanForChecksToDestroy(GameObject check, int x, int y)
        {
            forcedToKill = new List<GameObject>();

            if (checksArrayMove[x, y].GetComponent<ValidMove>().IsForcedToKill(checksArrayMove, x, y))
            {
                forcedToKill.Add(checksArrayMove[x, y]);
            }
            return forcedToKill;
        }

        private List<GameObject> ScanForChecksToDestroy()
        {
            forcedToKill = new List<GameObject>();
            //check all checkers
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (checksArrayMove[i, j] != null &&
                        ((checksArrayMove[i, j].GetComponent<ChipComponent>().GetColor == ColorType.Black && isBlackTurn == true) ||
                        (checksArrayMove[i, j].GetComponent<ChipComponent>().GetColor == ColorType.White && isBlackTurn == false)))
                        if (checksArrayMove[i, j].GetComponent<ValidMove>().IsForcedToKill(checksArrayMove, i, j))
                        {

                            forcedToKill.Add(checksArrayMove[i, j]);
                        }
                }
            }
            return forcedToKill;
        }

    }
}

