using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform headlight;
    [SerializeField] private float lightSwapSpeed = 10;
    private float lightTargetZRotation = -90f;


    void Update()
    {
        HandleSpriteFlipping();
    }


    private void HandleSpriteFlipping()
    {
        if (Input.GetKey(KeyCode.A))
        {
            spriteRenderer.flipX = true;
            lightTargetZRotation = 90f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            spriteRenderer.flipX = false;
            lightTargetZRotation = -90f;
        }

        // Smoothly rotate the headlight to the target direction
        Quaternion targetRotation = Quaternion.Euler(headlight.rotation.eulerAngles.x, headlight.rotation.eulerAngles.y, lightTargetZRotation);
        headlight.rotation = Quaternion.Lerp(headlight.rotation, targetRotation, Time.deltaTime * lightSwapSpeed);
    }
}