using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TextSignView : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    public void SetText(string message) => text.text = message;
    public void SetColor(Color color) => text.color = color;
}
