using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb_player;

    private Animator anim;
    
    private float dirX;
    [SerializeField] private float move_speed=7;
    [SerializeField] private float jump_speed=7;
    private SpriteRenderer sprite_dir;

    private float fallMult = 3f;
    private float lowThreshold = 2f;
    
    private enum MovementStates
    {
        idle, running, jumping, falling
    }

    private int jmp_counter = 0;
    [SerializeField] private int max_jumps = 2;

    // Start is called before the first frame update
    private void Start()
    {
        rb_player = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite_dir = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxis("Horizontal");
        rb_player.velocity = new Vector2(dirX * move_speed, rb_player.velocity.y);

        if (GroundCheck())
        {
            jmp_counter = 0;
        }
        
        // jumping
        bool isGrounded = GroundCheck();

        if (jmp_counter < max_jumps-1 && Input.GetButtonDown("Jump"))
        {
            rb_player.velocity = new Vector2(rb_player.velocity.x, jump_speed);
            jmp_counter++;
            if (rb_player.velocity.y < 0)
            {
                rb_player.velocity += Vector2.up * (Physics2D.gravity.y * (fallMult - 1) * Time.deltaTime);
            } else if (rb_player.velocity.y > 0 && Input.GetButtonDown("Jump"))
            {
                rb_player.velocity += Vector2.up * (Physics2D.gravity.y * (lowThreshold - 1) * Time.deltaTime);
            }
        }
        
        else if (isGrounded)
        {
            jmp_counter = 0;
        }

        UpdateAnimationState();
    }


    private bool GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, LayerMask.GetMask("Ground"));
        if (hit.collider != null)
        {
            return true;
        } else return false;
    }


    
    
    private void UpdateAnimationState()
    { //0 = idle 1 = running 2 = jumping 3 = falling
        MovementStates state;
        
        if (dirX > 0f)
        {
            state = MovementStates.running;
            sprite_dir.flipX = false; //if going right dont flip
        }
        else if (dirX < 0f)
        {
            state = MovementStates.running;
            sprite_dir.flipX = true; //if going left flip (auto-changes)
        }
        else
        {
            state = MovementStates.idle; //keep idle if 0
        }
        
        if (rb_player.velocity.y > .1f) //condition for jump and fall
        {
            state = MovementStates.jumping;
        } else if (rb_player.velocity.y < -.1f)
        {
            state = MovementStates.falling;
        }
        
        anim.SetInteger("state", (int)state);
    }
}
