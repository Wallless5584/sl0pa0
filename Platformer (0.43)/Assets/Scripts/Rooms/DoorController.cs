using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class DoorController : GameWorldPart
{
    [SerializeField] float doorSpeed;
    [SerializeField] Transform OpenTarget;
    [SerializeField] Transform CloseTarget;
    [SerializeField] GameObject Door;
    [SerializeField] AudioClip doorOpen;
    [SerializeField] AudioClip doorShut;
    AudioSource doorAS;

    [SerializeField] RoomTimer connectedTimer;
    float doorReachElapsedTime;
    bool opened;
    bool moving;
    bool shouldTime;
    Vector2 TargetLocation;

    void Start()
    {
        Register();
        doorAS = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (shouldTime)
        {
            UpdateTime();
        }

        if (moving)
        {
            Door.transform.position = Vector2.Lerp(Door.transform.position, TargetLocation, doorSpeed * Time.deltaTime);
            if (Vector2.Distance(Door.transform.position, TargetLocation) <= 0.01f)
            {
                Door.transform.position = TargetLocation;
                moving = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        shouldTime = false;
    }

    public void SetState(bool open)
    {
        if (open)
        {
            doorAS.PlayOneShot(doorOpen, 2f);
            TargetLocation = OpenTarget.position;
            Debug.Log($"{gameObject.name} opened!");
        }
        else
        {
            doorAS.PlayOneShot(doorShut, 1.5f);
            TargetLocation = CloseTarget.position;
        }
        opened = open;
        moving = true;
    }


    void UpdateTime()
    {
        doorReachElapsedTime += Time.deltaTime;
        connectedTimer.SetTime(doorReachElapsedTime);
    }


    override public void Prepare()
    {

    }
    override public void Activate()
    {
        enabled = true;
        shouldTime = true;
    }
    override public void Reset()
    {
        TargetLocation = CloseTarget.position;
        Door.transform.position = TargetLocation;
        moving = false;
        opened = false;
        enabled = false;
    }
    override public void Deactivate()
    {
        SetState(false);
    }


}