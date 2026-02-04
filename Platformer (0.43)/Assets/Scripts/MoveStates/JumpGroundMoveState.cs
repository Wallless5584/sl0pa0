using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpGroundMoveState : SlimeMoveState
{
    float exit_timer = 0.4f;

    public override void EnterState(SlimeController slime)
    {
        Debug.Log("Entered Jump State!");
        slime.rb.gravityScale = 1f;
        slime.rb.AddForce(-new Vector2(0, slime.rb.velocity.y) + Vector2.up * slime.CurrentSlimeState.JumpPower);
        slime.ChangeDirection(Direction.JumpUp);
        SlimeSoundManager.Instance.PlaySound(SoundType.Jump);
        //slime.face.localPosition = new Vector2(0,0.1f);
        //slime.sr.sprite = slime.slime_jump;
    }

    public override void UpdateState(SlimeController slime)
    {
        exit_timer -= Time.deltaTime;
        moveX = 0;
        const float c_ExitTime = 0.2f;
        if (Input.GetKey(KeyCode.LeftShift) && exit_timer < c_ExitTime)
        {
            IsControlled = false;
            slime.EnterMoveState(MoveState.Bounce);
            return;
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
        if (exit_timer < 0)
        {
            exit_timer = 0.4f;
            if (Input.GetKey(KeyCode.Space))
            {
                slime.EnterMoveState(MoveState.JumpAir);
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
