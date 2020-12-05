using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    // Start is called before the first frame update

    public Rigidbody2D rb;

    public BoxCollider2D coll;


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
        Judge();
    }
    


    public void WakeUp()
    {
        this.gameObject.SetActive(true);
        rb.velocity = new Vector2(0, 6);
 
    }


    public void Judge()
    {
        if (transform.position.y - pos.y > coll.size.y*1.6)
        {
            Destroy(this.gameObject);
        }
    }

}


