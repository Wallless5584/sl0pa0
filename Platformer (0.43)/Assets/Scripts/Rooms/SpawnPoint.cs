using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public void PlayerEntered()
    {
        GameManager.instance.SpawnTriggered(this);
    }
}
