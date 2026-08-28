using UnityEngine;

public class JumpDownRightMoveState : JumpUpRightMoveState
{
    public override void EnterState(SlimeController slime)
    {
        //Debug.Log($"{Time.time} | Entered JumpDownRight Move State");

        exitTimer = 0f;

        slime.rb.gravityScale = slime.CurrentSlimeState.SpaceGravityMod;
        slime.rb.AddForce(new Vector2(slime.CurrentSlimeState.WallJumpPower, -slime.CurrentSlimeState.JumpPower * c_verticalJumpPowerBoost));
    }


    public override void ReactToGround(Sides touching_side, SlimeController slime)
    {
        switch (touching_side)
        {
            case Sides.Bottom:
                slime.EnterMoveState(MoveState.Ground);
                break;
            case Sides.Right:
                slime.EnterMoveState(MoveState.Wall);
                break;
            case Sides.Right | Sides.Bottom://diagonal
                slime.EnterMoveState(MoveState.WallRightGround);
                break;

        }
    }
}
