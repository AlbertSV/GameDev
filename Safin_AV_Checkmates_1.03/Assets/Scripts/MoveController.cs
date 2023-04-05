using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Checks
{
    public class MoveController : MonoBehaviour
    {
        private ColorType selectedCheckColor;
        private GameObject selectedCheck;
        private FieldCreation fieldCreation;

        private Vector2 startPosition;
        private Vector2 endPosition;
        private Vector2 mouseOver;

        private GameObject[,] checksArray;

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

            if(selectedCheck != null)
            {
                UpdatePieceDrag(selectedCheck);
            }

            if(Input.GetMouseButtonUp(0))
            {
                FindSelected();
                TryMove((int)startPosition.x, (int)startPosition.y, (int)endPosition.x, (int)endPosition.y);
            }

            endPosition.x = mouseOver.x;
            endPosition.y = mouseOver.y;
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
            selectedCheck = FindObjectOfType<Selected>().gameObject;
            selectedCheckColor = selectedCheck.GetComponent<ChipComponent>().GetColor;
            startPosition.x = selectedCheck.transform.position.x + 0.5f;
            startPosition.y = selectedCheck.transform.position.z + 0.5f;
        }

        //Moving availability check
        private void TryMove(int x1, int y1, int x2, int y2)
        {
            ValidMove selectedCheckValidMove = selectedCheck.GetComponent<ValidMove>();
            checksArray = fieldCreation.ChecksArray;

            //forcedToKill = ScanForChecksToDestroy();

            //check if we are out of bounds
            if (x2 < 0 || x2 >= 8 || y2 < 0 || y2 >= 8)
            {
                if (selectedCheck != null)
                {
                    MoveChecks(selectedCheck, x1, y1);
                }

                startPosition = Vector2.zero;
                selectedCheck = null;
                ChipComponent.IsClicked = false;
            }

            //if the check wasn't moved
            if (selectedCheck != null)
            {
                if (endPosition == startPosition)
                {
                    MoveChecks(selectedCheck, x1, y1);
                    startPosition = Vector2.zero;
                    selectedCheck = null;
                    ChipComponent.IsClicked = false;
                }
            }

            //check if its a valid move
            if (selectedCheckValidMove.MoveApproved(checksArray, x1, y1, x2, y2))
            {
                //Did the checked killed?
                //if this is a jump
                if(Mathf.Abs(x1-x2) == 2)
                {
                    GameObject checkBetweenMove = checksArray[(x1 + x2) / 2, (y1 + y2) / 2];
                    if (checkBetweenMove != null)
                    {
                        checksArray[(x1 + x2) / 2, (y1 + y2) / 2] = null;
                        Destroy(checkBetweenMove.gameObject);
                    }
                }

                MoveChecks(selectedCheck, x2, y2);
                checksArray[x2, y2] = selectedCheck;
                checksArray[x1, y1] = null;
                isBlackTurn = !isBlackTurn;

            }
            else
            {
                MoveChecks(selectedCheck, x1, y1);
                startPosition = Vector2.zero;
                selectedCheck = null;
                ChipComponent.IsClicked = false;
            }

            if (forcedToKill.Count != 0 && !hasKilled)
            {
                MoveChecks(selectedCheck, x1, y1);
                startPosition = Vector2.zero;
                selectedCheck = null;
                ChipComponent.IsClicked = false;

            }
        }

        private void MoveChecks(GameObject check, int x, int y)
        {
            check.transform.position = Vector3.right * x + Vector3.forward * y + Vector3.up * 0.1f;
        }
    }
}

