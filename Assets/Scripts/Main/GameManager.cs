using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameManager : MonoBehaviour {

    public GameStatistics gameStatistics;
    public GameObject[] rolesArray;
    public scriptable_difficulty[] dificulties;
    public TextMeshProUGUI playerQtdText;
    public TextMeshProUGUI difficultyText;
    public TextMeshProUGUI wordQuantityText;
    public TextMeshProUGUI timerValueText;
    public TextMeshProUGUI timerOptionsValueText;
    public TextMeshProUGUI roleTimerOptionsValueText;
    public TextMeshProUGUI timerOptionsDescriptionText;
    public TextMeshProUGUI wordListOptionsDescriptionText;
    public TextMeshProUGUI wordQtdOptionsDescriptionText;
    public TextMeshProUGUI statisticsOptionsDescriptionText;
    public TextMeshProUGUI statisticsText;
    public Image playButtonImage;
    public Image timerButtonImage;
    public Image optionsTimerButtonImage;
    public Image minuteWarningButtonImagem;
    public Image halfMinuteWarningButtonImage;
    public GameObject mainPanel;
    public GameObject optionsPanel;
    public GameObject errorPanel;

    private ListsManager listsManager;

    private void Start()
    {
        listsManager = FindObjectOfType<ListsManager>();

        LoadGameStatistics();
        LoadGameLists();
        UpdateStatisticsText();

        Methods.ChangeText(timerOptionsValueText     , static_mainGameValues.gameTimer.ToString());
        Methods.ChangeText(roleTimerOptionsValueText , static_mainGameValues.roleTimer.ToString());
        Methods.ChangeText(wordQuantityText          , static_mainGameValues.wordQuantity.ToString());
    }

    private void Update()
    {
        //Update the number of players
        static_mainGameValues.playerQtd = Methods.UpdateQuantity(static_mainGameValues.activeRolesList.Count);
        //Update the number of players UI text
        Methods.ChangeText(playerQtdText, static_mainGameValues.activeRolesList.Count.ToString());

        UpdateOptionsDescription();
    }

    private void LoadGameStatistics()
    {
        gameStatistics = Methods.LoadStatistics();

        static_mainGameValues.games             = gameStatistics.games;
        static_mainGameValues.wordsFound        = gameStatistics.wordsFound;
        static_mainGameValues.villgersVictory   = gameStatistics.villgersVictory;
        static_mainGameValues.werewolfVictory   = gameStatistics.werewolfVictory;
        static_mainGameValues.mayorVillager     = gameStatistics.mayorVillager;
        static_mainGameValues.mayorWerewolf     = gameStatistics.mayorWerewolf;
        static_mainGameValues.mayorSeer         = gameStatistics.mayorSeer;
    }

    private void LoadGameLists()
    {
        static_mainGameValues.allWordLists = Methods.LoadGame();

        foreach (WordList l in static_mainGameValues.allWordLists)
        {
            listsManager.UpdateListOfWordLists(l);
        }
    }

    private void UpdateOptionsDescription()
    {
        string tempString = "Jogo: ";

        if (static_mainGameValues.gameTimer.ToString().Count() == 2)
        {
            tempString += static_mainGameValues.gameTimer + ":00, ";
        }
        else
        {
            tempString += "0" + static_mainGameValues.gameTimer + ":00, ";
        }

        tempString += "Role: 00:";

        if (static_mainGameValues.roleTimer.ToString().Count() == 2)
        {
            tempString += static_mainGameValues.roleTimer;
        }
        else
        {
            tempString += "0" + static_mainGameValues.roleTimer;
        }

        timerOptionsDescriptionText.text = tempString;

        tempString = static_mainGameValues.allWordLists.Count() + " Listas, ";

        int words = 0;
        foreach (var list in static_mainGameValues.allWordLists)
        {
            words += list.words.Count();
        }
        tempString += words + "Palavras.";

        wordListOptionsDescriptionText.text = tempString;

        wordQtdOptionsDescriptionText.text = static_mainGameValues.wordQuantity.ToString() + " Opções.";

        statisticsOptionsDescriptionText.text = static_mainGameValues.games.ToString() + " Partidas.";
    }

    public void UpdateStatisticsText()
    {
        statisticsText.text = "Total de partidas: "       + static_mainGameValues.games +
                            "\nPalavras descobertas: "    + static_mainGameValues.wordsFound +
                            "\nVitórias dos Aldeões: "    + static_mainGameValues.villgersVictory +
                            "\nVitórias dos Lobisomens: " + static_mainGameValues.werewolfVictory +
                            "\n" + 
                            "\nPrefeito Aldeão: "         + static_mainGameValues.mayorVillager + 
                            "\nPrefeito Lobisomem: "      + static_mainGameValues.mayorWerewolf + 
                            "\nPrefeito Vidente: "        + static_mainGameValues.mayorSeer;

    }


    public string GetNameOfDifficulty(int index)
    {
        string n = "";
        switch (index)
        {
            case 0:
                n = "F";
                break;
            case 1:
                n = "M";
                break;
            case 2:
                n = "D";
                break;
            case 3:
                n = "R";
                break;
        }
        return n;
    }

    private bool IsInList(GameObject target)
    {
        return static_mainGameValues.activeRolesList.Any((obj) => obj.name == target.name);
    }

    private int HowManyInList(string target)
    {
        int counter = 0;

        foreach (GameObject obj in static_mainGameValues.activeRolesList)
        {
            if (obj.CompareTag(target))
            {
                counter++;
            }
        }

        return counter;
    }

    private GameObject SelectFirstAvailable(string title, int target)
    {
        for (int i = 0; i < static_mainGameValues.activeRolesList.Count; i++)
        {
            if (static_mainGameValues.activeRolesList[i].CompareTag(title))
            {
                return static_mainGameValues.activeRolesList[i];
            }
        }
        return null;
    }

    private void ActivateToTargetNumber(int target, string tag)
    {
        int i = 0;
        while (target > 0)
        {
            Debug.Log(i);
            if (rolesArray[i].CompareTag(tag))
            {
                if (!IsInList(rolesArray[i]))
                {
                    target--;
                    ToggleStatus(rolesArray[i]);
                }
            }
            i++;
        }
    }

    private void ToggleAll()
    {
        foreach (GameObject target in static_mainGameValues.activeRolesList)
        {
            Methods.ChangeAlpha(target.GetComponent<Image>());
        }
    }

    private void SelectRandomWords()
    {
        if (static_mainGameValues.wordsListInGame.Count() > 0)
        {
            foreach (var list in static_mainGameValues.wordsListInGame)
            {
                if (list.difficulty == static_mainGameValues.currentDifficulty)
                {
                    foreach (var word in list.words)
                    {
                        static_mainGameValues.allWords.Add(word);
                    }
                }
            }

            if (static_mainGameValues.allWords.Count() >= static_mainGameValues.wordQuantity)
            {
                for (int i = 0; i < static_mainGameValues.wordQuantity; i++)
                {
                    int randomIndex = Random.Range(0, static_mainGameValues.allWords.Count());
                    static_mainGameValues.wordsInGame.Add(static_mainGameValues.allWords[randomIndex]);
                    static_mainGameValues.allWords.RemoveAt(randomIndex);
                }

            }
        }
    }

    public void ToggleStatus(GameObject obj)
    {
        Image objImage = obj.GetComponent<Image>();

        if (IsInList(obj))
        {
            static_mainGameValues.activeRolesList.Remove(obj);
        }
        else
        {
            static_mainGameValues.activeRolesList.Add(obj);
        }

        Methods.ChangeAlpha(objImage);
    }

    //Botões//

    public void ChangePlayerQtd()
    {
        int seer = HowManyInList("Seer");
        int werewolves = HowManyInList("Werewolf");
        int villagers = HowManyInList("Villager");

        if (static_mainGameValues.playerQtd == 10 || static_mainGameValues.playerQtd < 4)
        {
            ToggleAll();
            static_mainGameValues.activeRolesList.Clear();
            ToggleStatus(rolesArray[0]);
            ToggleStatus(rolesArray[1]);
            ActivateToTargetNumber(2, "Villager");
        }
        else
        {
            if (seer == 0)
            {
                ActivateToTargetNumber(1, "Seer");
            }
            else if (static_mainGameValues.playerQtd >= 6 && werewolves < 2)
            {
                ActivateToTargetNumber(1, "Werewolf");
            }
            else
            {
                ActivateToTargetNumber(1, "Villager");
            }
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
        Methods.ChangeText(difficultyText, "Palavras " + dificulties[static_mainGameValues.currentDifficulty].pluralTitle);
        Methods.ChangeColor(dificulties[static_mainGameValues.currentDifficulty].color, playButtonImage, difficultyText);
    }

    public void ToggleTimer()
    {
        if (static_mainGameValues.isTimer)
        {
            Methods.ChangeColor(Color.red, timerButtonImage);
            Methods.ChangeColor(Color.red, optionsTimerButtonImage);
            Methods.ChangeText(timerValueText, "\u221E");
        }
        else
        {
            string timerString = static_mainGameValues.gameTimer.ToString() + ":00";
            Methods.ChangeColor(Color.white, timerButtonImage);
            Methods.ChangeColor(Color.white, optionsTimerButtonImage);
            Methods.ChangeText(timerValueText, timerString);
            
        }
        static_mainGameValues.isTimer = !static_mainGameValues.isTimer;
    }

    public void ToggleWarning(string target)
    {
        if (target == "minute")
        {
            static_mainGameValues.minuteWarning = !static_mainGameValues.minuteWarning;
            if (!static_mainGameValues.minuteWarning)
            {
                Methods.ChangeColor(Color.red, minuteWarningButtonImagem);
            }
            else
            {
                Methods.ChangeColor(Color.white, minuteWarningButtonImagem);
            }
        }
        else if (target == "seconds")
        {
            static_mainGameValues.halfMinuteWarning = !static_mainGameValues.halfMinuteWarning;
            if (!static_mainGameValues.halfMinuteWarning)
            {
                Methods.ChangeColor(Color.red, halfMinuteWarningButtonImage);
            }
            else
            {
                Methods.ChangeColor(Color.white, halfMinuteWarningButtonImage);
            }
        }
        
    }

    public void IncraseWordQtd()
    {
        if (static_mainGameValues.wordQuantity < 5)
        {
            static_mainGameValues.wordQuantity++;
            Methods.ChangeText(wordQuantityText, static_mainGameValues.wordQuantity.ToString());
        }
    }

    public void DecreaseWordQtd()
    {
        if (static_mainGameValues.wordQuantity > 1)
        {
            static_mainGameValues.wordQuantity--;
            Methods.ChangeText(wordQuantityText, static_mainGameValues.wordQuantity.ToString());
        }
    }

    public void IncreaseGameTime()
    {
        if (static_mainGameValues.gameTimer < 10)
        {
            static_mainGameValues.gameTimer++;
            Methods.ChangeText(timerOptionsValueText, static_mainGameValues.gameTimer.ToString());
        }
    }

    public void DecreaseGameTime()
    {
        if (static_mainGameValues.gameTimer > 1)
        {
            static_mainGameValues.gameTimer--;
            Methods.ChangeText(timerOptionsValueText, static_mainGameValues.gameTimer.ToString());
        }
    }

    public void IncreaseRoleTime()
    {
        if (static_mainGameValues.roleTimer < 30)
        {
            static_mainGameValues.roleTimer++;
            Methods.ChangeText(roleTimerOptionsValueText, static_mainGameValues.roleTimer.ToString());
        }
    }

    public void DecreaseRoleTime()
    {
        if (static_mainGameValues.roleTimer > 1)
        {
            static_mainGameValues.roleTimer--;
            Methods.ChangeText(roleTimerOptionsValueText, static_mainGameValues.roleTimer.ToString());
        }
    }

    public void ChangePanel(GameObject target)
    {
        string tag = "Main Panel";
        GameObject panel = Methods.FindParentWithTag(EventSystem.current.currentSelectedGameObject, tag);
        Methods.SwapPanels(target, panel);
    }

    public void ClosePanel(GameObject target)
    {
        target.SetActive(false);
    }

    public void Play()
    {
        SelectRandomWords();
        if (static_mainGameValues.allWords.Count() >= static_mainGameValues.wordQuantity)
        {
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }
        else
        {
            errorPanel.SetActive(true);
        }
    }
}
