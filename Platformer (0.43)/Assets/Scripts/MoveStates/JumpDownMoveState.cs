using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpDownMoveState : JumpGroundMoveState
{

    public new string debugStateName = "Jump Down State";
    public new Vector2 forceDirection = Vector2.down;
    public new Direction direction = Direction.JumpDown;
    public new SoundType sound = SoundType.Jump;

    public override void EnterState(SlimeController slime)
    {
        Debug.Log("Entered Jump Down State!");
        slime.rb.gravityScale = 1f;
        slime.rb.AddForce(-new Vector2(0, slime.rb.velocity.y) + Vector2.down * slime.CurrentSlimeState.JumpPower);
        slime.ChangeDirection(Direction.JumpDown);
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
            case Side.Left://jumps off right
                if (Input.GetKey(KeyCode.Space))
                {
                    slime.EnterMoveState(MoveState.JumpWallRight);
                }
                else
                {
                    slime.EnterMoveState(MoveState.Wall);
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
