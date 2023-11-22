using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject Highlight;
    [SerializeField] private Tilemap digTilemap;
    public UnityAction<Vector3, float> OnDig;
    public Tool CurrentTool;
    public bool Digging;
    private Vector3 lookPosition;
    public float diggingDistance = 3;
    public LayerMask digLayer;
    private RaycastHit2D hitTest;


    private void FixedUpdate()
    {
        Movement();
        MouseLook();
        Dig();
    }


    void Movement()
    {
        if (Input.GetKey(KeyCode.D))
            transform.position += Vector3.right * 0.1f;
        if (Input.GetKey(KeyCode.A))
            transform.position += Vector3.left * 0.1f;
        if (Input.GetKey(KeyCode.W))
            transform.position += Vector3.up * 0.1f;
    }


    void Dig()
    {
        if (Digging)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Digging = true;
            OnDig?.Invoke(lookPosition, 1.5f);
        }
    }


    public void EndDig()
    {
        Digging = false;
    }


    void MouseLook()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        Vector3 direction = mousePos - transform.position;

        // Perform the raycast, ignoring the specified layer
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, diggingDistance, digLayer);

        // Draw a debug ray
        // Debug.DrawLine(transform.position, transform.position + direction * diggingDistance, Color.red, 0.1f);

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