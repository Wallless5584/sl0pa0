using UnityEngine;

public class WallRightGroundMoveState : SlimeMoveState
{
    public override void EnterState(SlimeController slime)
    {
        //Debug.Log("Entered WallRightGround Move State!");

        slime.rb.gravityScale = 10f * slime.CurrentSlimeState.SpaceGravityMod;
        slime.ChangeDirectionDisplay(Direction.GroundWallRight);
    }
    public override void UpdateState(SlimeController slime)
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S))
        {
            //switch to Ground
            slime.EnterMoveState(MoveState.Ground);
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
            slime.EnterMoveState(MoveState.JumpUpLeft);
        }
        
    }

    public override void FixedUpdateState(SlimeController slime, Vector2 move_vector)
    {

    }

    public override void ReactToGround(SideData side, SlimeController slime)
    {

    }
}
