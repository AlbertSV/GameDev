using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Checks
{
    public class ChipComponent : BaseClickComponent
    {
        //private features
        #region
        private static bool isClicked = false;
        private static bool cellFocusedAdded = true;

        private bool addFocusMaterial = false;

        private ColorType selectedCheckColorFocus;
        private GameObject selectedCheck;
        private MeshRenderer cellRenderer;

        private GameObject[,] arrayChecks;
        private GameObject[,] arrayCells;
        private Material[] cellMaterial = new Material[2];
        #endregion

        public static bool IsClicked
        {
            get { return isClicked; }
            set { isClicked = value; }
        }
        public static bool CellFocusAdded
        {
            get { return cellFocusedAdded; }
            set { cellFocusedAdded = value; }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnClickEventHandler += ClickController;
                RemoveCellMaterial();
            }
            if (Input.GetMouseButtonUp(0))
            {
                OnClickEventHandler -= ClickController;
            }

            if(isClicked == false && addFocusMaterial == false)
            {
                RemoveAdditionalMaterial();
                cellFocusedAdded = false;
                RemoveCellMaterial();
                Destroy(FindObjectOfType<Selected>());
             
            }

            if (isClicked == false && cellFocusedAdded == true)
            {
                cellFocusedAdded = false;
                RemoveCellMaterial();
            }

        }

        //add focus material to the check
        public override void OnPointerEnter(PointerEventData eventData)
        {
            CallBackEvent((CellComponent)Pair, true);

            if (isClicked == false)
            {
                AddAdditionalMaterial(focusMaterial);
                _mesh.material = _mesh.materials[1];
                addFocusMaterial = true;
            }
        }
        //remove focus material to the check
        public override void OnPointerExit(PointerEventData eventData)
        {
            CallBackEvent((CellComponent)Pair, false);

            if (addFocusMaterial)
            {
                RemoveAdditionalMaterial();
                addFocusMaterial = false;
            }

        }


        //material control on the click
        private void ClickController()
        {
            //add material for check, if it's clicked
            AddAdditionalMaterial(clickMaterial);
            _mesh.material = _meshMaterials[1];
            _mesh.gameObject.AddComponent<Selected>();
            addFocusMaterial = false;

            isClicked = !isClicked;

            selectedCheck = FindObjectOfType<Selected>().gameObject;
            selectedCheckColorFocus = selectedCheck.GetComponent<ChipComponent>().GetColor;
            arrayCells = FindObjectOfType<FieldCreation>().CellArray;
            arrayChecks = FindObjectOfType<FieldCreation>().ChecksArray;

            int x = (int)selectedCheck.transform.position.x;
            int y = (int)selectedCheck.transform.position.z;

            //for top cell
            if (selectedCheckColorFocus == ColorType.Black || selectedCheck.GetComponent<isKing>() != null)
            {
                //for left
                if (x >= 1 && y <= 6)
                {

                    GameObject leftFromBlack = arrayChecks[x - 1, y + 1];
                    if (leftFromBlack == null)
                    {
                        cellRenderer = arrayCells[x - 1, y + 1].GetComponent<MeshRenderer>();
                        AddAdditionalMaterial(cellRenderer, cellMaterial, focusMaterial);

                        arrayCells[x - 1, y + 1].AddComponent<CellIsFocused>();
                        cellFocusedAdded = true;
                    }
                    else if (leftFromBlack != null && ((leftFromBlack.GetComponent<ChipComponent>().GetColor == ColorType.White && selectedCheckColorFocus == ColorType.Black) ||
                        (selectedCheck.GetComponent<isKing>() != null && leftFromBlack.GetComponent<ChipComponent>().GetColor == ColorType.Black && selectedCheckColorFocus == ColorType.White)))
                    {
                        if (x >= 2 && y <= 5)
                        {
                            GameObject doubleLeftForBlack = arrayChecks[x - 2, y + 2];
                            if (doubleLeftForBlack == null)
                            {
                                cellRenderer = arrayCells[x - 2, y + 2].GetComponent<MeshRenderer>();
                                AddAdditionalMaterial(cellRenderer, cellMaterial, focusMaterial);
                                arrayCells[x - 2, y + 2].AddComponent<CellIsFocused>();
                                cellFocusedAdded = true;
                            }
                        }
                    }
                }

                
                //for right
                if (x <= 6 && y <= 6)
                {
                    GameObject rightFromBalck = arrayChecks[x + 1, y + 1];
                    if (rightFromBalck == null)
                    {
                        cellRenderer = arrayCells[x + 1, y + 1].GetComponent<MeshRenderer>();
                        AddAdditionalMaterial(cellRenderer, cellMaterial, focusMaterial);
                        arrayCells[x + 1, y + 1].AddComponent<CellIsFocused>();
                        cellFocusedAdded = true;

                    }
                    else if (rightFromBalck != null && ((rightFromBalck.GetComponent<ChipComponent>().GetColor == ColorType.White && selectedCheckColorFocus == ColorType.Black) ||
                        (selectedCheck.GetComponent<isKing>() != null && rightFromBalck.GetComponent<ChipComponent>().GetColor == ColorType.Black && selectedCheckColorFocus == ColorType.White)))
                    {

                        if (x <= 5 && y <= 5)
                        {
                            GameObject doubleRightForBlack = arrayChecks[x + 2, y + 2];
                            if (doubleRightForBlack == null)
                            {
                                cellRenderer = arrayCells[x + 2, y + 2].GetComponent<MeshRenderer>();
                                AddAdditionalMaterial(cellRenderer, cellMaterial, focusMaterial);
                                arrayCells[x + 2, y + 2].AddComponent<CellIsFocused>();
                                cellFocusedAdded = true;
                            }
                        }
                    }
                }

            }

            //for bottom cell
            if (selectedCheckColorFocus == ColorType.White || selectedCheck.GetComponent<isKing>() != null)
            {
                //for left
                if (x >= 1 && y >= 1)
                {
                    GameObject leftFromWhite = arrayChecks[x - 1, y - 1];

                    if (leftFromWhite == null)
                    {
                        cellRenderer = arrayCells[x - 1, y - 1].GetComponent<MeshRenderer>();
                        AddAdditionalMaterial(cellRenderer, cellMaterial, focusMaterial, 1);
                        arrayCells[x - 1, y - 1].AddComponent<CellIsFocused>();
                        cellFocusedAdded = true;

                    }
                    else if (leftFromWhite != null && ((leftFromWhite.GetComponent<ChipComponent>().GetColor == ColorType.Black && selectedCheckColorFocus == ColorType.White) ||
                       (selectedCheck.GetComponent<isKing>() != null && leftFromWhite.GetComponent<ChipComponent>().GetColor == ColorType.White && selectedCheckColorFocus == ColorType.Black)))
                    {
                        if (x >= 2 && y >= 2)
                        {
                            GameObject doubleLeftForWhite = arrayChecks[x - 2, y - 2];
                            if (doubleLeftForWhite == null)
                            {
                                cellRenderer = arrayCells[x - 2, y - 2].GetComponent<MeshRenderer>();
                                AddAdditionalMaterial(cellRenderer, cellMaterial, focusMaterial, 1);
                                arrayCells[x - 2, y - 2].AddComponent<CellIsFocused>();
                                cellFocusedAdded = true;
                            }
                        }
                    }
                }

                //for right
                if (x <= 6 && y >= 1)
                {
                    GameObject rightFromWhite = arrayChecks[x + 1, y - 1];
                    if (rightFromWhite == null)
                    {
                        cellRenderer = arrayCells[x + 1, y - 1].GetComponent<MeshRenderer>();
                        AddAdditionalMaterial(cellRenderer, cellMaterial, focusMaterial, 1);
                        arrayCells[x + 1, y - 1].AddComponent<CellIsFocused>();
                        cellFocusedAdded = true;

                    }
                    else if (rightFromWhite != null && ((rightFromWhite.GetComponent<ChipComponent>().GetColor == ColorType.Black && selectedCheckColorFocus == ColorType.White) ||
                       (selectedCheck.GetComponent<isKing>() != null && rightFromWhite.GetComponent<ChipComponent>().GetColor == ColorType.White && selectedCheckColorFocus == ColorType.Black)))
                    {

                        if (x <= 5 && y >= 2)
                        {
                            GameObject doubleRightForWhite = arrayChecks[x + 2, y - 2];
                            if (doubleRightForWhite == null)
                            {
                                cellRenderer = arrayCells[x + 2, y - 2].GetComponent<MeshRenderer>();
                                AddAdditionalMaterial(cellRenderer, cellMaterial, focusMaterial, 1);
                                arrayCells[x + 2, y - 2].AddComponent<CellIsFocused>();
                                cellFocusedAdded = true;
                            }
                        }
                    }
                }
            }
        }

        //removing cells added material
        public void RemoveCellMaterial()
        {
            Debug.Log(cellFocusedAdded);
            if (cellFocusedAdded == false)
            {
                Debug.Log(cellFocusedAdded);
                if (selectedCheck == null)
                {
                    CellIsFocused[] cellsWithFocus = FindObjectsOfType<CellIsFocused>();
                    foreach (CellIsFocused k in cellsWithFocus)
                    {
                        Material[] cellMaterial = k.gameObject.GetComponent<MeshRenderer>().materials;
                        Destroy(cellMaterial[1]);
                        //cellMaterial[0]  = Resources.Load<Material>("Materials/BlackCell");
                        Destroy(k.GetComponent<CellIsFocused>());
                    }
                }
                
            }
        }
    }
}
