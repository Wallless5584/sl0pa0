using UnityEngine;

public class WallMoveState : SlimeMoveState
{
    public float slideDownBaseSpeed = 0;

    const float c_controlledDownVelocity = 1f;
    const float c_controlledUpVelocity = 0.5f;
    const float c_controlledBreakTMultiplier = 1.5f;
    const float c_controlledAccelerationTMultiplier = 0.5f;
    //const float c_slideDownBaseSpeedMultiplier = 0.5f;
    const float c_slideDownBaseSpeedMultiplier = 0.5f;
    const float c_slideStopYVelocity = 0.01f;
    bool isDownPressed = false;
    bool isUpPressed = false;
    
    public override void EnterState(SlimeController slime)
    {
        const float c_wall_hug_velocity = 0.5f;


        //Debug.Log("Entered Wall State!");
        slime.rb.gravityScale = 0f;
        Vector2 slime_velocity = slime.rb.velocity;

        //if (slime_velocity.y > 0.1f)
        //{
        //    slideDownBaseSpeed = (1f - slime.CurrentSlimeState.FrictionWall) * c_slideDownBaseSpeedMultiplier;
        //}

        isDownPressed = Input.GetKey(KeyCode.S); //Player pressing down - give minimal speed down for control;
        isUpPressed = Input.GetKey(KeyCode.W); //Player pressing down - give minimal speed down for control;

        if (slime.CurrentSides.HasFlag(Sides.Right))
        {
            slime.rb.AddForce(new Vector2(c_wall_hug_velocity - slime_velocity.x, 0));
        }
        else
        {
            slime.rb.AddForce(new Vector2(-c_wall_hug_velocity - slime_velocity.x, 0));
        }
    }

