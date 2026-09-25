using UnityEngine;

public class JumpDownLeftMoveState : JumpUpRightMoveState
{
    public override void EnterState(SlimeController slime)
    {
        //Debug.Log($"{Time.time} | Entered JumpUpLeft Move State");

        exitTimer = 0f;

        slime.rb.gravityScale = slime.CurrentSlimeState.SpaceGravityMod;
        slime.rb.AddForce(new Vector2(-slime.CurrentSlimeState.WallJumpPower, -slime.CurrentSlimeState.JumpPower * c_verticalJumpPowerBoost));

        if (slime.WaterSides.FlagCount > 1)
        {
            slime.WaterBubblesBurst.Play();
        }
        SlimeSoundManager.Instance.PlaySound(SoundType.Jump);
    }


    public override void ReactToGround(SideData touching_side, SlimeController slime)
    {
        switch (touching_side.Flags)
        {
            case Sides.Bottom:
                slime.EnterMoveState(MoveState.Ground);
                break;
            case Sides.Left:
                slime.EnterMoveState(MoveState.Wall);
                break;
            case Sides.Left | Sides.Bottom://diagonal
                slime.EnterMoveState(MoveState.WallLeftGround);
                break;

        }
    }
}
