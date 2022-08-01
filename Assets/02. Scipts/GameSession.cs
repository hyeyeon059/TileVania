using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLifes = 3;
    [SerializeField] int score = 0;
    [SerializeField] TextMeshProUGUI lifeText;
    [SerializeField] TextMeshProUGUI scoreText;

    private void Awake()
    {
        int numGameSession = FindObjectsOfType<GameSession>().Length;

        if(numGameSession > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        lifeText.text = playerLifes.ToString();
        scoreText.text = score.ToString();
    }

    public void PrecessPlayerDeath()
    {
        if(playerLifes > 1)
        {
            Lifes();
        }
        else
        {
            ResetGameSession();
        }
    }

    void Lifes()
    {
        playerLifes--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        lifeText.text = playerLifes.ToString();
    }

    public void AddInScore(int pointToAdd)
    {
        score += pointToAdd;
        scoreText.text = score.ToString();
    }

    void ResetGameSession()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
