using UnityEngine;

public class WallMoveState : SlimeMoveState
{
    public float WallBlockCounter = 0;
    
    public override void EnterState(SlimeController slime)
    {
        const float c_wall_hug_velocity = 0.5f;


        Debug.Log("Entered Wall State!");
        slime.rb.gravityScale = 0f;
        Vector2 slime_velocity = slime.rb.velocity;

        if (slime_velocity.y > 0.1f)
        {
            WallBlockCounter = 1f;
        }

        switch (slime.CurrentSide)
        {
            case Side.Right:
                slime.rb.AddForce(new Vector2(c_wall_hug_velocity,0));
                break;
            case Side.Left:
                slime.rb.AddForce(new Vector2(-c_wall_hug_velocity, 0));
                break;
        }
    }

    public override void UpdateState(SlimeController slime)
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            IsControlled = false;
            slime.EnterMoveState(MoveState.Bounce);
            return;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            FixedUpdateState(slime, slime.Move);
            if (slime.CurrentSide == Side.Left) 
            {
                slime.EnterMoveState(MoveState.JumpWallRight);
                return;
            }
            else
            {
                slime.EnterMoveState(MoveState.JumpWallLeft);
                return;
            }
        }
        //float move_vector_y = move_vector.y;
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    if (slime.CurrentSide == Side.Left)
        //    {
        //        slime.EnterMoveState(MoveState.JumpWallRight);
        //    }
        //    else
        //    {
        //        slime.EnterMoveState(MoveState.JumpWallLeft);
        //    }
        //    return;
        //}
        //slime.ChangeDirection(Direction.Wall);
        //move_vector.x = 0;
        ////move_vector.y = Mathf.Lerp(move_vector.y, 0, slime.CurrentSlimeState.FrictionWall);
        //if (move_vector.y != 0)
        //{
        //    move_vector.y -= slime.CurrentSlimeState.FrictionWall;
        //}
        ////move_vector.y * slime.CurrentSlimeState.FrictionWall;
        //slime.Move = move_vector;
        //Debug.Log($"Initial Y speed: {move_vector_y}" +
        //    $"\nNew Y speed: {move_vector.y}" +
        //    $"\nNew Y speed (applied): {slime.Move.y}");
    }

    public override void FixedUpdateState(SlimeController slime, Vector2 move_vector)
    {
        if (move_vector.y < 0.1f && move_vector.y > -0.1f)
        {
            move_vector.y = 0;
            if (slime.CurrentSide == Side.Left)
            {
                slime.ChangeDirection(Direction.WallLeft);
            }
            else
            {
                slime.ChangeDirection(Direction.WallRight);
            }
        }
        else if (move_vector.y > 0f)
        {
            move_vector.y = Mathf.Sign(move_vector.y) * Mathf.Lerp(Mathf.Abs(move_vector.y), 0, slime.CurrentSlimeState.FrictionWallUp);
            if (slime.CurrentSide == Side.Left)
            {
                slime.ChangeDirection(Direction.WallLeftUp);
            }
            else
            {
                slime.ChangeDirection(Direction.WallRightUp);
            }
        }
        else
        {
            move_vector.y = Mathf.Sign(move_vector.y) * Mathf.Lerp(Mathf.Abs(move_vector.y), WallBlockCounter, slime.CurrentSlimeState.FrictionWall);
            if (slime.CurrentSide == Side.Left)
            {
                slime.ChangeDirection(Direction.WallLeftDown);
            }
            else
            {
                slime.ChangeDirection(Direction.WallRightDown);
            }
            WallBlockCounter = Mathf.Clamp(WallBlockCounter-Time.fixedDeltaTime, 0, WallBlockCounter);
        }
        move_vector.x = Mathf.Sign(move_vector.x) * Mathf.Lerp(Mathf.Abs(move_vector.x), 0, slime.CurrentSlimeState.FrictionGround);
        slime.Move = move_vector;
        slime.rb.velocity = move_vector;
    }

    public override void ReactToGround(Side touching_side, SlimeController slime)
    {
        switch (touching_side)
        {
            case Side.None://falling
                slime.EnterMoveState(MoveState.Air);
                break;
            case Side.Bottom://sticks to the ceiling
                slime.EnterMoveState(MoveState.Ground);
                break;
        }
    }
}
