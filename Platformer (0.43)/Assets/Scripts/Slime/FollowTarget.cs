using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector2 objectSpeed;
    Vector2 old_object_pos;
    [SerializeField, Delayed] float followTime;
    [SerializeField] float extrapolateMod;
    float currentExtrapolateMod = 1f;
    bool shouldStopExtrapolation = false;

    private void Awake()
    {
        old_object_pos = Vector2.one;
    }

    private void Update()
    {
        Vector2 final_target_pos = old_object_pos + ((Vector2)target.position - old_object_pos) * currentExtrapolateMod;
        transform.position = Vector2.SmoothDamp(transform.position, final_target_pos, ref objectSpeed, followTime);
        old_object_pos = transform.position;
        if (shouldStopExtrapolation)
        {
            if (Vector2.Distance(final_target_pos, transform.position) < 0.5f)
            {
                enabled = false;
            }
        }
        //transform.position = Vector2.Lerp(transform.position, target.position, tMod);
    }

    public void SetTarget(Transform new_target)
    {
        target = new_target;
    }

    public void StopExtrapolation()
    {
        shouldStopExtrapolation = true;
        currentExtrapolateMod = 1f;
    }

    public void StartExtrapolation()
    {
        shouldStopExtrapolation = false;
        enabled = true;
        currentExtrapolateMod = extrapolateMod;
    }
}
