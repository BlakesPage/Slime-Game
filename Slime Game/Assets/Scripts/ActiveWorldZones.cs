using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWorldZones : MonoBehaviour
{
    [HideInInspector] public Zone activeZone; 
    [HideInInspector] public Zone lastZoneVisited; 
    [SerializeField] private Zone[] zones = new Zone[100];
    [SerializeField] private Camera cam;
    
    private void Awake()
    {
        foreach(Transform t in transform)
        {
            if(t != null)
            {
                t.gameObject.TryGetComponent<Zone>(out Zone z);
                if(zones[z.Id] == null) { zones[z.Id] = z; }
                else { Debug.LogError("Two or more zones have the same Id"); }
            }
        }
    }

    private void Start()
    {
        activeZone = zones[0];
        lastZoneVisited = zones[0];
    }

    public void UpdateZones(Zone zone)
    {
        if(zones[zone.Id] != null && activeZone != zone)
        {
            lastZoneVisited = activeZone;
            activeZone = zone;
        }

        foreach(Zone z in zones)
        {
            if(z != null)
            {
                int id = z.Id;
                if(id == activeZone.Id - 1 || id == activeZone.Id || id == activeZone.Id + 1)
                {
                    z.gameObject.SetActive(true);
                    continue;
                }

                z.gameObject.SetActive(false);
            }
        }
    }

    public void UpdateCamera(Zone zone)
    {
        cam.transform.position = zone.transform.position;
    }

    int GetCurrentZoneId()
    {
        return activeZone.Id;
    }

    Vector3 GetLastCheckPoint(Zone zone)
    {
        return zone.checkPoint.transform.position;
    }
}
