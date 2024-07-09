using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece3D : MonoBehaviour
{
    Rigidbody rb;

    public float? closestSquareDistance;
    public Vector2Int closestSquarePosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        closestSquareDistance = null;
    }

    private void Update()
    {
        
    }

    public void ActivateRigidbody()
    {
        rb.isKinematic = false;
        transform.SetParent(null);
    }

    public void DeactivateRigidbody()
    {
        rb.isKinematic = true;
        transform.SetParent(Board3D.instance.transform);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Piece3D"))
        {
            ActivateRigidbody();
        }
    }
}
