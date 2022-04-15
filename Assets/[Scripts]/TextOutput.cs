using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextOutput : MonoBehaviour
{
    [SerializeField] int MaxLines;

    private List<string> TextLog = new List<string>();
    private string StringForOutput = "";

    private TextMeshProUGUI outputTextObject;

    void Start()
    {
        outputTextObject = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void AddLine(string input)
    {
        TextLog.Add(input);

        if(TextLog.Count >= MaxLines)
        {
            TextLog.RemoveAt(0);
        }

        StringForOutput = "";

        foreach (string line in TextLog)
        {
            StringForOutput += line;
            StringForOutput += "\n";
        }

        outputTextObject.text = StringForOutput;

    }
}
