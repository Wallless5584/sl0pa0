using UnityEngine;

public class JumpWallLeftMoveState : JumpWallRightMoveState
{
    public override void EnterState(SlimeController slime)
    {
        Debug.Log("Entered Jump Wall Left State!");
        slime.rb.gravityScale = 1f * slime.CurrentSlimeState.SpaceGravityMod;
        slime.rb.AddForce(-new Vector2(slime.rb.velocity.x, 0) + Vector2.left * slime.CurrentSlimeState.WallJumpPower);
        slime.ChangeDirection(Direction.JumpWallLeft);
        //slime.face.localPosition = new Vector2(-0.1f, 0);
    }

    public override void ReactToGround(Sides touching_side, SlimeController slime)
    {
        switch (touching_side)
        {
            case Sides.Bottom://sticks to the ground
                if (Input.GetKey(KeyCode.Space))
                {
                    slime.EnterMoveState(MoveState.Jump);
                }
                else
                {
                    slime.EnterMoveState(MoveState.Ground);
                }
                break;
            case Sides.Top://sticks to the ceiling
                if (Input.GetKey(KeyCode.Space))
                {
                    slime.EnterMoveState(MoveState.JumpDown);
                }
                else
                {
                    slime.EnterMoveState(MoveState.Ceiling);
                }
                break;
            case Sides.Left://jumps off right
                if (Input.GetKey(KeyCode.Space))
                {
                    slime.EnterMoveState(MoveState.JumpWallRight);
                }
                else
                {
                    slime.EnterMoveState(MoveState.Wall);
                }
                break;
        }
    }
}
