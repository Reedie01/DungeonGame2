using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public bool gameStarted;

    public int enemyCount;
    private int previousCount;

    public TMP_Text textMeshProText;

    public GameObject startScreen;
    public GameObject gameScreen;
    public GameObject gameOver;
    public GameObject winScreen;

    public bool game = false;

    void Start()
    {
        gameStarted = true;
        enemyCount = 1;
        StartScreen();
    }

    public void StartScreen()
    {
        startScreen.SetActive(true);
        gameScreen.SetActive(false);
        gameOver.SetActive(false);
        winScreen.SetActive(false);
        game = false;
    }

    public void GameScreen()
    {
        startScreen.SetActive(false);
        gameScreen.SetActive(true);
        gameOver.SetActive(false);
        winScreen.SetActive(false);
        game = true;
    }

    public void GameOver()
    {
        startScreen.SetActive(false);
        gameScreen.SetActive(false);
        gameOver.SetActive(true);
        winScreen.SetActive(false);
        game = false;
    }
    public void Win()
    {
        startScreen.SetActive(false);
        gameScreen.SetActive(false);
        gameOver.SetActive(false);
        winScreen.SetActive(true);
        game = false;
    }

    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int enemyCount = enemies.Length;
        if (gameStarted && enemyCount > 0)
        {
            previousCount = enemyCount;
            enemyCount = EnemyCount();
        }
        if (previousCount != enemyCount && enemyCount > 0)
        {
            Debug.Log(enemyCount);
        }
        if (enemyCount == 0 && game)
        {
            Win();
        }
        

        if (textMeshProText != null)
        {
            textMeshProText.text = enemyCount.ToString();
        }
        else
        {
            Debug.LogWarning("TextMeshProUGUI reference not set!");
        }
    }

    int EnemyCount()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        return enemies.Length;
    }
}