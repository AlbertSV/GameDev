using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Checks
{
    public class ChipComponent : BaseClickComponent
    {
        private static bool isClicked = false;
        private static bool needRemove = false;
        private static bool cellFocusedAdded = false;

        private bool addFocusMaterial = false;

        private ColorType selectedCheckColorFocus;
        private GameObject selectedCheck;
        private MeshRenderer cellRenderer;

        private GameObject[,] arrayChecks;
        private GameObject[,] arrayCells;

        private Material[] cellMaterial = new Material[2];

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
            }
            if (Input.GetMouseButtonUp(0))
            {
                OnClickEventHandler -= ClickController;
            }

            if(isClicked == false && addFocusMaterial == false)
            {
                RemoveAdditionalMaterial();
             
            }

            if(cellFocusedAdded == false)
            {
                RemoveCellMaterial();
            }

        }

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

        public override void OnPointerExit(PointerEventData eventData)
        {
            CallBackEvent((CellComponent)Pair, false);

            if (addFocusMaterial)
            {
                RemoveAdditionalMaterial();
                addFocusMaterial = false;
            }

        }

        private void ClickController()
        {
            AddAdditionalMaterial(clickMaterial);
            _mesh.material = _meshMaterials[2];
            _mesh.gameObject.AddComponent<Selected>();
            addFocusMaterial = false;

            isClicked = !isClicked;
            _mesh.material = _mesh.materials[1];

            selectedCheck = FindObjectOfType<Selected>().gameObject;
            Debug.Log(selectedCheck);
            selectedCheckColorFocus = FindObjectOfType<MoveController>().SelectedCheckColor;
            Debug.Log(selectedCheckColorFocus);
            arrayCells = FindObjectOfType<FieldCreation>().CellArray;
            arrayChecks = FindObjectOfType<FieldCreation>().ChecksArray;

            int x = (int)selectedCheck.transform.position.x;
            int y = (int)selectedCheck.transform.position.z;

            //for top
            if (selectedCheckColorFocus == ColorType.Black)
            {
                Debug.Log("1s");
                //for left
                if (x >= 1 && y <= 6)
                {
                    Debug.Log("2");
                    GameObject leftFromBlack = arrayChecks[x - 1, y + 1];

                    Debug.Log(leftFromBlack);

                    if (leftFromBlack == null)
                    {
                        Debug.Log("3s");
                        cellRenderer = arrayCells[x - 1, y + 1].GetComponent<MeshRenderer>();
                        AddAdditionalMaterial(cellRenderer, cellMaterial, focusMaterial, 1);
                        arrayCells[x - 1, y + 1].AddComponent<CellIsFocused>();
                        cellFocusedAdded = true;
                    }
                    else if (leftFromBlack != null && leftFromBlack.GetComponent<ChipComponent>().GetColor == ColorType.White)
                    {
                        Debug.Log("4s");
                        if (x >= 2 && y <= 5)
                        {
                            GameObject doubleLeftForBlack = arrayChecks[x - 2, y + 2];
                            if (doubleLeftForBlack == null)
                            {
                                Debug.Log("5s");
                                cellRenderer = arrayCells[x - 2, y + 2].GetComponent<MeshRenderer>();
                                AddAdditionalMaterial(cellRenderer, cellMaterial, focusMaterial, 1);
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
                        AddAdditionalMaterial(cellRenderer, cellMaterial, focusMaterial, 1);
                        arrayCells[x + 1, y + 1].AddComponent<CellIsFocused>();
                        cellFocusedAdded = true;

                    }
                    else if (rightFromBalck != null && rightFromBalck.GetComponent<ChipComponent>().GetColor == ColorType.White)
                    {

                        if (x <= 5 && y <= 5)
                        {
                            GameObject doubleRightForBlack = arrayChecks[x + 2, y + 2];
                            if (doubleRightForBlack == null)
                            {
                                cellRenderer = arrayCells[x + 2, y + 2].GetComponent<MeshRenderer>();
                                AddAdditionalMaterial(cellRenderer, cellMaterial, focusMaterial, 1);
                                arrayCells[x + 2, y + 2].AddComponent<CellIsFocused>();
                                cellFocusedAdded = true;
                            }
                        }
                    }
                }

            }

            //for bottom
            if (selectedCheckColorFocus == ColorType.White)
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
                    else if (leftFromWhite != null)
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
                    GameObject rightFromBlack = arrayChecks[x + 1, y - 1];
                    if (rightFromBlack == null)
                    {
                        cellRenderer = arrayCells[x + 1, y - 1].GetComponent<MeshRenderer>();
                        AddAdditionalMaterial(cellRenderer, cellMaterial, focusMaterial, 1);
                        arrayCells[x + 1, y - 1].AddComponent<CellIsFocused>();
                        cellFocusedAdded = true;

                    }
                    else if (rightFromBlack != null)
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

                //RemoveCellMaterial();

            }
        }

        private void RemoveCellMaterial()
        {
            if (selectedCheck == null)
            {
                CellIsFocused[] cellsWithFocus = FindObjectsOfType<CellIsFocused>();
                Debug.Log("hekko");
                foreach (CellIsFocused k in cellsWithFocus)
                {
                    Material[] cellMaterial = k.gameObject.GetComponent<MeshRenderer>().materials;
                    Destroy(cellMaterial[1]);

                }
            }
                
        }
    }
}
