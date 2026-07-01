using System;
using UnityEngine;

#region Enums
public enum MoveState
{
    None,
    Ground,
    Ceiling,
    Jump,
    JumpDown,
    JumpWallRight,
    JumpWallLeft,
    JumpAir,
    Air,
    Wall,
    Bounce,
    WaterGlide,
} 

[Flags] public enum Sides
{
    None = 0,
    Top = 1,
    Bottom = 1 << 1,
    Left = 1 << 2, 
    Right = 1 << 3
}

public enum Direction
{
    None,

    Ground,
    GroundLeft,
    GroundRight,

    JumpUp,
    JumpDown,
    JumpWallRight,
    JumpWallLeft,

    InAir,
    InAirLeft,
    InAirRight,

    WallLeft,
    WallLeftUp,
    WallLeftDown,
    WallRight,
    WallRightUp,
    WallRightDown,

    Ceiling,
    CeilingLeft,
    CeilingRight,
}

public enum Reaction
{
    None,
    Death,
    Push,
    GainSalt,
    LoseSalt,
    GetDry,
    GetNormal,
    GetWet,
}

public enum WetnessState
{
    None,
    Dry,
    DrySalty,
    Normal,
    NormalSalty,
    Wet,
}

public enum Element
{
    None,
    SaltPiece,
    Salt,
    Fire,
    Water,
    Electricity,
    Frost,
}
#endregion


[RequireComponent(typeof(Rigidbody2D))]
public class SlimeController : MonoBehaviour
{
    #region Fields
    [SerializeField] LayerMask GroundLayer;
    [SerializeField] LayerMask Water;
    [SerializeField] LayerMask Fire;
    [SerializeField] LayerMask ReactElements;
    [SerializeField] LayerMask Spaces;
    public Vector2 Move = Vector2.zero;
    const float c_MoveForce = 80;
    bool debugVelocity = false;
    public Collider2D SlimeCollider;
    public ParticleSystem DirChangeParticles;
    public ParticleSystem JumpDownBoostParticles;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public SpriteRenderer sr;
    [SerializeField] public Transform face;

