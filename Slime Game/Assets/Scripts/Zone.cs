using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[ExecuteInEditMode]
[RequireComponent(typeof(BoxCollider2D))]
public class Zone : MonoBehaviour
{
    public new string name;
    public Vector2Int Id;
    public GameObject checkPoint;
    public bool isCheckPoint;
    [HideInInspector] public bool visited = false;
    [HideInInspector] public BoxCollider2D zoneCollider;

    private List<GameObject> zoneObjects = new List<GameObject>();
    [SerializeField] private bool checkpoint;

    #region Awake
    private void Awake()
    {
        transform.position = new Vector3(0, transform.position.y, transform.position.z);

        foreach (Transform t in this.transform)
        {
            GameObject go = t.gameObject;
            zoneObjects.Add(go);
        }

        float height = (Camera.main.orthographicSize * 2f) - 0.7f;
        float width = (height + 0.7f) * Camera.main.aspect;

        zoneCollider = GetComponent<BoxCollider2D>();
        zoneCollider.isTrigger = true;
        zoneCollider.size = new Vector2(width, height);
    }
    #endregion Awake

    #region Draw Gizmos
    void OnDrawGizmosSelected()
    {
        // Draw a red wirecube at the transforms position
        float height = (Camera.main.orthographicSize * 2f) - 0.7f;
        float width = (height + 0.7f) * Camera.main.aspect;
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawWireCube(new Vector3(0, transform.position.y, 0), new Vector3(width, height, 1));
    }
    #endregion Draw Gizmos

    private void OnValidate()
    {
        gameObject.name = name + " Id: " + Id.x + ", " + Id.y;
    }

    public void SetChildrenActive()
    {
        for (int i = 0; i < zoneObjects.Count; i++)
        {
            zoneObjects[i].SetActive(true);
        }
    }

    public void SetChildrenDeactive()
    {
        for (int i = 0; i < zoneObjects.Count; i++)
        {
            zoneObjects[i].SetActive(false);
        }
    }

    public Vector3 GetZoneCheckPoint
    {
        get { return checkPoint.transform.position; }
    }

}
