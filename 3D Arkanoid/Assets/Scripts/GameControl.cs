using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace Arkanoid
{
    public class GameControl : MonoBehaviour
    {
        #region
        [SerializeField] public GameObject ball;
        [SerializeField] public GameObject firstBoarder;
        [SerializeField] public GameObject secondBoarder;
        [SerializeField] public Transform firstBallHolder;
        [SerializeField] public Transform secondBallHolder;

        [Header("Parameters Settings")]
        [Tooltip("Amount of lifes before game ended"), SerializeField] public int lifes = 3;
        [Tooltip("Players platform speed"), SerializeField] public float playerSpeed;
        [Tooltip("Starting ball speed"), SerializeField] public float ballSpeed;

        private MoveControl moveControl;
        private BallControl ballControl;
        private List<GameObject> blocks;
        private TriggerControl triggerControl;

        private Vector3 playerOneStartPos;
        private Vector3 playerTwoStartPos;
        private float nextLevelStep = -20f;
        private GameObject nextLevel;

        public static GameControl Manager;
        #endregion

        private void Awake()
        {
            Manager = this;
            moveControl = new MoveControl();
            ballControl = new BallControl();
            triggerControl = new TriggerControl();

            nextLevel = FindObjectOfType<LevelTwo>().gameObject;
            nextLevel.SetActive(false);

        }
        private void Start()
        {
            playerOneStartPos = firstBallHolder.parent.gameObject.transform.position;
            playerTwoStartPos = secondBallHolder.parent.gameObject.transform.position;

            blocks = new List<GameObject>(FindObjectsOfType<IsBlock>().Select(stat => stat.gameObject));

            foreach(GameObject block in blocks)
            {
                block.transform.rotation = Quaternion.Euler(GetRandomRotation());
            }
        }

        private void Update()
        {
            blocks = new List<GameObject>(FindObjectsOfType<IsBlock>().Select(stat => stat.gameObject));
            EndGame();
            WinCondition();
        }

        //Destroy blocks on collision with ball
        public IEnumerator SetDestroy(GameObject objectTriggered)
        {
            if (ball != null && objectTriggered.GetComponent<IsBlock>() != null)
            {
                blocks.Remove(objectTriggered);
                Destroy(objectTriggered);
                
                yield return new WaitForSeconds(0f);
            }
        }

        //Life and region control for ball
        public IEnumerator SetCross(GameObject objectTriggered)
        {

            if (ball != null && objectTriggered.GetComponent<TriggerControl>().isBoarder)
            {
                if (objectTriggered == firstBoarder)
                {
                    ball.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
                    ball.transform.position = firstBallHolder.transform.position;
                    ball.transform.rotation = firstBallHolder.transform.rotation;
                    lifes--;
                    Debug.Log("Lifes left:");
                    Debug.Log(lifes);
                }
                else if (objectTriggered == secondBoarder)
                {
                    ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    ball.transform.position = secondBallHolder.transform.position;
                    ball.transform.rotation = secondBallHolder.transform.rotation;
                    lifes--;
                    Debug.Log("Lifes left:");
                    Debug.Log(lifes);
                }

                yield return new WaitForSeconds(0f);
            }
        }

        //Rotation for blocks on start
        private Vector3 GetRandomRotation()
        {
            float x = Random.Range(0f, 10f);
            float y = Random.Range(0f, 10f);
            float z = Random.Range(0f, 10f);

            return new Vector3(x, y, z);
        }

        //Condition on loose game
        private void EndGame()
        {
            if(lifes <= 0)
            {
                ball.SetActive(false);
                Debug.Log("Game ended. You lost!");
                Debug.Log("Restart the game");


            }
        }

        //Condition on win game
        private void WinCondition()
        {
            if(blocks.Count == 0)
            {
                Debug.Log("You Won!");
                ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                NextLevel();
            }
        }

        //Activating next level on win
        private void NextLevel()
        {
            if (nextLevel.activeSelf == false)
            {
                nextLevel.SetActive(true);
                lifes = 3;

                blocks = new List<GameObject>(FindObjectsOfType<IsBlock>().Select(stat => stat.gameObject));
                foreach (GameObject block in blocks)
                {
                    block.transform.rotation = Quaternion.Euler(GetRandomRotation());
                }

                Debug.Log("Next level is ready. Player 1's turn!");
                MovePlayers();
                SetBallOnStart();
            }
            else
            {
                Debug.Log("You completed all levels. Congrats!");
            }
        }


        //Moving players platform on win
        private void MovePlayers()
        {
            var playerOne = firstBallHolder.parent.gameObject;
            var playerTwo = secondBallHolder.parent.gameObject;

            playerOne.transform.position = new Vector3(playerOneStartPos.x + nextLevelStep, playerOneStartPos.y, playerOneStartPos.z);
            playerTwo.transform.position = new Vector3(playerTwoStartPos.x + nextLevelStep, playerTwoStartPos.y, playerTwoStartPos.z);
        }


        //Set ball to start position on win
        private void SetBallOnStart()
        {
            ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            ball.transform.position = firstBallHolder.transform.position;

        }
    }
}