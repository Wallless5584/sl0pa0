using UnityEngine;

public class BounceMoveState : SlimeMoveState
{
    Vector2 move;
    Vector2 new_move;
    float bounce_time;

    public override void EnterState(SlimeController slime)
    {
        Debug.Log("Entered Bounce State!");
        new_move = Vector2.zero;
        slime.rb.sharedMaterial.bounciness = 1;
        slime.SlimeCollider.enabled = false;
        slime.SlimeCollider.enabled = true;
        slime.rb.gravityScale = 1f;
        slime.ChangeDirection(Direction.InAirRight);
    }

    public override void UpdateState(SlimeController slime)
    {

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            slime.rb.sharedMaterial.bounciness = 0;
            slime.SlimeCollider.enabled = false;
            slime.SlimeCollider.enabled = true;
            slime.EnterMoveState(MoveState.Air);
            return;
        }
        bounce_time += Time.deltaTime;
    }

    public override void FixedUpdateState(SlimeController slime, Vector2 move_vector)
    {
        move = move_vector;
        if (new_move != Vector2.zero)
        {
            slime.Move = new_move;
            slime.rb.velocity = new_move;
            new_move = Vector2.zero;
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
        if (Vector2.Angle(velocity, delta_direction) < 5 || Vector2.Angle(velocity, delta_direction) > 175 || bounce_time < 0.1f)
        {
            delta_direction = Vector2.zero;
        }
        if (delta_direction != Vector2.zero)
        {
            slime.DirChangeParticles.Play();
            delta_direction = delta_direction.normalized;
        }
        new_move = Vector2.Reflect((velocity.normalized + delta_direction/5).normalized * velocity.magnitude * 0.99f, normal);
        Debug.Log($"{debug_text}, Velocity (norm): {velocity.normalized}, Delta dir: {delta_direction}, Normal: {normal}");
        bounce_time = 0;
    }

    public override void ReactToGround(Side side, SlimeController slime)
    {
        
    }
}
