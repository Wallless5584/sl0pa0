using UnityEngine;

public class FreezeMoveState : SlimeMoveState
{
    public float freezeTimer;

    public override void EnterState(SlimeController slime)
    {
        //Debug.Log("Entered Freeze State!");
    }

    public override void UpdateState(SlimeController slime)
    {
        freezeTimer -= Time.deltaTime;
        if (freezeTimer < 0f)
        {
            slime.EnterMoveState(MoveState.Air);
        }
    }

    public override void FixedUpdateState(SlimeController slime, Vector2 move_vector)
    {
        slime.rb.gravityScale = slime.CurrentSlimeState.SpaceGravityMod;
        move_vector = Vector2.zero;
        slime.Move = move_vector;
    }

    public override void ReactToGround(SideData touching_side, SlimeController slime)
    {
        
    }
}
