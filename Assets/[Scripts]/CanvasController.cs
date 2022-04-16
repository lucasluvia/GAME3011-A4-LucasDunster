using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private Canvas worldCanvas;
    [SerializeField] private Canvas gameCanvas;

    private TimerController timerController;

    void Start()
    {
        worldCanvas.enabled = true;
        gameCanvas.enabled = false;
        timerController = GameObject.FindWithTag("Timer").GetComponent<TimerController>();
        timerController.isPaused = worldCanvas.enabled;
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
        timerController.isPaused = worldCanvas.enabled;
    }
}
