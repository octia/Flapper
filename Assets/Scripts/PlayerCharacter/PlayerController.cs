using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject gameManagerGO;
    [Space]
    [Range(0, 5)]
    [SerializeField] private float gravityRate = 1f;
    [Range(0, 3)]
    [SerializeField] private float jumpPower = 1f;
    [Range(0, 3)]
    [SerializeField] private float maxSpeed = 1f;
    [Space]
    [Range(0, 1)]
    [SerializeField] private float DoubletapSpeed = 0.1f; //Time window in which second tap is turned from a jump to a bomb.


    private float lastGravity = 0.0f;
    private float velocityMod = 10f;
    private bool isAlive = true;
    private bool canJump = true;
    private bool isTouched = false;
    private bool readyToBomb = false;

    private Rigidbody2D rb;
    private GamestateManager gameState;


    public void Jump()
    {
        if (readyToBomb)
        {
            gameState.UseBomb();
        }
        else
        {
            if (canJump && isAlive)
            {
                StartCoroutine(JumpCoroutine());
                StartCoroutine(PrepareBomb());
            }
        }
        
    }

    public void Resurrect()
    {
        canJump = true;
        Jump();
        isAlive = true;

    }

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        gameState = gameManagerGO.GetComponent<GamestateManager>();
        Jump();
    }

    private void Update()
    {
        // If gravityRate is changed in the inspector, update gravity in Rigidbody.
        if (gravityRate != lastGravity)
        {
            SetGravity();
        }

        if (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 && !isTouched))
        {
            if (canJump)
            {
                Jump();
            }
            
        }

        isTouched = Input.touchCount > 0 ? true : false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAlive)
        {
            switch (collision.transform.tag)
            {
                case "Death":
                    Die();
                    break;
                case "Reward":
                    gameState.AddPoint();
                    break;
                default:
                    break;
            }

        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Due to a bug, this check has to be in OnTriggerStay instead of OnTriggerEnter
        if (collision.transform.tag == "Wall")
        {
            if (rb.velocity.y > 0) // Passing through the top of the screen is blocked.
            {
                rb.velocity = Vector2.zero;
                canJump = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canJump = true;
    }

    private IEnumerator JumpCoroutine() // Due to a bug, jumping has to be applied after a fixedUpdate instead of immediately on press.
    {
        yield return new WaitForFixedUpdate();
        rb.velocity = Vector2.up * jumpPower * velocityMod;
    }

    private IEnumerator PrepareBomb()
    {
        readyToBomb = true;
        yield return new WaitForSeconds(DoubletapSpeed);
        readyToBomb = false;
    }

    private void Die()
    {
        canJump = true;
        Jump();
        isAlive = false;
        gameState.Die();
    }

    private void SetGravity()
    {
        rb.gravityScale = gravityRate;
        lastGravity = gravityRate;
    }
}
