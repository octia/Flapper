using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PositionScroller : MonoBehaviour
{

    [SerializeField] private GameObject prefabToScroll;

    [Range(0.1f, 20f)]
    [SerializeField] private float scrollingSpeed = 1.0f;

    private GameObject[] objectPool;
    private int poolSize = 3;
    private int toSwap = 0;

    private AdjustScreenSize screenSize;
    private GameObject objToSwap;

    private void Start()
    {
        screenSize = GetComponent<AdjustScreenSize>();

        objectPool = new GameObject[poolSize];
        Vector2 VPoffset = Vector2.left + Vector2.up * 0.5f; // y+=0.5f changes position from bottom left corner to middle left, on screen
        for (int i = 0; i < poolSize; i++)
        {
            // initiating the gameObject, assigning parent and velocity
            objectPool[i] = Instantiate(prefabToScroll);
            objectPool[i].transform.parent = gameObject.transform;

            Rigidbody2D rb = objectPool[i].AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.velocity = Vector2.left * scrollingSpeed;

            // assigning position
            VPoffset += Vector2.right; 
            objectPool[i].transform.position += Camera.main.ViewportToWorldPoint(VPoffset);
        }
    }

    private void Update()
    {
        objToSwap = objectPool[toSwap];

        // Screenspace pos of background center, between 0 and 1 means it is on screen
        Vector2 objPos= Camera.main.WorldToViewportPoint(objToSwap.transform.position, Camera.MonoOrStereoscopicEye.Mono);
        if (objPos.x <= -0.5) // checking if background is fully off-screen, assumes 
        {
            objToSwap.transform.position += Vector3.right * 2 * (-objToSwap.transform.position.x); // moving position from furthest left to right
            toSwap++;
            if (toSwap >= poolSize) // iterating through all objects each time position of one is changed
            {
                toSwap = 0;
            }
        }
    }
}
