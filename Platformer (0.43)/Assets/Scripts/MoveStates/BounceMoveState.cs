using System;
using UnityEngine;

public class BounceMoveState : SlimeMoveState
{
    Vector2 move;
    Vector2 reflectMove;
    float bounceTime;
    float groundTime;
    const float c_maxGroundTime = 0.1f;

    public override void EnterState(SlimeController slime)
    {
        Debug.Log("Entered Bounce State!");
        reflectMove = Vector2.zero;
        groundTime = 0;
        slime.SlimeCollider.enabled = false;
        slime.SlimeCollider.enabled = true;
        slime.rb.gravityScale = 1f * slime.CurrentSlimeState.SpaceBounceGravityMod;
        slime.ChangeDirection(Direction.InAirRight);
    }

    public override void UpdateState(SlimeController slime)
    {
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            //LeaveBounceState();
            slime.SlimeCollider.enabled = false;
            //slime.rb.sharedMaterial.friction = 0f;
            slime.SlimeCollider.enabled = true;
            slime.EnterMoveState(MoveState.Air);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //LeaveBounceState
            slime.SlimeCollider.enabled = false;
            //slime.rb.sharedMaterial.friction = 0f;
            slime.SlimeCollider.enabled = true;
            slime.EnterMoveState(MoveState.JumpAir);
            return;
        }
        bounceTime += Time.deltaTime;


        //void LeaveBounceState()
        //{
        //    slime.SlimeCollider.enabled = false;
        //    slime.SlimeCollider.enabled = true;
        //}
    }

    

    public override void FixedUpdateState(SlimeController slime, Vector2 move_vector)
    {
        if (slime.CurrentSlimeState.CurrentSpace != Space.Water)
        {
            move = move_vector;
            if (reflectMove != Vector2.zero)
            {
                slime.Move = reflectMove;
                slime.rb.velocity = reflectMove;
                reflectMove = Vector2.zero;
            }
            if (slime.CurrentSides != Sides.None)
            {
                groundTime += Time.fixedDeltaTime;
            }
            else
            {
                groundTime = 0;
            }
            if (groundTime > c_maxGroundTime)
            {
                move.x = Mathf.Sign(move.x) * Mathf.Lerp(Mathf.Abs(move.x), 0, slime.CurrentSlimeState.FrictionBounce * Time.fixedDeltaTime);
                slime.Move = reflectMove;
            }
        }
    }

    public override void OnCollisionEnterState(SlimeController slime, Vector2 normal)
    {
        //Debug.Log($"Collided with {CurrentSide}!");
        SlimeSoundManager.Instance.PlaySound(SoundType.Bounce);
        Vector2 velocity = move;
        Vector2 delta_direction = Vector2.zero;
        string debug_text = "Slime BOUNCED!";
        delta_direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (Vector2.Angle(velocity, delta_direction) < 5 || Vector2.Angle(velocity, delta_direction) > 175 || bounceTime < 0.1f)
        {
            delta_direction = Vector2.zero;
        }
        if (delta_direction != Vector2.zero)
        {
            slime.DirChangeParticles.Play();
            delta_direction = delta_direction.normalized;
        }
        reflectMove = Vector2.Reflect((velocity.normalized + delta_direction/5).normalized * velocity.magnitude * 0.99f, normal);
        Debug.Log($"{debug_text}, Velocity (norm): {velocity.normalized}, Delta dir: {delta_direction}, Normal: {normal}");
        bounceTime = 0;
    }

    public override void ReactToGround(Sides side, SlimeController slime)
    {
        slime.rb.gravityScale = 1f * slime.CurrentSlimeState.SpaceBounceGravityMod;
        //slime.rb.sharedMaterial.friction = slime.CurrentSlimeState.FrictionBounce;
    }
}
