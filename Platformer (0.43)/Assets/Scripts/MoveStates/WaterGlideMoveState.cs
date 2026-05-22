using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGlideMoveState : CeilingMoveState
{
    Vector2 _move_vector;

    public override void EnterState(SlimeController slime)
    {
        Debug.Log("Entered ~Water Glide~ State!");
        slime.Move.y = 0f;
        slime.rb.velocity = new Vector2(slime.rb.velocity.x, 0f);
        slime.rb.gravityScale = 0f;
    }

    public override void ReactToGround(Side touching_side, SlimeController slime)
    {
        switch (touching_side)
        {
            case Side.None://gliding on Water
                //if (slime.CurrentWetnessState == WetnessState.Dry && slime.CheckWaterSurface())
                //{
                //    slime.Move.y = 0f;
                //    break;
                //}
                //else
                //{
                //    slime.Move.y = 0;
                //    slime.rb.velocity = new Vector2(slime.rb.velocity.x, 0f);
                //    slime.EnterMoveState(MoveState.Air);
                //}
                break;
            case Side.Top://sticks to the ceiling
                slime.EnterMoveState(MoveState.Ceiling);
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
