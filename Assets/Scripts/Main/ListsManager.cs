using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ListsManager : MonoBehaviour
{
    public GameObject wordListPrefab;
    public GameObject listContainer;
    
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void UpdateListOfWordLists(WordList l)
    {
        GameObject newList      = Instantiate(wordListPrefab, listContainer.transform.position, Quaternion.identity, listContainer.transform);
        GameObject listButton   = newList.transform.GetChild(0).gameObject;

        listButton.transform.GetComponent<List>().list = l;

        TextMeshProUGUI titleText = listButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        titleText.text = l.title;

        Methods.ChangeColor(gameManager.dificulties[l.difficulty].color, titleText);

        TextMeshProUGUI difficultyText = listButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        difficultyText.text = gameManager.GetNameOfDifficulty(l.difficulty);
        Methods.ChangeColor(gameManager.dificulties[l.difficulty].color, difficultyText);
        
    }

    public void ToggleSelection(GameObject list)
    {
        GameObject markerChildObject = list.transform.GetChild(0).GetChild(2).gameObject;
        WordList currentList         = list.GetComponent<List>().list;

        if (markerChildObject.activeSelf)
        {
            markerChildObject.SetActive(false);
            if (static_mainGameValues.wordsListInGame.Contains(currentList))
            {
                static_mainGameValues.wordsListInGame.Remove(currentList);
            }
        }
        else
        {
            markerChildObject.SetActive(true);
            if (!static_mainGameValues.wordsListInGame.Contains(currentList))
            {
                static_mainGameValues.wordsListInGame.Add(currentList);
            }
        }
    }

    public void DeleteListFromGame(WordList list, GameObject prefab)
    {
        Destroy(prefab);

        if (static_mainGameValues.wordsListInGame.Contains(list))
        {
            static_mainGameValues.wordsListInGame.Remove(list);
        }

        static_mainGameValues.allWordLists.Remove(list);
        Methods.SaveGame(static_mainGameValues.allWordLists, gameManager.gameStatistics);
    }
}
