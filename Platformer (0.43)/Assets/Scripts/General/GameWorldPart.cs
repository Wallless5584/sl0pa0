using UnityEngine;

public abstract class GameWorldPart : MonoBehaviour
{
    /// <summary>
    /// Called on previous room spawn first triggered
    /// </summary>
    public abstract void Prepare();

    /// <summary>
    /// Called on this room spawn first triggered
    /// </summary>
    public abstract void Activate();

    /// <summary>
    /// Called on next room spawn first triggered
    /// </summary>
    public abstract void Deactivate();

    /// <summary>
    /// Called on level reset
    /// </summary>
    public abstract void Reset();

    /// <summary>
    /// Should be called to connect with a RoomController, returns it.
    /// </summary>
    protected RoomController Register()
    {
        RoomController roomController;
        if (this.transform.parent.TryGetComponent<RoomController>(out roomController))
        {
            roomController.RegisterPart(this);
        }
        else if (this.transform.parent.parent.TryGetComponent<RoomController>(out roomController))
        {
            roomController.RegisterPart(this);
        }
        return roomController;
    }
}