    public override void UpdateState(SlimeController slime)
    {
        isDownPressed = Input.GetKey(KeyCode.S); //Player pressing down - give minimal speed down for control;
        isUpPressed = Input.GetKey(KeyCode.W); //Player pressing down - give minimal speed down for control;

        //if (Input.GetKey(KeyCode.S))
        //{

        //}

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
        //if (isDownPressed) //Player pressing down - give minimal speed down for control;
        //{
        //    //move_vector.y = -(1f - slime.CurrentSlimeState.FrictionWall) * c_slideDownBaseSpeedMultiplier;

        //    //move_vector.y = Mathf.Lerp(
        //    //    a: move_vector.y,
        //    //    b: slideDownBaseSpeed,
        //    //    t: current_t
        //    //    );

        //    move_vector.y = Mathf.Clamp(
        //        value: move_vector.y,
        //        min: -(1f - slime.CurrentSlimeState.FrictionWall) * c_slideDownBaseSpeedMultiplier,
        //        max: -slideDownBaseSpeed
        //        );
        //    slideDownBaseSpeed = (1f - slime.CurrentSlimeState.FrictionWall) * c_slideDownBaseSpeedMultiplier;
        //}
        //else
        //if (move_vector.y < c_slideStopYVelocity && move_vector.y > -c_slideStopYVelocity)  // Already going slow, so stop if should
        //{

        //    move_vector.y = 0;

        //    if (slime.CurrentSides.HasFlag(Sides.Right))
        //    {
        //        slime.ChangeDirectionDisplay(Direction.WallRight);
        //    }
        //    else
        //    {
        //        slime.ChangeDirectionDisplay(Direction.WallLeft);
        //    }
        //}
        //else
        //if (move_vector.y > 0f) // Going Up before contacting the wall
        //{
        //    move_vector.y = Mathf.Sign(move_vector.y) * Mathf.Lerp(Mathf.Abs(move_vector.y), 0, slime.CurrentSlimeState.FrictionWallUp);

        //    if (slime.CurrentSides.HasFlag(Sides.Right))
        //    {
        //        slime.ChangeDirectionDisplay(Direction.WallRightUp);

        //    }
        //    else
        //    {
        //        slime.ChangeDirectionDisplay(Direction.WallLeftUp);
        //    }
        //}
        //else // Going Down before contacting the wall
        //{
        //    float current_t = slime.CurrentSlimeState.FrictionWall;
        //    if (isUpPressed == true)
        //    {
        //        current_t = Mathf.Clamp(current_t * c_controlledBreakTMultiplier, 0f, 1f);
        //    }

        //    move_vector.y = Mathf.Sign(move_vector.y) * Mathf.Lerp(
        //        a: Mathf.Abs(move_vector.y),
        //        b: slideDownBaseSpeed,
        //        t: current_t
        //        );

        //    if (slime.CurrentSides.HasFlag(Sides.Right))
        //    {
        //        slime.ChangeDirectionDisplay(Direction.WallRightDown);
        //    }
        //    else
        //    {
        //        slime.ChangeDirectionDisplay(Direction.WallLeftDown);
        //    }

        //    slideDownBaseSpeed = Mathf.Clamp(
        //        value: slideDownBaseSpeed-Time.fixedDeltaTime,
        //        min: 0,
        //        max: slideDownBaseSpeed); // slowly reduce the slide down speed to slow down
        //}
        //move_vector.x = Mathf.Sign(move_vector.x) * Mathf.Lerp(Mathf.Abs(move_vector.x), 0, slime.CurrentSlimeState.FrictionGround);
        //slime.Move = move_vector;
        //slime.rb.velocity = move_vector;





        //  if (stationary)
        //      set slime sprite to wall
        //      set speed to 0 OR to minimal speed if should
        //  if going up
        //      if down is pressed
        //          increase T to slow down faster
        //      else if UP is pressed
        //          reduce T to slow down less
        //      process up movement
        //      use up friction
        //      set slime sprite to wall up
        //  else if going down
        //      if up is pressed
        //          slow down faster
        //          up to a fixed speed
        //      if down is pressed
        //          slow down slower
        //          up to a fixed speed
        //      process down movement
        //      use down friction
        //      use stopping on up button press
        //      set slime sprite to wall down
        //  
        //      


        float target_y_speed = 0;
        float current_t = 0f;

        //-------------------------------------------------------sliding down
        if (move_vector.y < - c_slideStopYVelocity)
        {
            if (isUpPressed)
            {
                //breaking
                current_t = slime.CurrentSlimeState.WallDownBreak;
                target_y_speed = -slime.CurrentSlimeState.MinWallYMove;
            }
            else if (isDownPressed)
            {
                //accelerating
                current_t = slime.CurrentSlimeState.WallDownAcceleration;

                float target_y_speed_min = -c_controlledDownVelocity ;
                float target_y_speed_max = -slime.CurrentSlimeState.MinWallYMove;
                float sub_target_speed = Mathf.Lerp(move_vector.y, -c_controlledDownVelocity, slime.CurrentSlimeState.WallDownAcceleration);
                target_y_speed = Mathf.Clamp(sub_target_speed, target_y_speed_min, target_y_speed_max);
            }
            else
            {
                // nothing is pressed
                current_t = slime.CurrentSlimeState.FrictionWall;
                target_y_speed = -slime.CurrentSlimeState.MinWallYMove;
            }
            Debug.Log($"t: {current_t}, target speed: {target_y_speed}, speed: {move_vector.y}  DOWN");
            if (slime.CurrentSides.HasFlag(Sides.Right))
                slime.ChangeDirectionDisplay(Direction.WallRightDown);
            else
                slime.ChangeDirectionDisplay(Direction.WallLeftDown);

        }
        //--------------------------------------------------------------sliding up
        else if (move_vector.y > c_slideStopYVelocity)
        {
            if (isUpPressed)
            {
                //accelerating up
                current_t = slime.CurrentSlimeState.WallUpAcceleration;
                //float target_y_speed_min = 0f;
                //float target_y_speed_max = c_controlledUpVelocity;
                target_y_speed = 0f;
            }
            else if (isDownPressed)
            {
                //breaking
                current_t = slime.CurrentSlimeState.WallUpBreak;

                target_y_speed = 0f;
            }
            else
            {
                //nothing is pressed
                current_t = slime.CurrentSlimeState.FrictionWallUp;
                target_y_speed = 0f;
            }
            Debug.Log($"t: {current_t}, target speed: {target_y_speed}, speed: {move_vector.y}  UP");
            if (slime.CurrentSides.HasFlag(Sides.Right))
                slime.ChangeDirectionDisplay(Direction.WallRightUp);
            else
                slime.ChangeDirectionDisplay(Direction.WallLeftUp);

        }
        //-----------------------------------------------------------------stopped
        else
        {
            
            if (isDownPressed)
            {
                current_t = slime.CurrentSlimeState.WallDownAcceleration;
                target_y_speed = -(1 - slime.CurrentSlimeState.FrictionWall);
            }
            else
            {
                current_t = slime.CurrentSlimeState.FrictionWall;
                target_y_speed = -slime.CurrentSlimeState.MinWallYMove;
            }
            Debug.Log($"t: {current_t}, target speed: {target_y_speed}, speed: {move_vector.y}  ELSE");
            if (slime.CurrentSides.HasFlag(Sides.Right))
                slime.ChangeDirectionDisplay(Direction.WallRight);
            else
                slime.ChangeDirectionDisplay(Direction.WallLeft);
        }
        

        move_vector.y = Mathf.Lerp(move_vector.y, target_y_speed, current_t);
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
