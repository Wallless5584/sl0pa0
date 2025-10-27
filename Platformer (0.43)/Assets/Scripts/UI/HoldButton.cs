using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
[RequireComponent (typeof (Image))]

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    Image ButtonImg;
    Image TopImg;
    [SerializeField] Sprite defaultSprite, holdSprite;
    [SerializeField] Color HoverColor;
    [SerializeField] Color DefaultColor;
    [SerializeField] KeyCode PressKey;
    [SerializeField] float current_hold_time;
    [SerializeField] float holdThreshold;
    [SerializeField] float holdMinTime;
    float holdStartTime;
    public bool buttonPressed;
    public UnityEvent onButtonClick;
    public UnityEvent onButtonHold;

    private void Start()
    {
        Image[] img = GetComponentsInChildren<Image>();
        ButtonImg = img[0];
        if (img.Length > 1)
        {
            TopImg = img[1];
        }
        DefaultColor = ButtonImg.color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
        holdStartTime = Time.realtimeSinceStartup;
    }

    private void Update()
    {
        if (Input.GetKeyDown(PressKey))
        {
            buttonPressed = true;
            holdStartTime = Time.realtimeSinceStartup;
        }
        else if (Input.GetKeyUp(PressKey))
        {
            HoldCheck();
        }
        if (buttonPressed)
        {
            current_hold_time += (Time.realtimeSinceStartup - holdStartTime) - current_hold_time;
            if (current_hold_time > holdThreshold && TopImg)
            {
                TopImg.sprite = holdSprite;
                TopImg.fillAmount = Mathf.Clamp((current_hold_time-holdThreshold)/holdMinTime, 0f, 1f);
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        HoldCheck();
    }

    public void HoldCheck()
    {
        buttonPressed = false;
        if ((Time.realtimeSinceStartup - holdStartTime) >= holdMinTime)
        {
            //GameManager.instance.ResetLevel();
            Debug.Log("Hold activated!");
            onButtonHold.Invoke();
        }
        else
        {
            //GameManager.instance.Respawn();
            Debug.Log("Click activated!");
            onButtonClick.Invoke();
        }
        current_hold_time = 0;
        if (TopImg)
        {
            TopImg.sprite = defaultSprite;
            TopImg.fillAmount = 1;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ButtonImg.color = HoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ButtonImg.color = DefaultColor;
    }
}