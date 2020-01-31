using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class static_mainGameValues
{
    public static int games = 0;
    public static int wordsFound;
    public static int villgersVictory = 0;
    public static int werewolfVictory = 0;
    public static int mayorVillager = 0;
    public static int mayorWerewolf = 0;
    public static int mayorSeer = 0;

    public static int[] difficulty = { 0, 1, 2, 3 };
    public static int playerQtd;
    public static int currentDifficulty = 0;
    public static int wordQuantity = 5;
    public static int gameTimer = 2;
    public static int roleTimer = 4;
    public static int wordTimer = 3;
    public static float playerTimer = 60;
    public static float werewolfTimer = 15;
    public static bool isTimer = true;
    public static bool minuteWarning = true;
    public static bool halfMinuteWarning = true;
    public static List<GameObject> activeRolesList = new List<GameObject>();
    public static List<WordList> allWordLists = new List<WordList>();
    public static List<WordList> wordsListInGame = new List<WordList>();
    public static List<string> allWords = new List<string>();
    public static List<string> wordsInGame = new List<string>();
    public static string selectedWord;
    public static string mayorSecretRole;
    public static string finishedCondition;
}
