using UnityEngine;

public class JumpDownLeftMoveState : JumpUpRightMoveState
{
    public override void EnterState(SlimeController slime)
    {
        //Debug.Log($"{Time.time} | Entered JumpUpLeft Move State");

        exitTimer = c_exitTimeMaxValue;

        slime.rb.gravityScale = slime.CurrentSlimeState.SpaceGravityMod;
        slime.rb.AddForce(new Vector2(-slime.CurrentSlimeState.WallJumpPower, -slime.CurrentSlimeState.JumpPower * c_verticalJumpPowerBoost));
    }


    public override void ReactToGround(Sides touching_side, SlimeController slime)
    {
        switch (touching_side)
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
