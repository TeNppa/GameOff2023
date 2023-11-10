using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject Highlight;
    public UnityAction<Vector3, float> OnDig;

    private Vector3 lookPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay((Vector2)transform.position, (Vector2)Vector3.left);
    }

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
        if (Input.GetKey(KeyCode.Mouse0))
        {
            OnDig?.Invoke(lookPosition, 1.5f);
        }
    }
    void MouseLook()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var vector =((Vector2)(mousePos - transform.position)).normalized;
        Highlight.transform.position = lookPosition = FloorVector3(transform.position + (Vector3)GetDirection(vector));
    }

    Vector2 GetDirection(Vector2 vector)
    {
        if (vector.x is < 0.33f and > -0.33f)
        {
            return vector.y > 0 ? Vector2.up : Vector2.down;
        }

        if (vector.y is < 0.33f and > -0.33f)
        {
            return vector.x > 0 ? Vector2.right : Vector2.left;
        }

        if (vector.x > 0)
        {
            return vector.y > 0 ? Vector2.up + Vector2.right : Vector2.down + Vector2.right;
        }
        return vector.y > 0 ? Vector2.up + Vector2.left : Vector2.down + Vector2.left;
    }

    Vector3 FloorVector3(Vector3 vector)
    {
        return new Vector3(
            math.floor(vector.x) + 0.5f,
            math.floor(vector.y) + 0.5f,
            math.floor(vector.z) + 0.5f);
    }
}
