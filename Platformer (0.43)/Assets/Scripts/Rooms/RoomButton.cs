using System.Threading;
using TMPro;
using UnityEngine;

[SelectionBase]
public class RoomButton : MonoBehaviour, ICollectable
{
    public CollectableType Type { get; set; }
    [SerializeField] SpriteRenderer ActiveSR;
    [SerializeField] SpriteRenderer InactiveSR;
    [SerializeField] TMP_Text TimerText;

    [SerializeField] Sprite UnpressedActive;
    [SerializeField] Sprite PressedActive;
    [SerializeField] Sprite UnpressedInactive;
    [SerializeField] Sprite PressedInactive;
    bool active = false;
    float recordedTime;



    private void Start()
    {
        ResetButton();
    }

    private void Update()
    {
        if (active)
        {
            recordedTime += Time.deltaTime;
            int minTimer = (int)(recordedTime / 60);
            float secTimer = recordedTime + Time.deltaTime - minTimer * 60;

            float secDisplayTime = Mathf.Floor(secTimer * 1000f) / 1000;
            TimerText.text = $"{minTimer:D2}:{secDisplayTime.ToString("00.000")}";
        }
        else
        {
            ActiveSR.color = new Color(1f, 1f, 1f, ActiveSR.color.a - Time.deltaTime);
            if (ActiveSR.color.a <= 0f)
            {
                enabled = false;
            }
        }
    }

    public void Collect()
    {
        ActiveSR.sprite = PressedActive;
        InactiveSR.sprite = PressedInactive;
        if (active == true)
        {
            active = false;
            GameManager.instance.OnRoomButtonPress(this);
        }
        //Debug.Log($"Enabled is {gameObject.active}!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collect();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ActiveSR.sprite = UnpressedActive;
        InactiveSR.sprite = UnpressedInactive;
    }

    public void PlayerEntered()
    {
        //Collect();
    }

    void ResetButton()
    {
        enabled = true;
        ActiveSR.sprite = UnpressedActive;
        InactiveSR.sprite = UnpressedInactive;
        ActiveSR.color = new Color(1f, 1f, 1f, 1f);
        ActiveSR.sprite = UnpressedActive;
        TimerText.text = "00:00.000";
        recordedTime = 0f;
    }

    public void Activate()
    {
        active = true;
        ResetButton();
    }

    public void ResetObject()
    {
        active = false;
        ResetButton();
    }
}
