using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private string horizontal = "Horizontal";

    private string jump = "Jump";
    private Rigidbody2D rb;

    private Animator anim;

    private float limitPosX = 8.385f;
    private float limitPosY = 4.5f;

    public bool isFirstGenerateBallon;

    private float scale;

    public float moveSpeed;

    public float jumpPower;

    public bool isGrounded;

    public GameObject[] ballons;

    public int maxBallonCount;

    public Transform[] ballonTrans;

    public GameObject ballonPrefab;

    public float generateTime;

    public bool isGenerating;

    [SerializeField, Header("Linecast用　地面判定レイヤー")]
    private LayerMask groundLayer;

    [SerializeField]
    private StartChecker startChecker;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();

        ballons = new GameObject[maxBallonCount];

        scale = transform.localScale.x;
    }

    private void Update()
    {
        isGrounded = Physics2D.Linecast(transform.position + transform.up * 0.4f,
            transform.position - transform.up * 0.9f, groundLayer);
        Debug.DrawLine(transform.position + transform.up * 0.4f, transform.up * 0.9f,
            Color.red, 1.0f);

        if (ballons[0]!=null)
        {

            if (Input.GetButtonDown(jump))
            {
                Jump();
            }
            if (isGrounded == false && rb.velocity.y < 0.15f)
            {
                anim.SetTrigger("Fall");
            }
        }
        else
        {
            Debug.Log("バルーンがない。ジャンプ不可");
        }

        if (rb.velocity.y > 5.0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, 5.0f);
        }

        if (isGrounded == true && isGenerating == false)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                StartCoroutine(GenerateBallon());
            }
        }

    }
    private void Jump()
    {
        rb.AddForce(transform.up * jumpPower);

        anim.SetTrigger("Jump");
    }
    
    private void FixedUpdate()
    {
        Move();
    }
    void Move()
    {
        float x = Input.GetAxis(horizontal);

        if (x != 0)
        {
            rb.velocity = new Vector2(x * moveSpeed, rb.velocity.y);


            Vector3 temp = transform.localScale;

            temp.x = x;

            if (temp.x > 0)
            {
                temp.x = scale;
            }
            else
            {
                temp.x = -scale;
            }

            transform.localScale = temp;

            anim.SetBool("Idle", false);
            anim.SetFloat("Run", 0.5f);
        }


        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);

            anim.SetFloat("Run", 0.0f);
            anim.SetBool("Idle", true);
        }

        float posX = Mathf.Clamp(transform.position.x, -limitPosX, limitPosY);
        float posY = Mathf.Clamp(transform.position.y, -limitPosY, limitPosY);

        transform.position = new Vector2(posX, posY);
    }

    private IEnumerator GenerateBallon()
    {
        if (ballons[1] != null)
        {
            yield break;
        }
        isGenerating = true;

        if (isFirstGenerateBallon == false)
        {
            isFirstGenerateBallon = true;

            Debug.Log("初回のバルーン生成");

            startChecker.SetInitialSpeed();
        }

        if (ballons[0] == null)
        {
            ballons[0] = Instantiate(ballonPrefab, ballonTrans[0]);
        }
        else
        {
            ballons[1] = Instantiate(ballonPrefab, ballonTrans[1]);
        }
        yield return new WaitForSeconds(generateTime);

        isGenerating = false;
    }
}
