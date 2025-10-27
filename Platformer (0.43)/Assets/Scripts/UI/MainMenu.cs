using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainMenu : MonoBehaviour
{
    public UnityEvent OnLevelSelectButtonPressed;
    public UnityEvent OnOptionsButtonPressed;
    public UnityEvent OnExitButtonPressed;

    public void LevelSelectButtonPressed()
    {
        OnLevelSelectButtonPressed.Invoke();
        UIManager.instance.SwitchState(UIManager.UIState.LevelMenu);
    }

    public void OptionsButtonPressed()
    {
        OnOptionsButtonPressed.Invoke();
        UIManager.instance.SwitchState(UIManager.UIState.Options);
    }

    public void ExitButtonPressed()
    {
        OnExitButtonPressed.Invoke();
        Application.Quit();
    }
}
