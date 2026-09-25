using UnityEngine;

public class WallMoveState : SlimeMoveState
{
    public float WallBlockCounter = 0; //WTF???

    const float c_controlledDownVelocity = 0.3f;
    bool isDownPressed = false;
    
    public override void EnterState(SlimeController slime)
    {
        const float c_wall_hug_velocity = 0.5f;


        //Debug.Log("Entered Wall State!");
        slime.rb.gravityScale = 0f;
        Vector2 slime_velocity = slime.rb.velocity;

        if (slime_velocity.y > 0.1f)
        {
            WallBlockCounter = 1f;
        }

        if (slime.CurrentSides.HasFlag(Sides.Right))
        {
            slime.rb.AddForce(new Vector2(c_wall_hug_velocity, 0));
        }
        else
        {
            slime.rb.AddForce(new Vector2(-c_wall_hug_velocity, 0));
        }
    }

    public override void UpdateState(SlimeController slime)
    {
        isDownPressed = Input.GetKey(KeyCode.S); //Player pressing down - give minimal speed down for control;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            IsControlled = false;
            slime.EnterMoveState(MoveState.Bounce);
            return;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            FixedUpdateState(slime, slime.Move);
            if (slime.CurrentSides.HasFlag(Sides.Right)) 
            {
                slime.EnterMoveState(MoveState.JumpWallLeft);
                return;
            }
            else
            {
                slime.EnterMoveState(MoveState.JumpWallRight);
                return;
            }
        }
    }

    public override void FixedUpdateState(SlimeController slime, Vector2 move_vector)
    {
        if (move_vector.y < 0.1f && move_vector.y > -0.1f)
        {
            if (isDownPressed) //Player pressing down - give minimal speed down for control;
            {
                move_vector.y = -c_controlledDownVelocity;
            }
            else
            {
                move_vector.y = 0;
            }

            if (slime.CurrentSides.HasFlag(Sides.Right))
            {
                slime.ChangeDirectionDisplay(Direction.WallRight);
            }
            else
            {
                slime.ChangeDirectionDisplay(Direction.WallLeft);
            }
        }
        else if (move_vector.y > 0f)
        {
            move_vector.y = Mathf.Sign(move_vector.y) * Mathf.Lerp(Mathf.Abs(move_vector.y), 0, slime.CurrentSlimeState.FrictionWallUp);
            if (slime.CurrentSides == Sides.Left)
            {
                slime.ChangeDirectionDisplay(Direction.WallLeftUp);
            }
            else
            {
                slime.ChangeDirectionDisplay(Direction.WallRightUp);
            }
        }
        else
        {
            move_vector.y = Mathf.Sign(move_vector.y) * Mathf.Lerp(Mathf.Abs(move_vector.y), WallBlockCounter, slime.CurrentSlimeState.FrictionWall);
            if (slime.CurrentSides == Sides.Left)
            {
                slime.ChangeDirectionDisplay(Direction.WallLeftDown);
            }
            else
            {
                slime.ChangeDirectionDisplay(Direction.WallRightDown);
            }
            WallBlockCounter = Mathf.Clamp(WallBlockCounter-Time.fixedDeltaTime, 0, WallBlockCounter);
        }
        move_vector.x = Mathf.Sign(move_vector.x) * Mathf.Lerp(Mathf.Abs(move_vector.x), 0, slime.CurrentSlimeState.FrictionGround);
        slime.Move = move_vector;
        slime.rb.velocity = move_vector;
    }

    public override void ReactToGround(SideData touching_side, SlimeController slime)
    {
        switch (touching_side.Flags)
        {
            case Sides.None://falling
                slime.EnterMoveState(MoveState.Air);
                break;
            case Sides.Bottom://goes to ground
                slime.EnterMoveState(MoveState.Ground);
                break;

            case Sides.Bottom | Sides.Right://goes to diagonal
                if (!Input.GetKey(KeyCode.D))
                {
                    slime.EnterMoveState(MoveState.WallRightGround);
                }
                break;
            case Sides.Bottom | Sides.Left://goes to diagonal
                if (!Input.GetKey(KeyCode.A))
                {
                    slime.EnterMoveState(MoveState.WallLeftGround);
                }
                break;
            case Sides.Top | Sides.Right://goes to diagonal
                if (!Input.GetKey(KeyCode.D))
                {
                    slime.EnterMoveState(MoveState.WallRightCeiling);
                }
                break;
            case Sides.Top | Sides.Left://goes to diagonal
                if (!Input.GetKey(KeyCode.A))
                {
                    slime.EnterMoveState(MoveState.WallLeftCeiling);
                }
                break;

            case Sides.Top | Sides.Left | Sides.Bottom://leaves dead end to right
                if (Input.GetKey(KeyCode.D))
                {
                    slime.EnterMoveState(MoveState.Ground);
                }
                break;
            case Sides.Top | Sides.Right | Sides.Bottom://leaves dead end to left
                if (Input.GetKey(KeyCode.A))
                {
                    slime.EnterMoveState(MoveState.Ground);
                }
                break;
        }
    }
}
