using UnityEngine;

public class GroundMoveState : SlimeMoveState
{
    //float move_x;
    public override void EnterState(SlimeController slime)
    {
        Debug.Log("Entered Ground State!");
        slime.rb.gravityScale = 10f;
    }

    public override void UpdateState(SlimeController slime)
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            IsControlled = false;
            slime.EnterMoveState(MoveState.Bounce);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            slime.EnterMoveState(MoveState.Jump);
            return;
        }
        moveX = 0;
        if (Input.GetKey(KeyCode.D))
        {
            IsControlled = true;
            moveX++;
            slime.ChangeDirection(Direction.GroundRight);
        }
        if (Input.GetKey(KeyCode.A))
        {
            IsControlled = true;
            moveX--;
            slime.ChangeDirection(Direction.GroundLeft);
        }
        if (moveX == 0)
        {
            IsControlled = false;
            slime.ChangeDirection(Direction.Ground);
        }
    }

    public override void FixedUpdateState(SlimeController slime, Vector2 move_vector)
    {
        if (IsControlled)
        {
            move_vector.x = Mathf.Clamp(
                move_vector.x + slime.CurrentSlimeState.Acceleration * moveX,
                -slime.CurrentSlimeState.MaxSpeed,
                slime.CurrentSlimeState.MaxSpeed);
        }
        else
        {
            //move_vector.x = move_vector.x - move_vector.x * slime.CurrentSlimeState.Decceleration;
            move_vector.x = Mathf.Sign(move_vector.x) * Mathf.Lerp(Mathf.Abs(move_vector.x), 0, slime.CurrentSlimeState.FrictionGround);
        }
        slime.Move = move_vector;
    }

    public override void ReactToGround(Side touching_side, SlimeController slime)
    {
        switch(touching_side)
        {
            case Side.None://falling
                slime.EnterMoveState(MoveState.Air);
                break;
            case Side.Top://sticks to the ceiling
                slime.EnterMoveState(MoveState.Ceiling);
                break;
            case Side.Left://sticks to a left wall
                break;
            case Side.Right://sticks to a right wall
                break;
        }
    }
}
