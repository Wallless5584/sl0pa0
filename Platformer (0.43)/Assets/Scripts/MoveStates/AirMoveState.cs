using UnityEngine;

public class AirMoveState : SlimeMoveState
{
    public override void EnterState(SlimeController slime)
    {
        //Debug.Log("Entered Air State!");
        slime.ChangeDirectionDisplay(Direction.Air);
    }

    public override void UpdateState(SlimeController slime)
    {
        slime.rb.gravityScale = slime.CurrentSlimeState.SpaceGravityMod;
        moveX = 0;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveX = 0;
            IsControlled = false;
            slime.EnterMoveState(MoveState.Bounce);
            return;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            moveX = 0;
            IsControlled = false;
            slime.EnterMoveState(MoveState.JumpAir);
            return;
        }
        if (Input.GetKey(KeyCode.D))
        {  
            slime.ChangeDirectionDisplay(Direction.AirRight);
            moveX++;
            IsControlled = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            slime.ChangeDirectionDisplay(Direction.AirLeft);
            moveX--;
            IsControlled = true;
        }
        if (moveX == 0)
        {
            //slime.ChangeDirection(Direction.Air);
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
               move_vector.x + slime.CurrentSlimeState.AccelerationAir * moveX,
               -slime.CurrentSlimeState.MaxSpeedAir,
               slime.CurrentSlimeState.MaxSpeedAir);
        }
        //else
        //{
        //move_vector.x = move_vector.x - move_vector.x * slime.CurrentSlimeState.Decceleration;
        // move_vector.x = Mathf.Sign(move_vector.x) * Mathf.Lerp(Mathf.Abs(move_vector.x), 0, slime.CurrentSlimeState.Decceleration);
        //}
        //Debug.Log($"Move: {move_vector.x}, {move_vector.y}");
        slime.Move = move_vector;
    }

    public override void ReactToGround(Sides touching_side, SlimeController slime)
    {
        switch (touching_side)
        {
            case Sides.None:
                if (slime.CurrentWetnessState == WetnessState.Dry && slime.CheckWaterSurface())
                {
                    slime.EnterMoveState(MoveState.WaterGlide);
                }
                break;

            case Sides.Top://sticks to the ceiling
                slime.EnterMoveState(MoveState.Ceiling);
                break;
            case Sides.Bottom://sticks to the ground
                slime.EnterMoveState(MoveState.Ground);
                break; 
            case Sides.Left://sticks to a left wall
                Debug.Log("Hit a wall left!");
                slime.EnterMoveState(MoveState.Wall);
                break;
            case Sides.Right://sticks to a right wall
                Debug.Log("Hit a wall right!");
                slime.EnterMoveState(MoveState.Wall);
                break;

            case Sides.Top | Sides.Bottom://tunnel horizontal
                if(Input.GetKey(KeyCode.S))
                    slime.EnterMoveState(MoveState.Ground);
                else if(Input.GetKey(KeyCode.W))
                    slime.EnterMoveState(MoveState.Ceiling);
                break;

            case Sides.Top | Sides.Left://goes to diagonal
                slime.EnterMoveState(MoveState.WallLeftCeiling);
                break;
            case Sides.Top | Sides.Right://goes to diagonal
                slime.EnterMoveState(MoveState.WallRightCeiling);
                break;
            case Sides.Bottom | Sides.Left://goes to diagonal
                slime.EnterMoveState(MoveState.WallLeftGround);
                break;
            case Sides.Bottom | Sides.Right://goes to diagonal
                slime.EnterMoveState(MoveState.WallRightGround);
                break;

            case Sides.Top | Sides.Left | Sides.Bottom://leaves dead end left
                slime.EnterMoveState(MoveState.Ground);
                break;
            case Sides.Top | Sides.Right | Sides.Bottom://leaves dead end right
                slime.EnterMoveState(MoveState.Ground);
                break;
            case Sides.Right | Sides.Left | Sides.Bottom://leaves dead end bottom
                slime.EnterMoveState(MoveState.Ground);
                break;
            case Sides.Top | Sides.Right | Sides.Left://leaves dead end ceiling
                slime.EnterMoveState(MoveState.Ceiling);
                break;
        }
    }
}
