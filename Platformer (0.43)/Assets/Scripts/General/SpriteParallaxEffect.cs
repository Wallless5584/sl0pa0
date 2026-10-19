using UnityEngine;

public class SpriteParallaxEffect : GameWorldPart
{
    [SerializeField] Vector3 RefPoint;
    [SerializeField] float ParallaxSpeed;
    [SerializeField] Vector3 HintOffset;

    void Start()
    {
        HintOffset = gameObject.transform.localPosition;
        //transform.parent.parent.GetComponent<RoomController>().AddHint(this);
        Register();
        Disable();
    }

    void Update()
    {
        gameObject.transform.position = RefPoint + HintOffset + (new Vector3(
            Camera.main.transform.position.x,
            Camera.main.transform.position.y,
            0) - RefPoint) * ParallaxSpeed;
    }

    void Setup()
    {
        gameObject.SetActive(true);
        RefPoint = gameObject.transform.parent.position;
    }

    void Disable()
    {
        gameObject.SetActive(false);
        gameObject.transform.position = RefPoint + HintOffset;
    }


    override public void Prepare()
    {
        
    }

    override public void Activate()
    {
        Setup();
    }

    override public void Deactivate()
    {
        Disable();
    }

    override public void Reset()
    {
        Disable();
    }
}
