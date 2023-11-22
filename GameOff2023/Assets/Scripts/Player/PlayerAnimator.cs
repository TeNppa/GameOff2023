using UnityEngine;


public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform headlight;
    [SerializeField] private float lightSwapSpeed = 10;
    private float lightTargetZRotation = -90f;
    private Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        HandleSpriteFlipping();
        Example_MoveToPlayerControllerWhenReady();
    }


    private void Example_MoveToPlayerControllerWhenReady()
    {
        if (Input.GetKey(KeyCode.W)) SetIsJumping(true);
        else SetIsJumping(false);

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)) SetIsMoving(true);
        else SetIsMoving(false);

        if (Input.GetMouseButtonDown(0)) animator.SetTrigger("Dig Iron Pick");
    }


    public void SetIsMoving(bool isMoving)
    {
        animator.SetBool("isMoving", isMoving);
    }


    public void SetIsJumping(bool isJumping)
    {
        animator.SetBool("isJumping", isJumping);
    }


    public void TriggerDigging(int toolTier)
    {
        switch (toolTier)
        {
            case 1:
                animator.SetTrigger("Dig Iron Pick");
                break;
            case 2:
                animator.SetTrigger("Dig Gold Pick");
                break;
            case 3:
                animator.SetTrigger("Dig Diamond Pick");
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
    }


    private void HandleSpriteFlipping()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Compare the player's position to the mouse position
        if (mousePosition.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
            lightTargetZRotation = 90f;
        }
        else
        {
            spriteRenderer.flipX = false;
            lightTargetZRotation = -90f;
        }

        // Smoothly rotate the headlight to the target direction
        Quaternion targetRotation = Quaternion.Euler(headlight.rotation.eulerAngles.x, headlight.rotation.eulerAngles.y, lightTargetZRotation);
        headlight.rotation = Quaternion.Lerp(headlight.rotation, targetRotation, Time.deltaTime * lightSwapSpeed);
    }
}