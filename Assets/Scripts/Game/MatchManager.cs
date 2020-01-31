using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MatchManager : MonoBehaviour
{
    public GameObject[] panels;
    public GameObject gamePanel;
    public GameObject wordPanel;
    public GameObject wordContainer;
    public GameObject wordPrefab;
    public scriptable_difficulty[] dificulties;
    public TextMeshProUGUI werewolfTimerText;
    public TextMeshProUGUI playersTimerText;
    public TextMeshProUGUI explanationText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI warningText;
    public float warningTimer = 5;
    public bool canCountdown = false;

    public bool gameStarted = false;
    public float countdown;
    public int panelIndex = 0;

    private AudioManager audioManager;

    void Start()
    {
        audioManager = AudioManager.instance;

        ChangePanel();

        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
    }

    private void Update()
    {
        Countdown();

        if (countdown == 0 && panelIndex < panels.Length && canCountdown)
        {
            ChangePanel();
        }

        if (panelIndex == panels.Length && !gameStarted && countdown == 0)
        {
            panels[panelIndex-1].SetActive(false);
            StartGame();
        }

        if (gameStarted)
        {
            timerText.text = ConvertCountdownToFormat();

            if (timerText.text == "01:00" && (countdown > 55 && countdown < 65 ) && static_mainGameValues.minuteWarning)
            {
                Debug.Log("1 minuto");
                warningText.gameObject.SetActive(true);
                warningText.text = "Um minuto restante!";
                warningText.color = Color.yellow;
                audioManager.PlaySound("11");
                DelayToDeactivate(warningText.gameObject, warningTimer);
            }

            if (timerText.text == "00:30" && static_mainGameValues.halfMinuteWarning)
            {
                Debug.Log("30 segundos");
                warningText.gameObject.SetActive(true);
                warningText.text = "Trinta segundos restantes!";
                warningText.color = Color.red;
                audioManager.PlaySound("12");
                DelayToDeactivate(warningText.gameObject, warningTimer);
            }

            if (countdown == 0)
            {
                StartWerewolfFindSeerGame();
            }
        }
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
    }

    public string ConvertCountdownToFormat()
    {
        string convertedTime = "";

        string minutes = Mathf.Floor(countdown / 60).ToString("00");
        string seconds = (countdown % 60).ToString("00");

        if (seconds != "60")
        {
            convertedTime = string.Format("{0}:{1}", minutes, seconds);
        }
        else
        {
            convertedTime = string.Format("{0}:{1}", minutes, "00");
        }
       
        return convertedTime;
    }

    public void ChangePanel()
    {
        if ((panelIndex - 1) >= 0 && (panelIndex < panels.Length))
        {
            panels[panelIndex - 1].SetActive(false);
        }
        panels[panelIndex].SetActive(true);
        switch (panelIndex)
        {
            case 0:
                audioManager.PlaySound("0");
                canCountdown = false;
                StartCoroutine(DelayToNextCountdown());
                break;
            case 1:
                audioManager.PlaySound("1");
                canCountdown = false;
                break;
            case 2:
                audioManager.PauseAllSounds();
                audioManager.PlaySound("2");
                canCountdown = false;
                DisplayWords();
                break;
            case 3:
                audioManager.PauseAllSounds();
                audioManager.PlaySound("3");
                canCountdown = false;
                StartCoroutine(DelayToNextCountdown());
                break;
            case 4: //MW
                canCountdown = true;
                countdown = static_mainGameValues.wordTimer;
                break;
            case 5:
                audioManager.PlaySound("4");
                canCountdown = false;
                StartCoroutine(DelayToNextCountdown());
                break;
            case 6:
                audioManager.PlaySound("5");
                canCountdown = false;
                StartCoroutine(DelayToNextCountdown());
                break;
            case 7: //MW
                canCountdown = true;
                countdown = static_mainGameValues.wordTimer;
                break;
            case 8:
                audioManager.PlaySound("6");
                canCountdown = false;
                StartCoroutine(DelayToNextCountdown());
                break;
            case 9:
                audioManager.PlaySound("7");
                canCountdown = false;
                StartCoroutine(DelayToNextCountdown());
                break;
            case 10: //MW
                canCountdown = true;
                countdown = static_mainGameValues.wordTimer;
                break;
            case 11:
                audioManager.PlaySound("8");
                canCountdown = false;
                StartCoroutine(DelayToNextCountdown());
                break;
            case 12:
                audioManager.PlaySound("9");
                canCountdown = false;
                StartCoroutine(DelayToNextCountdown());
                break;
            case 13:
                audioManager.PlaySound("10");
                explanationText.text = "Vocês têm " + static_mainGameValues.gameTimer.ToString() + " minutos para descobrir a Palavra Mágica!";
                canCountdown = true;
                countdown = static_mainGameValues.roleTimer;
                break;
        }
        panelIndex++;
    }

    public void StartGame()
    {
        gameStarted = true;
        gamePanel.SetActive(true);
        countdown = static_mainGameValues.gameTimer * 60;
    }

    public void StartWerewolfFindSeerGame()
    {
        static_mainGameValues.finishedCondition = "players";
        SceneManager.LoadScene("Success", LoadSceneMode.Single);
    }

    public void StartVillagersFindWerewolf()
    {
        static_mainGameValues.finishedCondition = "werewolf";
        SceneManager.LoadScene("Failure", LoadSceneMode.Single);
    }

    public void DisplayWords()
    {
        foreach (var word in static_mainGameValues.wordsInGame)
        {
            GameObject newWord = Instantiate(wordPrefab, wordContainer.transform.position, Quaternion.identity, wordContainer.transform);
            newWord.GetComponentInChildren<TextMeshProUGUI>().text = word;
        }
    }

    public void SelectRole(TextMeshProUGUI roleTitle)
    {
        static_mainGameValues.mayorSecretRole = roleTitle.text;
        canCountdown = true;
        countdown = 0;

    }

    public void SelectWord(TextMeshProUGUI word)
    {
        static_mainGameValues.selectedWord = word.text;
        wordPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = word.text;
        wordPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = word.text;
        canCountdown = true;
        countdown = 0;
    }

    public void PlayPause(TextMeshProUGUI btnText)
    {
        canCountdown = !canCountdown;
        if (btnText.text == "pausar")
        {
            btnText.text = "Continuar";
        }
        else
        {
            btnText.text = "pausar";
        }
    }

    public void CancelGame()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public IEnumerator DelayToDeactivate(GameObject obj, float t)
    {
        yield return new WaitForSecondsRealtime(t);
        obj.SetActive(false);
    }

    public IEnumerator DelayToNextCountdown()
    {
        yield return new WaitForSeconds(static_mainGameValues.roleTimer);
        canCountdown = true;
    }
}
