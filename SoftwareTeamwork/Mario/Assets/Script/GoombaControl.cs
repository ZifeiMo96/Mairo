using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaControl : MonoBehaviour
{
    public float speed;

    public int direction;

    public bool isDie;

    public BoxCollider2D coll;

    public Rigidbody2D rb;

    public Animator anim;

    public LayerMask mapLayer;

    public float distance;

    public bool isStart;

    public float dis;

    public PlayerControl player;



    // Start is called before the first frame update

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetDis();
        CheckIsStart();
        if (isStart)
        {
            Move();
            changeDirection();
        }
        
    }

    void Move()
    {
        if (!isDie)
        {
            rb.velocity = new Vector2(direction * speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }
        
    }

    void GetDis()
    {
        dis = Mathf.Abs(player.getX()-transform.position.x);
    }

    void CheckIsStart()
    {
        if(dis < distance)
        {
            isStart = true;
        }
    }



    private void changeDirection()
    {
        RaycastHit2D mapLeftRay = Raycast(new Vector2(-(coll.size.x / 2f - 0.1f), coll.size.y/2f), Vector2.left, 0.2f, mapLayer);
        RaycastHit2D mapRightRay = Raycast(new Vector2((coll.size.x / 2f - 0.1f), coll.size.y / 2f), Vector2.right, 0.2f, mapLayer);

        if (mapLeftRay)
        {
            direction = 1;
        }
        else if (mapRightRay)
        {
            direction = -1;
        }
    }

    private void DestroySelf()
    {
        Destroy(transform.gameObject);
    }

    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDiraction, float length, LayerMask layer)
    {
        Vector2 pos = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDiraction, length, layer);

        Color color = hit ? Color.red : Color.green;

        Debug.DrawRay(pos + offset, rayDiraction * length, color);

        return hit;
    }

    private void Die() 
    {
        isDie = true;
        anim.SetBool("isDie", isDie);
    }
}
