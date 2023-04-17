using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Checks
{
    public class CameraMoveControl : MonoBehaviour
    {
        private Camera mainCamera;

        [SerializeField] private GameObject pointRotation;

        private Vector3 rotationVector;
        private Quaternion rotationAngle;

        private void Awake()
        {
            mainCamera = Camera.main;
            rotationVector = pointRotation.transform.position;
            rotationAngle = Quaternion.AngleAxis(10f * Time.deltaTime, Vector3.up);
        }

        private void Update()
        {
            CameraMove();
        }

        private void CameraMove()
        {
            if (MoveController.CameraHasToMove == true)
            {
                var v = mainCamera.transform.position - rotationVector;
                v = rotationAngle * v;

                if (MoveController.IsBlackTurn)
                    //if ()
                    {
                        {
                            mainCamera.transform.position = rotationVector + v;
                        }
                        MoveController.CameraHasToMove = false;
                    }
            }
        }
    }
}