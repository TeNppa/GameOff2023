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
    public UnityAction<Vector3, float> OnDig;
    public Tool CurrentTool;
    public bool Digging;
    private Vector3 lookPosition;
    public float diggingDistance = 3;
    private float runSpeed = 0.1f;
    private float runSpeedUpgrade = 0.125f;


    private void Start()
    {
        InvokeRepeating("PassiveStaminaDrain", 1, 1);
    }


    private void FixedUpdate()
    {
        Movement();
    }


    private void Update()
    {
        MouseLook();
        Dig();
        UseTorch();
        UseStaminaPotion();
    }


    private void PassiveStaminaDrain()
    {
        if (playerInventory.RemoveStamina(1) == false)
        {
            Debug.Log("Player is out of stamina!");
        }
    }


    void Movement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.up * 0.1f;
            playerAnimator.SetIsJumping(true);
        }
        else playerAnimator.SetIsJumping(false);


        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)) playerAnimator.SetIsMoving(true);
        else playerAnimator.SetIsMoving(false);


        if (Input.GetKey(KeyCode.D))
            transform.position += Vector3.right * runSpeed;

        if (Input.GetKey(KeyCode.A))
            transform.position += Vector3.left * runSpeed;
    }


    public void ActivateRunBoost()
    {
        runSpeed = runSpeedUpgrade;
    }


    void UseStaminaPotion()
    {
        if (Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.F))
        {
            if (playerInventory.HasStaminaPotions())
            {
                playerInventory.RemoveStaminaPotion(1);
                playerInventory.AddStamina(200);
            }
        }
    }


    void UseTorch()
    {
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.T))
        {
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
        if (Digging)
            return;
    
        if (Input.GetMouseButton(0))
        {
            playerAnimator.TriggerDigging(2); // TODO: hard coded iron pick, get tool tier from somewhere
            Digging = true;
        }
    }


    // Called from animator to time block breaking with animations
    public void BreakBlock()
    {
        OnDig?.Invoke(lookPosition, 1.5f);
        playerInventory.RemoveStamina(20);
    }


    public void EndDig()
    {
        Digging = false;
    }


    void MouseLook()
    {
        if (Digging)
            return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        Vector3 direction = mousePos - transform.position;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, diggingDistance, digLayer);

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
}