using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpDownMoveState : JumpGroundMoveState
{

    public override void EnterState(SlimeController slime)
    {
        exitTimer = 0f;
        //Debug.Log("Entered Jump Down State!");
        slime.rb.gravityScale = slime.CurrentSlimeState.SpaceGravityMod;
        slime.rb.AddForce(Vector2.up * slime.rb.velocity - Vector2.up * slime.CurrentSlimeState.JumpPower);
        slime.ChangeDirectionDisplay(Direction.CeilingJump);
        slime.JumpDownBoostParticles.Play();
        //Debug.Log("Jump Down Particles PLAYED!");
    }

    public override void UpdateState(SlimeController slime)
    {
        exitTimer += Time.deltaTime;
        moveX = 0;

        if (Input.GetKey(KeyCode.D))
        {
            IsControlled = true;
            moveX++;
            //slime.ChangeDirection(Direction.GroundRight);
        }
        if (Input.GetKey(KeyCode.A))
        {
            IsControlled = true;
            moveX--;
            //slime.ChangeDirection(Direction.GroundLeft);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            slime.EnterMoveState(MoveState.Bounce);
        }
        if (moveX == 0)
        {
            IsControlled = false;
        }
        if (exitTimer > c_exitStateTime)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                slime.EnterMoveState(MoveState.JumpAir);
            }
            //else if (Input.GetKey(KeyCode.LeftShift))
            //{
            //    slime.EnterMoveState(MoveState.Bounce);
            //}
            else
            {
                slime.EnterMoveState(MoveState.Air);//inAir
            }
            return;
        }
    }

    public override void ReactToGround(Sides touching_side, SlimeController slime)
    {
        switch (touching_side)
        {
            case Sides.Bottom://sticks to the ground
                if (Input.GetKey(KeyCode.Space))
                {
                    slime.JumpDownBoostParticles.Stop();
                    slime.EnterMoveState(MoveState.Jump);
                }
                ////else if (Input.GetKey(KeyCode.LeftShift))
                //{
                //    slime.EnterMoveState(MoveState.Bounce);
                //}
                else
                {
                    slime.JumpDownBoostParticles.Stop();
                    slime.EnterMoveState(MoveState.Ground);
                }
                break;
            //case Sides.Left://jumps off right
            //    if (Input.GetKey(KeyCode.Space))
            //    {
            //        slime.JumpDownBoostParticles.Stop();
            //        slime.EnterMoveState(MoveState.JumpWallRight);
            //    }
            //    else
            //    {
            //        slime.JumpDownBoostParticles.Stop();
            //        slime.EnterMoveState(MoveState.Wall);
            //    }
            //    break;
            //case Sides.Right://jumps off left
            //    if (Input.GetKey(KeyCode.Space))
            //    {
            //        slime.JumpDownBoostParticles.Stop();
            //        slime.EnterMoveState(MoveState.JumpWallLeft);
            //    }
            //    else
            //    {
            //        slime.JumpDownBoostParticles.Stop();
            //        slime.EnterMoveState(MoveState.Wall);
            //    }
            //    break;
        }
    }
}
