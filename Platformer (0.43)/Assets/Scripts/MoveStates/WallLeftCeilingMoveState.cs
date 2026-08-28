using UnityEngine;

public class WallLeftCeilingMoveState : WallRightGroundMoveState
{
    public override void EnterState(SlimeController slime)
    {
        //Debug.Log("Entered WallLeftCeiling Move State!");

        slime.rb.gravityScale = -1f * slime.CurrentSlimeState.SpaceGravityMod;
    }
    public override void UpdateState(SlimeController slime)
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W))
        {
            //switch to Ground
            slime.EnterMoveState(MoveState.Ceiling);
            return;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            //switch to WallLeft
            slime.EnterMoveState(MoveState.Wall);
            return;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            slime.EnterMoveState(MoveState.JumpDownRight);
        }
        
    }

    public override void FixedUpdateState(SlimeController slime, Vector2 move_vector)
    {

    }

    public override void ReactToGround(Sides side, SlimeController slime)
    {

    }
}
