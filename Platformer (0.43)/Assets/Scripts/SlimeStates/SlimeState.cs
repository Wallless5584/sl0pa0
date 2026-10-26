using System;
using UnityEngine;

[Serializable] public struct SlimeState
{
    [Header("Ground, Ceiling and Air:")]
    [SerializeField] float _maxSpeed;
    [SerializeField] float _maxSpeedAir;
    [SerializeField] float _acceleration;
    [SerializeField] float _accelerationAir;
    [SerializeField] float _frictionGround;
    [SerializeField] float _frictionCeiling;
    [SerializeField] float _jumpPower;

    [Header("Wall:")]
    [SerializeField] float _frictionWall;
    [SerializeField] float _frictionWallUp;
    [SerializeField][Range(0f, 0.5f)] float _minWallYMove;  // max value set to 0.5f according to the value of c_controlledDownVelocity in WallMoveState
    [SerializeField][Range(0f, 1f)] float _wallDownAcceleration;
    [SerializeField][Range(0f, 1f)] float _wallDownBreak;
    [SerializeField][Range(0f, 1f)] float _wallUpAcceleration;
    [SerializeField][Range(0f, 1f)] float _wallUpBreak;
    [SerializeField] float _wallJumpPower;

    [Header("Bounce:")]
    [SerializeField] float _frictionBounce;
    [SerializeField] float _bounceEnergyMultiplier;
    [SerializeField] float _bounceReflectControlMultiplier;

    public float MaxSpeed { get => _maxSpeed * SpaceStates[(int)CurrentSpace].SlimeMaxSpeedMod; }
    public float MaxSpeedAir { get => _maxSpeedAir * SpaceStates[(int)CurrentSpace].SlimeMaxSpeedAirMod; }
    public float Acceleration { get => _acceleration * SpaceStates[(int)CurrentSpace].SlimeAccelerationMod; }
    public float AccelerationAir { get => _accelerationAir * SpaceStates[(int)CurrentSpace].SlimeAccelerationAirMod; }
    public float FrictionGround { get => _frictionGround * SpaceStates[(int)CurrentSpace].SlimeFrictionGroundMod; }
    public float FrictionCeiling { get => _frictionCeiling * SpaceStates[(int)CurrentSpace].SlimeFrictionCeilingMod; }
    public float FrictionWall { get => _frictionWall * SpaceStates[(int)CurrentSpace].SlimeFrictionWallMod; }
    public float FrictionWallUp { get => _frictionWallUp * SpaceStates[(int)CurrentSpace].SlimeFrictionWallUpMod; }
    public float FrictionBounce { get => _frictionBounce * SpaceStates[(int)CurrentSpace].SlimeFrictionBounceMod; }
    public float MinWallYMove { get => _minWallYMove * SpaceStates[(int)CurrentSpace].SlimeWallMinYMoveMod; }
    public float WallDownAcceleration { get => _wallDownAcceleration * SpaceStates[(int)CurrentSpace].SlimeWallDownAccelerationMod; }
    public float WallDownBreak { get => _wallDownBreak * SpaceStates[(int)CurrentSpace].SlimeWallDownBreakMod; }
    public float WallUpAcceleration { get => _wallUpAcceleration * SpaceStates[(int)CurrentSpace].SlimeWallUpAccelerationMod; }
    public float WallUpBreak { get => _wallUpBreak * SpaceStates[(int)CurrentSpace].SlimeWallUpBreakMod; }
    public float JumpPower { get => _jumpPower * SpaceStates[(int)CurrentSpace].SlimeJumpPowerMod; }
    public float WallJumpPower { get => _wallJumpPower * SpaceStates[(int)CurrentSpace].SlimeWallJumpPowerMod; }
    public float BounceEnergyMultiplier { get => _bounceEnergyMultiplier * SpaceStates[(int)CurrentSpace].SlimeBounceEnergyMod; }
    public float BounceReflectControlMultiplier { get => _bounceReflectControlMultiplier * SpaceStates[(int)CurrentSpace].SlimeReflectControlMod; }


    [Header("Space States and Reactions:")]
    [HideInInspector] public Space CurrentSpace;
    [SerializeField] SpaceState[] SpaceStates;
    public float SpaceDrag { get => SpaceStates[(int)CurrentSpace].SlimeDrag; }
    public float SpaceGravityMod { get => SpaceStates[(int)CurrentSpace].SlimeGravityMod; }
    public float SpaceBounceGravityMod { get => SpaceStates[(int)CurrentSpace].SlimeBounceGravityMod; }

    public Reaction[] SaltPiece;
    public Reaction[] Salt;
    public Reaction[] Fire;
    public Reaction[] Water;
    public Reaction[] Electricity;
    public Reaction[] Frost;
}
