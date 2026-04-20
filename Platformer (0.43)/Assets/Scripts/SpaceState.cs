using UnityEngine;

public enum Space
{
    Air,
    Water,
    Cold,
}

[CreateAssetMenu]
public class SpaceState : ScriptableObject
{
    [Range(0,3)] public float SlimeMaxSpeedMod = 1;
    [Range(0, 3)] public float SlimeAccelerationMod = 1;
    [Range(0, 3)] public float SlimeFrictionGroundMod = 1;
    [Range(0, 3)] public float SlimeFrictionWallMod = 1;
    [Range(0, 3)] public float SlimeFrictionWallUpMod = 1;
    [Range(0, 3)] public float SlimeFrictionCeilingMod = 1;
    [Range(-10, 10)] public float SlimeGravityMod = 1;
    [Range(-10, 10)] public float SlimeBounceGravityMod = 1;
    [Range(0, 10)] public float SlimeDrag = 0;
    [Range(0, 3)] public float SlimeWallJumpPowerMod = 1;
    [Range(0, 3)] public float SlimeJumpPowerMod = 1;
}
