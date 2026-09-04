using UnityEngine;

public class BounceMoveState : SlimeMoveState
{
    Vector2 move;
    Vector2 reflectMove;
    float bounceTime;
    float groundTime;

    const float c_maxGroundTime = 0.1f;
    const float c_minBounceAngle = 5;
    const float c_maxBounceAngle = 180 - c_minBounceAngle;
    const float c_shadowBounceProtectionTime = 0.0001f;
    const float c_reflectVelocityModifier = 0.99f;
    const float c_reflectControlModifier = 0.2f;

    bool shouldReflectNextReactToGround = true;


    public override void EnterState(SlimeController slime)
    {
        //Debug.Log("Entered Bounce State!");
        reflectMove = Vector2.zero;
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
            if (reflectMove != Vector2.zero)
            {
                Debug.Log($"Reflect move applied! Move was:{move} now: {reflectMove}");
                slime.Move = reflectMove;
                slime.rb.velocity = reflectMove;
                move = reflectMove;
                reflectMove = Vector2.zero;
                return;
            }
            else
            {
                //Debug.Log($"Move overwritten to {move}");
                move = move_vector;
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

    public override void OnCollisionEnter(SlimeController slime, Collision2D collision, SideData side)
    {
        Debug.Log("Collision!!!!!!");
        ReflectSlime(slime, side.Flags);
        shouldReflectNextReactToGround = false;
    }

    private void ReflectSlime(SlimeController slime, Sides sides)
    {
        Vector2 delta_direction;
        //slime.rb.gravityScale = slime.CurrentSlimeState.SpaceBounceGravityMod;
        switch (sides)
        {
            case Sides.Bottom:
                delta_direction = Reflect(new Vector2(0, 1));
                if (delta_direction.x > 0)
                {
                    //moving intention right
                    slime.DirChangeParticles.BotLeft.Play();
                }
                else if (delta_direction.x < 0)
                {
                    //moving intention left
                    slime.DirChangeParticles.BotRight.Play();
                }
                else
                {
                    //NO moving intention left
                    slime.BounceParticles.Play();
                }
                break;

            case Sides.Top:
                delta_direction = Reflect(new Vector2(0, -1));
                if (delta_direction.x > 0)
                {
                    //moving intention right
                    slime.DirChangeParticles.TopLeft.Play();
                }
                else if (delta_direction.x < 0)
                {
                    //moving intention left
                    slime.DirChangeParticles.TopRight.Play();
                }
                else
                {
                    //NO moving intention left
                    slime.BounceParticles.Play();
                }
                break;

            case Sides.Left:
                delta_direction = Reflect(new Vector2(1, 0));
                if (delta_direction.y > 0)
                {
                    //moving intention up
                    slime.DirChangeParticles.LeftBottom.Play();
                }
                else if (delta_direction.y < 0)
                {
                    //moving intention down
                    slime.DirChangeParticles.LeftTop.Play();
                }
                else
                {
                    //NO moving intention left
                    slime.BounceParticles.Play();
                }
                break;

            case Sides.Right:
                delta_direction = Reflect(new Vector2(-1, 0));
                if (delta_direction.y > 0)
                {
                    //moving intention up
                    slime.DirChangeParticles.RightBottom.Play();
                }
                else if (delta_direction.y < 0)
                {
                    //moving intention down
                    slime.DirChangeParticles.RightTop.Play();
                }
                else
                {
                    //NO moving intention left
                    slime.BounceParticles.Play();
                }
                break;

            case Sides.Bottom | Sides.Right:
                delta_direction = Reflect(new Vector2(-1f, 1f));
                break;

            case Sides.Bottom | Sides.Left:
                delta_direction = Reflect(new Vector2(1f, 1f));
                break;

            case Sides.Top | Sides.Right:
                delta_direction = Reflect(new Vector2(-1f, -1f));
                break;

            case Sides.Top | Sides.Left:
                delta_direction = Reflect(new Vector2(1f, -1f));
                break;
        }


        Vector2 Reflect(Vector2 normal)
        {
            string debug_text = $"Slime BOUNCED! Move {move},";
            SlimeSoundManager.Instance.PlaySound(SoundType.Bounce);

            delta_direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));


            if (Vector2.Angle(move, delta_direction) < c_minBounceAngle ||
                Vector2.Angle(move, delta_direction) > c_maxBounceAngle ||
                bounceTime < c_shadowBounceProtectionTime)
            {
                delta_direction = Vector2.zero;
            }

            if (delta_direction != Vector2.zero)
            {
                delta_direction = delta_direction.normalized;
            }
            Vector2 before_collision_move = slime.PreviousMove;

            reflectMove = Vector2.Reflect((before_collision_move.normalized + delta_direction * c_reflectControlModifier).normalized * before_collision_move.magnitude * c_reflectVelocityModifier, normal.normalized);
            Debug.Log($"{debug_text} reflect {reflectMove}, Delta dir: {delta_direction}, Normal: {normal}");
            bounceTime = 0;

            return delta_direction;
        }
    }

    public override void ReactToGround(SideData side, SlimeController slime)
    {
        if(side.Difference > 0)
        {
            if (shouldReflectNextReactToGround)
            {
                Debug.Log($"Difference detected: {side.Difference} Reflecting!");
                ReflectSlime(slime, side.Difference);
            }
            else
            {
                Debug.Log($"Difference detected: {side.Difference} but should be ignored.");
            }
            shouldReflectNextReactToGround = true;
        }
    }
}
