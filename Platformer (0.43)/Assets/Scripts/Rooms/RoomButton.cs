using UnityEngine;

[SelectionBase]
public class RoomButton : MonoBehaviour, ICollectable
{
    public CollectableType Type { get; set; }
    [SerializeField] SpriteRenderer SpriteRenderer;
    [SerializeField] Sprite UnpressedActive;
    [SerializeField] Sprite PressedActive;
    [SerializeField] Sprite UnpressedInactive;
    [SerializeField] Sprite PressedInactive;
    [SerializeField] TrapAnimator ButtonAnimator;
    bool Active = true;

    private void Start()
    {
        SpriteRenderer.sprite = UnpressedActive;
        ButtonAnimator.SwitchState(TrapState.Active);
    }

    public void Collect()
    {
        if (Active == true)
        {
            Active = false;
            ButtonAnimator.SwitchState(TrapState.Off);
            SpriteRenderer.sprite = PressedActive;
            GameManager.instance.OnRoomButtonPress(this);
        }
        else
        {
            SpriteRenderer.sprite = PressedInactive;
        }
        //Debug.Log($"Enabled is {gameObject.active}!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collect();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Active == true)
        {
            SpriteRenderer.sprite = UnpressedActive;
        }
        else
        {
            SpriteRenderer.sprite = UnpressedInactive;
        }
    }

    public void PlayerEntered()
    {
        Collect();
    }

    public void ResetObject()
    {
        Active = true;
        ButtonAnimator.SwitchState(TrapState.Active);
        SpriteRenderer.sprite = UnpressedActive;
    }
}
