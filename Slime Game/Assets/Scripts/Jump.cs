using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jump : Subject
{
    public float power = 10f;
    private Rigidbody2D rb;
    [SerializeField] private follow _follow;
    [SerializeField] private ActiveWorldZones world;

    public bool canJump;
    public bool StuckToWall;

    float height;

    Trajectory tl;

    public Vector2 minPower;
    public Vector2 maxPower;

    Camera cam;
    Vector2 force;
    Vector3 startPoint;
    Vector3 endPoint;
    Vector3 wallPoint;

    public Text Jumps;
    public Text Height;

    void Start()
    {
        cam = Camera.main;
        tl = GetComponent<Trajectory>();
        rb = GetComponent<Rigidbody2D>();
        PlayerStats.jumpCount = 0;
        height = 0;
    }

    void Update()
    {
        Movement();
        MaxHeight();
        text();
    }

    void MaxHeight() 
    {
        height = transform.position.y;
        if (height > PlayerStats.maxHeight)
        {
            PlayerStats.maxHeight = height;
        }
    }
    void text()
    {
        Jumps.text = ("Jumps: ");
        Jumps.text += PlayerStats.jumpCount.ToString();
        Height.text = PlayerStats.maxHeight.ToString("F0");
        Height.text += " m";
    }
    void Movement()
    {
        if (canJump)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
                startPoint.z = 15;
            }
            if (Input.GetMouseButton(0))
            {
                Vector3 currentPoint = cam.ScreenToWorldPoint(Input.mousePosition);
                currentPoint.z = 15;
                tl.LineRenderer(startPoint, currentPoint);
            }
            if (Input.GetMouseButtonUp(0))
            {
                StuckToWall = false;
                endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
                endPoint.z = 15;

                force = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x, minPower.x, maxPower.x), Mathf.Clamp(startPoint.y - endPoint.y, minPower.y, maxPower.y));
                rb.AddForce(force * power, ForceMode2D.Impulse);
                canJump = false;
                PlayerStats.jumpCount++;
                tl.Endline();
                NotifyObserver(PlayerActions.Jump);
            }
        }
        if (StuckToWall)
        {
            transform.position = wallPoint;
            rb.velocity = Vector3.zero;
        }
    }


    // idk what the fuck i was thinking here fix this shit

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "StickyWall")
        {
            wallPoint = transform.position;
            StuckToWall = true;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            canJump = true;
        }
        if (collision.collider.tag == "StickyWall")
        {
            canJump = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground")
        {
            canJump = false;
        }
        if (collision.collider.tag == "StickyWall")
        {
            canJump = false;
            StuckToWall = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Zone>(out Zone zone))
        {
            world.UpdateZones(zone);
            world.UpdateCamera(zone);
        }
    }
}