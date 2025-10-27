using UnityEngine;

public class JumpWallLeftMoveState : JumpMoveState
{
    public override void EnterState(SlimeController slime)
    {
        Debug.Log("Entered Jump Wall Left State!");
        slime.rb.gravityScale = 1f;
        slime.rb.AddForce(Vector2.left * slime.CurrentSlimeState.JumpPower);
        slime.ChangeDirection(Direction.JumpWall);
        //slime.face.localPosition = new Vector2(-0.1f, 0);
    }

    public override void ReactToGround(Side touching_side, SlimeController slime)
    {
        switch (touching_side)
        {
            case Side.Bottom://sticks to the ceiling
                slime.EnterMoveState(MoveState.Ground);
                break;
            case Side.Left://sticks to right wall
                slime.EnterMoveState(MoveState.Wall);
                break;
        }
    }
}
