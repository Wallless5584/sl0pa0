using UnityEngine;

public class LevelGeneratorController : MonoBehaviour
{

    public LevelSettings LevelToGenerate;
    public LevelSettings SelectedLevel;
    public LevelSettings GeneratedLevel;
    public int SelectedLevelIndex;

    GameManager gm;

    

    void OnValidate()
    {
        gm = GetComponentInChildren<GameManager>();
    }

    void Update()
    {
        
    }

    public void SwitchSelectedLevel(bool positiveDirection)
    {
        int delta;
        if (positiveDirection)
        {
            delta = 1;
        }
        else
        {
            delta = -1;
        }

        int maxIndex = gm.GameLevels.Length - 1;
        const int minIndex = 0;
        if (SelectedLevelIndex + delta > maxIndex)
        {
            SelectedLevelIndex = minIndex;
        }
        else if(SelectedLevelIndex + delta < minIndex)
        {
            SelectedLevelIndex = maxIndex;
        }
        else
        {
            SelectedLevelIndex += delta;
        }

        UpdateSelectedLevel();
    }

    public void SwitchSelectedLevel(int index)
    {
        int maxIndex = gm.GameLevels.Length - 1;
        const int minIndex = 0;
        if (index > maxIndex)
        {
            SelectedLevelIndex = maxIndex;
        }
        else if (index < minIndex)
        {
            SelectedLevelIndex = minIndex;
        }
        else
        {
            SelectedLevelIndex = index;
        }

        UpdateSelectedLevel();
    }

    void UpdateSelectedLevel()
    {
        SelectedLevel = gm.GameLevels[SelectedLevelIndex];
    }

    public void SelectedLevelToGenerate()
    {
        LevelToGenerate = SelectedLevel;
    }

    public void GenerateLevel()
    {
        if (CheckForLevelToClear())
        {
            ClearLevel();
        }
        GeneratedLevel = LevelToGenerate;
        gm.LoadLevel(GeneratedLevel);
    }

    public bool RegenerateLevel()
    {
        ClearLevel();
        gm.LoadLevel(GeneratedLevel);
        return !GeneratedLevel.KeepRoomOrder;
    }

    public bool CheckForLevelToGenerate()
    {
        return LevelToGenerate != null;
    }

    public bool CheckGeneratedVsToGenerateLevels()
    {
        return LevelToGenerate == GeneratedLevel;
    }


    public bool CheckForLevelToClear()
    {
        return gm.CheckHasLevelLoaded();
    }

    public void ClearLevel()
    {
        gm.ClearLevelEditor();
    }

}
