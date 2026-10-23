using UnityEngine;
using UnityEngine.Events;

[SelectionBase]
public class RoomButtonRelay : GameWorldPart
{
    [SerializeField] float relayTime;
    [SerializeField] SpriteRenderer lightSR;
    [SerializeField] ParticleSystem relayPS;
    float relayTimer;
    //RoomButton relayedSignalOrigin;

    public UnityEvent OnRelay;


    void Start()
    {
        Register();
        Activate();
    }

    void Update()
    {
        relayTimer -= Time.deltaTime;
        if (relayTimer < 0)
        {
            Relay();
        }
    }


    public void RecieveSignal(RoomButton origin)
    {
        enabled = true;
        //relayedSignalOrigin = origin;
    }

    void Relay()
    {
        lightSR.enabled = true;
        relayPS.Play();
        OnRelay.Invoke();
        enabled = false;
    }


    override public void Prepare()
    {
        
    }
    override public void Activate()
    {
        enabled = false;
        lightSR.enabled = false;
        relayTimer = relayTime;
    }
    override public void Reset()
    {
        lightSR.enabled = false;
        relayTimer = relayTime;
        enabled = false;
    }
    override public void Deactivate()
    {
        
    }


}
