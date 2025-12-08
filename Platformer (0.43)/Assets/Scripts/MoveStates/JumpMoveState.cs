using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMoveState : SlimeMoveState
{
    float exit_timer = 0.4f;

    public override void EnterState(SlimeController slime)
    {
        Debug.Log("Entered Jump State!");
        slime.rb.gravityScale = 1f;
        slime.rb.AddForce(Vector2.up * slime.CurrentSlimeState.JumpPower);
        slime.ChangeDirection(Direction.JumpUp);
        SlimeSoundManager.Instance.PlaySound(SoundType.Jump);
        //slime.face.localPosition = new Vector2(0,0.1f);
        //slime.sr.sprite = slime.slime_jump;
    }

    public override void UpdateState(SlimeController slime)
    {
        exit_timer -= Time.deltaTime;
        moveX = 0;
        if (Input.GetKey(KeyCode.LeftShift) && exit_timer < 0.2f)
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
            slime.EnterMoveState(MoveState.Air);//inAir
            return;
        }
        return;
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
                slime.EnterMoveState(MoveState.Ceiling);
                break;
        }
    }
}
