using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public Difficulty DifficultyLevel;
    public Skill SkillLevel;

    private TextOutput textOutput;
    private RectTransform slotContainerTransform;

    private TimerController timerController;

    //List of Button Components
    public List<ButtonComponent> buttonList = new List<ButtonComponent>();
    public List<SlotComponent> slotList = new List<SlotComponent>();
    public List<char> availableLetters = new List<char>();
    public List<char> password = new List<char>();
    public List<char> attemptInput = new List<char>();

    public int charsInPassword = 4;

    int guessedLetters = 0;

    int lettersCorrect;
    int lettersMisplaced;


    void Start()
    {
        textOutput = GameObject.FindWithTag("Log").GetComponent<TextOutput>();
        slotContainerTransform = GameObject.FindWithTag("SlotContainer").GetComponent<RectTransform>();
        timerController = GameObject.FindWithTag("Timer").GetComponent<TimerController>();

        foreach (GameObject button in GameObject.FindGameObjectsWithTag("Button"))
        {
            if (button.GetComponent<ButtonComponent>())
                buttonList.Add(button.GetComponent<ButtonComponent>());
        }
        
        foreach (GameObject slot in GameObject.FindGameObjectsWithTag("Slot"))
        {
            if (slot.GetComponent<SlotComponent>())
                slotList.Add(slot.GetComponent<SlotComponent>());
        }

        RandomizeButtonValues();
        GeneratePassword();
        TriggerSkillEffect();
    }

    public void RecieveButtonInput(char inputValue)
    {
        if(guessedLetters < charsInPassword)
        {
            slotList[guessedLetters].slotText.text = inputValue.ToString();
            guessedLetters++;

            attemptInput.Add(inputValue);
            Debug.Log(inputValue);
        }
        else
        {
            textOutput.AddLine("All slots full! CLEAR or SUBMIT your guess!");
        }
    }

    private void RandomizeButtonValues()
    {
        CheckDifficulty();

        if (availableLetters.Count > 0)
            availableLetters.Clear();

        char value = (char)('A' + Random.Range(0, 26));
        foreach (ButtonComponent button in buttonList)
        {
            while (availableLetters.Contains(value))
            {
                value = (char)('A' + Random.Range(0, 26));
            }
            button.SetValue(value);
            availableLetters.Add(value);
        }
    }

    private void GeneratePassword()
    {
        CheckDifficulty();

        char randomValue;

        for (int i = 0; i < charsInPassword; i++)
        {
            randomValue = availableLetters[Random.Range(0, availableLetters.Count)];

            while (password.Contains(randomValue))
            {
                randomValue = availableLetters[Random.Range(0, availableLetters.Count)]; 
            }

            password.Add(randomValue);
        }

    }

    private void CheckDifficulty()
    {
        switch (DifficultyLevel)
        {
            case Difficulty.EASY:
                charsInPassword = 4;
                break;
            case Difficulty.MEDIUM:
                charsInPassword = 5;
                break;
            case Difficulty.HARD:
                charsInPassword = 6;
                break;
        }

        if(charsInPassword == 4)
        {
            slotList[4].gameObject.SetActive(false);
            slotList[5].gameObject.SetActive(false);
            slotContainerTransform.anchoredPosition = new Vector3(200f, 300f);
        }
        else if(charsInPassword == 5)
        {
            slotList[4].gameObject.SetActive(true);
            slotList[5].gameObject.SetActive(false);
            slotContainerTransform.anchoredPosition = new Vector3(100f, 300f);
        }
        else if(charsInPassword == 6)
        {
            slotList[4].gameObject.SetActive(true);
            slotList[5].gameObject.SetActive(true);
            slotContainerTransform.anchoredPosition = new Vector3(0f, 300f);
        }

    }

    private void TriggerSkillEffect()
    {
        foreach(ButtonComponent button in buttonList)
        {
            button.gameObject.GetComponent<Button>().interactable = true;
        }

        int value = 0;
        char valueChar;
        switch (SkillLevel)
        {
            case Skill.EXPERT:
                for (int i = 0; i < 2; i++)
                {
                    value = Random.Range(0, buttonList.Count);
                    valueChar = buttonList[value].GetValue();
                    while (password.Contains(valueChar))
                    {
                        value = Random.Range(0, buttonList.Count);
                        valueChar = buttonList[value].GetValue();
                    }
                    buttonList[value].gameObject.GetComponent<Button>().interactable = false;
                }
                break;
            case Skill.ADVANCED:
                value = Random.Range(0, buttonList.Count);
                valueChar = buttonList[value].GetValue();
                while (password.Contains(valueChar))
                {
                    value = Random.Range(0, buttonList.Count);
                    valueChar = buttonList[value].GetValue();
                }
                buttonList[value].gameObject.GetComponent<Button>().interactable = false;
                break;
        }
    }

    private void CompareToSolution()
    {
        List<char> CorrectInputs = new List<char>();
        lettersCorrect = 0;
        lettersMisplaced = 0;
        for (int i = 0; i < charsInPassword; i++)
        {
            if(attemptInput[i] == password[i])
            {
                lettersCorrect++;
                CorrectInputs.Add(password[i]);
            }
        }
        if (lettersCorrect < password.Count)
        {
            for (int i = 0; i < charsInPassword; i++)
            {
                if (attemptInput[i] != password[i])
                {
                    if(password.Contains(attemptInput[i]))
                    {
                        lettersMisplaced++;
                    }
                }
            }
        }
    }

    string attemptAsString;
    public void SubmitPasswordAttempt()
    {
        if(attemptInput.Count == charsInPassword)
        {   
            attemptAsString = "";
            foreach (char guess in attemptInput)
            {
                attemptAsString += guess.ToString();
            }

            CompareToSolution();
            textOutput.AddLine("GUESS: " + attemptAsString + " - Correct: " + lettersCorrect + " Misplaced: " + lettersMisplaced);

            ClearAttempt();

            if (lettersCorrect == charsInPassword)
            {
                textOutput.AddLine("CORRECT!");
                RandomizeButtonValues();
                GeneratePassword();
                TriggerSkillEffect();
                EndGamePhase(true);
            }
        }
        else
        {
            textOutput.AddLine("Please enter all " + charsInPassword.ToString() + " letters to guess!");
        }

    }

    public void ClearAttempt()
    {
        attemptInput.Clear();
        for (int i = 0; i < guessedLetters; i++)
        {
            slotList[i].slotText.text = "-";
        }
        guessedLetters = 0;
    }

    public void RestartGame()
    {
        RandomizeButtonValues();
        GeneratePassword();
        TriggerSkillEffect();
        timerController.ResetTimer();
        textOutput.AddLine("RESETTING GAME!");
    }

    public void EndGamePhase(bool isWinState)
    {
        foreach (ButtonComponent button in buttonList)
        {
            button.gameObject.GetComponent<Button>().interactable = false;
        }

        timerController.isPaused = true;

        if(isWinState)
            textOutput.AddLine("YOU WON!");
        else
            textOutput.AddLine("TIME'S OUT! YOU LOST!");

        textOutput.AddLine("Reset on main page to play again!");
    }

}