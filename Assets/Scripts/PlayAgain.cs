using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayAgain : MonoBehaviour
{
    public GameObject difficultyButton;
    public GameObject failurePanel;
    public GameObject successPanel;
    public GameObject villagerButton;
    public GameObject werewolfButton;
    public scriptable_difficulty[] difficulties;

    void Start()
    {
        if (static_mainGameValues.finishedCondition == "werewolf")
        {
            successPanel.SetActive(true);
        }
        else
        {
            failurePanel.SetActive(true);
        }
    }

    public void ChangeGameDifficulty()
    {
        if (static_mainGameValues.currentDifficulty < 3)
        {
            static_mainGameValues.currentDifficulty++;
        }
        else
        {
            static_mainGameValues.currentDifficulty = 0;
        }
        TextMeshProUGUI difficultyTxt = difficultyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        Methods.ChangeText(difficultyTxt, "Palavras " + difficulties[static_mainGameValues.currentDifficulty].pluralTitle);
        Methods.ChangeColor(difficulties[static_mainGameValues.currentDifficulty].color, difficultyButton.GetComponent<Image>(), difficultyTxt);
    }

    public void UpdateGameStatistics(string v)
    {
        villagerButton.SetActive(false);
        werewolfButton.SetActive(false);

        static_mainGameValues.games++;
        if (static_mainGameValues.finishedCondition == "players")
        {
            static_mainGameValues.wordsFound++;
        }

        if (v == "villagers")
        {
            static_mainGameValues.villgersVictory++;
        }

        if (v == "werewolf")
        {
            static_mainGameValues.werewolfVictory++;
        }

        if (static_mainGameValues.mayorSecretRole == "Aldeão")
        {
            static_mainGameValues.mayorVillager++;
        }
        else if (static_mainGameValues.mayorSecretRole == "Lobisomem")
        {
            static_mainGameValues.mayorWerewolf++;
        }
        else if (static_mainGameValues.mayorSecretRole == "Vidente")
        {
            static_mainGameValues.mayorSeer++;
        }

    }

    public void Play()
    {
        SelectRandomWords();
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    private void SelectRandomWords()
    {
        static_mainGameValues.wordsInGame.Clear();
        static_mainGameValues.allWords.Clear();
        foreach (var list in static_mainGameValues.wordsListInGame)
        {
            foreach (var word in list.words)
            {
                static_mainGameValues.allWords.Add(word);
            }
        }

        for (int i = 0; i < static_mainGameValues.wordQuantity; i++)
        {
            int randomIndex = Random.Range(0, static_mainGameValues.allWords.Count());
            static_mainGameValues.wordsInGame.Add(static_mainGameValues.allWords[randomIndex]);
            static_mainGameValues.allWords.RemoveAt(randomIndex);
        }
    }
}
