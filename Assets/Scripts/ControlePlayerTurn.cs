using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControlePlayerTurn : MonoBehaviour
{
    public InputField placeHolder1;
    public Button playButton1;
    public InputField placeHolder2;
    public Button playButton2;
    private int minNumber = 1;
    private int maxNumber = 10;
    private int count = 1;
    
    public WinSceneScript scoreCount;

    private void Start()
    {
        playButton2.GetComponent<Button>().interactable = false;
        playButton1.onClick.AddListener(GenerateRandomNumber1);
        playButton2.onClick.AddListener(GenerateRandomNumber2);
    }

    public void GenerateRandomNumber1()
    {
        count++;
        int randomNum1 = Random.Range(minNumber, maxNumber);
        placeHolder1.text = randomNum1.ToString();
        scoreCount.totalScore1 += randomNum1;

        PlayerPrefs.SetInt("TotalScore1", scoreCount.totalScore1);
        PlayerPrefs.Save();

        playButton1.GetComponent<Button>().interactable = false;
        playButton2.GetComponent<Button>().interactable = true;
        Debug.Log("totalScore1 value: " + scoreCount.totalScore1);
        if (count>10)
        {
            playButton1.GetComponent<Button>().interactable = false;
            playButton2.GetComponent<Button>().interactable = false;
            Invoke("LoadScene", 1f);
        }
    }

    public void GenerateRandomNumber2()
    {
        count++;
        int randomNum2 = Random.Range(minNumber, maxNumber);
        placeHolder2.text = randomNum2.ToString();
        scoreCount.totalScore2 += randomNum2;

        PlayerPrefs.SetInt("TotalScore2", scoreCount.totalScore2);
        PlayerPrefs.Save();

        playButton2.GetComponent<Button>().interactable = false;
        playButton1.GetComponent<Button>().interactable = true;
        Debug.Log("totalScore2 value: " + scoreCount.totalScore2);
        if (count > 10)
        {
            playButton1.GetComponent<Button>().interactable = false;
            playButton2.GetComponent<Button>().interactable = false;
            Invoke("LoadScene", 1f);
        }
    }
    public void LoadScene()
    {
        SceneManager.LoadScene("WinScene");
    }
}
