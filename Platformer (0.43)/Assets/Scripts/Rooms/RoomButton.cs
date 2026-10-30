using UnityEngine;
using UnityEngine.Events;

[SelectionBase]
public class RoomButton : GameWorldPart
{
    public CollectableType Type { get; set; }
    [SerializeField] RoomTimer connectedTimer;
    [SerializeField] SpriteRenderer ActiveSR;
    [SerializeField] SpriteRenderer InactiveSR;

    [SerializeField] Sprite UnpressedActive;
    [SerializeField] Sprite PressedActive;
    [SerializeField] Sprite UnpressedInactive;
    [SerializeField] Sprite PressedInactive;

    [SerializeField] AudioSource buttonAS;
    [SerializeField] AudioClip buttonPressedSound;
    [SerializeField] AudioClip buttonUnpressedSound;

    bool active = false;
    float recordedTime;

    public UnityEvent OnPressed;



    private void Start()
    {
        Register().RegisterButton(this);
        ResetButton();
    }

    private void Update()
    {
        if (active)
        {
            recordedTime += Time.deltaTime;
            //int minTimer = (int)(recordedTime / 60);
            //float secTimer = recordedTime + Time.deltaTime - minTimer * 60;

            //float secDisplayTime = Mathf.Floor(secTimer * 1000f) / 1000;
            //TimerText.text = $"{minTimer:D2}:{secDisplayTime.ToString("00.000")}";
            connectedTimer.SetTime(recordedTime);
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

        buttonAS.PlayOneShot(buttonPressedSound);

        if (active == true)
        {
            active = false;
            //GameManager.instance.OnRoomButtonPress(this);

            if (OnPressed.GetPersistentEventCount() > 0)
            {
                OnPressed.Invoke();
            }
            else
            {
                GameManager.instance.OnRoomButtonPress(this);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collect();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ActiveSR.sprite = UnpressedActive;
        InactiveSR.sprite = UnpressedInactive;
        buttonAS.PlayOneShot(buttonUnpressedSound);
    }

    void ResetButton()
    {
        enabled = true;

        ActiveSR.sprite = UnpressedActive;
        InactiveSR.sprite = UnpressedInactive;
        ActiveSR.color = new Color(1f, 1f, 1f, 1f);
        ActiveSR.sprite = UnpressedActive;

        recordedTime = 0f;
        connectedTimer.SetTime(recordedTime);
    }

    //public void Activate()
    //{
    //    active = true;
    //    ResetButton();
    //}

    //public void ResetObject()
    //{
    //    active = false;
    //    ResetButton();
    //}


    override public void Prepare()
    {
        
    }
    override public void Activate()
    {
        active = true;
        ResetButton();
    }
    override public void Reset()
    {
        active = false;
        ResetButton();
    }
    override public void Deactivate()
    {
        
    }
}
