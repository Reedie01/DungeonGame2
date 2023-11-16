using UnityEngine;
using TMPro;

public class EnemiesRemaining : MonoBehaviour
{
    public TMP_Text textMeshProText;

    public GameObject startScreen;
    public GameObject gameScreen;
    public GameObject gameOver;
    public GameObject winScreen;

    void Start()
    {
        StartScreen();
    }

    public void StartScreen()
    {
        startScreen.SetActive(true);
        gameScreen.SetActive(false);
        gameOver.SetActive(false);
        winScreen.SetActive(false);
    }

    public void GameScreen()
    {
        startScreen.SetActive(false);
        gameScreen.SetActive(true);
        gameOver.SetActive(false);
        winScreen.SetActive(false);
    }

    public void GameOver()
    {
        startScreen.SetActive(false);
        gameScreen.SetActive(false);
        gameOver.SetActive(true);
        winScreen.SetActive(false);
    }
    public void Win()
    {
        startScreen.SetActive(false);
        gameScreen.SetActive(false);
        gameOver.SetActive(false);
        winScreen.SetActive(true);
    }

    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int enemyCount = enemies.Length;

        if (textMeshProText != null)
        {
            textMeshProText.text = enemyCount.ToString();
        }
        else
        {
            Debug.LogWarning("TextMeshProUGUI reference not set!");
        }
    }
}