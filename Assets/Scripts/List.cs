using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class List : MonoBehaviour
{
    public WordList list;
    public GameObject markerChildObject;

    public void ToggleSelection()
    {
        if (markerChildObject.activeSelf)
        {
            markerChildObject.SetActive(false);
            if (static_mainGameValues.wordsListInGame.Contains(list))
            {
                static_mainGameValues.wordsListInGame.Remove(list);
            }
        }
        else
        {
            markerChildObject.SetActive(true);
            if (!static_mainGameValues.wordsListInGame.Contains(list))
            {
                static_mainGameValues.wordsListInGame.Add(list);
            }
        }
    }

    public void DeleteList(GameObject prefab)
    {
        FindObjectOfType<ListsManager>().DeleteListFromGame(list, prefab);
    }
}
