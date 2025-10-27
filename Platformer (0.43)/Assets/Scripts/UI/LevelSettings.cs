using UnityEngine;

[CreateAssetMenu]
public class LevelSettings : ScriptableObject
{
    public string LevelName;
    public Sprite MenuSprite;
    public float RoomCount;
    public RoomController[] RoomVars;
    public RoomController StartRoom;
    public RoomController EndRoom;
    public bool KeepRoomOrder;
}
