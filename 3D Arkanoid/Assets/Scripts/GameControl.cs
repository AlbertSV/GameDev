using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid
{
    public class GameControl : MonoBehaviour
    {
        [SerializeField] public GameObject ball;
        [SerializeField] public GameObject firstBoarder;
        [SerializeField] public GameObject secondBoarder;
        [SerializeField] public int lifes = 3;
        [SerializeField] public float playerSpeed;
        [SerializeField] public float ballSpeed;
        [SerializeField] public Transform firstBallHolder;
        [SerializeField] public Transform secondBallHolder;

        private MoveControl moveControl;
        private IsBlock[] blocks;
        private TriggerControl triggerControl;

        public static GameControl Manager;

        private void Awake()
        {
            Manager = this;
            moveControl = new MoveControl();
            triggerControl = new TriggerControl();
        }
        private void Start()
        {
            blocks = FindObjectsOfType<IsBlock>();

            foreach(IsBlock block in blocks)
            {
                block.gameObject.transform.rotation = Quaternion.Euler(GetRandomRotation());
            }
        }

        private void Update()
        {
            EndGame();
            WinCondition();

        }


        public IEnumerator SetDestroy(Collider objectTriggered)
        {
            if (ball != null && objectTriggered.GetComponent<IsBlock>() != null)
            {
                Destroy(objectTriggered);
                yield return new WaitForSeconds(0f);
            }
        }

        public IEnumerator SetCross(Collider objectTriggered)
        {
            if (ball != null && objectTriggered.GetComponent<IsBoarder>() != null)
            {
                if (objectTriggered == firstBoarder)
                {
                    ball.transform.position = firstBallHolder.transform.position;
                    lifes--;
                    Debug.Log(lifes);
                }
                else if (objectTriggered == secondBoarder)
                {
                    ball.transform.position = secondBallHolder.transform.position;
                    lifes--;
                    Debug.Log(lifes);
                }

                moveControl.NeedToMove = false;
                moveControl.NeedChangeDirection = false;

                yield return new WaitForSeconds(0f);
            }
        }

        private Vector3 GetRandomRotation()
        {
            float x = Random.Range(0f, 10f);
            float y = Random.Range(0f, 10f);
            float z = Random.Range(0f, 10f);

            return new Vector3(x, y, z);
        }

        private void EndGame()
        {
            if(lifes <= 0)
            {
                Debug.Log("Game ended. You lost!");

            }
        }

        private void WinCondition()
        {
            if(blocks == null)
            {
                Debug.Log("You Won!");
            }
        }
    }
}