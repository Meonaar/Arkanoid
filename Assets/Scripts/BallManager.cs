using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    #region Singleton

    private static BallManager _instance;

    public static BallManager Instance => _instance;



    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion
    public Ball ballPrefab;

    private Ball initialBall;

    private Rigidbody2D initialBallRb;

    public float initialBallSpeed = 250f; 
    public List<Ball> Balls { get; set; }

    private void Start()
    {
        InitializeBall();
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameStarted)
        {
            Vector3 paddlePos = Paddle.Instance.gameObject.transform.position;
            Vector3 ballPos = new Vector3(paddlePos.x, paddlePos.y + 0.27f, 0);
            initialBall.transform.position = ballPos;
        }
        if (Input.GetMouseButtonDown(0))
        {
            initialBallRb.isKinematic = false;
            initialBallRb.AddForce(new Vector2(0, initialBallSpeed));
            GameManager.Instance.IsGameStarted = true;
        }
    }

    private void InitializeBall()
    {
        Vector3 paddlePos = Paddle.Instance.gameObject.transform.position;
        Vector3 startPosition = new Vector3(paddlePos.x, paddlePos.y + 0.27f, 0);

        initialBall = Instantiate(ballPrefab, startPosition, Quaternion.identity);
        initialBallRb = initialBall.GetComponent<Rigidbody2D>();

        this.Balls = new List<Ball>
        {
            initialBall
        };
    }

    public void ResetBalls()
    {
        foreach (var ball in Balls.ToList())
        {
            Destroy(ball.gameObject);
        }

        InitializeBall();
    }
}
