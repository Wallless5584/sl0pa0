using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public void RespawnPressed()
    {
        GameManager.instance.RespawnOnSpawn();
    }

    public void ResetPressed()
    {
        GameManager.instance.ResetLevel();
    }
}
