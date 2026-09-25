using UnityEngine;

public class WallLeftGroundMoveState : WallRightGroundMoveState
{
    public override void EnterState(SlimeController slime)
    {
        //Debug.Log("Entered WallLeftGround Move State!");

        slime.rb.gravityScale = 10f * slime.CurrentSlimeState.SpaceGravityMod;
        slime.ChangeDirectionDisplay(Direction.GroundWallLeft);
    }
    public override void UpdateState(SlimeController slime)
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S))
        {
            //switch to Ground
            slime.EnterMoveState(MoveState.Ground);
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
            slime.EnterMoveState(MoveState.JumpUpRight);
        }
        
    }

    public override void FixedUpdateState(SlimeController slime, Vector2 move_vector)
    {

    }

    public override void ReactToGround(SideData side, SlimeController slime)
    {

    }
}
