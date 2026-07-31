using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[SelectionBase]
public class RoomController : MonoBehaviour
{
    [SerializeField] RoomButton[] buttonsInRoom;
    List<RoomButton> pressedButtons = new List<RoomButton>();
    [SerializeField] DoorController exit;
    public SpawnPoint spawn;
    public UnityEvent OnRoomCompletion;

    public void RoomButtonPressed(RoomButton pressed_button)
    {
        //pressed_button.Collect();
        pressedButtons.Add(pressed_button);
        Debug.Log("Coin picked up!");
        CheckRoomCompletion();
    }

    public bool CheckRoomCompletion()
    {
        if (buttonsInRoom.Length == pressedButtons.Count) 
        {
            //exit.SetState(true);
            OnRoomCompletion.Invoke();
            Debug.Log($"Level completed!");
            return true;
        }
        //Debug.Log($"Level incomplete! {pressedButtons.Count}/{buttonsInRoom.Length} coins collected. ");
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

    public void Activate()
    {
        spawn.Activate();
        foreach (RoomButton item in buttonsInRoom)
        {
            item.Activate();
        }
    }

    public void ResetRoom()
    {
        pressedButtons.Clear();
        spawn.Reset();
        foreach (RoomButton item in buttonsInRoom)
        {
            item.ResetObject();
        }
        if (exit)
        {
            exit.SetState(false);
        }
    }
}
