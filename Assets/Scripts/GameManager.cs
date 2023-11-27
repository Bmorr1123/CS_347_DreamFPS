using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static int TotalSkeletonsOnMap;
    public static int SkeletonsKilled;

    public Text scoreText, skeleAliveText, survivedTimeText, gameOverText;

    public bool gameOver;

    // Start is called before the first frame update
    void Awake()
    {
        TotalSkeletonsOnMap = 0;
        SkeletonsKilled = 0;
gameOverText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            scoreText.text = "Score: " + (100 * SkeletonsKilled);
            skeleAliveText.text = "Enemies Alives: " + TotalSkeletonsOnMap;
            survivedTimeText.text = "Time Survived: " + Math.Round(Time.timeSinceLevelLoad, 2);
        } else {
gameOverText.enabled = true;
        }

    }
}
