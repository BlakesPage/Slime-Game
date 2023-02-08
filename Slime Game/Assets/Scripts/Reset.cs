using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] Vector3 checkPoint;
    public GameObject checkedpoint;
    public Jump jump;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        checkPoint = checkedpoint.transform.position;
        if(Input.GetKeyDown(KeyCode.R))
        {
            transform.position = checkPoint;
            rb.velocity = Vector3.zero;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Spikes")
        {
            rb.velocity = Vector3.zero;
            transform.position = checkPoint;
        }   
    }
}
