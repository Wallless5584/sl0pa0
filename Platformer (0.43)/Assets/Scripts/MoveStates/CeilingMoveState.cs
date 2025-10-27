using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingMoveState : SlimeMoveState
{
    Vector2 _move_vector;

    public override void EnterState(SlimeController slime)
    {
        Debug.Log("Entered Ceiling State!");
        slime.rb.gravityScale = -10f;
        if (slime.CurrentSlimeState.FrictionCeiling == 1)
        {
            _move_vector.x *= 0;
            slime.rb.velocity = _move_vector;
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            slime.EnterMoveState(MoveState.JumpDown);
            return;
        }
        moveX = 0;
        if (Input.GetKey(KeyCode.D))
        {
            IsControlled = true;
            moveX++;
            slime.ChangeDirection(Direction.CeilingRight);
        }
        if (Input.GetKey(KeyCode.A))
        {
            IsControlled = true;
            moveX--;
            slime.ChangeDirection(Direction.CeilingLeft);
        }
        if (moveX == 0)
        {
            IsControlled = false;
            slime.ChangeDirection(Direction.Ceiling);
        }
    }

    public override void FixedUpdateState(SlimeController slime, Vector2 move_vector)
    {
        _move_vector = move_vector;
        if (IsControlled)
        {
            _move_vector.x = Mathf.Clamp(
                _move_vector.x + slime.CurrentSlimeState.Acceleration * moveX * (1 - slime.CurrentSlimeState.FrictionCeiling),
                -slime.CurrentSlimeState.MaxSpeed,
                slime.CurrentSlimeState.MaxSpeed);
        }
        else
        {
            _move_vector.x = Mathf.Sign(_move_vector.x) * Mathf.Lerp(Mathf.Abs(_move_vector.x), 0, slime.CurrentSlimeState.FrictionCeiling);
        }
        slime.Move = _move_vector;
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
            case Side.Left://sticks to a left wall
                slime.EnterMoveState(MoveState.Wall);
                break;
            case Side.Right://sticks to a right wall
                slime.EnterMoveState(MoveState.Wall);
                break;
        }
    }
}
