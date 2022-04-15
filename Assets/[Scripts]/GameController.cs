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

    //List of Button Components
    public List<ButtonComponent> buttonList = new List<ButtonComponent>();
    public List<SlotComponent> slotList = new List<SlotComponent>();
    public List<char> availableLetters = new List<char>();
    public List<char> password = new List<char>();
    public List<char> attemptInput = new List<char>();

    public int charsInPassword = 4;

    int numOfPasswords;
    int solvedPasswords;

    int guessedLetters = 0;

    int lettersCorrect;
    int lettersMisplaced;

    void Start()
    {
        textOutput = GameObject.FindWithTag("Log").GetComponent<TextOutput>();
        slotContainerTransform = GameObject.FindWithTag("SlotContainer").GetComponent<RectTransform>();

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
            //Debug.Log("Queue Full");
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
            //Debug.Log("Generated value: " + value.ToString());
            while (availableLetters.Contains(value))
            {
                //Debug.Log("Rerolling...");
                value = (char)('A' + Random.Range(0, 26));
                //Debug.Log("Generated value: " + value.ToString());
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
            Debug.Log(randomValue.ToString());

            while (password.Contains(randomValue))
            {
                Debug.Log("Rerolling...");
                randomValue = availableLetters[Random.Range(0, availableLetters.Count)]; 
                Debug.Log(randomValue.ToString());
            }

            password.Add(randomValue);
        }

        textOutput.AddLine("A new password has been set!");
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


        // Used to check if the player has manually altered the difficulty or skill level

        // If theres a higher skill level, there shouldnt be as many buttons, which will be handled elsewhere
        // BUT it should also give them more time

        // If the difficulty has changed, more letters should be added to the code length and more codes should be created.
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
                slotList[i].slotState = SlotState.CORRECT;
                CorrectInputs.Add(password[i]);
            }
        }
        if (lettersCorrect < 4)
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

            if (lettersCorrect == 4)
            {
                textOutput.AddLine("CORRECT! " + (numOfPasswords - solvedPasswords) + " passwords remain!");
                solvedPasswords++;
                RandomizeButtonValues();
                GeneratePassword();
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

}