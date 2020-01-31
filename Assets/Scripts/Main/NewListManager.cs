using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

[System.Serializable]
public class NewListManager : MonoBehaviour {
    public GameObject optaionsPanel;
    public TextMeshProUGUI listNameInput;
    public TextMeshProUGUI listWords;
    public TextMeshProUGUI difficultyButtonText;
    public Image difficultyButtonImage;
    private int currentListDifficulty;
    private GameManager gameManager;
    private ListsManager listsManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        listsManager = FindObjectOfType<ListsManager>();
        currentListDifficulty = 0;
    }

    public void ChangeListDifficulty()
    {
        if (currentListDifficulty < 3)
        {
            currentListDifficulty++;
        }
        else
        {
            currentListDifficulty = 0;
        }

        Methods.ChangeText(difficultyButtonText, gameManager.dificulties[currentListDifficulty].singularTitle);
        Methods.ChangeColor(gameManager.dificulties[currentListDifficulty].color, difficultyButtonImage);
    }
    
    public void SaveNewList()
    {
        WordList newList = new WordList();
        newList.title = listNameInput.text;
        newList.title = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(newList.title.ToLower());

        newList.words = listWords.text.Replace("\r", "").ToLower().Split('\n');
        newList.difficulty = currentListDifficulty;

        static_mainGameValues.allWordLists.Add(newList);
        listsManager.UpdateListOfWordLists(newList);
        Methods.SaveGame(static_mainGameValues.allWordLists, gameManager.gameStatistics);
        gameManager.ChangePanel(optaionsPanel);
    }
}
