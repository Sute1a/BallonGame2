using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private string horizontal = "Horizontal";

    private string jump = "Jump";
    private Rigidbody2D rb;

    private Animator anim;

    private float limitPosX = 8.385f;
    private float limitPosY = 4.5f;

    private bool isGameOver = false;

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

    public int coinPoint;

    public UIManager uiManager;

    public float knockbackPower;

    [SerializeField, Header("Linecast用　地面判定レイヤー")]
    private LayerMask groundLayer;

    [SerializeField]
    private StartChecker startChecker;

    [SerializeField]
    private AudioClip knockbackSE;

    [SerializeField]
    private GameObject knockbackeffectPrefab;

    [SerializeField]
    private AudioClip getCoinSE;

    [SerializeField]
    private GameObject getCoineffectPrefab;

    [SerializeField]
    private Joystick joystick;

    [SerializeField]
    private Button btnJump;

    [SerializeField]
    private Button btnDetach;

    private int ballonCount;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();

        ballons = new GameObject[maxBallonCount];

        btnJump.onClick.AddListener(OnClickJump);
        btnDetach.onClick.AddListener(OnClickDetachOrGenerate);

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
        if (isGameOver == true)
        {
            return;
        }
        Move();
    }
    void Move()
    {
#if UNITY_EDITOR
        float x = Input.GetAxis(horizontal);
        x = joystick.Horizontal;
#else
        float x = joystick.Horizontal;
#endif

        
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

            ballons[0].GetComponent<Ballon>().SetUpBallon(this);
        }
        else
        {
            ballons[1] = Instantiate(ballonPrefab, ballonTrans[1]);

            ballons[1].GetComponent<Ballon>().SetUpBallon(this);
        }

        ballonCount++;

        yield return new WaitForSeconds(generateTime);

        isGenerating = false;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            Vector3 direction = (transform.position - col.transform.position).normalized;

            transform.position += direction * knockbackPower;

            AudioSource.PlayClipAtPoint(knockbackSE, transform.position);

            GameObject knockbackEffect = Instantiate(knockbackeffectPrefab,
                col.transform.position, Quaternion.identity);

            Destroy(knockbackEffect, 0.5f);
        }
    }
    public void DestroyBallon()
    {
        if (ballons[1] != null)
        {
            Destroy(ballons[1]);
        }
        else if (ballons[0] != null)
        {
            Destroy(ballons[0]);
        }
        ballonCount--;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Coin")
        {
            coinPoint += col.gameObject.GetComponent<Coin>().point;

            uiManager.UpdateDisplayScore(coinPoint);

            AudioSource.PlayClipAtPoint(getCoinSE, transform.position);

            GameObject getCoineffct = Instantiate(getCoineffectPrefab,
                col.transform.position, Quaternion.identity);

            Destroy(col.gameObject);

            Destroy(getCoineffct, 0.5f);
        }    
    }

    public void GameOver()
    {
        isGameOver = true;

        Debug.Log(isGameOver);

        uiManager.DisplayGameOverInfo();
    }

    private void OnClickJump()
    {
        if (ballonCount > 0)
        {
            Jump();
        }
    }

    private void OnClickDetachOrGenerate()
    {
        if(isGrounded==true && ballonCount<maxBallonCount && isGenerating == false)
        {
            StartCoroutine(GenerateBallon());
        }
    }
}