    [Header("Dry state")]
    [SerializeField] float dryMaxSpeed;
    [SerializeField] float dryAcceleration;
    [SerializeField] float dryFrictionGround;
    [SerializeField] float dryFrictionCeiling;
    [SerializeField] float dryFrictionWall;
    [SerializeField] float dryFrictionWallUp;
    [SerializeField] float dryFrictionBounce;
    [SerializeField] float dryJumpPower;
    [SerializeField] float dryWallJumpPower;
    [SerializeField] Reaction[] drySaltPiece;
    [SerializeField] Reaction[] drySalt;
    [SerializeField] Reaction[] dryFire;
    [SerializeField] Reaction[] dryWater;
    [SerializeField] Reaction[] dryElectricity;
    [SerializeField] Reaction[] dryFrost;
    [SerializeField] SpaceState dryAirSpace;
    [SerializeField] SpaceState dryWaterSpace;
    [SerializeField] SpaceState dryColdSpace;
    [Header("Salty Dry state")]
    [SerializeField] Reaction[] drySaltySaltPiece;
    [SerializeField] Reaction[] drySaltySalt;
    [SerializeField] Reaction[] drySaltyFire;
    [SerializeField] Reaction[] drySaltyWater;
    [SerializeField] Reaction[] drySaltyElectricity;
    [SerializeField] Reaction[] drySaltyFrost;
    [Header("Normal state")]
    [SerializeField] float normalMaxSpeed;
    [SerializeField] float normalAcceleration;
    [SerializeField] float normalFrictionGround;
    [SerializeField] float normalFrictionCeiling;
    [SerializeField] float normalFrictionWall;
    [SerializeField] float normalFrictionWallUp;
    [SerializeField] float normalFrictionBounce;
    [SerializeField] float normalJumpPower;
    [SerializeField] float normalWallJumpPower;
    [SerializeField] Reaction[] normalSaltPiece;
    [SerializeField] Reaction[] normalSalt;
    [SerializeField] Reaction[] normalFire;
    [SerializeField] Reaction[] normalWater;
    [SerializeField] Reaction[] normalElectricity;
    [SerializeField] Reaction[] normalFrost;
    [SerializeField] SpaceState normalAirSpace;
    [SerializeField] SpaceState normalWaterSpace;
    [SerializeField] SpaceState normalColdSpace;
    [Header("Salty Normal state")]
    [SerializeField] Reaction[] normalSaltySaltPiece;
    [SerializeField] Reaction[] normalSaltySalt;
    [SerializeField] Reaction[] normalSaltyFire;
    [SerializeField] Reaction[] normalSaltyWater;
    [SerializeField] Reaction[] normalSaltyElectricity;
    [SerializeField] Reaction[] normalSaltyFrost;
    [Header("Wet state")]
    [SerializeField] float wetMaxSpeed;
    [SerializeField] float wetAcceleration;
    [SerializeField] float wetFrictionGround;
    [SerializeField] float wetFrictionCeiling;
    [SerializeField] float wetFrictionWall;
    [SerializeField] float wetFrictionWallUp;
    [SerializeField] float wetFrictionBounce;
    [SerializeField] float wetJumpPower;
    [SerializeField] float wetWallJumpPower;
    [SerializeField] Reaction[] wetSaltPiece;
    [SerializeField] Reaction[] wetSalt;
    [SerializeField] Reaction[] wetFire;
    [SerializeField] Reaction[] wetWater;
    [SerializeField] Reaction[] wetElectricity;
    [SerializeField] Reaction[] wetFrost;
    [SerializeField] SpaceState wetAirSpace;
    [SerializeField] SpaceState wetWaterSpace;
    [SerializeField] SpaceState wetColdSpace;
    [Header("Body Sprites")]
    public Sprite slime_inAir;
    public Sprite slime_ground;
    public Sprite slime_ground_right;
    public Sprite slime_ceiling;
    public Sprite slime_ceiling_right;
    public Sprite slime_wall;
    public Sprite slime_wall_slide;
    public Sprite slime_jump;
    public Sprite slime_wall_jump;
    public Sprite slime_jump_prepare;
    [Header("Face Sprites")]
    public Sprite slime_face_normal;
    public Sprite slime_face_up;
    public Sprite slime_face_down;
    public Sprite slime_face_sad;
    [Header("Particles")]
    public ParticleSystem WaterSplash;
    public ParticleSystem WaterBubbles;
    public ParticleSystem WaterDrops;
    public SideParticles HeatParticles;
    public SideParticles SplashParticles;

    Vector2 contact_point;
    public SlimeMoveState CurrentMoveState;

    SideData currentSides;
    public Sides CurrentSides { get => currentSides.Flags; }
    SideData waterSides;
    SideData heatSides;

    Direction CurrentDirection = Direction.None;

    float CurrentScale;
    const float c_GroundColOffset = 0.5f;
    const float c_GroundColThickness = 0.05f;
    const float c_GroundColWidth = 0.9f;

    const float c_WaterColOffset = 0.475f;
    const float c_WaterColThickness = 0.05f;
    const float c_WaterColWidth = 0.9f;
    const float c_UnderwaterSlideColThickness = 0.1f;
    const float c_UnderwaterSlideColWidth = 0.1f;
    const float c_SplashColWidth = 0.1f;
    //const float c_WaterColScaleFactor = 2f;

    const float c_HeatColOffset = 1.5f;
    const float c_HeatColThickness = 1f;
    const float c_HeatColWidth = 1f;
    

    public SlimeMoveState[] MoveStates =
    {
        new GroundMoveState(),
        new CeilingMoveState(),
        new JumpGroundMoveState(),
        new JumpDownMoveState(),
        new JumpWallRightMoveState(),
        new JumpWallLeftMoveState(),
        new JumpAirMoveState(),
        new AirMoveState(),
        new WallMoveState(),
        new BounceMoveState(),
        new WaterGlideMoveState(),
    };

    public WetnessState CurrentWetnessState { get; private set; }

    public SlimeState CurrentSlimeState;

    public SlimeState[] SlimeStates;
    #endregion


