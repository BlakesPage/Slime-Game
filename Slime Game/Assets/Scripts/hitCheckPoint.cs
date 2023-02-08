using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitCheckPoint : MonoBehaviour
{
    public CheckPoint cp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        cp.Apply(collision.gameObject);
    }
}
