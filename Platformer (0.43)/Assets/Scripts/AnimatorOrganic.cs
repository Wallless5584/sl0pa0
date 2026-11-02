using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatorOrganic : MonoBehaviour
{
    [Header("Time:")]
    [SerializeField,Range(0f, 10f)] float switchTimeMin;
    [SerializeField, Range(0f, 20f)] float switchTimeMax;
    [SerializeField] bool autoCalculateMedian;
    [SerializeField, Delayed, Range(0f, 20f)] float switchTimeMedian;
    [Header("Chance:")]
    [SerializeField, Range(0f, 100f)] float chanceMedian;
    [SerializeField, Range(0f, 1f)] float minToMaxCoefficient;
    [SerializeField, Range(0f, 100f)] float chanceBackwards;
    [SerializeField, Range(0f, 100f)] float chanceStationary;
    [Header("Images:")]
    [SerializeField] Sprite[] sprites;
    SpriteRenderer sr;

    float switchTimer;
    byte currentImageIndex;



    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        CheckMedianValues();
    }

    void Update()
    {
        switchTimer -= Time.deltaTime;

        if(switchTimer < 0)
        {
            if ((float)Random.Range(0, 101) > chanceStationary)
            {
                // Not stationary
                // Set Sprite
                if ((float)Random.Range(0, 101) < chanceBackwards)
                {
                    currentImageIndex--;
                }
                else
                {
                    currentImageIndex++;
                }

                if (currentImageIndex < sprites.Length)
                {

                }
                else
                {
                    currentImageIndex = 0;
                }
                sr.sprite = sprites[currentImageIndex];


                // Reset timer
                float time_roll = ((float)Random.Range(0, 10001)) / 100f; //get a float in 99.99 format
                float median_low_time = CalculateMedian(switchTimeMedian, switchTimeMin);
                float median_high_time = CalculateMedian(switchTimeMedian, switchTimeMin);
                if (time_roll < chanceMedian)
                {
                    // setting a random median time
                    switchTimer = Random.Range(median_low_time, median_high_time);
                }
                else
                {
                    if(time_roll < (100f - chanceMedian) * minToMaxCoefficient)
                    {
                        // rolled min time
                        switchTimer = Random.Range(switchTimeMin, median_low_time);
                    }
                    else
                    {
                        // rolled max time
                        switchTimer = Random.Range(median_high_time, switchTimeMax);
                    }
                    
                }
            }
            else
            {
                // Stationary, set max time;
                switchTimer = switchTimeMax;
            }
        }
    }

    private void OnValidate()
    {
        if(autoCalculateMedian)
            switchTimeMedian = CalculateMedian(switchTimeMin, switchTimeMax);
        else
            CheckMedianValues();
    }

    private void CheckMedianValues()
    {
        if (switchTimeMedian == 0f)
        {
            Debug.LogWarning("switchTimeMedian was 0. Automatically recalculating");
            switchTimeMedian = CalculateMedian(switchTimeMin, switchTimeMax);
        }
        else if (switchTimeMedian <= switchTimeMin)
        {
            Debug.LogWarning("switchTimeMedian was less than switchTimeMin. Automatically recalculating");
            switchTimeMedian = CalculateMedian(switchTimeMin, switchTimeMax);
        }
        else if (switchTimeMedian >= switchTimeMax)
        {
            Debug.LogWarning("switchTimeMedian was bigger than switchTimeMax. Automatically recalculating");
            switchTimeMedian = CalculateMedian(switchTimeMin, switchTimeMax);
        }
    }

    private float CalculateMedian(float minValue, float maxValue)
    {
        return (minValue + maxValue) / 2f;
    }
}
