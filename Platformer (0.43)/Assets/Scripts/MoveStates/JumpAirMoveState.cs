using UnityEngine;

public class JumpAirMoveState : SlimeMoveState
{
    public override void EnterState(SlimeController slime)
    {
        Debug.Log("Entered Jump Air State!");
        slime.rb.gravityScale = 1f;
        //slime.sr.sprite = slime.slime_inAir;
    }

    public override void UpdateState(SlimeController slime)
    {
        moveX = 0;
        if (Input.GetKeyUp(KeyCode.Space))
        {
            IsControlled = true;
            slime.EnterMoveState(MoveState.Air);
            return;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            IsControlled = false;
            slime.EnterMoveState(MoveState.Bounce);
            return;
        }
        if (Input.GetKey(KeyCode.D))
        {  
            slime.ChangeDirection(Direction.InAirRight);
            moveX++;
            IsControlled = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            slime.ChangeDirection(Direction.InAirLeft);
            moveX--;
            IsControlled = true;
        }
        if (moveX == 0)
        {
            slime.ChangeDirection(Direction.InAir);
            IsControlled = false;
        }
        //else
        //{
        //move_vector.x = Mathf.Lerp(move_vector.x, 0, slime.CurrentSlimeState.Decceleration);
        //slime.ChangeDirection(Direction.InAir);
        //}
        //move_vector_x = Mathf.Lerp(move_vector_x, slime.CurrentSlimeState.MaxSpeed * move_x, slime.CurrentSlimeState.Acceleration * Mathf.Abs(move_x));
        //slime.Move.x = move_vector_x;
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
        //else
        //{
        //move_vector.x = move_vector.x - move_vector.x * slime.CurrentSlimeState.Decceleration;
        // move_vector.x = Mathf.Sign(move_vector.x) * Mathf.Lerp(Mathf.Abs(move_vector.x), 0, slime.CurrentSlimeState.Decceleration);
        //}
        //Debug.Log($"Move: {move_vector.x}, {move_vector.y}");
        slime.Move = move_vector;
    }

    public override void ReactToGround(Side touching_side, SlimeController slime)
    {
        switch (touching_side)
        {
            case Side.Bottom://sticks to the ground
                slime.EnterMoveState(MoveState.Jump);
                break; 
            case Side.Top://sticks to the ceiling
                slime.EnterMoveState(MoveState.JumpDown);
                break;
            case Side.Left://sticks to a left wall
                slime.EnterMoveState(MoveState.JumpWallRight);
                break;
            case Side.Right://sticks to a right wall
                slime.EnterMoveState(MoveState.JumpWallLeft);
                break;
        }
    }
}