    #region Unity Messages
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        EnterMoveState(MoveState.Air);
        CreateStates();
        ChangeSlimeState(WetnessState.Normal);
        Physics2D.queriesHitTriggers = true;
    }

    void Update()
    {
        CurrentMoveState.UpdateState(this);
        face.localPosition = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * 0.1f;
    }

    void FixedUpdate()
    {
        if (debugVelocity)
        {
            Debug.Log($"New Move: {Move}, Final Velocity: {rb.velocity}");
            //debugVelocity = false;
        }
        Move = rb.velocity;
        CurrentMoveState.FixedUpdateState(this, Move);
        rb.AddForce(-(rb.velocity - Move) * c_MoveForce);

        CheckGroundSides();

        CheckHeatParticles();
        CheckWaterParticles();

        IdentifyElement(Physics2D.OverlapBox(transform.position, transform.localScale, angle: 0, ReactElements));
        //IdentifySpace();
        if (CheckEnteredNewSpace())
        {
            PlayParticlesBySpace();
        }

    }

    //private void OnDrawGizmosSelected()
    //{
    //    Collider2D TopWater = Physics2D.OverlapBox(
    //        point: new Vector2(transform.position.x, transform.position.y + c_GroundColOffset * CurrentScale),
    //        size: new Vector2(c_GroundColWidth, c_GroundColThickness * c_WaterColScaleFactor) * CurrentScale,
    //        angle: 0, layerMask: Water);

    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireCube(
    //        center: new Vector3(transform.position.x, transform.position.y + c_GroundColOffset * CurrentScale, 0f),
    //        size: new Vector3(c_GroundColWidth, c_GroundColThickness * c_WaterColScaleFactor, 0f) * CurrentScale);


    //    if (TopWater)
    //    {
    //        Collider2D TopNoWater = Physics2D.OverlapBox(
    //        point: new Vector2(transform.position.x, transform.position.y + (c_GroundColOffset + c_GroundColThickness * c_WaterColScaleFactor) * CurrentScale),
    //        size: new Vector2(c_GroundColWidth, c_GroundColThickness * c_WaterColScaleFactor) * CurrentScale,
    //        angle: 0, layerMask: Water);

    //        if (TopNoWater)
    //        {
    //            Gizmos.color = Color.red;
    //        }
    //        else
    //        {
    //            Gizmos.color = Color.green;
    //        }

    //        Gizmos.DrawWireCube(
    //            center: new Vector3(transform.position.x, transform.position.y + (c_GroundColOffset + c_GroundColThickness * c_WaterColScaleFactor) * CurrentScale, 0f),
    //            size: new Vector3(c_GroundColWidth, c_GroundColThickness * c_WaterColScaleFactor, 0f) * CurrentScale);
    //    }
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    IdentifyElement(collision);
    //    CheckSplashSides();
    //    if (CheckEnteredNewSpace())
    //    {
    //        PlayParticlesBySpace();
    //    }
    //}
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    IdentifySpace();
    //}
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    CurrentSlimeState.CurrentSpace = Space.Air;
    //    SlimeStates[0].CurrentSpace = Space.Air;
    //    SlimeStates[1].CurrentSpace = Space.Air;
    //    SlimeStates[2].CurrentSpace = Space.Air;
    //    SlimeStates[3].CurrentSpace = Space.Air;
    //    SlimeStates[4].CurrentSpace = Space.Air;
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        contact_point = collision.contacts[0].normal;
        CurrentMoveState.OnCollisionEnterState(this, contact_point);
        //IdentifyElement(collision.collider);
    }
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    //CurrentMoveState.OnCollisionExitState(this, contact_point);
    //}
    #endregion


    #region Move and Wetness States
    void CreateStates()
    {
        SlimeStates = new SlimeState[]
        {
            new SlimeState(dryMaxSpeed, dryAcceleration, dryFrictionGround, dryFrictionCeiling, dryFrictionWall, dryFrictionWallUp, dryFrictionBounce,
            dryJumpPower, dryWallJumpPower, drySaltPiece, drySalt, dryFire, dryWater, dryElectricity, dryFrost, dryAirSpace, dryWaterSpace, dryColdSpace),

            new SlimeState(dryMaxSpeed, dryAcceleration, dryFrictionGround, dryFrictionCeiling, dryFrictionWall, dryFrictionWallUp, dryFrictionBounce,
            dryJumpPower, dryWallJumpPower, drySaltySaltPiece, drySaltySalt, drySaltyFire, drySaltyWater, drySaltyElectricity, drySaltyFrost, dryAirSpace, dryWaterSpace, dryColdSpace),

            new SlimeState(normalMaxSpeed, normalAcceleration, normalFrictionGround, normalFrictionCeiling, normalFrictionWall, normalFrictionBounce,
            normalFrictionWallUp, normalJumpPower, normalWallJumpPower, normalSaltPiece, normalSalt, normalFire, normalWater, normalElectricity, normalFrost, normalAirSpace, normalWaterSpace, normalColdSpace),

             new SlimeState(normalMaxSpeed, normalAcceleration, normalFrictionGround, normalFrictionCeiling, normalFrictionWall, normalFrictionBounce,
            normalFrictionWallUp, normalJumpPower, normalWallJumpPower, normalSaltySaltPiece, normalSaltySalt, normalSaltyFire, normalSaltyWater, normalSaltyElectricity, normalSaltyFrost, normalAirSpace, normalWaterSpace, normalColdSpace),

            new SlimeState(wetMaxSpeed, wetAcceleration, wetFrictionGround, wetFrictionCeiling, wetFrictionWall, wetFrictionWallUp, wetFrictionBounce,
            wetJumpPower, wetWallJumpPower, wetSaltPiece, wetSalt, wetFire, wetWater, wetElectricity, wetFrost, wetAirSpace, wetWaterSpace, wetColdSpace),
        };
    }
    public void EnterMoveState(MoveState new_move_state)
    {
        SlimeMoveState new_state = MoveStates[(int)new_move_state - 1];
        new_state.EnterState(this);
        CurrentMoveState = new_state;
        //Debug.Log($"CURRENT SPACE STATE -> {CurrentSlimeState.CurrentSpace}");
        //Debug.Log($"CURRENT GRAVITYMOD -> {CurrentSlimeState.SpaceGravityMod}");
    }

    void ChangeSlimeState(WetnessState newSlimeState)
    {
        Debug.Log($"{gameObject.name} changed slime (wetness) state!");
        CurrentSlimeState = SlimeStates[(int)newSlimeState - 1];
        CurrentWetnessState = newSlimeState;
        switch (newSlimeState)
        {
            case WetnessState.DrySalty:
            case WetnessState.Dry:
                gameObject.transform.localScale = new Vector2(0.7f, 0.7f);
                break;
            case WetnessState.NormalSalty:
            case WetnessState.Normal:
                gameObject.transform.localScale = new Vector2(1f, 1f);
                break;
            case WetnessState.Wet:
                gameObject.transform.localScale = new Vector2(1.2f, 1.2f);
                break;
        }
        CurrentScale = gameObject.transform.localScale.x;
    }
    #endregion


    #region Space
    public Space IdentifySpace()//Collider2D collision
    {
        const byte c_min_colliders_for_water_space = 2;
        //Sides currentWaterSides = CheckSides(c_WaterColOffset, c_WaterColWidth, c_WaterColThickness, Water, out byte flagCount);
        if (waterSides.FlagCount >= c_min_colliders_for_water_space) //collision.tag == "Water"
        { 
            CurrentSlimeState.CurrentSpace = Space.Water;
            rb.drag = CurrentSlimeState.SpaceDrag;
            //if (waterSides.HasFlag(Side.Top) & !currentWaterSides.HasFlag(Side.Top))
            //Debug.Log("Current Space is WATER!");
            return Space.Water;
        }
        //else if (collision.tag == "ColdSpace")
        //{
        //    CurrentSlimeState.CurrentSpace = Space.Cold;
        //    rb.drag = CurrentSlimeState.SpaceDrag;
        //    return Space.Cold;
        //}
        CurrentSlimeState.CurrentSpace = Space.Air;
        rb.drag = CurrentSlimeState.SpaceDrag;
        return Space.Air;
    }

    bool CheckEnteredNewSpace()
    {
        Space old_Space = CurrentSlimeState.CurrentSpace;
        Space new_Space = IdentifySpace();

        if (new_Space == old_Space)
        {
            return false;
        }
        else
        {
            CurrentSlimeState.CurrentSpace = new_Space;
            SlimeStates[0].CurrentSpace = new_Space;
            SlimeStates[1].CurrentSpace = new_Space;
            SlimeStates[2].CurrentSpace = new_Space;
            SlimeStates[3].CurrentSpace = new_Space;
            SlimeStates[4].CurrentSpace = new_Space;
            return true;
        }
    }
    #endregion


    #region Elements and Reactions
    void IdentifyElement(Collider2D collision)
    {
        //if (collision.tag == "Coin")
        //{
        //GameManager.instance.OnRoomButtonPress(collision.gameObject.GetComponentInParent<RoomButton>());
        //collision.gameObject.GetComponentInParent<RoomButton>().Collect();
        //CoinCount++;
        //}
        if (collision == null)
        {
            return;
        }
        if (collision.tag == "SaltPiece")
        {
            ReactToElement(Element.SaltPiece, collision);
            //Debug.Log("Salt Piece IDENTIFIED!");
        }
        else if (collision.tag == "Salt")
        {
            ReactToElement(Element.Salt, collision);
            //Debug.Log("Salt IDENTIFIED!");
        }
        else if (collision.tag == "Fire")
        {
            ReactToElement(Element.Fire, collision);
            //Debug.Log("Fire IDENTIFIED!");
        }
        else if (collision.tag == "Water")
        {
            ReactToElement(Element.Water, collision);
            //Debug.Log("Water IDENTIFIED!");
        }
        else if (collision.tag == "Electricity")
        {
            ReactToElement(Element.Electricity, collision);
            //Debug.Log("Electricity IDENTIFIED!");
        }
        else if (collision.tag == "Frost")
        {
            ReactToElement(Element.Frost, collision);
            //Debug.Log("Frost IDENTIFIED!");
        }
    }

    void ReactToElement(Element element, Collider2D collision)
    {
        switch (element)
        {
            case Element.SaltPiece:
                React(ref CurrentSlimeState.SaltPiece);
                break;
            case Element.Salt:
                React(ref CurrentSlimeState.Salt);
                break;
            case Element.Fire:
                React(ref CurrentSlimeState.Fire);
                break;
            case Element.Water:
                React(ref CurrentSlimeState.Water);
                break;
            case Element.Electricity:
                React(ref CurrentSlimeState.Electricity);
                break;
            case Element.Frost:
                React(ref CurrentSlimeState.Frost);
                break;
        }
        void React(ref Reaction[] reactions)
        {
            foreach (Reaction reaction in reactions)
            {
                switch (reaction)
                {
                    case Reaction.Death:
                        Die();
                        break;
                    case Reaction.Push:
                        GetPushed(collision);
                        break;
                    case Reaction.GainSalt:
                        ChangeSalt(true);
                        break;
                    case Reaction.LoseSalt:
                        ChangeSalt(false);
                        break;
                    case Reaction.GetDry:
                        ChangeSlimeState(WetnessState.Dry);
                        break;
                    case Reaction.GetNormal:
                        ChangeSlimeState(WetnessState.Normal);
                        break;
                    case Reaction.GetWet:
                        ChangeSlimeState(WetnessState.Wet);
                        break;
                }
            }
        }
    }

    void GetPushed(Collider2D collision)
    {
        float PushPower = 10;
        Debug.Log($"{gameObject.name} got pushed!");
        Vector3 closestPoint = collision.ClosestPoint(transform.position);
        Vector2 collisionNormal = (transform.position - closestPoint).normalized;

        //collisionNormal.normalized* Vector3.Dot(Move.normalized, collisionNormal.normalized) * Move
        float magnitude = Move.magnitude * Mathf.Cos(Mathf.Deg2Rad * (180-Vector2.Angle(Move.normalized, collisionNormal)));
        Vector2 opposingForce = (collisionNormal * magnitude);
        Vector2 appliedForce = collisionNormal * PushPower;
        Vector2 totalAddedForce = opposingForce + appliedForce;

        //rb.AddForce(Vector3.Dot(Move.normalized, VectorFromClosestPoint.normalized) * Move + VectorFromClosestPoint.normalized * PushPower - new Vector2(0, Move.y) * 20);
        
        //Debug.Log($"Move dot product: {Vector3.Dot(Move.normalized, collisionNormal)}, Push vector: {collisionNormal * PushPower}");
        Debug.Log($"Move: {Move}, Initial Velocity: {rb.velocity}, Collision Normal: {collisionNormal}");
        Debug.Log($"Angle: {180 - Vector2.Angle(Move.normalized, collisionNormal)}, Cos: {Mathf.Cos(Mathf.Deg2Rad * (180 - Vector2.Angle(Move.normalized, collisionNormal)))}, Magnitude: {magnitude}");
        Debug.Log($"Move magnitude: {Move.magnitude}, Opposing force: {opposingForce}, Applied Force: {appliedForce}");
        //rb.AddForce(totalAddedForce);
        Move += totalAddedForce;
        rb.velocity = Move;
        //debugVelocity = true;
        Debug.Log($"New Move: {Move}, Added force: {totalAddedForce}");
    }

    void ChangeSalt(bool new_salt)
    {
        string txt = $"Current Wetness state: {CurrentWetnessState}, ";
        if (new_salt == false)
        {
            if (CurrentWetnessState == WetnessState.NormalSalty)
            {
                ChangeSlimeState(WetnessState.Normal);
            }
            else if (CurrentWetnessState == WetnessState.DrySalty)
            {
                ChangeSlimeState(WetnessState.Dry);
            }
            txt += $"{gameObject.name} is NOT salty";
        }
        else 
        {
            if (CurrentWetnessState == WetnessState.Normal)
            {
                ChangeSlimeState(WetnessState.NormalSalty);
            }
            else if (CurrentWetnessState == WetnessState.Dry) 
            {
                ChangeSlimeState(WetnessState.DrySalty);
            }
            txt += $"{gameObject.name} is salty";
        }
        Debug.Log(txt);
    }
    #endregion


    #region Physics Checks
    public void CheckGroundSides()
    {
        currentSides = CheckSides(c_GroundColOffset, c_GroundColWidth, c_GroundColThickness, GroundLayer);
        CurrentMoveState.ReactToGround(currentSides.Flags, this);
    }

    public bool CheckWaterSurface()
    {
        //Physics2D.queriesHitTriggers = true;

        Collider2D TopWater = Physics2D.OverlapBox(
            point: new Vector2(transform.position.x, transform.position.y + c_GroundColOffset * CurrentScale),
            size: new Vector2(c_UnderwaterSlideColWidth, c_UnderwaterSlideColThickness) * CurrentScale,
            angle: 0, layerMask: Water);

        if (TopWater)
        {
            Collider2D TopNoWater = Physics2D.OverlapBox(
            point: new Vector2(transform.position.x, transform.position.y + (c_GroundColOffset + c_UnderwaterSlideColThickness) * CurrentScale),
            size: new Vector2(c_UnderwaterSlideColWidth, c_UnderwaterSlideColThickness) * CurrentScale,
            angle: 0, layerMask: Water);

            //Physics2D.queriesHitTriggers = false;
            return !TopNoWater;
        }
        else
        {
            //Physics2D.queriesHitTriggers = false;
            return false;
        }
        
    }
    #endregion


    #region Gameplay
    public void ResetSlime()
    {
        rb.velocity = Vector2.zero;
        Move = Vector2.zero;
        //PlayParticlesBySpace();
        ChangeSlimeState(WetnessState.Normal);
    }
    void Die()
    {
        //Play death effect
        GameManager.instance.Respawn();
    }
    #endregion


    #region Slime Display
    public void ChangeDirection(Direction new_direction)
    {
        if (new_direction == CurrentDirection)
        {
            return;
        }
        else
        {
            switch (new_direction)
            {
                case Direction.None:
                    break;
                case Direction.Ground:
                    sr.sprite = slime_ground;
                    sr.flipY = false;
                    break;
                case Direction.GroundRight:
                    sr.sprite = slime_ground_right;
                    sr.flipX = false;
                    sr.flipY = false;
                    break;
                case Direction.GroundLeft:
                    sr.sprite = slime_ground_right;
                    sr.flipX = true;
                    sr.flipY = false;
                    break;
                case Direction.JumpUp:
                    sr.sprite = slime_jump;
                    sr.flipY = false;
                    break;
                case Direction.JumpDown:
                    sr.sprite = slime_jump;
                    sr.flipY = true;
                    break;
                case Direction.InAir:
                    sr.sprite = slime_inAir;
                    break;
                case Direction.InAirRight:
                    sr.sprite = slime_inAir;
                    sr.flipX = false;
                    break;
                case Direction.InAirLeft:
                    sr.sprite = slime_inAir;
                    sr.flipX = true;
                    break;
                case Direction.Ceiling:
                    sr.sprite = slime_ceiling;
                    sr.flipY = false;
                    break;
                case Direction.CeilingRight:
                    sr.sprite = slime_ceiling_right;
                    sr.flipY = false;
                    sr.flipX = false;
                    break;
                case Direction.CeilingLeft:
                    sr.sprite = slime_ceiling_right;
                    sr.flipY = false;
                    sr.flipX = true;
                    break;
                case Direction.WallRight:
                    sr.sprite = slime_wall;
                    sr.flipX = false;
                    sr.flipY = false;
                    break;
                case Direction.WallRightUp:
                    sr.sprite = slime_wall_slide;
                    sr.flipY = true;
                    sr.flipX = false;
                    break;
                case Direction.WallRightDown:
                    sr.sprite = slime_wall_slide;
                    sr.flipY = false;
                    sr.flipX = false;
                    break;
                case Direction.WallLeft:
                    sr.sprite = slime_wall;
                    sr.flipX = true;
                    sr.flipY = false;
                    break;
                case Direction.WallLeftUp:
                    sr.sprite = slime_wall_slide;
                    sr.flipY = true;
                    sr.flipX = true;
                    break;
                case Direction.WallLeftDown:
                    sr.sprite = slime_wall_slide;
                    sr.flipY = false;
                    sr.flipX = true;
                    break;
                case Direction.JumpWallRight:
                    sr.sprite = slime_wall_jump;
                    sr.flipX = false;
                    break;
                case Direction.JumpWallLeft:
                    sr.sprite = slime_wall_jump;
                    sr.flipX = true;
                    break;
            }
        }
    }
    #endregion


    #region Particle effects
    public void CheckHeatParticles()
    {
        CheckPlaySideParticles(c_HeatColOffset, c_HeatColWidth, c_HeatColThickness, Fire, HeatParticles, ref heatSides);
    }

    public void CheckWaterParticles()
    {
        const float c_particle_velocity_mod = 0.5f;

        SplashParticles.SetRadialVelocity(Move.magnitude * c_particle_velocity_mod);

        CheckPlaySideParticlesDoublesided(c_WaterColOffset, c_SplashColWidth, c_WaterColThickness, Water, SplashParticles, ref waterSides);
    }

    public void CheckPlaySideParticles(float colOffset, float colWidth, float colThickness, LayerMask layer,
        SideParticles particles, ref SideData currentSides)
    {
        SideData collisionSides = CheckSides(colOffset, colWidth, colThickness, layer);

        if (currentSides.Flags == collisionSides.Flags)
        {
            return;
        }
        else
        {
            if (collisionSides.HasFlag(Sides.Right) != currentSides.HasFlag(Sides.Right))
            {
                if (collisionSides.HasFlag(Sides.Right))
                {
                    particles.Right.Play();
                }
                else
                {
                    particles.Right.Stop();
                }
            }
            if (collisionSides.HasFlag(Sides.Left) != currentSides.HasFlag(Sides.Left))
            {
                if (collisionSides.HasFlag(Sides.Left))
                {
                    particles.Left.Play();
                }
                else
                {
                    particles.Left.Stop();
                }
            }
            if (collisionSides.HasFlag(Sides.Top) != currentSides.HasFlag(Sides.Top))
            {
                if (collisionSides.HasFlag(Sides.Top))
                {
                    particles.Top.Play();
                }
                else
                {
                    particles.Top.Stop();
                }
            }
            if (collisionSides.HasFlag(Sides.Bottom) != currentSides.HasFlag(Sides.Bottom))
            {
                if (collisionSides.HasFlag(Sides.Bottom))
                {
                    particles.Bottom.Play();
                }
                else
                {
                    particles.Bottom.Stop();
                }
            }
            currentSides = collisionSides;

            return;
        }
        //Physics2D.queriesHitTriggers = false;
    }

    public void CheckPlaySideParticlesDoublesided(float colOffset, float colWidth, float colThickness, LayerMask layer,
        SideParticles particles, ref SideData currentSides)
    {
        SideData collisionSides = CheckSides(colOffset, colWidth, colThickness, layer);
        if (currentSides.Flags == collisionSides.Flags)
        {
            //do nothing
            return;
        }
        else
        {
            if (collisionSides.HasFlag(Sides.Top) != currentSides.HasFlag(Sides.Top))
            {
                particles.Top.Play();
            }
            if (collisionSides.HasFlag(Sides.Bottom) != currentSides.HasFlag(Sides.Bottom))
            {
                particles.Bottom.Play();
            }
            if (collisionSides.HasFlag(Sides.Left) != currentSides.HasFlag(Sides.Left))
            {
                particles.Left.Play();
            }
            if (collisionSides.HasFlag(Sides.Right) != currentSides.HasFlag(Sides.Right))
            {
                particles.Right.Play();
            }
            currentSides = collisionSides;
        }
        //Physics2D.queriesHitTriggers = false;
    }

    SideData CheckSides(float colOffset, float colWidth, float colThickness, LayerMask layer)
    {
        float slime_pos_x = gameObject.transform.position.x;
        float slime_pos_y = gameObject.transform.position.y;
        Sides current_sides = Sides.None;

        //Water Bottom
        Collider2D Bot = Physics2D.OverlapBox(new Vector2(slime_pos_x, slime_pos_y - colOffset * CurrentScale), new Vector2(colWidth, colThickness) * CurrentScale, angle: 0, layer);
        //Water Top
        Collider2D Top = Physics2D.OverlapBox(new Vector2(slime_pos_x, slime_pos_y + colOffset * CurrentScale), new Vector2(colWidth, colThickness) * CurrentScale, angle: 0, layer);
        //Water Left
        Collider2D Left = Physics2D.OverlapBox(new Vector2(slime_pos_x - colOffset * CurrentScale, slime_pos_y), new Vector2(colThickness, colWidth) * CurrentScale, angle: 0, layer);
        //Water Right
        Collider2D Right = Physics2D.OverlapBox(new Vector2(slime_pos_x + colOffset * CurrentScale, slime_pos_y), new Vector2(colThickness, colWidth) * CurrentScale, angle: 0, layer);

        //Debug.Log($"Slime pos: ({slime_pos_x}, {slime_pos_y})");

        byte flagCount = new byte();
        if (Bot)
        {
            current_sides |= Sides.Bottom;
            flagCount++;
        }
        if (Top)
        {
            current_sides |= Sides.Top;
            flagCount++;
        }
        if (Right)
        {
            current_sides |= Sides.Right;
            flagCount++;
        }
        if (Left)
        {
            current_sides |= Sides.Left;
            flagCount++;
        }

        return new SideData(current_sides, flagCount);
    }

    void PlayParticlesBySpace()
    {
        switch (CurrentSlimeState.CurrentSpace)
        {
            case Space.Air:
                WaterBubbles.Stop();
                break;
            case Space.Water:
                WaterBubbles.Play();
                break;
        }
    }

    #endregion
}

#region Structs
[Serializable] public struct SideParticles
{
    public ParticleSystem Right;
    public ParticleSystem Left;
    public ParticleSystem Top;
    public ParticleSystem Bottom;

    public void SetRadialVelocity(float speedMod)
    {
        var emitter = Right.velocityOverLifetime;
        emitter.radial = speedMod;

        emitter = Left.velocityOverLifetime;
        emitter.radial = speedMod;

        emitter = Top.velocityOverLifetime;
        emitter.radial = speedMod;

        emitter = Bottom.velocityOverLifetime;
        emitter.radial = speedMod;
    }
}

public struct SideData
{
    public Sides Flags;
    public byte FlagCount;

    public SideData(Sides flags, byte flagCount)
    {
        Flags = flags;
        FlagCount = flagCount;
    }

    public bool HasFlag(Sides flag)
    {
        return Flags.HasFlag(flag);
    }
}
#endregion