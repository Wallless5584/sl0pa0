using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//[SelectionBase]
public class RoomController : GameWorldPart
{
    List<RoomButton> buttonsInRoom = new List<RoomButton>();
    List<RoomButton> pressedButtons = new List<RoomButton>();
    [SerializeField] DoorController exit;
    List<GameWorldPart> roomParts;
    public SpawnPoint spawn;
    public UnityEvent OnRoomCompletion;


    public void RoomButtonPressed(RoomButton pressed_button)
    {
        //pressed_button.Collect();
        pressedButtons.Add(pressed_button);
        Debug.Log("Button pressed!");
        CheckRoomCompletion();
    }

    public bool CheckRoomCompletion()
    {
        Debug.Log($"{buttonsInRoom.Count} ? {pressedButtons.Count}");
        if (buttonsInRoom.Count == pressedButtons.Count) 
        {
            //exit.SetState(true);
            OnRoomCompletion.Invoke();
            Debug.Log($"Room completed!");
            GameManager.instance.RoomComplete();
            //ActivateSpawn();
            return true;
        }
        //Debug.Log($"Level incomplete! {pressedButtons.Count}/{buttonsInRoom.Length} coins collected. ");
        return false;
    }

    //public void CloseExit()
    //{
    //    exit.SetState(false);
    //    foreach (SpriteParallaxEffect parallax in ParallaxBGs)
    //    {
    //        parallax.Disable();
    //    }
    //}

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

    //public void AddHint(SpriteParallaxEffect hint)
    //{
    //    SpriteParallaxEffect[] newParallaxBGs = new SpriteParallaxEffect[ParallaxBGs.Length + 1];
    //    if (ParallaxBGs.Length > 0)
    //    {
    //        for (int i = 0; i < newParallaxBGs.Length; i++)
    //        {
    //            newParallaxBGs[i] = ParallaxBGs[i];
    //        }
    //    }
    //    newParallaxBGs[newParallaxBGs.Length - 1] = hint;
    //    ParallaxBGs = newParallaxBGs;
    //}

    //public void ActivateSpawn()
    //{
    //    //spawn.Activate();
    //}
    //void ActivateButtons()
    //{
    //    foreach (RoomButton item in buttonsInRoom)
    //    {
    //        item.Activate();
    //    }
    //}
    //void ActivateParallax()
    //{
    //    foreach (SpriteParallaxEffect parallax in ParallaxBGs)
    //    {
    //        parallax.Setup();
    //    }
    //}

    //public void SetupOnSpawnEnter()
    //{
    //    ActivateButtons();
    //    ActivateParallax();
    //}

    //public void ResetRoom()
    //{
    //    //pressedButtons.Clear();
    //    //spawn.Reset();
    //    //foreach (IRoomPart item in buttonsInRoom)
    //    //{
    //    //    item.Reset();
    //    //}
    //    //if (exit)
    //    //{
    //    //    exit.SetState(false);
    //    //}
    //}

    public void RegisterPart(GameWorldPart part)
    {
        Debug.Log($"Registering part {part.name}");
        if(roomParts == null)
        {
            roomParts = new List<GameWorldPart>();
        }
        else if (roomParts.Contains(part))
        {
            return;
        }
        roomParts.Add(part);
    }

    public void RegisterButton(RoomButton button)
    {
        Debug.Log($"Registering Button {button.name}");
        if (buttonsInRoom == null)
        {
            buttonsInRoom = new List<RoomButton>();
        }
        else if (buttonsInRoom.Contains(button))
        {
            return;
        }
        buttonsInRoom.Add(button);
    }

    override public void Prepare()
    {
        Debug.Log($"Preparing Room {name}");
        foreach (GameWorldPart part in roomParts)
        {
            Debug.Log($">> Preparing part {part.name}");
            part.Prepare();
        }
    }

    override public void Activate()
    {
        Debug.Log($"Activating Room {name}");
        foreach (GameWorldPart part in roomParts)
        {
            Debug.Log($">> Activating part {part.name}");
            part.Activate();
        }
        CheckRoomCompletion();
    }

    override public void Reset()
    {
        pressedButtons.Clear();
        foreach (GameWorldPart part in roomParts)
        {
            part.Reset();
        }
    }

    override public void Deactivate()
    {
        foreach (GameWorldPart part in roomParts)
        {
            part.Deactivate();
        }
    }
}
