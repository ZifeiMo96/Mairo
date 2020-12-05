using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    
    
    private Rigidbody2D rb;

    private BoxCollider2D coll;

    [Header(header: "移动参数")]

    public float speed = 8f;

    public float crouchSpeedDivisor = 3f;

    [Header(header: "跳跃参数")]

    public float jumpForce ;

    public float jumpHoldForce ;

    public float jumpHoldDuration;

    public float crouchJumpBoost;

    float jumpTime;

    [Header(header: "无敌参数")]

    public float flashTime;

    [Header(header: "其他参数")]

    public int coin;

    [Header(header: "状态")]

    public bool isCrouch=false;

    public bool isOnGround;

    public bool isJump;

    public bool isJumpEnd;

    public bool isHeadBlocked;

    public bool isLevelUp;

    public bool isFlash;

    public bool isDie;

    public bool isFall;

    [Header(header: "环境检测")]

    public LayerMask groundLayer;

    public LayerMask enemyLayer;

    public float FootOffSet { get { return (coll.size.x*0.5f); } }

    public float headClearance;

    public float groundDistance;

    public float xVelocity;

    [Header(header: "音效")]

    public AudioSource levelUpAudio;

    public AudioSource jumpAudio;

    public AudioSource coinAudio;

    public AudioSource hitAudio;

    public AudioSource brokenAudio;

    //按键设置

    bool jumpPressed;

    bool jumpHeld;

    bool crouchHeld;
    
    //碰撞体尺寸

    Vector2 colliderStandSize;

    Vector2 colliderStandOffSet;

    Vector2 colliderCrouchSize;

    Vector2 colliderCrouchOffSet;

    Vector2 colliderSmallSize;

    Vector2 colliderSmallOffSet;
    
    // Start is called before the first frame update
    void Start()
    {
        

        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();

        colliderSmallSize = coll.size;
        colliderSmallOffSet = coll.offset;
        colliderStandSize = new Vector2(coll.size.x * 6f / 5f, coll.size.y * 2f);
        colliderStandOffSet = new Vector2(coll.offset.x, coll.offset.y * 2f);
        colliderCrouchSize = new Vector2(coll.size.x * 6f / 5f, coll.size.y);
        colliderCrouchOffSet = new Vector2(coll.offset.x, coll.offset.y);

        
    }

    // Update is called once per frame
    void Update()
    {
        jumpPressed = Input.GetButtonDown("Jump");
        jumpHeld = Input.GetButton("Jump");
        crouchHeld = Input.GetButton("Crouch");
    }

    public float getX()
    {
        return transform.position.x;
    }

    private void FixedUpdate()
    {
        if (!isDie)
        {
            PhysicsCheck();
            GroundMovement();
            MidAirMovement();
            Attack();
            Hit();
        }
    }

    void PhysicsCheck()
    {
        RaycastHit2D leftCheck = Raycast(new Vector2(-FootOffSet, 0f), Vector2.down, groundDistance, groundLayer);
        RaycastHit2D rightCheck = Raycast(new Vector2(+FootOffSet, 0f), Vector2.down, groundDistance, groundLayer);
        RaycastHit2D headCheck = Raycast(new Vector2(0f, coll.size.y), Vector2.up, headClearance, groundLayer);
        CheckFall();
        if ((leftCheck || rightCheck)&&CheckFall()&&!isOnGround)
        {
            isOnGround = true;
            isJump = false;
            isFall = false;
            rb.velocity = new Vector3(rb.velocity.x, 0, 0);
        }
        else if(!isOnGround)
        {
            isOnGround = false;
        }
        if (headCheck)
        {
            isHeadBlocked = true;
        }
        else
        {
            isHeadBlocked = false;
        }
    }

    private void Hit()
    {
        if (!isFlash)
        {
            if (isLevelUp)
            {
                RaycastHit2D rightCheck1 = Raycast(new Vector2(FootOffSet, coll.size.y / 4f), Vector2.right, groundDistance, enemyLayer);
                RaycastHit2D rightCheck2 = Raycast(new Vector2(FootOffSet, coll.size.y / 4f * 3), Vector2.right, groundDistance, enemyLayer);
                RaycastHit2D leftCheck1 = Raycast(new Vector2(-FootOffSet, coll.size.y / 4f), Vector2.left, groundDistance, enemyLayer);
                RaycastHit2D leftCheck2 = Raycast(new Vector2(-FootOffSet, coll.size.y / 4f * 3), Vector2.left, groundDistance, enemyLayer);
                if (rightCheck1 || rightCheck2 || leftCheck1 || leftCheck2)
                {
                    LevelDown();
                }

            }
            else
            {
                RaycastHit2D rightCheck = Raycast(new Vector2(+FootOffSet, coll.size.y / 2f), Vector2.right, groundDistance, enemyLayer);
                RaycastHit2D leftCheck1 = Raycast(new Vector2(-FootOffSet, coll.size.y / 2f), Vector2.left, groundDistance, enemyLayer);

                if (rightCheck || leftCheck1)
                {
                    Die();
                }
            }
        }
     
    }

    private void Attack()
    {
        
        RaycastHit2D leftCheck = Raycast(new Vector2(-FootOffSet, 0f), Vector2.down, groundDistance, enemyLayer);
        RaycastHit2D rightCheck = Raycast(new Vector2(+FootOffSet, 0f), Vector2.down, groundDistance, enemyLayer);
        RaycastHit2D middleCheck = Raycast(new Vector2(0f, 0f), Vector2.down, groundDistance, enemyLayer);
        if (leftCheck)
        {
            rb.AddForce(new Vector2(0f, jumpForce ), ForceMode2D.Impulse);
            isJump = true;
            isOnGround = false;
            leftCheck.transform.SendMessage("Die");
            hitAudio.Play();
            return;            
        }
        else if (rightCheck)
        {
            rb.AddForce(new Vector2(0f, jumpForce ), ForceMode2D.Impulse);
            isJump = true;
            isOnGround = false;
            rightCheck.transform.SendMessage("Die");
            hitAudio.Play();
            return;
        }
        else if (middleCheck)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isJump = true;
            isOnGround = false;
            middleCheck.transform.SendMessage("Die");
            hitAudio.Play();
            return;
        }
    }

    private void GroundMovement()
    {
        if (crouchHeld&&!isCrouch&&isOnGround&&isLevelUp)
        {
            Crouch();
        }else if (!crouchHeld && isCrouch&&!isHeadBlocked&&isLevelUp)
        {
            StandUp();
        }else if (!isOnGround && isCrouch&&isLevelUp)
        {
            StandUp();
        }
        xVelocity = Input.GetAxis("Horizontal");

        if (isCrouch)
        {
            xVelocity /= crouchSpeedDivisor;
        }

        rb.velocity = new Vector2(xVelocity * speed , rb.velocity.y);

        FilpDirection();
    }

    private void FilpDirection()
    {
        if (xVelocity < 0)
        {
            transform.localScale = new Vector3(-1,1,transform.localScale.z);
        }
        else if (xVelocity > 0)
        {
            transform.localScale = new Vector3(1, 1, transform.localScale.z);
        }
    }

    private void MidAirMovement()
    {
        if (jumpPressed && isOnGround && !isJump&&!isHeadBlocked)
        {
            if (isCrouch&&isLevelUp)
            {
                StandUp();
                rb.AddForce(new Vector2(0f, crouchJumpBoost ), ForceMode2D.Impulse);
            }
            
            isOnGround = false;
            isJump = true;
            isJumpEnd = false;
            JumpAudioPlay();

            jumpTime = Time.time + jumpHoldDuration;
            rb.AddForce(new Vector2(0f, jumpForce ), ForceMode2D.Impulse);
        }
        else if(isJump&&!isJumpEnd)
        {
            if (jumpHeld)
            {
                rb.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);
            }
            if (jumpTime < Time.time)
            {

                isJumpEnd = true ; 
            }
        }
    }

    private void Crouch()
    {
        isCrouch = true;
        coll.size = colliderCrouchSize;
        coll.offset = colliderCrouchOffSet;
    }

    private void StandUp()
    {
        isCrouch = false;
        coll.size = colliderStandSize;
        coll.offset = colliderStandOffSet;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Coin")
        {
            Destroy(collision.gameObject);
            AddCoin();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Mushroom")
        {
            if (collision.gameObject.GetComponent<Mushroom>().awake)
            {
                LevelUp();
                Destroy(collision.gameObject);
            }
        }
        if (collision.gameObject.tag == "Bonus" && isHeadBlocked)
        {
            collision.gameObject.tag = "hollow";
        }
        if (collision.gameObject.tag == "Coin")
        {
            coin++;
            Destroy(collision.gameObject);
        }
    }

    private void LevelUp()
    {
        isLevelUp = true;
        coll.size = colliderStandSize;
        coll.offset = colliderStandOffSet;
        levelUpAudio.Play();
    }

    private void LevelDown()
    {
        isLevelUp = false;
        coll.size = colliderSmallSize;
        coll.offset = colliderSmallOffSet;
        Flash();
        Invoke("FlashEnd", flashTime);
    }

    private void Die()
    {
        isDie = true;
        transform.gameObject.layer = 12;
        rb.velocity = new Vector2(0,0);
        rb.velocity = new Vector2(0, 5);
        Invoke("ReSet", 2f);
    }

    private void Flash()
    {
        isFlash = true;
        transform.gameObject.layer = 11;
    }

    private void ReSet()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void FlashEnd()
    {
        isFlash = false;
        transform.gameObject.layer = 0;
    }

    RaycastHit2D Raycast(Vector2 offset,Vector2 rayDiraction,float length,LayerMask layer)
    {
        Vector2 pos = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDiraction, length, layer);

        Color color = hit ? Color.red : Color.green;

        Debug.DrawRay(pos + offset, rayDiraction * length,color);

        return hit;
    }

    public bool CheckFall()
    {
        if(rb.velocity.y <= 0 )
        {
            isFall = true;
            return true;
        }
        else
        {
            return false;
        }

    }

    public void JumpAudioPlay()
    {
        jumpAudio.Play();
    }

    public void AddCoin()
    {
        coin++;
        coinAudio.Play();
    }

    public void BrokenAudioPlay()
    {
        brokenAudio.Play();
    }

}
