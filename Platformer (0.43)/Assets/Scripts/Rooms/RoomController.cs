using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[SelectionBase]
public class RoomController : MonoBehaviour
{
    [SerializeField] RoomButton[] buttonsInRoom;
    List<RoomButton> pressedButtons = new List<RoomButton>();
    [SerializeField] DoorController exit;
    [SerializeField] SpriteParallaxEffect[] ParallaxBGs;
    public SpawnPoint spawn;
    public UnityEvent OnRoomCompletion;

    public void RoomButtonPressed(RoomButton pressed_button)
    {
        //pressed_button.Collect();
        pressedButtons.Add(pressed_button);
        Debug.Log("Coin picked up!");
        if (CheckRoomCompletion())
        {
            GameManager.instance.RoomComplete();
        }

    }

    public void AddHint(SpriteParallaxEffect hint)
    {
        SpriteParallaxEffect[] newParallaxBGs = new SpriteParallaxEffect[ParallaxBGs.Length+1];
        if (ParallaxBGs.Length > 0)
        {
            for (int i = 0; i < newParallaxBGs.Length; i++)
            {
                newParallaxBGs[i] = ParallaxBGs[i];
            }
        }
        newParallaxBGs[newParallaxBGs.Length-1] = hint;
        ParallaxBGs = newParallaxBGs;
    }

    public bool CheckRoomCompletion()
    {
        if (buttonsInRoom.Length == pressedButtons.Count) 
        {
            //exit.SetState(true);
            OnRoomCompletion.Invoke();
            Debug.Log($"Room completed!");
            ActivateSpawn();
            return true;
        }
        //Debug.Log($"Level incomplete! {pressedButtons.Count}/{buttonsInRoom.Length} coins collected. ");
        return false;
    }

    public void CloseExit()
    {
        exit.SetState(false);
        foreach (SpriteParallaxEffect parallax in ParallaxBGs)
        {
            parallax.Disable();
        }
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

    public void ActivateSpawn()
    {
        spawn.Activate();
    }

    void ActivateButtons()
    {
        foreach (RoomButton item in buttonsInRoom)
        {
            item.Activate();
        }
    }

    void ActivateParallax()
    {
        foreach (SpriteParallaxEffect parallax in ParallaxBGs)
        {
            parallax.Setup();
        }
    }

    public void SetupOnSpawnEnter()
    {
        ActivateButtons();
        ActivateParallax();
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
