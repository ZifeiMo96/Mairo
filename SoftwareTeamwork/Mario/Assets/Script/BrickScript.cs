using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickScript : MonoBehaviour
{
    // Start is called before the first frame update

    public Rigidbody2D brickItem1;
    public Rigidbody2D brickItem2;
    public Rigidbody2D brickItem3;
    public Rigidbody2D brickItem4;

    public BoxCollider2D coll;

    public PlayerControl player;

    public LayerMask playerLayer;

    public float speed;



    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckCollided();
    }

    private void CheckCollided()
    {
        PlayerControl p;
        RaycastHit2D leftCheck = Raycast(new Vector2((-coll.size.x / 2), -coll.size.y / 2), Vector2.down, 0.2f, playerLayer);
        RaycastHit2D rightCheck = Raycast(new Vector2((coll.size.x / 2), -coll.size.y / 2), Vector2.down, 0.2f, playerLayer);
        RaycastHit2D middleCheck = Raycast(new Vector2(0f, -coll.size.y / 2), Vector2.down, 0.2f, playerLayer);
        if ((leftCheck || rightCheck ||middleCheck)&&player.isLevelUp && !player.CheckFall())
        {
            
            BrokeUp();
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

    private void BrokeUp()
    {
        if (gameObject.activeSelf) {
            player.BrokenAudioPlay();
            this.gameObject.SetActive(false);
            brickItem1.gameObject.SetActive(true);
            brickItem2.gameObject.SetActive(true);
            brickItem3.gameObject.SetActive(true);
            brickItem4.gameObject.SetActive(true);
            brickItem1.AddForce(new Vector2(-speed, speed), ForceMode2D.Impulse);
            brickItem2.AddForce(new Vector2(speed, speed), ForceMode2D.Impulse);
            brickItem3.AddForce(new Vector2(-speed, -speed), ForceMode2D.Impulse);
            brickItem4.AddForce(new Vector2(speed, -speed), ForceMode2D.Impulse);
        }

    }
}
