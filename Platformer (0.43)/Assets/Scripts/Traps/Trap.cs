using UnityEngine;

[SelectionBase]
public class Trap : MonoBehaviour
{
    const float c_pairRayMaxDistance = 30f;
    const float c_minimal_distance = 2;
    const float c_colliderOffset = 0.5f;


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
    [SerializeField] TrapAnimator[] animators;
    [SerializeField] LineRenderer lineEffect;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] BoxCollider2D boxCollider;

    [Header("Sounds")]
    AudioSource soundSource;
    [SerializeField] bool playSound;
    [SerializeField] AudioClip soundActivating;
    [SerializeField] AudioClip soundActive;
    [SerializeField] AudioClip soundDeactivating;

    void Start()
    {
        soundSource = GetComponent<AudioSource>();
        if (maxActiveTime == 0 || maxOffTime == 0)
        {
            useTimer = false;
            Debug.LogWarning($"{gameObject.name} - Timer not set. Switching timer off.");
        }
        timer = timeOffset;
        UpdateTrap(trapState);
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

    void UpdateTrap(TrapState new_state)
    {
        UpdateAnimatorState(new_state);
        if (animators.Length > 1)
            SetPairBasePosition();
    }

    public void UpdateAnimatorState(TrapState new_state)
    {
        foreach (var animator in animators)
        {
            animator.SwitchState(new_state);
        }
        //trapState = new_state;
    }

    public void SetStateFromAnimator()
    {
        trapState = animators[0].State;
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

    private void SetPairBasePosition()
    {
        Vector2 rayStartPos = animators[0].transform.position;

        // Raycast
        Vector2 rayHitPos = Vector2.zero;

        if (gameObject.scene.IsValid())
        {
            PhysicsScene2D _scene = gameObject.scene.GetPhysicsScene2D();
            Physics2D.queriesHitTriggers = false;
            RaycastHit2D rayHit = _scene.Raycast(
                origin: rayStartPos,
                direction: animators[0].transform.right,
                distance: c_pairRayMaxDistance,
                layerMask: groundLayer);
            //Physics2D.queriesHitTriggers = true; //commented out because whats the point?

            rayHitPos = rayHit.point;
        }

        if (rayHitPos == Vector2.zero)
        {
            Debug.Log($"Position is zero: {rayHitPos == Vector2.zero}");
            rayHitPos = rayStartPos + (Vector2)animators[0].transform.right * c_minimal_distance;
        }


        // calculations for pair pos
        Vector2 baseVsTrapOffset = (Vector2)transform.position - rayStartPos;
        Vector2 pairNewPos = new Vector2(
            x: rayHitPos.x + baseVsTrapOffset.x,
            y: rayHitPos.y + baseVsTrapOffset.y);
        animators[animators.Length-1].transform.position = pairNewPos;
        
        Debug.Log($"SetPairBasePosition || rayHitPos {rayHitPos}, baseVsTrapOffset {baseVsTrapOffset}, pairNewPos {pairNewPos}");
        
        // Line Effect
        if (lineEffect)
        {   // Currently only for Lightning. Other throw an error
            Vector2 localEndPosition = pairNewPos - (Vector2)transform.position;
            lineEffect.GetComponent<Lightning>().SetEndPosition(localEndPosition);         
        }
        else
            Debug.LogWarning($"{gameObject.name} : SetPairBasePosition || LineEffect not set!");


        boxCollider.offset = new Vector2(((rayHitPos - (Vector2)transform.position) / 2).magnitude, 0);      //half-size offset = center
        //boxCollider.offset = (rayHitPos - (Vector2)animators[0].transform.position) / 2 + new Vector2(c_colliderOffset, 0);      //half-size offset = center
        boxCollider.size = new Vector2((rayHitPos - (Vector2)transform.position).magnitude, boxCollider.size.y);
        //boxCollider.size = new Vector2(rayHitPos.x - animators[0].transform.position.x + 1, boxCollider.size.y);
    }

    private void UpdateSound()
    {

    }
}
