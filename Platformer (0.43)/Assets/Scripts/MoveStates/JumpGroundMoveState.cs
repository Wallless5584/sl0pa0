using UnityEngine;

public class JumpGroundMoveState : SlimeMoveState
{
    protected const float c_exitTimeMaxValue = 0.4f;
    protected float c_exitTimeMinValue = 0.2f;

    protected float exit_timer = c_exitTimeMaxValue;

    protected const float c_gravityJumpBalance = 2.4f;
    protected const float c_timeJumpBalance = 0.2f;



    public string debugStateName = "Jump State";
    public Vector2 forceDirection = Vector2.up;
    public Direction direction = Direction.JumpUp;
    public SoundType sound = SoundType.Jump;
    bool JumpExt = true;



    public override void EnterState(SlimeController slime)
    { 
        Debug.Log($"Entered {debugStateName}!");
        JumpExt = true;
        slime.rb.gravityScale = 0f;
        slime.rb.AddForce(-forceDirection * slime.rb.velocity + forceDirection * slime.CurrentSlimeState.JumpPower);
        slime.ChangeDirection(direction);
        SlimeSoundManager.Instance.PlaySound(sound);
        //slime.face.localPosition = new Vector2(0,0.1f);
        //slime.sr.sprite = slime.slime_jump;
    }

    public override void UpdateState(SlimeController slime)
    {
        if (Input.GetKeyUp(KeyCode.Space) && JumpExt == true)
        {
            exit_timer = Mathf.Clamp(c_exitTimeMaxValue * exit_timer, c_exitTimeMinValue, c_exitTimeMaxValue);
            JumpExt = false;
        }
        exit_timer -= Time.deltaTime;
        moveX = 0;
        
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
        if (exit_timer < c_timeJumpBalance)
        {
            slime.rb.gravityScale = c_gravityJumpBalance;
        }
        if (exit_timer < 0)
        {
            exit_timer = c_exitTimeMaxValue;

            if (Input.GetKey(KeyCode.Space))
            {
                slime.EnterMoveState(MoveState.JumpAir);
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                slime.EnterMoveState(MoveState.Bounce);
            }
            else
            {
                slime.EnterMoveState(MoveState.Air);//inAir
            }
            return;
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
            //move_vector.x = Mathf.Sign(move_vector.x) * Mathf.Lerp(Mathf.Abs(move_vector.x), 0, slime.CurrentSlimeState.Decceleration);
        }
        slime.Move = move_vector;
    }

    public override void ReactToGround(Side touching_side, SlimeController slime)
    {
        switch (touching_side)
        {
            case Side.Top://sticks to the ceiling
                if (Input.GetKey(KeyCode.Space))
                {
                    slime.EnterMoveState(MoveState.JumpDown);
                }
                else
                {
                    slime.EnterMoveState(MoveState.Ceiling);
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
