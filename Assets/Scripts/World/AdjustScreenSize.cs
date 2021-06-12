using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Transform))] // This script should only be applied to scripts with transforms, since there are easier ways for RectTransforms
[ExecuteInEditMode]
public class AdjustScreenSize : MonoBehaviour
{
    [Header("Desired size in screenSpace")]
    [Range(0.0f, 2f)]
    public float desiredX = 1;
    [Range(0.0f, 2f)]
    public float desiredY = 1;
    private Vector2 desiredScreenSize;
    private Vector2 desiredWorldSize;

    private SpriteRenderer spriteRend;
    private Sprite sprite;
    private Bounds bounds;
    private Camera mainCam;

    public void Adjust()
    {
        if (!mainCam) // this function is sometimes called in Start() of other gameObjects, this is the easiest way to avoid bugs
        {
            Start(); 
        }
        // transforming desired screensize to worldsize
        desiredScreenSize = new Vector2(desiredX, desiredY) + Vector2.one * 0.5f;
        desiredWorldSize = mainCam.ViewportToWorldPoint(desiredScreenSize);
        desiredWorldSize *= 0.5f; // externs are more useful than size, for localScale manipulation

        float scaleX, scaleY;
        Vector3 newScale;

        // calculating new scales
        scaleX = desiredWorldSize.x / bounds.extents.x;
        scaleY = desiredWorldSize.y / bounds.extents.y;
        newScale = new Vector3(scaleX, scaleY);

        transform.localScale = newScale;

    }

    void Start()
    {
        if (!mainCam)
        {
            // setting up variables
            mainCam = Camera.main;

            if (!mainCam.orthographic) // never too safe!
            {
                Debug.LogWarning("Main camera is not in orthographic mode! Screen size adjustment might not work correctly.\nPlease put camera in orthographic mode, or adjust code for perspective mode.");
            }

            transform.localScale = Vector3.one;
            spriteRend = GetComponent<SpriteRenderer>();
            sprite = spriteRend.sprite;
            bounds = sprite.bounds;
        }
        Adjust();
    }
}
