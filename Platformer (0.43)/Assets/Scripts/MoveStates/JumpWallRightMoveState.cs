using UnityEngine;

public class JumpWallRightMoveState : JumpGroundMoveState
{
    const float c_allowBounceTime = 0.2f;

    public override void EnterState(SlimeController slime)
    {
        //Debug.Log("Entered Jump Wall Right State!");

        exitTimer = 0f;

        slime.rb.gravityScale = slime.CurrentSlimeState.SpaceGravityMod;
        slime.rb.AddForce(-new Vector2(slime.rb.velocity.x, 0) + Vector2.right * slime.CurrentSlimeState.WallJumpPower);

        slime.ChangeDirectionDisplay(Direction.WallJumpRight);
        if (slime.WaterSides.FlagCount > 1)
        {
            slime.WaterBubblesBurst.Play();
        }
        //slime.face.localPosition = new Vector2(0.1f, 0);
    }

    public override void UpdateState(SlimeController slime)
    {
        exitTimer += Time.deltaTime;

        if (exitTimer > c_exitStateTime)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                slime.EnterMoveState(MoveState.JumpAir);
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                slime.EnterMoveState(MoveState.Bounce);
            }
            else
            {
                slime.EnterMoveState(MoveState.Air);//inAir
            }
        }
        else if (exitTimer < c_allowBounceTime)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                slime.EnterMoveState(MoveState.Bounce);
                return;
            }
        }
    }

    public override void FixedUpdateState(SlimeController slime, Vector2 move_vector)
    {
    }

    public override void ReactToGround(SideData touching_side, SlimeController slime)
    {
        switch (touching_side.Flags)
        {
            //case Sides.Bottom://sticks to the ground
            //    if (Input.GetKey(KeyCode.Space))
            //    {
            //        slime.EnterMoveState(MoveState.Jump);
            //    }
            //    else
            //    {
            //        slime.EnterMoveState(MoveState.Ground);
            //    }
            //    break;
            //case Sides.Top://sticks to the ceiling
            //    if (Input.GetKey(KeyCode.Space))
            //    {
            //        slime.EnterMoveState(MoveState.JumpDown);
            //    }
            //    else
            //    {
            //        slime.EnterMoveState(MoveState.Ceiling);
            //    }
            //    break;
            case Sides.Right://jumps off left
                if (Input.GetKey(KeyCode.Space))
                {
                    slime.EnterMoveState(MoveState.JumpWallLeft);
                }
                else
                {
                    slime.EnterMoveState(MoveState.Wall);
                }
                break;
        }
    }
}
