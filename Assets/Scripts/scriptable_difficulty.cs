using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Difficulty", menuName = "New Difficulty")]
public class scriptable_difficulty : ScriptableObject
{
    public string singularTitle;
    public string pluralTitle;
    public Color color;
}
