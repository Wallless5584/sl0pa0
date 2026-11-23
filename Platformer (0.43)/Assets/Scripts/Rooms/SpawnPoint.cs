using UnityEngine;

public class SpawnPoint : GameWorldPart
{
    [SerializeField] float openTime;
    [SerializeField] float closeTime;
    float gateTimer;
    float slimeTimer;

    [SerializeField] SpriteRenderer GateSR;
    [SerializeField] Sprite[] GateSprites;
    [SerializeField] SpriteRenderer BackplateSR;
    [SerializeField] Sprite[] BackplateSprites;

    [SerializeField] SpriteRenderer LightSR;
    [SerializeField] Sprite[] LightSprites;

    [SerializeField] AudioSource SpawnAS;
    [SerializeField] AudioClip spawnOpen;
    [SerializeField] AudioClip spawnClose;

    byte currentGateSpriteIndex = 0;
    byte currentBackplateSpriteIndex = 0;
    bool shouldOpenGate = false;
    bool shouldAnimateSlimeSpawn = false;


    void Start()
    {
        Register();
        enabled = shouldOpenGate;
    }

    void Update()
    {
        if (shouldOpenGate)
        {
            if (shouldAnimateSlimeSpawn)
            {
                AnimateSlimeExitingTheSpawn();
            }
            else
            {
                AnimateGateOpening();
            }
        }
        else
        {
            AnimateGateClosing();
        }






        void AnimateSlimeExitingTheSpawn()
        {
            slimeTimer += Time.deltaTime;
            float current_slime_animation_frame_time = (openTime / (GateSprites.Length + BackplateSprites.Length)) * currentBackplateSpriteIndex;
            if (slimeTimer > current_slime_animation_frame_time)
            {
                currentBackplateSpriteIndex++;
                if (currentBackplateSpriteIndex < GateSprites.Length)
                {
                    BackplateSR.sprite = BackplateSprites[currentBackplateSpriteIndex];
                }
                else
                {
                    // finished slime exit.
                    // start closing gate
                    shouldOpenGate = false;
                    shouldAnimateSlimeSpawn = false;

                    slimeTimer = 0f;

                    currentBackplateSpriteIndex = 0;
                    BackplateSR.sprite = BackplateSprites[currentBackplateSpriteIndex];
                    GameManager.instance.Respawn();
                    SpawnAS.PlayOneShot(spawnClose);
                }
            }
        }

        void AnimateGateOpening()
        {
            gateTimer += Time.deltaTime;
            float current_gate_animation_frame_time = (openTime / (GateSprites.Length + BackplateSprites.Length)) * currentGateSpriteIndex;
            if (gateTimer > current_gate_animation_frame_time)
            {
                currentGateSpriteIndex++;
                if (currentGateSpriteIndex < GateSprites.Length)
                {
                    GateSR.sprite = GateSprites[currentGateSpriteIndex];
                }
                else
                {
                    // finished gate opening
                    // start slime exit.
                    shouldAnimateSlimeSpawn = true;
                }
            }
        }

        void AnimateGateClosing()
        {
            gateTimer -= Time.deltaTime;
            if (gateTimer < (closeTime / GateSprites.Length) * currentGateSpriteIndex)
            {
                currentGateSpriteIndex--;
                GateSR.sprite = GateSprites[currentGateSpriteIndex];
                if (currentGateSpriteIndex == 0)
                {
                    // finished animating gate closing.
                    // disable this
                    gateTimer = 0f;

                    enabled = false;
                }
            }
        }
    }


    public void PlayerEntered()
    {
        if (GameManager.instance.SpawnTriggered(this))
        {
            //LightSR.enabled = true;
            //LightSR.sprite = LightSprites[1];
        }
    }
    public void Spawn()
    {
        enabled = true;
        shouldOpenGate = true;
        SpawnAS.PlayOneShot(spawnOpen);
    }

    //public void Activate()
    //{
    //    Debug.Log($"Spawn in {transform.parent.name} activated!");
    //    LightSR.enabled = true;
    //    LightSR.sprite = LightSprites[0];
    //}

    //public void Reset()
    //{
    //    LightSR.enabled = false;
    //    LightSR.sprite = LightSprites[0];
    //}




    override public void Prepare()
    {
        Debug.Log($"Spawn in {transform.parent.name} is prepared!");
        LightSR.enabled = true;
        LightSR.sprite = LightSprites[0];
    }
    override public void Activate()
    {
        LightSR.enabled = true;
        LightSR.sprite = LightSprites[1];
    }

    override public void Reset()
    {
        LightSR.enabled = false;
        LightSR.sprite = LightSprites[0];
    }

    override public void Deactivate()
    {
        Reset();
    }
}
