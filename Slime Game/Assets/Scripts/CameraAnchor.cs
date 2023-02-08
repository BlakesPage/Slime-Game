using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnchor : MonoBehaviour 
{
    public int Id;
    [HideInInspector] public Vector3 Position;

    private void Awake()
    {
        Position = transform.position;
    }
}
