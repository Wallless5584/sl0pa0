using UnityEngine;

public class GroundMoveState : SlimeMoveState
{
    //float move_x;
    public override void EnterState(SlimeController slime)
    {
        //Debug.Log("Entered Ground State!");
    }

    public override void UpdateState(SlimeController slime)
    {
        slime.rb.gravityScale = 10f * Mathf.Abs(slime.CurrentSlimeState.SpaceGravityMod);
        if (Input.GetKey(KeyCode.LeftShift))
        {
            IsControlled = false;
            slime.EnterMoveState(MoveState.Bounce);
            return;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            slime.EnterMoveState(MoveState.Jump);
            return;
        }
        moveX = 0;
        if (Input.GetKey(KeyCode.D))
        {
            SlimeSoundManager.Instance.PlaySound(SoundType.Slide);
            IsControlled = true;
            moveX++;
            slime.ChangeDirectionDisplay(Direction.GroundRight);
        }
        if (Input.GetKey(KeyCode.A))
        {
            SlimeSoundManager.Instance.PlaySound(SoundType.Slide);
            IsControlled = true;
            moveX--;
            slime.ChangeDirectionDisplay(Direction.GroundLeft);
        }
        if (moveX == 0)
        {
            SlimeSoundManager.Instance.PlaySound(SoundType.None);
            IsControlled = false;
            slime.ChangeDirectionDisplay(Direction.Ground);
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
            move_vector.x = Mathf.Sign(move_vector.x) * Mathf.Lerp(Mathf.Abs(move_vector.x), 0, slime.CurrentSlimeState.FrictionGround);
        }
        slime.Move = move_vector;
    }

    public override void ReactToGround(SideData touching_side, SlimeController slime)
    {
        switch(touching_side.Flags)
        {
            case Sides.None://falling
                slime.EnterMoveState(MoveState.Air);
                break;
            case Sides.Top://sticks to the ceiling
                slime.EnterMoveState(MoveState.Ceiling);
                break;
            case Sides.Bottom | Sides.Left://diagonal
                if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.S))
                {
                    slime.EnterMoveState(MoveState.WallLeftGround);
                }
                break;
            case Sides.Bottom | Sides.Right://diagonal
                if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S))
                {
                    slime.EnterMoveState(MoveState.WallRightGround);
                }
                break;

            case Sides.Bottom | Sides.Right | Sides.Top://dead end right
                if (!Input.GetKey(KeyCode.A))
                {
                    slime.EnterMoveState(MoveState.Wall);
                }
                break;
            case Sides.Bottom | Sides.Left | Sides.Top://dead end left
                if (!Input.GetKey(KeyCode.D))
                {
                    slime.EnterMoveState(MoveState.Wall);
                }
                break;

            case Sides.Bottom | Sides.Left | Sides.Right://diagonal
                break;
        }
    }
}
