using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectWord : MonoBehaviour
{
    public void Select(TextMeshProUGUI wordText)
    {
        MatchManager gameManager = FindObjectOfType<MatchManager>();
        gameManager.SelectWord(wordText);
    }
}
