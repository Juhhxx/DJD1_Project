using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightMovement : MonoBehaviour
{
    [SerializeField] private LayerMask excludeLayersOnDie;

    [SerializeField] private float speed = 100f;
    [SerializeField] private float stopDistance = 128f;
    [SerializeField] private float sightDistance = 256f;
    [SerializeField] private LayerMask collidables;

    private Rigidbody2D rb;
    private Rigidbody2D rbP;
    private GameObject player;

    private Vector2 bufferVelocity;
    private bool dead;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rbP = player.GetComponent<Rigidbody2D>();

        if ( dead && (Mathf.Abs(rb.velocity.magnitude) <  15) )
        {
            Destroy(gameObject);
        }
        else if ( rbP != null && !dead )
        {
            Vector2 dirToPlayer = rbP.position - rb.position;
            dirToPlayer = dirToPlayer.normalized;

            // make sure there are no obstacles like abscesses or walls and that the player is in sight
            Vector3 distance = transform.position;
            distance += new Vector3(stopDistance * Mathf.Sign(dirToPlayer.x), 0f, 0f);
            RaycastHit2D hit;
            hit = Physics2D.Raycast(distance, dirToPlayer, sightDistance, collidables);

            if (( hit.collider != null ))
            {
                if ( hit.transform.tag == "Player" )
                {
                    // Animation
                    if (( dirToPlayer.x < 0 ))
                    {
                        transform.rotation = Quaternion.Euler( 0, 180, 0);
                    }
                    else if (( dirToPlayer.x > 0 ))
                    {
                        transform.rotation = Quaternion.identity;
                    }

                    Vector2 newVelocity = rb.velocity;
                    newVelocity.x = Mathf.Lerp(rb.velocity.x, speed * Mathf.Sign(dirToPlayer.x), 0.3f);
                    rb.velocity = newVelocity;
                }
                
            }

            //update the buffervelocity
            bufferVelocity = rbP.velocity;
        }
    }
    public void DieSequence()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.excludeLayers = excludeLayersOnDie;

        Vector2 bounce = new Vector2(bufferVelocity.x, bufferVelocity.y + 100f * rb.mass);

        rb.AddForce(bounce, ForceMode2D.Impulse);

        rbP.AddForce(bufferVelocity, ForceMode2D.Impulse);

        dead = true;
    }
}
