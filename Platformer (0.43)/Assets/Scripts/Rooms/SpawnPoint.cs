using UnityEngine;

public class SpawnPoint : GameWorldPart
{
    [SerializeField] float openTime;
    [SerializeField] float closeTime;
    float gateTimer;

    [SerializeField] SpriteRenderer GateSR;
    [SerializeField] Sprite[] GateSprites;

    [SerializeField] SpriteRenderer LightSR;
    [SerializeField] Sprite[] LightSprites;

    byte currentGateSpriteIndex = 0;
    bool shouldSpawn = false;


    void Start()
    {
        Register();
        enabled = shouldSpawn;
    }

    void Update()
    {

        if (shouldSpawn)
        {
            gateTimer += Time.deltaTime;
            if (gateTimer > (openTime/GateSprites.Length) * currentGateSpriteIndex)
            {
                currentGateSpriteIndex++;
                if(currentGateSpriteIndex < GateSprites.Length)
                {
                    GateSR.sprite = GateSprites[currentGateSpriteIndex];
                }
                else
                {
                    shouldSpawn = false;
                    //gateTimer = 0f;

                    GameManager.instance.Respawn();
                }
            }
        }
        else
        {
            gateTimer -= Time.deltaTime;
            if (gateTimer < (closeTime / GateSprites.Length) * currentGateSpriteIndex)
            {
                currentGateSpriteIndex--;
                GateSR.sprite = GateSprites[currentGateSpriteIndex];
                if(currentGateSpriteIndex == 0)
                {
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
        shouldSpawn = true;
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
        this.Reset();
    }
}
