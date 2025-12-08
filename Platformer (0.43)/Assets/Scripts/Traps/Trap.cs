using UnityEngine;

[SelectionBase]
public class Trap : MonoBehaviour
{
    const float pairRayMaxDistance = 20f;
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
    [SerializeField] BoxCollider2D boxCollider;

    void Start()
    {
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
        Physics2D.queriesHitTriggers = false;
        RaycastHit2D hit = Physics2D.Raycast(animators[0].transform.position, animators[0].transform.right, pairRayMaxDistance);
        Physics2D.queriesHitTriggers = true;
        Vector2 position = hit.point;
        Debug.LogWarning($"{gameObject.name} : SetPairBasePosition() || position: {position}");
        if (position == Vector2.zero)
        {
            Debug.Log($"Position is zero: {position == Vector2.zero}");
            position = (Vector2)animators[0].transform.position + (Vector2)animators[0].transform.right * c_minimal_distance;
        }

        float horizontalCoefficient = Vector3.Dot(transform.right, Vector3.right);
        float verticalCoefficient = Vector3.Dot(transform.right, Vector3.up);
        Vector2 offsetPos = new Vector2(
            x: position.x - (animators[0].transform.localPosition.x * horizontalCoefficient),
            y: position.y - (animators[0].transform.localPosition.y * verticalCoefficient)
            );
        Debug.Log($"Offset position: {offsetPos}");
        animators[animators.Length-1].transform.position = offsetPos;

        if (lineEffect)
            lineEffect.GetComponent<Lightning>().SetEndPosition(position-(Vector2)transform.position);         // Currently only for Lightning. Other throw an error
        else
            Debug.LogWarning($"{gameObject.name} : SetPairBasePosition() || LineEffect not set!");

        boxCollider.offset = (position - (Vector2)animators[0].transform.position) / 2 + new Vector2(c_colliderOffset, 0);
        boxCollider.size = new Vector2(position.x - animators[0].transform.position.x + 1, boxCollider.size.y);
    }
}
