using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class LevelCard : MonoBehaviour
{
    [SerializeField] Image BgImg;
    [SerializeField] Image BlockImg;
    [SerializeField] Image PanelImg;
    [SerializeField] TMP_Text LevelNameText;
    [SerializeField] TMP_Text BlockText;
    [SerializeField] Button PlayButton;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Setup(LevelSettings levelInfo)
    {
        if (levelInfo.BlockSprite != null)
        {
            BlockImg.sprite = levelInfo.BlockSprite;
        }

        if (levelInfo.MenuSprite != null)
        {
            BgImg.sprite = levelInfo.MenuSprite;
        }
        else
        {
            Debug.LogWarning("Level info has no background image!");
        }
        LevelNameText.text = levelInfo.LevelName;
        CheckAccess(levelInfo);
    }

    public void PlayButtonPressed()
    {
        GameManager.instance.SelectedLevel = (ushort)transform.GetSiblingIndex();
        GameManager.instance.SwitchState(GameManager.GameState.Load);
    }

    public void CheckAccess(LevelSettings levelInfo)
    {
        if (levelInfo.Accessible)
        {
            BlockImg.enabled = false;
            BlockText.enabled = false;
            PlayButton.interactable = true;
            LevelNameText.enabled = true;
            BgImg.enabled = true;
            PanelImg.color = Color.white;
        }
        else
        {
            BlockImg.enabled = true;
            BlockText.enabled = true;
            PlayButton.interactable = false;
            LevelNameText.enabled = false;
            BgImg.enabled = false;
            PanelImg.color = Color.clear;
        }
    }
}
