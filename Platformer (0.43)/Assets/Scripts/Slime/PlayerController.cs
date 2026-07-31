using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D col2D;
    [SerializeField] bool IsJumping, IsBouncing, ShouldBounce;
    [SerializeField] float speed, jump_force;
    [SerializeField] int hp;
    [Range(0,10f)][SerializeField] float bounce_factor, non_bounce_factor;
    [Range(0, 10f)][SerializeField] float friction_factor;
    [SerializeField] Vector2 current_velocity;
    [SerializeField] Transform legs_hitbox;
    [SerializeField] LayerMask ground_layer;
    [SerializeField] float max_coyote_time;
    [SerializeField] float jump_ground_value;
    [SerializeField] float stickDistanceMod;
    float coyote_timer;
    //bool coyote_time_active;
    [Space]
    [Header ("Score")]
    public int JumpCount;
    public float TimeCount;
    public int CoinCount;
    public int BounceCount;
    public int RespawnCount;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col2D = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && coyote_timer > 0)
        {
            IsJumping = true;
        }
        ShouldBounce = Input.GetKey(KeyCode.LeftShift);
    }

    void FixedUpdate()
    {
        CheckIsOnGround();
        current_velocity = rb.velocity;
        current_velocity.x = Input.GetAxis("Horizontal") * speed;
        legs_hitbox.transform.localPosition = new Vector3(0, Input.GetAxis("Vertical")*stickDistanceMod);
        //transform.localScale += new Vector3(0, Input.GetAxis("Vertical") * stickDistanceMod/2);
        if (IsJumping)
        {
            Jump();
        }
        ChangeBounce();
        if (!IsBouncing)
        {
            rb.velocity = current_velocity;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin")
        {
            //GameManager.instance.OnButtonPress(collision.gameObject.GetComponentInParent<Coin>());
            CoinCount++;
        }
        if (IsBouncing && collision.tag == "Ground")
        {
            BounceCount++;
        }
    }

    void ChangeBounce()
    {
        if (ShouldBounce != IsBouncing)
        {
            float new_bounciness;
            float new_friction;
            if (ShouldBounce)
            {
                new_bounciness = bounce_factor;
                new_friction = 0;
                rb.gravityScale = 1.1f;
            }
            else
            {
                new_bounciness = non_bounce_factor;
                new_friction = friction_factor;
            }
            col2D.enabled = false;
            rb.sharedMaterial.bounciness = new_bounciness;
            rb.sharedMaterial.friction = new_friction;
            col2D.enabled = true;
            IsBouncing = ShouldBounce;
        }
    }

    void CheckIsOnGround()
    {
        RaycastHit2D hit = Physics2D.BoxCast(legs_hitbox.position, legs_hitbox.lossyScale, 0, Vector2.down, 0, ground_layer);
        coyote_timer -= Time.fixedTime;
        if (hit.collider)
        {
            jump_ground_value = Mathf.Clamp((transform.position.y - hit.point.y) * 10000, -1, 1);
            coyote_timer = max_coyote_time;
            rb.gravityScale = 0;
        }
        else
        {
            jump_ground_value = 0;
            rb.gravityScale = 1.1f;
        }
    }

    void Jump()
    {
        rb.gravityScale = 1.1f;
        current_velocity.y = jump_force * jump_ground_value;
        //Debug.Log("Jumped!");
        IsJumping = false;
        JumpCount++;
    }

    public void RecieveDmg(int dmg = 1)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            Die();
        }
    }

    public void ZeroVelocity()
    {
        rb.velocity = new Vector2(0, 0);
    }

    void Die()
    {
        GameManager.instance.RespawnOnSpawn();
        RespawnCount++;
    } 
}
