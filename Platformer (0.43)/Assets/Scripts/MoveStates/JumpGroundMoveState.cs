using UnityEngine;

public class JumpGroundMoveState : SlimeMoveState
{
    protected const float c_exitStateTime = 0.4f; //Affects high gravity time during below-max jumps 
    protected const float c_minExitStateTime = 0.2f; //Earliest premature time to enter bounce state
    protected float c_lowGravityStartTime = 0.1f; //Affects all jumps (most effect on Max jumps)

    protected float exitTimer;

    protected const float c_lowGravity = 2f; //Affects all jumps (most effect on Max jumps)
    protected const float c_highGravity = 5f; //Affects Below-max jumps

    //Alternative values:
    //0.35
    //0.15
    //2.45
    //7

    //bool JumpExt = true;



    public override void EnterState(SlimeController slime)
    { 
        //Debug.Log($"{Time.time} | Entered Jump State!");

        //JumpExt = true;
        exitTimer = 0f;

        slime.rb.gravityScale = 0f;
        slime.rb.AddForce(-Vector2.up * slime.rb.velocity + Vector2.up * slime.CurrentSlimeState.JumpPower);

        slime.ChangeDirectionDisplay(Direction.GroundJump);
        SlimeSoundManager.Instance.PlaySound(SoundType.Jump);
        if (slime.WaterSides.FlagCount > 1)
        {
            slime.WaterBubblesBurst.Play();
        }
        //slime.face.localPosition = new Vector2(0,0.1f);
        //slime.sr.sprite = slime.slime_jump;
    }

    public override void UpdateState(SlimeController slime)
    {
        //if (Input.GetKeyUp(KeyCode.Space) && JumpExt == true)
        //{
        //    exitTimer = Mathf.Clamp(c_exitTimeMaxValue * exitTimer, c_exitTimeMinValue, c_exitTimeMaxValue);
        //    JumpExt = false;
        //}
        if (Input.GetKeyUp(KeyCode.Space))
        {
            slime.rb.gravityScale = c_highGravity * slime.CurrentSlimeState.SpaceGravityMod;
        }

        exitTimer += Time.deltaTime;
        moveX = 0;

        if (exitTimer >= c_lowGravityStartTime)
        {
            slime.rb.gravityScale = c_lowGravity * slime.CurrentSlimeState.SpaceGravityMod;
        }

        if (exitTimer >= c_exitStateTime)
        {
            //Player reached maximum jump height/time
            if (Input.GetKey(KeyCode.Space))
            {
                slime.EnterMoveState(MoveState.JumpAir);
                return;
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                slime.EnterMoveState(MoveState.Bounce);
                return;
            }
            else
            {
                slime.EnterMoveState(MoveState.Air);//inAir
                return;
            }
        }
        else if(exitTimer >= c_minExitStateTime)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                slime.EnterMoveState(MoveState.Bounce);
                return;
            }
        }

        if (Input.GetKey(KeyCode.D))
        {
            IsControlled = true;
            moveX++;
            //slime.ChangeDirection(Direction.GroundRight);
        }
        if (Input.GetKey(KeyCode.A))
        {
            IsControlled = true;
            moveX--;
            //slime.ChangeDirection(Direction.GroundLeft);
        }
        if (moveX == 0)
        {
            IsControlled = false;
        }
        //if (exitTimer < c_timeJumpBalance)
        //{
        //    slime.rb.gravityScale = c_gravityJumpBalance * slime.CurrentSlimeState.SpaceGravityMod;
        //}

        //if (exitTimer < 0f)
        //{
           
        //} 
        //else if (exitTimer < c_exitTimeMaxValue - c_exitTimeMinValue)
        //{
        //    if (Input.GetKey(KeyCode.LeftShift))
        //    {
        //        exitTimer = c_exitTimeMaxValue;
        //        slime.EnterMoveState(MoveState.Bounce);
        //        return;
        //    }
        //}
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
        else
        {
            //move_vector.x = move_vector.x - move_vector.x * slime.CurrentSlimeState.Decceleration;
            //move_vector.x = Mathf.Sign(move_vector.x) * Mathf.Lerp(Mathf.Abs(move_vector.x), 0, slime.CurrentSlimeState.Decceleration);
        }
        slime.Move = move_vector;
    }

    public override void ReactToGround(SideData touching_side, SlimeController slime)
    {
        switch (touching_side.Flags)
        {
            case Sides.Top://sticks to the ceiling
                if (Input.GetKey(KeyCode.Space))
                {
                    slime.EnterMoveState(MoveState.JumpDown);
                    return;
                }
                else
                {
                    slime.EnterMoveState(MoveState.Ceiling);
                    return;
                }
                break;
            //case Sides.Left://jumps off right
            //    if (Input.GetKey(KeyCode.Space))
            //    {
            //        slime.EnterMoveState(MoveState.JumpWallRight);
            //    }
            //    else
            //    {
            //        slime.EnterMoveState(MoveState.Wall);
            //    }
            //    break;
            //case Sides.Right://jumps off left
            //    if (Input.GetKey(KeyCode.Space))
            //    {
            //        slime.EnterMoveState(MoveState.JumpWallLeft);
            //    }
            //    else
            //    {
            //        slime.EnterMoveState(MoveState.Wall);
            //    }
            //    break;
        }
    }
}
