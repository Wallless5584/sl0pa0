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

    private void Awake()
    {
        old_object_pos = Vector2.one;
    }

    private void Update()
    {
        Vector2 final_target_pos = old_object_pos + ((Vector2)target.position - old_object_pos) * extrapolateMod;
        transform.position = Vector2.SmoothDamp(transform.position, final_target_pos, ref objectSpeed, followTime);
        old_object_pos = transform.position;
        //transform.position = Vector2.Lerp(transform.position, target.position, tMod);
    }

    public void SetTarget(Transform new_target)
    {
        target = new_target;
    }
}
