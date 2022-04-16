using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonComponent : MonoBehaviour
{
    private GameController gameController;
    private TextMeshProUGUI buttonText;
    [SerializeField] char value = 'A';

    void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetValue(char newVal)
    {
        value = newVal;
        buttonText.text = value.ToString();
    }

    public char GetValue()
    {
        return value;
    }

    public void SendInputToController()
    {
        gameController.RecieveButtonInput(value);
    }

}
