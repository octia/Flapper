using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject gameManager;
    [Space]
    [Range(0, 5)] public float gravityRate = 1f;
    [Range(0, 3)] public float jumpPower = 1f;
    [Range(0, 3)] public float maxSpeed = 1f;
    [Space]
    [Range(0, 1)] public float DoubletapSpeed = 0.1f; //Time window in which second tap is turned from a jump to a bomb.


    private float lastGravity = 0.0f;
    private float velocityMod = 10f;
    private bool isAlive = true;
    private bool canJump = true;

    private Rigidbody2D rb;
    private GamestateManager gameState;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        gameState = gameManager.GetComponent<GamestateManager>();
    }

    void Update()
    {
        // If gravityRate is changed in the inspector, update gravity in Rigidbody.
        if (gravityRate != lastGravity)
        {
            SetGravity();
        }

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.transform.tag)
        {
            case "Wall":
                if (rb.velocity.y > 0)
                {
                    rb.velocity = new Vector2(0, 0);
                    canJump = false;
                }
                break;
            case "Death":
                transform.position = new Vector2(-5, 0);
                rb.velocity = Vector2.zero;

                break;
            case "Reward":
                gameState.AddPoint();
                break;
            default:
                break;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Wall")
        {
            canJump = true;
        }
    }

    public void Jump()
    {
        if (canJump)
        {
            rb.velocity = new Vector2(0, jumpPower * velocityMod);
        }
    }



    void SetGravity()
    {
        rb.gravityScale = gravityRate;
        lastGravity = gravityRate;
    }
}
