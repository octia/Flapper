using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Range(0, 5)] public float gravityRate = 1f;
    [Range(0, 3)] public float jumpPower = 1f;
    [Range(0, 3)] public float maxSpeed = 1f;
    [Space]
    [Range(0, 1)] public float DoubletapSpeed = 0.1f; //Time window in which second tap is turned from a jump to a bomb.

    public float bombCount = 0;
    public float bombMax = 3;

    private float lastGravity = 0.0f;
    private float velocityMod = 10f;

    private Rigidbody2D rb;


    public void Jump()
    {
        rb.velocity = new Vector2(0, jumpPower * velocityMod);
    }

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // If gravityRate is changed in the inspector, update gravity in Rigidbody.
        if (gravityRate != lastGravity)
        {
            SetGravity();
        }

        
    }




    void SetGravity()
    {
        rb.gravityScale = gravityRate;
        lastGravity = gravityRate;
    }
}
