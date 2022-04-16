using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float totalTime = 120.0f;
    private float targetTime;

    private GameController gameController;
    public bool isPaused;

    void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        timerText = GetComponentInChildren<TextMeshProUGUI>();
        ResetTimer();
    }

    void Update()
    {
        if(!isPaused)
        {
            targetTime -= Time.deltaTime;
            timerText.text = ((int)targetTime).ToString();
            if (targetTime <= 0)
            {
                gameController.EndGamePhase(false);
            }
        }
    }

    public void ResetTimer()
    {
        targetTime = totalTime;
    }
}
