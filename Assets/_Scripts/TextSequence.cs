using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TextSequence", menuName = "ScriptableObjects/TextSequence", order = 1)]
public class TextSequence : ScriptableObject
{
    public string[] texts;
    public bool activeText;
}
