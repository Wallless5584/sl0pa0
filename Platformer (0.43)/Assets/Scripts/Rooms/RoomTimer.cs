using UnityEngine;
using TMPro;

public class RoomTimer : MonoBehaviour
{
    [SerializeField] TMP_Text timerText;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetTime(float recordedTime)
    {
        int minTimer = (int)(recordedTime / 60);
        float secTimer = recordedTime - minTimer * 60;

        float secDisplayTime = Mathf.Floor(secTimer * 1000f) / 1000;
        timerText.text = $"{minTimer:D2}:{secDisplayTime.ToString("00.000")}";
    }
}
