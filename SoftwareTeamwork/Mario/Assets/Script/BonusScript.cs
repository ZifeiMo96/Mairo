
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusScript : MonoBehaviour
{
    // Start is called before the first frame update

    public Mushroom mus;

    public CoinScript coin;

    BoxCollider2D coll;

    public LayerMask playerLayer;

    PlayerControl player;

    public int hitNumber;

    public bool isHollow;

    Animator anim;

    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckCollided();
    }

    private void CheckCollided()
    {
        PlayerControl p = null;
        RaycastHit2D leftCheck = Raycast(new Vector2(-coll.size.x/2, -coll.size.y/2), Vector2.down, 0.1f, playerLayer);
        RaycastHit2D rightCheck = Raycast(new Vector2(+coll.size.x / 2, -coll.size.y / 2), Vector2.down, 0.1f, playerLayer);
        RaycastHit2D middleCheck = Raycast(new Vector2(0f, -coll.size.y / 2), Vector2.down, 0.1f, playerLayer);
        if ((leftCheck || rightCheck || middleCheck)&&!player.CheckFall()&&!isHollow)
        {
            if (coin != null)
            {
                coin.WakeUp();
                player.AddCoin();
            }
            if (mus != null)
                mus.WakeUp();
            hitNumber--;
            if (hitNumber <= 0)
            {
                isHollow = true;
                anim.SetBool("isHollow", isHollow);
            }
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

}
