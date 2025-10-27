using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMenu : MonoBehaviour
{
    [SerializeField] LevelCard cardPrefab;
    [SerializeField] Transform content;

    public void PlayButtonPressed()
    {
        GameManager.instance.SwitchState(GameManager.GameState.Load);
    }
    public void BackButtonPressed()
    {
        //GameManager.instance.SwitchState(GameManager.GameState.None);
    }

    public void UpdateLevelContent()
    {
        int level_card_count = content.childCount;
        for (int i = 0; i < level_card_count; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }
        Debug.Log("Updating level content!");
        foreach (LevelSettings levelInfo in GameManager.instance.GameLevels)
        {
            Instantiate(cardPrefab, content).Setup(levelInfo);
        }
    }
}
