using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingMoveState : SlimeMoveState
{
    Vector2 _move_vector;

    public override void EnterState(SlimeController slime)
    {
        //Debug.Log("Entered Ceiling State!");
        _move_vector = slime.Move;
        slime.rb.gravityScale = -10f * Mathf.Abs(slime.CurrentSlimeState.SpaceGravityMod);
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
        if (Input.GetKey(KeyCode.Space))
        {
            slime.EnterMoveState(MoveState.JumpDown);
            return;
        }
        moveX = 0;
        if (Input.GetKey(KeyCode.D))
        {
            IsControlled = true;
            moveX++;
            slime.ChangeDirectionDisplay(Direction.CeilingRight);
        }
        if (Input.GetKey(KeyCode.A))
        {
            IsControlled = true;
            moveX--;
            slime.ChangeDirectionDisplay(Direction.CeilingLeft);
        }
        if (moveX == 0)
        {
            IsControlled = false;
            slime.ChangeDirectionDisplay(Direction.Ceiling);
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

    public override void ReactToGround(SideData touching_side, SlimeController slime)
    {
        switch (touching_side.Flags)
        {
            case Sides.None://falling or Water
                slime.Move.y = 0;
                slime.rb.velocity = new Vector2(slime.rb.velocity.x, 0f);
                slime.EnterMoveState(MoveState.Air);
                break;
            case Sides.Top | Sides.Right:
                if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.W))
                {
                    slime.EnterMoveState(MoveState.WallRightCeiling);
                }
                break;
            case Sides.Top | Sides.Left:
                if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W))
                {
                    slime.EnterMoveState(MoveState.WallLeftCeiling);
                }
                break;
            case Sides.Bottom://sticks to the ceiling
                slime.EnterMoveState(MoveState.Ground);
                break;
            case Sides.Left://sticks to a left wall
                slime.EnterMoveState(MoveState.Wall);
                break;
            case Sides.Right://sticks to a right wall
                slime.EnterMoveState(MoveState.Wall);
                break;
        }
    }
}
