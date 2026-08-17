using UnityEngine;

public class JumpUpRightMoveState : JumpGroundMoveState
{
    protected const float c_verticalJumpPowerBoost = 1.25f;
    public override void EnterState(SlimeController slime)
    {
        //Debug.Log($"{Time.time} | Entered JumpUpRight Move State");

        exitTimer = 0f;

        slime.rb.gravityScale = slime.CurrentSlimeState.SpaceGravityMod;
        slime.rb.AddForce(new Vector2(slime.CurrentSlimeState.WallJumpPower, slime.CurrentSlimeState.JumpPower * c_verticalJumpPowerBoost));
    }


    public override void ReactToGround(Sides touching_side, SlimeController slime)
    {
        switch (touching_side)
        {
            case Sides.Top:
                slime.EnterMoveState(MoveState.Ceiling);
                break;
            case Sides.Right:
                slime.EnterMoveState(MoveState.Wall);
                break;
            case Sides.Right | Sides.Top://diagonal
                slime.EnterMoveState(MoveState.WallRightCeiling);
                break;

        }
    }
}
