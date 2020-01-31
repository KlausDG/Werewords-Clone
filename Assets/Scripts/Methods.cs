using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Methods : MonoBehaviour
{
    public static void SaveGame(List<WordList> list, GameStatistics game)
    {
        string json = JsonHelper.ToJson(list, game, true);
        PlayerPrefs.SetString("Save", json);
        PlayerPrefs.Save();
    }

    public static List<WordList> LoadGame()
    {
        List<WordList> list = new List<WordList>();
        if (PlayerPrefs.HasKey("Save"))
        {
            string jsonData = PlayerPrefs.GetString("Save");
            list = JsonHelper.FromJson<WordList>(jsonData);
        }
        else
        {
            Debug.Log("Não existe");
        }
        return list;
    }

    public static GameStatistics LoadStatistics()
    {
        GameStatistics gs = null;
        if (PlayerPrefs.HasKey("Save"))
        {
            string jsonData = PlayerPrefs.GetString("Save");
            gs = JsonHelper.StatisticsFromJson<GameStatistics>(jsonData);
        }
        else
        {
            Debug.Log("Não existe");
        }
        return gs;
    }

    public static void ChangeText(TextMeshProUGUI obj, string text)
    {
        obj.text = text;
    }

    public static void ChangeColor(Color color, Image image)
    {
        image.color = color;
    }

    public static void ChangeColor(Color color, TextMeshProUGUI text)
    {
        text.color = color;
    }

    public static void ChangeColor(Color color, Image image, TextMeshProUGUI text)
    {
        image.color = color;
        text.color = color;
    }

    public static void ChangeAlpha(Image image)
    {
        Color imageColor = image.color;
        if (imageColor.a == 1)
        {
            imageColor.a = 0.4f;
        }
        else
        {
            imageColor.a = 1f;
        }
        image.color = imageColor;
    }

    public static int UpdateQuantity(int qtd)
    {
        return qtd;
    }

    public static GameObject FindParentWithTag(GameObject childObject, string tag)
    {
        Transform t = childObject.transform;
        while (t.parent != null)
        {
            if (t.parent.tag == tag)
            {
                return t.parent.gameObject;
            }
            t = t.parent.transform;
        }
        return null;
    }

    public static void SwapPanels(GameObject on, GameObject off)
    {
        on.SetActive(true);
        off.SetActive(false);
    }
}
