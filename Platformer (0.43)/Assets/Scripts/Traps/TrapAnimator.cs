using TMPro;
using UnityEngine;
using UnityEngine.Events;

public enum TrapState
{
    None,
    Activating,
    Active,
    Deactivating,
    Off
}

public class TrapAnimator : MonoBehaviour
{

    const int c_activatingImageAmount = 3;
    const int c_activeImageAmount = 2;
    const int c_deactivatingImageAmount = 1;
    const int c_lastSpriteIndexOffset = 1;

    [SerializeField] TrapState state;
    public TrapState State { get=>state; }
    [SerializeField] float activationTime;
    [SerializeField] float deactivationTime;
    [SerializeField] float imageChangePeriod;
    [SerializeField] float imageFlickerPeriod;
    [SerializeField][Range(0,100)] float imageFlickerChance;

    [SerializeField] bool mirrorX;
    [SerializeField] bool mirrorY;
    [SerializeField] float mirrorPeriod;
    [SerializeField] bool mirrorActivation;
    [SerializeField] bool mirrorDectivation;
    [SerializeField] bool mirrorBloomEffect;
    [SerializeField] float mirrorBloomPeriod;

    [Header("References:")]
    [SerializeField] Sprite[] sprites;
    [SerializeField] SpriteRenderer bloomEffectSR;
    [SerializeField] SpriteRenderer energySR;

    [Header("Events:")]
    public UnityEvent OnActivating;
    public UnityEvent OnActive;
    public UnityEvent OnDeactivation;
    public UnityEvent OnTurnOff;

    int currentImageIndex = 0;
    float imageChangeTimer = 0f;
    float mirrorTimer = 0f;
    float mirrorBloomTimer = 0f;

    void Start()
    {
    }

    void Update()
    {
        imageChangeTimer -= Time.deltaTime;
        switch (state)
        {
            case TrapState.Activating:

                if(mirrorActivation)
                    MirrorEnergy();

                if (imageChangeTimer < 0)
                {
                    currentImageIndex += 1;
                    imageChangeTimer = activationTime / c_activatingImageAmount;

                    if (currentImageIndex >= c_activatingImageAmount)
                    {
                        SwitchState(TrapState.Active);
                    }
                    else
                    {
                        energySR.sprite = sprites[currentImageIndex]; 
                    }
                }
                bloomEffectSR.color = new Color(1f, 1f, 1f, (activationTime / c_activatingImageAmount * currentImageIndex + imageChangeTimer) / activationTime);
                break;

            case TrapState.Active:

                MirrorEnergy();

                if (imageChangeTimer < 0f)
                {
                    if (Random.Range(1, 101) <= (int)imageFlickerChance)
                    {
                        imageChangeTimer = imageFlickerPeriod;
                        energySR.sprite = null;
                    }
                    else
                    {
                        currentImageIndex += 1;
                        if (currentImageIndex >= c_activatingImageAmount + c_activeImageAmount)
                        {
                            currentImageIndex -= c_activeImageAmount;
                            imageChangeTimer = imageChangePeriod;
                        }
                        energySR.sprite = sprites[currentImageIndex];
                    }
                }
                break;

            case TrapState.Deactivating:

                if (mirrorDectivation)
                    MirrorEnergy();

                if (imageChangeTimer < 0)
                {
                    currentImageIndex += 1;
                    imageChangeTimer = deactivationTime / (c_deactivatingImageAmount + c_lastSpriteIndexOffset);
                    if (currentImageIndex >= sprites.Length)
                    {
                        SwitchState(TrapState.Off);
                    }
                    else
                        energySR.sprite = sprites[currentImageIndex];
                }
                bloomEffectSR.color = new Color(1f, 1f, 1f, deactivationTime/(deactivationTime / c_deactivatingImageAmount * currentImageIndex + imageChangeTimer));
                break;
        }

        if (mirrorBloomEffect)
        {
            MirrorBloom();
        }
    }

    private void MirrorBloom()
    {
        mirrorBloomTimer -= Time.deltaTime;
        if (mirrorBloomTimer < 0f)
        {
            if (Random.Range(0, 2) == 0)
            {
                if (mirrorX)
                {
                    bloomEffectSR.flipX = !bloomEffectSR.flipX;
                }
                else
                {
                    if (mirrorY)
                    {
                        bloomEffectSR.flipY = !bloomEffectSR.flipY;
                    }
                }
            }
            else
            {
                if (mirrorY)
                {
                    bloomEffectSR.flipY = !bloomEffectSR.flipY;
                }
                else
                {
                    if (mirrorX)
                    {
                        bloomEffectSR.flipX = !bloomEffectSR.flipX;
                    }
                }
            }
            mirrorBloomTimer = mirrorBloomPeriod;
        }
    }

    private void MirrorEnergy()
    {
        mirrorTimer -= Time.deltaTime;
        if (mirrorTimer < 0f)
        {
            if (Random.Range(0, 2) == 0)
            {
                if (mirrorX)
                {
                    energySR.flipX = !energySR.flipX;
                }
                else
                {
                    if (mirrorY)
                    {
                        energySR.flipY = !energySR.flipY;
                    }
                }
            }
            else
            {
                if (mirrorY)
                {
                    energySR.flipY = !energySR.flipY;
                }
                else
                {
                    if (mirrorX)
                    {
                        energySR.flipX = !energySR.flipX;
                    }
                }
            }
            mirrorTimer = mirrorPeriod;
        }
    }

    public void SwitchState(TrapState newState)
    {
        switch (newState)
        {
            case TrapState.Activating:
                state = TrapState.Activating;
                energySR.enabled = true;
                bloomEffectSR.enabled = true;
                currentImageIndex = -1;
                imageChangeTimer = 0f;

                OnActivating.Invoke();
                break;

            case TrapState.Active:
                state = TrapState.Active;
                energySR.enabled = true;
                bloomEffectSR.enabled = true;
                bloomEffectSR.color = Color.white;
                currentImageIndex = c_activatingImageAmount -1;
                imageChangeTimer = 0f;

                OnActive.Invoke();
                break;

            case TrapState.Deactivating:
                state = TrapState.Deactivating;
                energySR.enabled = true;
                bloomEffectSR.enabled = true;
                currentImageIndex = c_activatingImageAmount + c_activeImageAmount -1;
                imageChangeTimer = deactivationTime / (c_deactivatingImageAmount + c_lastSpriteIndexOffset);
                
                OnDeactivation.Invoke();
                break;

            case TrapState.Off:
                state = TrapState.Off;
                energySR.enabled = false;
                bloomEffectSR.enabled = false;
                currentImageIndex = 0;
                imageChangeTimer = 0f;
                
                OnTurnOff.Invoke();
                break;
        }
    }
}
