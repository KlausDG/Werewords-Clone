using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LastStepManager : MonoBehaviour {

    public GameObject[] panels;
    public GameObject gamePanel;

    public TextMeshProUGUI panelText;
    public TextMeshProUGUI timerText;

    public int panelIndex;
    public float countdown;

    public bool canCountdown;
    public bool gameStarted;
    public bool gameEnded;

    private AudioManager audioManager;

    void Start()
    {
        audioManager = AudioManager.instance;
        gameEnded = false;
        countdown = 0;
        panelIndex = 0;
        ChangePanel();
        SetUpWord();
    }

    void Update()
    {
        Countdown();

        if (countdown == 0 && panelIndex < panels.Length)
        {
            ChangePanel();
        }

        if (panelIndex == panels.Length && !gameStarted && countdown == 0)
        {
            panels[panelIndex - 1].SetActive(false);
            StartGame();
        }

        if (gameStarted)
        {
            if (countdown.ToString("f0").Length >= 2)
            {
                timerText.text = ":" + countdown.ToString("f0");
            }
            else
            {
                timerText.text = ":0" + countdown.ToString("f0");
            }

            if (countdown == 0 && gameEnded)
            {
                EdnGame();
            }
        }
    }

    public void SetUpWord()
    {
        gamePanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = static_mainGameValues.selectedWord;
        gamePanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = static_mainGameValues.selectedWord;
    }

    public void Countdown()
    {
        if (canCountdown)
        {
            countdown -= Time.deltaTime;
        }
        if (countdown < 0)
        {
            countdown = 0;
        }

        if (countdown == 0 && !gameEnded && gameStarted)
        {
            gameEnded = true;
        }
    }

    public void ChangePanel()
    {
        if ((panelIndex - 1) >= 0 && (panelIndex < panels.Length))
        {
            panels[panelIndex - 1].SetActive(false);
        }
        panels[panelIndex].SetActive(true);

        if (static_mainGameValues.finishedCondition == "werewolf")
        {
            switch (panelIndex)
            {
                case 0:
                    audioManager.PlaySound("0");
                    break;
                case 1:
                    audioManager.PlaySound("1");
                    break;
                case 2:
                    audioManager.PlaySound("2");
                    panelText.text = "Agora vocês têm " + static_mainGameValues.playerTimer.ToString() + " segundos para encontrar o lobisomem";
                    break;
            }
        }
        else
        {
            switch (panelIndex)
            {
                case 0:
                    audioManager.PlaySound("0");
                    break;
                case 1:
                    audioManager.PlaySound("1");
                    break;

            }
        }

        canCountdown = true;
        countdown = static_mainGameValues.roleTimer;

        panelIndex++;
    }

    public void StartGame()
    {
        gameStarted = true;
        gamePanel.SetActive(true);
        if (static_mainGameValues.finishedCondition == "werewolf")
        {
            countdown = static_mainGameValues.playerTimer;
        }
        else
        {
            countdown = static_mainGameValues.werewolfTimer;
        }
    }

    public void EdnGame()
    {
        SceneManager.LoadScene("PlayAgain", LoadSceneMode.Single);
    }
}
