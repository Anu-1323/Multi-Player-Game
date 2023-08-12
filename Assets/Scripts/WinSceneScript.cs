using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinSceneScript : MonoBehaviour
{
    private string player1 = "Player 1 Win";
    private string player2 = "Player 2 Win";
    public Text scoreValue;
    [HideInInspector]
    public int totalScore1;
    [HideInInspector]
    public int totalScore2;

    private void Start()
    {
        int score1 = PlayerPrefs.GetInt("TotalScore1", 0);
        int score2 = PlayerPrefs.GetInt("TotalScore2", 0);

        if (score1 > score2)
        {
            scoreValue.text = player1;
        }
        else if (score2 > score1)
        {
            scoreValue.text = player2;
        }
        else
        {
            scoreValue.text = "Both Win";
        }
    }
    public void LoadScene()
    {
        SceneManager.LoadScene("StartScene");
    }
}
