﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    #region Singleton

    private static Paddle _instance;

    public static Paddle Instance => _instance;



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

    private Camera mainCamera;
    private float paddlePosY;
    private float defauldPaddleWidth = 400f;
    private float leftBorder = 270f;
    private float rightBorder = 820f;
    private SpriteRenderer sr;


    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
        paddlePosY = transform.position.y;
    }
    // Update is called once per frame
    void Update()
    {
        PaddleMove();
    }

    private void PaddleMove()
    {
        float paddleShift = (defauldPaddleWidth - ((defauldPaddleWidth/2) * sr.size.x)) / 2;
        float leftMargin = leftBorder - paddleShift;
        float rightMargin = rightBorder + paddleShift;
        float mousePosPixels = Mathf.Clamp(Input.mousePosition.x, leftMargin, rightMargin);
        float mousePosWorldX = mainCamera.ScreenToWorldPoint(new Vector3(mousePosPixels, 0, 0)).x;
        transform.position = new Vector3(mousePosWorldX, paddlePosY, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            Rigidbody2D ballRb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector3 hitPoint = collision.contacts[0].point;
            Vector3 paddleCenter = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0);

            ballRb.velocity = Vector3.zero;

            float diff = paddleCenter.x - hitPoint.x;

            if (hitPoint.x < paddleCenter.x)
            {
                ballRb.AddForce(new Vector2(-(Mathf.Abs(diff * 200)), BallManager.Instance.initialBallSpeed));
            }
            else
            {
                ballRb.AddForce(new Vector2((Mathf.Abs(diff * 200)), BallManager.Instance.initialBallSpeed));
            }
        }
    }
}
