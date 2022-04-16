using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldController : MonoBehaviour
{
    private GameController gameController;

    [SerializeField] Dropdown diffDropdown;
    [SerializeField] Dropdown skillDropdown;

    void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        SetDifficulty();
        SetSkillLevel();
    }

    public void SetDifficulty()
    {
        string newDiff = diffDropdown.options[diffDropdown.value].text;

        if (newDiff == "EASY")
            gameController.DifficultyLevel = Difficulty.EASY;
        if (newDiff == "MEDIUM")
            gameController.DifficultyLevel = Difficulty.MEDIUM;
        if (newDiff == "HARD")
            gameController.DifficultyLevel = Difficulty.HARD;
    }

    public void SetSkillLevel()
    {
        string newDiff = skillDropdown.options[skillDropdown.value].text;

        if (newDiff == "BEGINNER")
            gameController.SkillLevel = Skill.BEGINNER;
        if (newDiff == "ADVANCED")
            gameController.SkillLevel = Skill.ADVANCED;
        if (newDiff == "EXPERT")
            gameController.SkillLevel = Skill.EXPERT;
    }
}
