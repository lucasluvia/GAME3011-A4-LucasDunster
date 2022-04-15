using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SlotComponent : MonoBehaviour
{
    public SlotState slotState = SlotState.WRONG;
    public TextMeshProUGUI slotText;

    void Start()
    {
        slotText = GetComponentInChildren<TextMeshProUGUI>();
    }
}
