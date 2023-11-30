using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject Highlight;
    [SerializeField] private Tilemap digTilemap;
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private GameObject torch;
    [SerializeField] private BoxCollider2D torchChecker;
    [SerializeField] private LayerMask digLayer;

    [Header("Movement")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float climbSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldownSeconds;
    [SerializeField] private Vector2 groundCheckBoxSize;
    [SerializeField] private float groundCheckCastDistance;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 climbCheckBoxSize;
    [SerializeField] private float climbCheckCastDistance;

    [Header("Stamina costs")]
    [SerializeField] private int passiveStaminaCost = 1;
    [SerializeField] private int climbStaminaCost = 3;
    [SerializeField] private int jumpStaminaCost = 5;
    [SerializeField] [Range(0,1)]private float staminaPotionReplenish = 0.2f;
    private bool isPassiveStaminaCostActive = true;
    
    [Header("Tools")]
    [SerializeField] private ToolBase startingTool;
    public ToolBase CurrentTool;
    
    public UnityAction<Vector3, float> OnJump;
    public UnityAction<Vector3, float> OnLand; // TODO: Implement.
    public UnityAction<Vector3, float> OnWalk;
    public UnityAction<Vector3, float> OnClimb;
    public UnityAction<Vector3, float> OnDig;
    public UnityAction OnPlaceTorch;
    [HideInInspector] public bool Digging;
    [HideInInspector] public bool isFacingRight = true;
    [HideInInspector] public bool isClimbing = false;
    [HideInInspector] public bool isClimbingMoving = false;
    private Vector3 lookPosition;
    private float horizontal;
    private float vertical;
    private bool shouldJump = false;
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    private bool isJumping = false;
    private bool isGrounded = true;
    private float tickrate = 0.6f;

    // For gizmos
    RaycastHit2D groundHit;

    private void Start()
    {
        InvokeRepeating(nameof(PassiveStaminaDrain), 1, 1);
        InvokeRepeating(nameof(TriggerMovementActions), tickrate, tickrate);
        EquipTool(startingTool);
    }


    private void OnEnable()
    {
        Digging = false;
        isPassiveStaminaCostActive = true;
    }

    private void OnDisable()
    {
        isPassiveStaminaCostActive = false;
    }


    private void Update()
    {
        MouseLook();
        Dig();
        UseTorch();
        UseStaminaPotion();
        Movement();
    }


    private void FixedUpdate()
    {
        Movementphysics();
        isGrounded = IsGrounded();
    }


    private void PassiveStaminaDrain()
    {
        if (!isPassiveStaminaCostActive) return;

        int passiveEnergyCost = passiveStaminaCost;

        if (isClimbing)
        {
            passiveEnergyCost = climbStaminaCost;
        }

        playerInventory.RemoveStamina(passiveEnergyCost);
    }

    private void TriggerMovementActions()
    {
        if (isClimbing)
        {
            OnClimb?.Invoke(transform.position, Math.Max(0, MathF.Abs(vertical)));
        }
        else if (IsGrounded() && rb.velocity.magnitude > 0)
        {
            OnWalk?.Invoke(transform.position, rb.velocity.magnitude);
        }
    }
    
    void Movement()
    {
        // Capture movement
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        // Coyote jumping
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Jumping inputs
        if (Input.GetButtonDown("Jump") && coyoteTimeCounter > 0)
        {
            shouldJump = true;
            coyoteTimeCounter = 0;
            playerAnimator.TriggerJumping();
        }

        // Variable jump height
        if (Input.GetButtonUp("Jump") && isJumping)
        {
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }
            isJumping = false;
        }
    }


    void Movementphysics()
    {
        #region Jumping
        if (shouldJump)
        {
            playerInventory.RemoveStamina(jumpStaminaCost);
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(rb.velocity.x, jumpForce));
            shouldJump = false;
            isJumping = true;
            OnJump?.Invoke(transform.position, jumpForce);
        }
        #endregion

        #region Walking
        rb.velocity = new Vector2(horizontal * walkSpeed, rb.velocity.y);

        if (horizontal != 0)
        {
            playerAnimator.SetIsMoving(true);
        }
        else
        {
            playerAnimator.SetIsMoving(false);
        }
        #endregion

        #region Climbing
        if (CanClimb() && !isGrounded)
        {
            isClimbing = true;
            isClimbingMoving = false;

            if (vertical != 0)
            {
                isClimbingMoving = true;
                rb.velocity = new Vector2(0, climbSpeed * vertical);
            }
            else
            {
                isClimbingMoving = false;
            }
        }
        else
        {
            isClimbing = false;
            isClimbingMoving = false;
        }

        playerAnimator.SetClimbingMoving(isClimbingMoving);
        playerAnimator.SetIsClimbing(isClimbing);
        #endregion

        #region Facing
        if (horizontal > .01f)
        {
            climbCheckCastDistance = Math.Abs(climbCheckCastDistance);
            climbCheckBoxSize = new Vector2(Math.Abs(climbCheckBoxSize.x), climbCheckBoxSize.y);
            isFacingRight = true;
        }

        if (horizontal < -.01f)
        {
            climbCheckCastDistance = -Math.Abs(climbCheckCastDistance);
            climbCheckBoxSize = new Vector2(Math.Abs(climbCheckBoxSize.x), climbCheckBoxSize.y);
            isFacingRight = false;
        }
        #endregion
    }


    public void ActivateRunBoost()
    {
        walkSpeed *= 1.25f;
    }


    public void EquipTool(ToolBase newTool)
    {
        CurrentTool = newTool;
    }


    void UseStaminaPotion()
    {
        if (Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.F))
        {
            if (playerInventory.HasStaminaPotions())
            {
                playerInventory.RemoveStaminaPotion(1);
                playerInventory.AddStamina(Mathf.Floor(playerInventory.GetMaxStamina() * staminaPotionReplenish));
            }
        }
    }


    void UseTorch()
    {
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.T))
        {
            OnPlaceTorch?.Invoke();
            List<GameObject> nearbyTorches = GetNearbyTorches();
            if (nearbyTorches.Count > 0)
            {
                foreach (GameObject torchObj in nearbyTorches)
                {
                    Destroy(torchObj);
                }

                playerInventory.AddTorch(nearbyTorches.Count);
            }
            else if (playerInventory.HasTorches())
            {
                Instantiate(torch, transform.position, Quaternion.identity);
                playerInventory.RemoveTorch(1);
            }
        }
    }


    List<GameObject> GetNearbyTorches()
    {
        List<GameObject> foundTorches = new List<GameObject>();

        Vector2 size = torchChecker.size;
        Vector2 center = (Vector2)torchChecker.transform.position + torchChecker.offset;
        float angle = torchChecker.transform.eulerAngles.z;

        Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, angle);
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.CompareTag("torch"))
            {
                foundTorches.Add(hit.gameObject);
            }
        }

        return foundTorches;
    }


    void Dig()
    {
        if (Digging || isClimbing || !isGrounded)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            playerAnimator.TriggerDigging(CurrentTool.Tier);
            Digging = true;
        }
    }


    // Called from animator to time block breaking with animations
    public void BreakBlock()
    {
        if (playerInventory.HasStamina())
        {
            OnDig?.Invoke(lookPosition, CurrentTool.Damage);
            playerInventory.RemoveStamina(CurrentTool.EnergyConsumption);
        }
    }


    public void EndDig()
    {
        Digging = false;
    }


    void MouseLook()
    {
        if (isClimbing || !isGrounded)
        {
            Highlight.SetActive(false);
            return;
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        Vector3 direction = mousePos - transform.position;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, CurrentTool.Range, digLayer);

        Highlight.SetActive(false);

        if (hit.collider != null)
        {
            Vector3 hitPoint = hit.point;
            hitPoint.x = hit.point.x - 0.01f * hit.normal.x;
            hitPoint.y = hit.point.y - 0.01f * hit.normal.y;
            lookPosition = FloorVector3(hitPoint);

            if (CheckTileAtPosition(lookPosition))
            {
                Highlight.SetActive(true);
                Highlight.transform.position = lookPosition;
            }
        }
    }


    bool CheckTileAtPosition(Vector3 position)
    {
        return digTilemap.GetTile(digTilemap.WorldToCell(position)) != null;
    }


    Vector3 FloorVector3(Vector3 vector)
    {
        return new Vector3(
            math.floor(vector.x) + 0.5f,
            math.floor(vector.y) + 0.5f,
            math.floor(vector.z) + 0.5f
        );
    }


    bool IsGrounded()
    {
        var hit = Physics2D.BoxCast(transform.position - transform.up * groundCheckCastDistance / 2, groundCheckBoxSize, 0, -transform.up, groundCheckCastDistance, groundLayer);
        groundHit = hit;

        return hit;
    }


    bool CanClimb()
    {
        return Physics2D.BoxCast(
            new Vector2(transform.position.x, transform.position.y - .5f),
            climbCheckBoxSize,
            0,
            transform.right,
            climbCheckCastDistance,
            groundLayer);
    }


    private void OnDrawGizmos()
    {
        // REF: IsGrounded
        Gizmos.DrawWireCube(transform.position - transform.up * groundCheckCastDistance, groundCheckBoxSize);
        Gizmos.DrawWireSphere(groundHit.point, .25f);

        // REF: CanClimb
        Gizmos.DrawWireCube(new Vector2(transform.position.x + climbCheckCastDistance, transform.position.y - .5f), climbCheckBoxSize);
    }
}