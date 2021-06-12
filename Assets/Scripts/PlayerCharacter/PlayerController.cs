using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject gameManagerGO;
    [Space]

    [Range(0, 5)]
    [SerializeField] private float gravityRate = 1f; // automatically sets gravity in Rigidbody2D component
    [Range(0, 5)]
    [SerializeField] private float dragRate = 1f; // automatically sets drag in Rigidbody2D component
    [Range(0, 3)]
    [SerializeField] private float jumpPower = 1f;
    [Space]

    // Holding down input while jumping increases jump height
    [Range(0, 2)]
    [SerializeField] private float jumpBoostMaxTime = 0.3f; // Maximum length of boost in realtime
    [Range(0, 3)]
    [SerializeField] private float jumpBoostMaxPower = 2f; //maximum power of boost in realtime, see JumpCoroutine for implementation details
    [Space]

    [Range(0, 1)]
    [SerializeField] private float doubleTapSpeed = 0.1f; // Time window in which second tap is turned from a jump to a bomb.


    private float lastGravity = 0.0f; // used to detect changes to gravity in the editor
    private float lastDrag = 0.0f; // same as above, for drag
    private float velocityMod = 10f; // value multiplied by jumpPower when applying force or velocity, for convenience
    
    private bool isAlive = true;
    private bool canJump = true;
    private bool isTouched = false;
    private bool readyToBomb = false;
    
    private Rigidbody2D rb;
    private GamestateManager gameState;


    public void Jump()
    {
        if (readyToBomb) // See IEnumerator PrepareBomb() for explanation
        {
            gameState.UseBomb();
        }
        else
        {
            if (canJump && isAlive)
            {
                StartCoroutine(JumpCoroutine());
                StartCoroutine(PrepareBomb()); //readyToBomb is true for doubleTapSpeed seconds realitme
            }
        }
        
    }

    private IEnumerator JumpCoroutine()
    {
        float jumpBoost = jumpBoostMaxPower;
        float boostTimeLeft = jumpBoostMaxTime;

        rb.velocity = Vector2.up * jumpPower * velocityMod; // initially setting velocity to cancel out current speed

        while (boostTimeLeft > 0 && isTouched)
        {
            yield return new WaitForFixedUpdate(); // Due to a bug, jumping has to be applied after a fixedUpdate instead of immediately on press.
            rb.AddForce(Vector2.up * jumpBoost * velocityMod, ForceMode2D.Force);
            jumpBoost = Mathf.Lerp(0, jumpBoostMaxPower, boostTimeLeft / jumpBoostMaxTime); // linear decrease of boost power in time
            boostTimeLeft -= Time.fixedDeltaTime;
        }
    }

    private IEnumerator PrepareBomb()
    {
        if (!readyToBomb)
        {
            readyToBomb = true;
            yield return new WaitForSeconds(doubleTapSpeed);
            readyToBomb = false;
        }
    }

    private void Die()
    {
        canJump = true;
        readyToBomb = false;
        Jump();
        isAlive = false;
        gameState.Die();
    }

    private void SetPhysicParams()
    {
        rb.gravityScale = gravityRate;
        lastGravity = gravityRate;
        rb.drag = dragRate;
        lastDrag = dragRate;
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
        if (gravityRate != lastGravity || dragRate != lastDrag)
        {
            SetPhysicParams();
        }

        bool jumpInput = (Input.GetKey(KeyCode.Space) || Input.touchCount > 0);

        if (jumpInput && !isTouched)
        {
            isTouched = true;
            if (canJump)
            {
                Jump();
            }
        }
        else
        {
            isTouched = jumpInput;
        }

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
            canJump = false;
            if (rb.velocity.y > 0) 
            {
                rb.velocity = Vector2.zero; // Passing through the top of the screen is blocked
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Wall")
        {
            canJump = true;
        }
        
    }


}
