using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform headLight;
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

        // smoothly rotate the headLight to the target dirtection
        Quaternion targetRotation = Quaternion.Euler(headLight.rotation.eulerAngles.x, headLight.rotation.eulerAngles.y, lightTargetZRotation);
        headLight.rotation = Quaternion.Lerp(headLight.rotation, targetRotation, Time.deltaTime * lightSwapSpeed);
    }
}