using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int TotalSkeletonsOnMap;
    public static int SkeletonsKilled;

    public Text scoreText, skeleAliveText, survivedTimeText, gameOverText;

    public InputActionAsset actions;
    private InputAction leftAction;
    public bool gameOver;

    // Start is called before the first frame update
    void Awake()
    {
        TotalSkeletonsOnMap = 0;
        SkeletonsKilled = 0;
        gameOverText.enabled = false;

        InputActionMap actionMap = actions.FindActionMap("Weapon");
        leftAction = actionMap.FindAction("Left Action");
        leftAction.performed += TryReturnToMenu;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            scoreText.text = "Score: " + (100 * SkeletonsKilled);
            skeleAliveText.text = "Enemies Alives: " + TotalSkeletonsOnMap;
            survivedTimeText.text = "Time Survived: " + Math.Round(Time.timeSinceLevelLoad, 2);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            gameOverText.enabled = true;
        }

    }

    void TryReturnToMenu(InputAction.CallbackContext context) {
        if (gameOver) {
            SceneManager.LoadScene("StartMenu");
            leftAction.performed -= TryReturnToMenu;
        }
    }
}
