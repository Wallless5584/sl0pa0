//using System.Text;
//using System;
//using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
//using UnityEngine.Events;

public class BounceMoveState : SlimeMoveState
{
    Vector2 move;
    Vector2 reflectMove;
    Vector2 normal = Vector2.zero;
    Vector2 beforeReflectMove;
    float bounceTime;
    float groundTime;


    const float c_maxGroundTime = 0.1f;
    const float c_minBounceAngle = 5;
    const float c_maxBounceAngle = 180 - c_minBounceAngle;
    const float c_shadowBounceProtectionTime = 0.0001f;
    const float c_reflectVelocityModifier = 0.99f;
    const float c_reflectControlModifier = 0.2f;

    public override void EnterState(SlimeController slime)
    {
        //Debug.Log("Entered Bounce State!");
        reflectMove = Vector2.zero;
        normal = Vector2.zero;
        groundTime = 0;
        slime.rb.gravityScale = slime.CurrentSlimeState.SpaceBounceGravityMod;
        slime.ChangeDirectionDisplay(Direction.Bounce);
    }

    public override void UpdateState(SlimeController slime)
    {
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            slime.EnterMoveState(MoveState.Air);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            slime.EnterMoveState(MoveState.JumpAir);
            return;
        }
        bounceTime += Time.deltaTime;
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
            if (slime.CurrentSides > Sides.None)
            {
                groundTime += Time.fixedDeltaTime;

                if (groundTime > c_maxGroundTime)
                {
                    move.x = Mathf.Sign(move.x) * Mathf.Lerp(Mathf.Abs(move.x), 0, slime.CurrentSlimeState.FrictionBounce);
                    slime.Move = move;
                    slime.rb.velocity = move;
                }
            }
            else
            {
                groundTime = 0;
            }
        }
    }

    public override void OnCollisionEnter(SlimeController slime, Vector2 normal)
    {
        //Debug.Log($"Collided with {CurrentSide}!");
        
    }

    public override void ReactToGround(SideData side, SlimeController slime)
    {
        slime.rb.gravityScale = slime.CurrentSlimeState.SpaceBounceGravityMod;
        switch (side.Difference)
        {
            case Sides.Bottom:
                normal = new Vector2(0, 1);
                Reflect();
                break;
            case Sides.Top:
                normal = new Vector2(0, -1);
                Reflect();
                break;
            case Sides.Left:
                normal = new Vector2(1, 0);
                Reflect();
                break;
            case Sides.Right:
                normal = new Vector2(-1, 0);
                Reflect();
                break;
            case Sides.Bottom | Sides.Right:
                normal = new Vector2(-0.5f, 0.5f);
                Reflect();
                break;
            case Sides.Bottom | Sides.Left:
                normal = new Vector2(0.5f, 0.5f);
                Reflect();
                break;
            case Sides.Top | Sides.Right:
                normal = new Vector2(-0.5f, -0.5f);
                Reflect();
                break;
            case Sides.Top | Sides.Left:
                normal = new Vector2(0.5f, -0.5f);
                Reflect();
                break;
        }

        void Reflect()
        {
            string debug_text = "Slime BOUNCED!";
            beforeReflectMove = move;
            SlimeSoundManager.Instance.PlaySound(SoundType.Bounce);
            Vector2 delta_direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));


            if (Vector2.Angle(beforeReflectMove, delta_direction) < c_minBounceAngle ||
                Vector2.Angle(beforeReflectMove, delta_direction) > c_maxBounceAngle ||
                bounceTime < c_shadowBounceProtectionTime)
            {
                delta_direction = Vector2.zero;
            }
            if (delta_direction != Vector2.zero)
            {
                slime.DirChangeParticles.Play();
                delta_direction = delta_direction.normalized;
            }
            reflectMove = Vector2.Reflect((beforeReflectMove.normalized + delta_direction * c_reflectControlModifier).normalized * beforeReflectMove.magnitude * c_reflectVelocityModifier, normal);
            Debug.Log($"{debug_text}, Velocity (norm): {beforeReflectMove.normalized}, Delta dir: {delta_direction}, Normal: {normal}");
            bounceTime = 0;
        }
    }
}
