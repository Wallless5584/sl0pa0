using UnityEngine;

public class WaterGlideMoveState : CeilingMoveState
{
    Vector2 _move_vector;
    const float c_maxMoveSpeedMod = 2f;

    public override void EnterState(SlimeController slime)
    {
        //Debug.Log("Entered ~Water Glide~ State!");
        slime.Move.y = 0f;
        slime.rb.velocity = new Vector2(slime.rb.velocity.x, 0f);
        slime.rb.gravityScale = 0f;
    }

    public override void FixedUpdateState(SlimeController slime, Vector2 move_vector)
    {
        _move_vector = move_vector;
        if (IsControlled)
        {
            _move_vector.x = Mathf.Clamp(
                _move_vector.x + slime.CurrentSlimeState.Acceleration * moveX * (1 - slime.CurrentSlimeState.FrictionCeiling),
                -slime.CurrentSlimeState.MaxSpeedAir * c_maxMoveSpeedMod,
                slime.CurrentSlimeState.MaxSpeedAir * c_maxMoveSpeedMod);
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
            case Sides.None://gliding on Water
                if (slime.CurrentWetnessState == WetnessState.Dry && slime.CheckWaterSurface())
                {
                    slime.Move.y = 0f;
                    break;
                }
                else
                {
                    slime.Move.y = 0;
                    slime.rb.velocity = new Vector2(slime.rb.velocity.x, 0f);
                    slime.EnterMoveState(MoveState.Air);
                }
                break;
            case Sides.Top://sticks to the ceiling
                slime.EnterMoveState(MoveState.Ceiling);
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
