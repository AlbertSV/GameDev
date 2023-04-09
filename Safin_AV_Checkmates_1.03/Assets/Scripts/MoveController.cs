using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Checks
{
    public class MoveController : MonoBehaviour
    {
        private ColorType selectedCheckColor;
        private GameObject selectedCheckMove;
        private FieldCreation fieldCreation;
        private ValidMove selectedCheckValidMove;

        private Vector2 startPosition;
        private Vector2 endPosition;
        private Vector2 mouseOver;

        private GameObject[,] checksArrayMove;

        private static bool isBlackTurn = true;
        private bool hasKilled = true;

        private List<GameObject> forcedToKill;

        public static bool IsBlackTurn
        {
            get { return isBlackTurn; }
            set { isBlackTurn = value; }
        }

        public ColorType SelectedCheckColor
        {
            get { return selectedCheckColor; }
        }

        private void Update()
        {
            UpdateMouseOver();

            if(selectedCheckMove != null)
            {
                //UpdatePieceDrag(selectedCheckMove);
            }



            if (Input.GetMouseButtonDown(0))
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

        private void UpdatePieceDrag(GameObject selected)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50.0f, LayerMask.GetMask("Board")))
            {
                selected.transform.position = hit.point + Vector3.up;
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
            //Debug.Log(selectedCheckMove.GetComponent<ValidMove>());
            Debug.Log("you are in trymove");
            selectedCheckValidMove = selectedCheckMove.GetComponent<ValidMove>();

            checksArrayMove = new GameObject[8, 8];
            checksArrayMove = FindObjectOfType<FieldCreation>().ChecksArray;


            forcedToKill = ScanForChecksToDestroy();

            //check if we are out of bounds
            if (x2 < 0 || x2 >= 8 || y2 < 0 || y2 >= 8)
            {
                Debug.Log("Out of bonds");
                if (selectedCheckMove != null)
                {
                    MoveChecks(selectedCheckMove, x1, y1);
                }

                startPosition = Vector2.zero;
                Destroy(selectedCheckMove.GetComponent<Selected>());
                selectedCheckMove = null;
                ChipComponent.IsClicked = false;
            }
            
            
            //check if its a valid move
            else if (selectedCheckValidMove.MoveApproved(checksArrayMove, x1, y1, x2, y2))
            {
                Debug.Log("valid moved");
                //Did the checked killed?
                //if this is a jump
                if(Mathf.Abs(x1-x2) == 2)
                {
                    GameObject checkBetweenMove = checksArrayMove[(x1 + x2) / 2, (y1 + y2) / 2];
                    if (checkBetweenMove != null)
                    {
                        checksArrayMove[(x1 + x2) / 2, (y1 + y2) / 2] = null;
                        Destroy(checkBetweenMove.gameObject);
                    }
                }

                MoveChecks(selectedCheckMove, x2, y2);
                checksArrayMove[x2, y2] = selectedCheckMove;
                checksArrayMove[x1, y1] = null;
                Destroy(selectedCheckMove.GetComponent<Selected>());
                selectedCheckMove = null;
                ChipComponent.IsClicked = false;
                isBlackTurn = !isBlackTurn;

            }

            //if the check wasn't moved
            else if (selectedCheckMove != null && endPosition == startPosition)
            {
                Debug.Log("wasn't moved");

                {
                    Debug.Log("Boo,2");
                    MoveChecks(selectedCheckMove, x1, y1);
                    startPosition = Vector2.zero;
                    Destroy(selectedCheckMove.GetComponent<Selected>());
                    selectedCheckMove = null;
                    ChipComponent.IsClicked = false;
                }
            }

            else
            {
                Debug.Log("default");
                MoveChecks(selectedCheckMove, x1, y1);
                startPosition = Vector2.zero;
                Destroy(selectedCheckMove.GetComponent<Selected>());
                selectedCheckMove = null;
                ChipComponent.IsClicked = false;
            }

            
            if (forcedToKill.Count != 0 && !hasKilled)
            {
                MoveChecks(selectedCheckMove, x1, y1);
                startPosition = Vector2.zero;
                Destroy(selectedCheckMove.GetComponent<Selected>());
                selectedCheckMove = null;
                ChipComponent.IsClicked = false;

            }
        }

        private void MoveChecks(GameObject check, int x, int y)
        {
            Debug.Log("you are in move check");
            Debug.Log(Vector3.right * x);
            check.transform.position = Vector3.right * x + Vector3.forward * y + Vector3.up * 0.1f;
        }

        private List<GameObject> ScanForChecksToDestroy()
        {
            forcedToKill = new List<GameObject>();

            //check all checkers
            for (int i=0; i<8; i++)
                for (int j=0; j <8; j++)
                    if (checksArrayMove[i,j] != null &&
                        ((checksArrayMove[i,j].GetComponent<ChipComponent>().GetColor == ColorType.Black && isBlackTurn) ||
                        (checksArrayMove[i,j].GetComponent<ChipComponent>().GetColor == ColorType.White && !isBlackTurn)))
                    {
                        forcedToKill.Add(checksArrayMove[i, j]);
                    }
            return forcedToKill;
        }

    }
}

