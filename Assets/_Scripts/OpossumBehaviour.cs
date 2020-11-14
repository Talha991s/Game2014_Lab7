using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RampDirection
{
    NONE,
    UP,
    DOWN
}


public class OpossumBehaviour : MonoBehaviour
{
    public float runforce;
    public Rigidbody2D rigidbody2D;
    public Transform LookInFrontPoint;
    public Transform LookAheadPoint;
    public LayerMask collisionGroundLayer;
    public LayerMask CollisionWallLayer;
    public bool isGroundAhead;
    public bool OnRampHit;
    public RampDirection rampDir;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        rampDir = RampDirection.NONE;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _LookInFront();
        _LookAhead();
        _Move();
    }

    //looking ahead
    private void _LookAhead()
    {
        var GroundHit = Physics2D.Linecast(transform.position, LookAheadPoint.position, collisionGroundLayer);

        if (GroundHit)
        {
            if(GroundHit.collider.CompareTag("Ramps"))
            {
                OnRampHit = true;
            }

            if(GroundHit.collider.CompareTag("Platform"))
            {
                OnRampHit = false;
            }
            isGroundAhead = true;
        }
        else
        {
            isGroundAhead = false;
        }
        Debug.DrawLine(transform.position, LookAheadPoint.position, Color.green);
    }

    //looking in front 
    private void _LookInFront()
    {
        var wallhit = Physics2D.Linecast(transform.position, LookInFrontPoint.position, CollisionWallLayer);

        if (wallhit)
        {
            if(!wallhit.collider.CompareTag("Ramps"))
            {
                if(!OnRampHit && transform.rotation.z == 0)
                {
                    _flipX();
                }
               
                rampDir = RampDirection.DOWN;
            }
            else
            {
                rampDir = RampDirection.UP;
            }
          
        }
        
        Debug.DrawLine(transform.position, LookInFrontPoint.position, Color.black);
    }

    private void _Move()
    {
        if(isGroundAhead)
        {
            rigidbody2D.AddForce(Vector2.left * runforce * Time.deltaTime *transform.localScale.x);

            if(OnRampHit)
            {
                if(rampDir == RampDirection.UP)
                {
                    rigidbody2D.AddForce(Vector2.up * runforce * 0.5f * Time.deltaTime);
                }
                else
                {
                    rigidbody2D.AddForce(Vector2.down * runforce * 0.5f * Time.deltaTime);
                }
                StartCoroutine(Rotate());
            }
            else 
            {
                StartCoroutine(Normalize());
            }

            rigidbody2D.velocity *= 0.90f;
        }
        else
        {
            _flipX();
        }

    }

    IEnumerator Rotate()
    {
        yield return new WaitForSeconds(0.05f);
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, -26.0f);
    }

    IEnumerator Normalize()
    {
        yield return new WaitForSeconds(0.05f);
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }

    private void _flipX()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

}
