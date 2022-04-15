using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private Canvas worldCanvas;
    [SerializeField] private Canvas gameCanvas;

    void Start()
    {
        worldCanvas.enabled = true;
        gameCanvas.enabled = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SwapCanvases();
        }
    }

    private void SwapCanvases()
    {
        worldCanvas.enabled = !worldCanvas.enabled;
        gameCanvas.enabled = !gameCanvas.enabled;
    }
}
