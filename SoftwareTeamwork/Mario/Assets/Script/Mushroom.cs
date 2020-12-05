using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed;

    public int direction;

    public Rigidbody2D rb;

    public BoxCollider2D coll;

    public LayerMask mapLayer;

    public bool awake;

    public Vector3 pos;



    void Start()
    {
        pos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    
    // Update is called once per frame
    void FixedUpdate()
    {
        if(!awake)
        Judge();
        if (awake) 
        { 
            
            Move();
            changeDirection();
        }
        
    }

    void Move()
    {

        rb.velocity = new Vector2(direction * speed , rb.velocity.y);
    }

    private void changeDirection()
    {
        RaycastHit2D mapLeftRay = Raycast(new Vector2(-(coll.size.x / 2f ), 0), Vector2.left, 0.2f, mapLayer);
        RaycastHit2D mapRightRay = Raycast(new Vector2((coll.size.x / 2f ), 0), Vector2.right, 0.2f, mapLayer);


        if (mapLeftRay)
        {
            direction = 1;
        }
        else if (mapRightRay)
        {
            direction = -1;
        }
    }

    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDiraction, float length, LayerMask layer)
    {
        Vector2 pos = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDiraction, length, layer);

        Color color = hit ? Color.red : Color.green;

        Debug.DrawRay(pos + offset, rayDiraction * length, color);

        return hit;
    }


    public void WakeUp()
    {
        this.gameObject.SetActive(true);
        rb.velocity = new Vector2(0, 6 );
    }

    
    public  void Judge()
    {
        if (transform.position.y - pos.y > coll.size.y*1.5)
        {
            this.gameObject.layer = 9;
            awake = true;
        }


    }
   
}
