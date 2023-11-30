using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Transform headlight;
    [SerializeField] private float lightSwapSpeed = 10;
    private float lightTargetZRotation = -90f;
    private Animator animator;
    private float spriteFlipDeadzoneSize = 0.15f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        HandleSpriteFlipping();
    }


    public void SetIsMoving(bool isMoving)
    {
        animator.SetBool("isMoving", isMoving);
    }


    public void SetIsClimbing(bool isClimbing)
    {
        animator.SetBool("isClimbing", isClimbing);
    }


    public void SetClimbingMoving(bool isClimbingMoving)
    {
        animator.SetBool("isClimbingMoving", isClimbingMoving);
    }


    public void TriggerJumping()
    {
        animator.SetTrigger("Jump");
    }


    public void ActivateMiningBoost()
    {
        animator.SetFloat("miningSpeed", 1.5f);
    }


    public void TriggerDigging(int toolTier)
    {
        switch (toolTier)
        {
            case 0:
                animator.SetTrigger("Dig Hands");
                break;
            case 1:
                animator.SetTrigger("Dig Shovel");
                break;
            case 2:
                animator.SetTrigger("Dig Iron Pick");
                break;
            case 3:
                animator.SetTrigger("Dig Gold Pick");
                break;
            case 4:
                animator.SetTrigger("Dig Diamond Pick");
                break;
            case 5:
                animator.SetTrigger("Dig Super Drill");
                break;
            default:
                break;
        }
    }


    // Called within the dig animations as an event
    private void ResetDigging()
    {
        animator.ResetTrigger("Dig Iron Pick");
        animator.ResetTrigger("Dig Gold Pick");
        animator.ResetTrigger("Dig Diamond Pick");
        playerController.BreakBlock();
    }


    private void HandleSpriteFlipping()
    {
        if (playerController.isClimbing)
        {
            // Incase player is climbing, always point to the climb direction
            if (playerController.isFacingRight)
            {
                spriteRenderer.flipX = false;
                lightTargetZRotation = -90f;
            }
            else
            {
                spriteRenderer.flipX = true;
                lightTargetZRotation = 90f;
            }
        }
        else
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Compare the player's position to the mouse position
            var worldDeadzone = transform.right * (spriteFlipDeadzoneSize * (spriteRenderer.flipX ? 1 : -1));
            if (mousePosition.x < (transform.position + worldDeadzone).x)
            {
                spriteRenderer.flipX = true;
                lightTargetZRotation = 90f;
            }
            else
            {
                spriteRenderer.flipX = false;
                lightTargetZRotation = -90f;
            }
        }

        // Smoothly rotate the headlight to the target direction
        Quaternion targetRotation = Quaternion.Euler(headlight.rotation.eulerAngles.x, headlight.rotation.eulerAngles.y, lightTargetZRotation);
        headlight.rotation = Quaternion.Lerp(headlight.rotation, targetRotation, Time.deltaTime * lightSwapSpeed);
    }
}