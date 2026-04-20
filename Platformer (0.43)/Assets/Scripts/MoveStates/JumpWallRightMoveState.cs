using UnityEngine;

public class JumpWallRightMoveState : JumpGroundMoveState
{
    public override void EnterState(SlimeController slime)
    {
        Debug.Log("Entered Jump Wall Right State!");
        slime.rb.gravityScale = 1f * slime.CurrentSlimeState.SpaceGravityMod;
        slime.rb.AddForce(-new Vector2(slime.rb.velocity.x, 0) + Vector2.right * slime.CurrentSlimeState.WallJumpPower);
        slime.ChangeDirection(Direction.JumpWall);
        //slime.face.localPosition = new Vector2(0.1f, 0);
    }

    public override void UpdateState(SlimeController slime)
    {
        exit_timer -= Time.deltaTime;

        if (exit_timer < 0)
        {
            exit_timer = c_exitTimeMaxValue;
            if (Input.GetKey(KeyCode.Space))
            {
                slime.EnterMoveState(MoveState.JumpAir);
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                slime.EnterMoveState(MoveState.Bounce);
            }
            else
            {
                slime.EnterMoveState(MoveState.Air);//inAir
            }
        }
    }

    public override void FixedUpdateState(SlimeController slime, Vector2 move_vector)
    {
    }

    public override void ReactToGround(Side touching_side, SlimeController slime)
    {
        switch (touching_side)
        {
            case Side.Bottom://sticks to the ground
                if (Input.GetKey(KeyCode.Space))
                {
                    slime.EnterMoveState(MoveState.Jump);
                }
                else
                {
                    slime.EnterMoveState(MoveState.Ground);
                }
                break;
            case Side.Top://sticks to the ceiling
                if (Input.GetKey(KeyCode.Space))
                {
                    slime.EnterMoveState(MoveState.JumpDown);
                }
                else
                {
                    slime.EnterMoveState(MoveState.Ceiling);
                }
                break;
            case Side.Right://jumps off left
                if (Input.GetKey(KeyCode.Space))
                {
                    slime.EnterMoveState(MoveState.JumpWallLeft);
                }
                else
                {
                    slime.EnterMoveState(MoveState.Wall);
                }
                break;
        }
    }
}
