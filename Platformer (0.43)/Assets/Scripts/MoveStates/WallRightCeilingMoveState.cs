using UnityEngine;

public class WallRightCeilingMoveState : WallRightGroundMoveState
{
    public override void EnterState(SlimeController slime)
    {
        //Debug.Log("Entered WallRightCeiling Move State!");

        slime.rb.gravityScale = -1f * slime.CurrentSlimeState.SpaceGravityMod;
        slime.ChangeDirectionDisplay(Direction.CeilingWallRight);
    }
    public override void UpdateState(SlimeController slime)
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W))
        {
            //switch to Ground
            slime.EnterMoveState(MoveState.Ceiling);
            return;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            //switch to WallLeft
            slime.EnterMoveState(MoveState.Wall);
            return;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            slime.EnterMoveState(MoveState.JumpDownLeft);
        }
        
    }

    public override void FixedUpdateState(SlimeController slime, Vector2 move_vector)
    {

    }

    public override void ReactToGround(SideData side, SlimeController slime)
    {

    }
}
