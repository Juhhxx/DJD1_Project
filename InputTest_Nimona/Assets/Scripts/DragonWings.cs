using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonWings : MonoBehaviour
{
    // reference variables to change in player
    [SerializeField] private CapsuleCollider2D groundCollider;
    [SerializeField] private BoxCollider2D airCollider;

    [SerializeField] private float jumpSpeed = 200f;
    [SerializeField] private float glideClamp = 400f;
    private float glideRate;
    [SerializeField] private int jumpsAllowed = 3;
    [SerializeField] private float maxGlideTime = 5;
    [SerializeField] private float maxFallSpeed = 20f;

    
    private float glideTimer;
    private int jumpsExecuted;
    private Vector3 newJump;
    private Vector3 currentVelocity;

    // player variables
    private GameObject player;
    private Movement movement;
    private Rigidbody2D rb;

    // Shapeshift points mechanic
    private Shapeshifting shpshift;

    void Start()
    {
        player = transform.parent.gameObject;
        movement = player.GetComponent<Movement>();
        rb = player.GetComponent<Rigidbody2D>();

        jumpsExecuted = 0;

        glideRate = (movement.DefaultMoveClamp / glideClamp) * movement.DefaultMoveRate;
        
        movement.JumpSpeed = jumpSpeed;
    }
    void OnEnable()
    {
        shpshift = GetComponentInParent<Shapeshifting>();
        Debug.Log($"GET SHAPESHIFTING : {shpshift}");
        shpshift.DragonPoints -= 1;
    }
    void Update()
    {
        if ( Input.GetButtonDown("Jump") )
        {
            rb.gravityScale = movement.FallingGravity;
        }
        {
            rb.gravityScale = 4f;
        }

        if (!movement.IsGrounded)
        {
            if ( jumpsExecuted < jumpsAllowed && Input.GetButtonDown("Jump") )
            {
                Debug.Log("has flapped");

                newJump = rb.velocity;
                newJump.y = jumpSpeed;

                rb.velocity = newJump;

                jumpsExecuted++;
            }
            else if ( Input.GetKey(KeyCode.JoystickButton7) && (glideTimer > 0) )
            {
                if ( (movement.MoveClamp != glideClamp) || (movement.MoveRate != glideRate) )
                {
                    movement.MoveClamp = glideClamp;
                    movement.MoveRate = glideRate;
                }
                
                currentVelocity = rb.velocity;
                if (rb.velocity.y < -maxFallSpeed)
                {
                    currentVelocity.y = -maxFallSpeed;
                    rb.velocity = currentVelocity;
                }

                glideTimer -= Time.deltaTime;
            }

            if ( !Input.GetKey(KeyCode.JoystickButton7) )
            {
                if ( (movement.MoveClamp == glideClamp) || (movement.MoveRate == glideRate) )
                {
                    movement.MoveClamp = movement.DefaultMoveClamp;
                    movement.MoveRate = movement.DefaultMoveRate;
                }
            }

            movement.Jumped = false;
        }
        else if (movement.IsGrounded)
        {
            jumpsExecuted = 0;
            glideTimer = maxGlideTime;

            if ( (movement.MoveClamp == glideClamp) || (movement.MoveRate == glideRate) )
            {
                movement.MoveClamp = movement.DefaultMoveClamp;
                movement.MoveRate = movement.DefaultMoveRate;
            }
        }
    }
}
