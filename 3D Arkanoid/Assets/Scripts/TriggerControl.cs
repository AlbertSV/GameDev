using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid
{
    public class TriggerControl : MonoBehaviour
    {
        [SerializeField] bool isBlock;
        [SerializeField] bool isBoarder;

        private void Start()
        {

        }


        private void OnTriggerEnter(Collider other)
        {
            if(isBlock)
            {
                StartCoroutine(GameControl.Manager.SetDestroy(other));
            }
            else if(isBoarder)
            {
                StartCoroutine(GameControl.Manager.SetCross(other));
            }
            
        }


    }
}