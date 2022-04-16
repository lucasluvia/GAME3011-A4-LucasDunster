using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SlotComponent : MonoBehaviour
{
    public TextMeshProUGUI slotText;

    void Start()
    {
        slotText = GetComponentInChildren<TextMeshProUGUI>();
        RevertTextValue();
    }

    public void RevertTextValue()
    {
        slotText.text = "-";
    }
}
