﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BrickManager : MonoBehaviour
{
    #region Singleton

    private static BrickManager _instance;

    public static BrickManager Instance => _instance;



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

    private int maxRows = 17;
    private int maxCols = 12;
    private float initialBrickSpawnPosX = -1.96f;
    private float initialBrickSpawnPosY = 3.325f;
    private float shiftAmount = 0.365f;
    private GameObject brickContainer;

    public Brick brickPrefab;

    public Sprite[] Sprites;

    public Color[] BrickColors;

    public List<Brick> RemainingBricks { get; set; }
    public List<int[,]> levelData { get; set; }

    public int initialBrickCount { get; set; }

    public int currentLevel;

    private void Start()
    {
        brickContainer = new GameObject("BricksContainer");
        this.levelData = this.LoadLevelData();
        RemainingBricks = new List<Brick>();
        GenerateBricks();
    }

    private List<int [,]> LoadLevelData()
    {
        TextAsset textFile = Resources.Load("Levels") as TextAsset;

        string[] rows = textFile.text.Split(new string[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);

        List<int[,]> levelData = new List<int[,]>();
        int[,] currentLevel = new int[maxRows, maxCols];

        int currentRow = 0;

        for (int row = 0; row < rows.Length; row++)
        {
            string line = rows[row];

            if (line.IndexOf("--") == -1)
            {
                string[] bricks = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int col = 0; col < bricks.Length; col++)
                {
                    currentLevel[currentRow, col] = int.Parse(bricks[col]);
                }

                currentRow++;
            }
            else
            {
                currentRow = 0;
                levelData.Add(currentLevel);
                currentLevel = new int[maxRows, maxCols];
            }
        }

        return levelData;
    }

    private void GenerateBricks()
    {
        int[,] currentLevelDate = levelData[currentLevel];
        float currentSpawnX = initialBrickSpawnPosX;
        float currentSpawnY = initialBrickSpawnPosY;

        float zShift = 0;
        for (int row = 0; row < maxRows; row++)
        {
            for (int col = 0; col < maxCols; col++)
            {
                int BrickType = currentLevelDate[row, col];

                if (BrickType > 0)
                {
                    Brick newBrick = Instantiate(brickPrefab, new Vector3(currentSpawnX, currentSpawnY, 0.0f - zShift), Quaternion.identity) as Brick;
                    newBrick.Init(brickContainer.transform, Sprites[BrickType - 1], BrickColors[BrickType], BrickType);

                    RemainingBricks.Add(newBrick);
                    zShift += 0.0001f;
                }

                currentSpawnX += shiftAmount;
                if (col + 1 == maxCols)
                {
                    currentSpawnX = initialBrickSpawnPosX;
                }
            }

            currentSpawnY -= shiftAmount;

        }

        initialBrickCount = RemainingBricks.Count;
    }


}
