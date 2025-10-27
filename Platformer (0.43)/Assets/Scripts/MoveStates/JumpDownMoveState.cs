using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpDownMoveState : JumpMoveState
{
    public override void EnterState(SlimeController slime)
    {
        Debug.Log("Entered Jump Down State!");
        slime.rb.gravityScale = 1f;
        slime.rb.AddForce(Vector2.down * slime.CurrentSlimeState.JumpPower);
        slime.ChangeDirection(Direction.JumpDown);
    }

    public override void ReactToGround(Side touching_side, SlimeController slime)
    {
        switch (touching_side)
        {
            case Side.Bottom://sticks to the ceiling
                slime.EnterMoveState(MoveState.Ground);
                break;
        }
    }
}
