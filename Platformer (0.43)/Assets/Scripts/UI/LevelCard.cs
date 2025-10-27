using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCard : MonoBehaviour
{
    [SerializeField] Image BgImg;
    [SerializeField] TMP_Text LevelNameText;
    [SerializeField] Button PlayButton;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Setup(LevelSettings levelInfo)
    {
        if (levelInfo.MenuSprite != null)
        {
            BgImg.sprite = levelInfo.MenuSprite;
        }
        else
        {
            Debug.LogWarning("Level info has no background image!");
        }
        LevelNameText.text = levelInfo.LevelName;
    }

    public void PlayButtonPressed()
    {
        GameManager.instance.SelectedLevel = (ushort)transform.GetSiblingIndex();
        GameManager.instance.SwitchState(GameManager.GameState.Load);
    }
}
