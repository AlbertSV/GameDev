using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Checks
{
    public class FieldCreation : MonoBehaviour
    {
        [SerializeField] private GameObject blackCheck;
        [SerializeField] private GameObject whiteCheck;

        [SerializeField] private GameObject blackCell;
        [SerializeField] private GameObject whiteCell;

        private Transform checkContainer;
        private Transform cellContainer;

        private GameObject[,] checksArray;
        private GameObject cellCreator;

        private int numberOfRowsColumns = 8;

        public GameObject[,] ChecksArray
        {
            get { return checksArray; }
            set { checksArray = value; }
        }

        private void Awake()
        {
            checkContainer = FindObjectOfType<CheckContainer>().transform;
            cellContainer = FindObjectOfType<CellContainer>().transform;
            checksArray = new GameObject[8,8];
        }

        private void Start()
        {
            CreationChecks();
            CreationCell();
        }

        private void CreationCell()
        {
            for (int i = 0; i < numberOfRowsColumns; i++)
            {
                if (i % 2 == 0)
                {
                    for (int j =0; j < numberOfRowsColumns; j++)
                    {
                        if (j % 2 == 0)
                        {
                            cellCreator = Instantiate(blackCell, new Vector3(i, 0, j), transform.rotation, cellContainer);
                        }
                        else
                        {
                            cellCreator = Instantiate(whiteCell, new Vector3(i, 0, j), transform.rotation, cellContainer);
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < numberOfRowsColumns; j++)
                    {
                        if (j % 2 == 0)
                        {
                            cellCreator = Instantiate(whiteCell, new Vector3(i, 0, j), transform.rotation, cellContainer);
                        }
                        else
                        {
                            cellCreator = Instantiate(blackCell, new Vector3(i, 0, j), transform.rotation, cellContainer);
                        }
                    }
                }
            }
        }

        private void CreationChecks()
        {
            for (int i=0; i<3; i+=1)
            {
                if (i % 2 == 0)
                {
                    for (int j = 0; j < numberOfRowsColumns; j +=2)
                    {
                        checksArray[j, i] = Instantiate(blackCheck, new Vector3(j, 0.1f, i), transform.rotation, checkContainer);
                    }
                }
                else
                {
                    for (int j =1; j < numberOfRowsColumns; j +=2)
                    {
                        checksArray[j, i] = Instantiate(blackCheck, new Vector3(j, 0.1f, i), transform.rotation, checkContainer);
                    }
                }
                  
            }

            for (int i = 7; i > 4; i -= 1)
            {
                if (i % 2 == 0)
                {
                    for (int j = 0; j < numberOfRowsColumns; j += 2)
                    {
                        checksArray[j, i] = Instantiate(whiteCheck, new Vector3(j, 0.1f, i), transform.rotation, checkContainer);
                    }
                }
                else
                {
                    for (int j = 1; j < numberOfRowsColumns; j += 2)
                    {
                        checksArray[j, i] = Instantiate(whiteCheck, new Vector3(j, 0.1f, i), transform.rotation, checkContainer);
                    }
                }

            }
        }
    }
}

