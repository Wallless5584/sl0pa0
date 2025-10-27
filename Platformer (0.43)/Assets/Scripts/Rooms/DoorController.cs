using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class DoorController : MonoBehaviour
{
    [SerializeField] Transform OpenTarget;
    [SerializeField] Transform CloseTarget;
    [SerializeField] GameObject Door;
    [SerializeField] float doorSpeed;
    bool opened;
    bool moving;
    Vector2 TargetLocation;

    void Start()
    {
        SetState(false);
        //OpenTarget = GetComponentInChildren<Transform>();
    }

    public void SetState(bool open)
    {
        if (open)
        {
            TargetLocation = OpenTarget.position;
            Debug.Log($"{gameObject.name} opened!");
        }
        else
        {
            TargetLocation = CloseTarget.position;
        }
        opened = open;
        moving = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    void Update()
    {
        if (moving)
        {
            Door.transform.position = Vector2.Lerp(Door.transform.position, TargetLocation, doorSpeed * Time.deltaTime);
            if (Vector2.Distance(Door.transform.position, TargetLocation) <= 0.1f)
            {
                Door.transform.position = TargetLocation;
                moving = false;
            }
        }
    }
}
