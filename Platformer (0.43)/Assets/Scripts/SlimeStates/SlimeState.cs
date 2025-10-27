public struct SlimeState
{
    public float MaxSpeed { get; private set; }
    public float Acceleration { get; private set; }
    public float FrictionGround { get; private set; }
    public float FrictionCeiling { get; private set; }
    public float FrictionWall { get; private set; }
    public float FrictionWallUp { get; private set; }
    public float JumpPower { get; private set; }

    public Reaction[] SaltPiece;
    public Reaction[] Salt;
    public Reaction[] Fire;
    public Reaction[] Water;
    public Reaction[] Electricity;
    public Reaction[] Frost;
    //Reaction[] _SaltPiece;


    public SlimeState(
        float maxSpeed, float acceleration, float frictionGround, float frictionCeiling, float frictionWall, 
        float frictionWallUp, float jumpPower,
        Reaction[] saltPiece, Reaction[] salt, Reaction[] fire, Reaction[] water, Reaction[] electricity, Reaction[] frost)
    {
        MaxSpeed = maxSpeed;
        Acceleration = acceleration;
        FrictionGround = frictionGround;
        FrictionCeiling = frictionCeiling;
        FrictionWall = frictionWall;
        FrictionWallUp = frictionWallUp;
        JumpPower = jumpPower;

        SaltPiece = saltPiece;
        Salt = salt;
        Fire = fire;
        Water = water;
        Electricity = electricity;
        Frost = frost;
    }
}
