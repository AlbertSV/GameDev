using System.Collections;
using System.Collections.Generic;
using Arkanoid;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class BallControl : MonoBehaviour
{
    #region
    private MoveContrl playerMoves;
    private GameObject ball;
    private Vector3 lastVelocity;
    private bool needParent = true;
    private float velocityMultiplyer = 1.0f;
    #endregion


    public float VelocityMultiplyer
    {
        get { return velocityMultiplyer; }
        set { velocityMultiplyer = value; }
    }


    private void Awake()
    {
        playerMoves = new MoveContrl();
    }

    private void Start()
    {
        ball = GameControl.Manager.ball;
    }

    private void Update()
    {
        lastVelocity = ball.GetComponent<Rigidbody>().velocity;
        SetBallParent();

    }


    private void OnEnable()
    {
        playerMoves.PlayerController.Enable();

        playerMoves.PlayerController.PlayerSecondShoot.performed += OnShoot;
        playerMoves.PlayerController.PlayerFirstShoot.performed += OnShoot;

    }

    private void OnCollisionEnter(Collision collision)
    {
        var trigger = collision.gameObject.GetComponent<TriggerControl>();

        if (trigger != null)
        {
            if (trigger.isBlock)
            {
                SetBallBounce(collision);
                StartCoroutine(GameControl.Manager.SetDestroy(collision.gameObject));
            }

            else if (trigger.isBoarder)
            {
                StartCoroutine(GameControl.Manager.SetCross(collision.gameObject));
            }
            else
            {
                SetBallBounce(collision);
            }
        }
    }

    private void OnDisable()
    {
        playerMoves.PlayerController.Disable();
        playerMoves.PlayerController.PlayerFirstShoot.performed -= OnShoot;
        playerMoves.PlayerController.PlayerSecondShoot.performed -= OnShoot;
    }

    //Activate ball shooting
    private void OnShoot(CallbackContext context)
    {
        BallShoot();
    }

    //Shoot ball on key down
    private void BallShoot()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightShift))
        {
            ball.GetComponent<Rigidbody>().isKinematic = false;
            ball.transform.SetParent(null);

            SetBallMove();
        }

    }

    //Control the ball velocity and movement
    private void SetBallMove()
    {
        ball.GetComponent<Rigidbody>().velocity = (GameControl.Manager.ballSpeed * transform.forward * Time.deltaTime);
    }


    //Set ball to start player position
    private void SetBallParent()
    {
        if (ball.transform.position == GameControl.Manager.firstBallHolder.position && needParent)
        {
            ball.GetComponent<Rigidbody>().isKinematic = true;
            ball.transform.parent = GameControl.Manager.firstBallHolder;
            if (ball.transform.parent != null)
            {
                needParent = false;
            }
        }
        else if (ball.transform.position == GameControl.Manager.secondBallHolder.position && needParent)
        {
            ball.GetComponent<Rigidbody>().isKinematic = true;
            ball.transform.SetParent(GameControl.Manager.secondBallHolder);
            if (ball.transform.parent != null)
            {
                needParent = false;
            }
        }
        else if (ball.transform.position != GameControl.Manager.secondBallHolder.position || ball.transform.position != GameControl.Manager.firstBallHolder.position)
        {
            needParent = true;
        }


    }

    //Add bouncing for the ball
    private void SetBallBounce(Collision coll)
    {
        if (VelocityMultiplyer <= 1.5)
        {
            VelocityMultiplyer += 0.1f;
        }
        var speed = lastVelocity.magnitude;
        var direction = Vector3.Reflect(lastVelocity.normalized, coll.contacts[0].normal);

        ball.GetComponent<Rigidbody>().velocity = (direction * GameControl.Manager.ballSpeed * Time.deltaTime * VelocityMultiplyer);

    }
}
