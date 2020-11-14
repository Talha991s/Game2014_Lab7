using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpossumBehaviour : MonoBehaviour
{
    public float runforce;
    public Rigidbody2D rigidbody2D;
    public Transform LookInFrontPoint;
    public Transform LookAheadPoint;
    public LayerMask collisionGroundLayer;
    public LayerMask CollisionWallLayer;
    public bool isGroundAhead;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _LookInFront();
        _LookAhead();
        _Move();
    }

    private void _LookAhead()
    {
        isGroundAhead = Physics2D.Linecast(transform.position, LookAheadPoint.position, collisionGroundLayer);
        Debug.DrawLine(transform.position, LookAheadPoint.position, Color.green);
    }

    private void _LookInFront()
    {
        if (Physics2D.Linecast(transform.position, LookInFrontPoint.position, CollisionWallLayer))
        {
            _flipX();
        }
        
        Debug.DrawLine(transform.position, LookInFrontPoint.position, Color.black);
    }

    private void _Move()
    {
        if(isGroundAhead)
        {
            rigidbody2D.AddForce(Vector2.left * runforce * Time.deltaTime *transform.localScale.x);
            rigidbody2D.velocity *= 0.90f;
        }
        else
        {
            _flipX();
        }

    }
    private void _flipX()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

}
