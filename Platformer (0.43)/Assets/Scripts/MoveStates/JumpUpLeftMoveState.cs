using UnityEngine;

public class JumpUpLeftMoveState : JumpUpRightMoveState
{
    public override void EnterState(SlimeController slime)
    {
        //Debug.Log($"{Time.time} | Entered JumpUpLeft Move State");

        exitTimer = 0f;

        slime.rb.gravityScale = slime.CurrentSlimeState.SpaceGravityMod;
        slime.rb.AddForce(new Vector2(-slime.CurrentSlimeState.WallJumpPower, slime.CurrentSlimeState.JumpPower * c_verticalJumpPowerBoost));

        if (slime.WaterSides.FlagCount > 1)
        {
            slime.WaterBubblesBurst.Play();
        }

        SlimeSoundManager.Instance.PlaySound(SoundType.Jump);
    }


    public override void ReactToGround(SideData touching_side, SlimeController slime)
    {
        switch (touching_side.Flags)
        {
            case Sides.Top:
                slime.EnterMoveState(MoveState.Ceiling);
                break;
            case Sides.Left:
                slime.EnterMoveState(MoveState.Wall);
                break;
            case Sides.Left | Sides.Top://diagonal
                slime.EnterMoveState(MoveState.WallLeftCeiling);
                break;

        }
    }
}
