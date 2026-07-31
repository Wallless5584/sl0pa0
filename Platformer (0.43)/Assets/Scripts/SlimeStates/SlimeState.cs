using System;
using UnityEngine;

[Serializable] public struct SlimeState
{
    [SerializeField] float _maxSpeed;
    [SerializeField] float _maxSpeedAir;
    [SerializeField] float _acceleration;
    [SerializeField] float _accelerationAir;
    [SerializeField] float _frictionGround;
    [SerializeField] float _frictionCeiling;
    [SerializeField] float _frictionWall;
    [SerializeField] float _frictionWallUp;
    [SerializeField] float _frictionBounce;
    [SerializeField] float _jumpPower;
    [SerializeField] float _wallJumpPower;

    public float MaxSpeed { get => _maxSpeed * SpaceStates[(int)CurrentSpace].SlimeMaxSpeedMod; }
    public float MaxSpeedAir { get => _maxSpeedAir * SpaceStates[(int)CurrentSpace].SlimeMaxSpeedAirMod; }
    public float Acceleration { get => _acceleration * SpaceStates[(int)CurrentSpace].SlimeAccelerationMod; }
    public float AccelerationAir { get => _accelerationAir * SpaceStates[(int)CurrentSpace].SlimeAccelerationAirMod; }
    public float FrictionGround { get => _frictionGround * SpaceStates[(int)CurrentSpace].SlimeFrictionGroundMod; }
    public float FrictionCeiling { get => _frictionCeiling * SpaceStates[(int)CurrentSpace].SlimeFrictionCeilingMod; }
    public float FrictionWall { get => _frictionWall * SpaceStates[(int)CurrentSpace].SlimeFrictionWallMod; }
    public float FrictionWallUp { get => _frictionWallUp * SpaceStates[(int)CurrentSpace].SlimeFrictionWallUpMod; }
    public float FrictionBounce { get => _frictionBounce * SpaceStates[(int)CurrentSpace].SlimeFrictionBounceMod; }
    public float JumpPower { get => _jumpPower * SpaceStates[(int)CurrentSpace].SlimeJumpPowerMod; }
    public float WallJumpPower { get => _wallJumpPower * SpaceStates[(int)CurrentSpace].SlimeWallJumpPowerMod; }


    public Space CurrentSpace;
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
