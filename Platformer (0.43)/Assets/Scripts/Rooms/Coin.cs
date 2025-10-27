using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Coin : MonoBehaviour, ITriggerZone, ICollectable
{
    public CollectableType Type { get; set; }

    public void Collect()
    {
        gameObject.SetActive(false);
        //Debug.Log($"Enabled is {gameObject.active}!");
    }

    public void PlayerEntered()
    {
        Collect();
    }

    public void ResetObject()
    {
        gameObject.SetActive(true);
    }
}
