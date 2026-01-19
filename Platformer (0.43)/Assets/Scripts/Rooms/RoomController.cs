using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[SelectionBase]
public class RoomController : MonoBehaviour
{
    [SerializeField] Coin[] coinsInRoom;
    List<Coin> pickedCoins = new List<Coin>();
    [SerializeField] DoorController exit;
    public SpawnPoint spawn;
    public UnityEvent OnRoomCompletion;

    public void CoinPickedUp(Coin picked_coin)
    {
        picked_coin.Collect();
        pickedCoins.Add(picked_coin);
        Debug.Log("Coin picked up!");
        CheckRoomCompletion();
    }

    public bool CheckRoomCompletion()
    {
        if (coinsInRoom.Length == pickedCoins.Count) 
        {
            //exit.SetState(true);
            OnRoomCompletion.Invoke();
            Debug.Log($"Level completed!");
            return true;
        }
        Debug.Log($"Level incomplete! {pickedCoins.Count}/{coinsInRoom.Length} coins collected. ");
        return false;
    }

    public void CloseExit()
    {
        exit.SetState(false);
    }

    public Vector3 GetExitPos()
    {
        Debug.Log($"{gameObject.name}, Exit pos: {exit.transform.position}");
        return exit.transform.position;
    }

    public Vector3 GetSpawnPos()
    {
        Debug.Log($"{gameObject.name}, Spawn pos: {spawn.transform.position}");
        return spawn.transform.position;
    }

    public void ResetRoom()
    {
        pickedCoins.Clear();
        foreach (Coin item in coinsInRoom)
        {
            item.ResetObject();
        }
        if (exit)
        {
            exit.SetState(false);
        }
    }
}
