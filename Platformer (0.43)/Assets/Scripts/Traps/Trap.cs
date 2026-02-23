using Unity.VisualScripting;
using UnityEngine;

[SelectionBase]
public class Trap : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] TrapState startState; //for OnValidate
    TrapState trapState = TrapState.Off;

    [Header("Timing")]
    [SerializeField] bool useTimer;
    [SerializeField] float maxActiveTime;
    [SerializeField] float maxOffTime;
    [SerializeField] float timeOffset;
    float timer;

    [Header("References")]
    public TrapAnimator[] animators;
    public LineRenderer lineEffect;
    public LayerMask groundLayer;
    public BoxCollider2D boxCollider;

    [Header("Sounds")]
    [SerializeField] AudioSource soundSource;
    [SerializeField] bool playSound;
    [SerializeField] AudioClip soundActivating;
    [SerializeField] AudioClip soundActive;
    [SerializeField] AudioClip soundDeactivating;

    void Start()
    {
        Setup();
        if (playSound == false)
        {
            Destroy(soundSource);
        }
    }

    void Update()
    {
        if (useTimer)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                UpdateTiming();
            }
        }
    }

    private void OnValidate()
    {
        UpdateTrap(startState);
    }

    public void Setup()
    {
        if (useTimer)
        {
            if (maxActiveTime == 0 || maxOffTime == 0)
            {
                useTimer = false;
                Debug.LogWarning($"{gameObject.name} - Timer not set. Switching timer off.");
            }
            timer = timeOffset;
        }
        else
        {
            trapState = startState;
        }

        UpdateTrap(trapState);
    }

    void UpdateTrap(TrapState new_state)
    {
        UpdateAnimatorState(new_state);
    }

    public void UpdateAnimatorState(TrapState new_state)
    {
        foreach (var animator in animators)
        {
            animator.SwitchState(new_state);
        }
    }

    public void SetStateFromAnimator()
    {
        trapState = animators[0].State;

        UpdateSound();
    }

    void UpdateTiming()
    {
        switch (trapState)
        {
            case TrapState.Off:
                timer = maxActiveTime;
                trapState = TrapState.Activating;
                break;
            case TrapState.Active:
                timer = maxOffTime;
                trapState = TrapState.Deactivating;
                break;
        }
        UpdateAnimatorState(trapState);
    }
    private void UpdateSound()
    {
        if (soundSource)
        {
            if (playSound)
            {
                soundSource.enabled = true;
                switch (trapState)
                {
                    case TrapState.None:
                        break;
                    case TrapState.Off:
                        soundSource.clip = null;
                        soundSource.loop = false;
                        soundSource.Stop();
                        break;
                    case TrapState.Activating:
                        soundSource.clip = soundActivating;
                        soundSource.loop = false;
                        soundSource.Play();
                        break;
                    case TrapState.Active:
                        soundSource.clip = soundActive;
                        soundSource.loop = true;
                        soundSource.Play();
                        break;
                    case TrapState.Deactivating:
                        soundSource.clip = soundDeactivating;
                        soundSource.loop = false;
                        soundSource.Play();
                        break;
                }
            }
            else
            {
                soundSource.enabled = false;
            }
        }
    }
}
