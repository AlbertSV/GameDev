using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Checks
{
    public class CameraMoveControl : MonoBehaviour
    {
        private float speedBlack = -0.05f;
        private float speedWhite = 0.05f;
        private int tickPerSecond = 60;


        private void Update()
        {
            StartCoroutine(CameraRotate());
        }


        private IEnumerator CameraRotate()
        {
            WaitForSeconds wait = new WaitForSeconds(1f / tickPerSecond);

            if (MoveController.CameraHasToMove == true)
            {
                if (!MoveController.IsBlackTurn)
                {
                    while (transform.rotation.eulerAngles.y <= 180)
                    {
                        transform.Rotate(Vector3.up * speedWhite);
                        yield return wait;
                    }
                    MoveController.CameraHasToMove = false;
                    transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 180, transform.rotation.z));
                }
                if (MoveController.IsBlackTurn)
                {
                    while (transform.rotation.eulerAngles.y <= 359)
                    {
                        transform.Rotate(Vector3.up * speedBlack);
                        yield return wait;
                    }
                    MoveController.CameraHasToMove = false;
                    transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 0, transform.rotation.z));
                }
            }
        }
    }
}