using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    // Menu scene string
    public const string MENU_SCENE = "MainMenu";
    // Level scene string
    private const string LEVEL_SCENE = "GameLevel";

    // Control type text
    [SerializeField]
    private Text _controlTypeText;
    // Pitch inverted text
    [SerializeField]
    private Text _isPitchInvertedText;

    private void Start()
    {
        // Update UI
        _controlTypeText.text = GameSettings.ControlType.ToString();
        switch (GameSettings.IsPitchInverted)
        {
            case true:
                _isPitchInvertedText.text = "Pitch Inv.";
                break;
            case false:
                _isPitchInvertedText.text = "Pitch N-Inv.";
                break;
        }
    }

    // Loads game level
    public void LoadGameLevel()
    {
        // Loads level
        SceneManager.LoadScene(LEVEL_SCENE);
    }

    // Change control type
    public void ChangeControlType()
    {
        switch(GameSettings.ControlType)
        {
            case ControlType.YawControl:
                GameSettings.ControlType = ControlType.PitchControl;
                break;
            case ControlType.PitchControl:
                GameSettings.ControlType = ControlType.YawControl;
                break;
        }
        _controlTypeText.text = GameSettings.ControlType.ToString();
    }

    // Change pitch inversion
    public void ChangePitchInversion()
    {
        GameSettings.IsPitchInverted = !GameSettings.IsPitchInverted;
        switch (GameSettings.IsPitchInverted)
        {
            case true:
                _isPitchInvertedText.text = "Pitch Inv.";
                break;
            case false:
                _isPitchInvertedText.text = "Pitch N-Inv.";
                break;
        }
    }

    // Exits the game
    public void ExitGame()
    {
        Application.Quit();
    }
}
