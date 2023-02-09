using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWorldZones : MonoBehaviour
{
    [SerializeField] private Camera cam;

    [HideInInspector] public Zone currentZone;
    [SerializeField] private Zone[,] zones = new Zone[WorldInfo.WorldWidth + 1, WorldInfo.WorldHeight + 1];

    private List<Zone> activeZones = new List<Zone>();
    private List<Zone> setZonesDeactive = new List<Zone>();
    private List<Zone> setZonesActive = new List<Zone>();

    private void Awake()
    {
        foreach (Transform t in transform)
        {
            if (t != null)
            {
                t.gameObject.TryGetComponent<Zone>(out Zone z);
                if (zones[z.Id.x, z.Id.y] == null) { zones[z.Id.x, z.Id.y] = z; }
                else { Debug.LogError("Two or more zones have the same Id"); }
                //Debug.Log("Zone: " + z.Id.x + " " + z.Id.y);
            }
        }
    }

    private void Start()
    {
        currentZone = zones[50, 100];
        UpdateSurroundingZones(currentZone.Id);
    }

    public void UpdateZones(Zone zone)
    {
        if (zone == null || zones[zone.Id.x, zone.Id.y] == null) { return; }

        if (currentZone != zone) currentZone = zone;

        UpdateSurroundingZones(currentZone.Id);
        UpdateCamera(zone);
    }

    void UpdateSurroundingZones(Vector2Int id)
    {
        //Debug.Log("this worked");
        for (int x = id.x - 1; x <= id.x + 1; x++) // loops through the surrounding zones in the array
        {
            for (int y = id.y - 1; y <= id.y + 1; y++)
            {
                if (x < 0 || x > WorldInfo.WorldWidth || y < 0 || y > WorldInfo.WorldHeight) continue;
                if (zones[x, y] == null) continue;
                if (x == id.x && y == id.y) continue;
                //if (zones[x, y].isActive) continue;

                setZonesActive.Add(zones[x, y]); // zones to activate
                //Debug.Log("Zone: " + zones[x, y].Id.x + " " + zones[x, y].Id.y + " isActive");
            }
        }

        for(int i = 0; i < activeZones.Count; i++) // loop through active zones to check the current list of zones to set active remove the outliers 
        {
            Zone temp = activeZones[i];
            if(setZonesActive.Contains(temp))
            {
                setZonesDeactive.Add(temp);
            }
        }

        SetZonesActive();
        SetZonesDeactive();

        setZonesActive.Clear();
        setZonesDeactive.Clear();

        foreach(Zone z in activeZones)
        {
           // Debug.Log("Zone: " + z.Id.x + " " + z.Id.y + " isActive");
        }
    }

    void SetZonesActive()
    {
        foreach (Zone z in setZonesActive)
        {
            z.gameObject.SetActive(true);
            activeZones.Add(z);
        }
    }

    void SetZonesDeactive()
    {
        foreach (Zone z in setZonesDeactive)
        {
            z.gameObject.SetActive(false);
            activeZones.Remove(z);
        }
    }

    public void UpdateCamera(Zone zone)
    {
        cam.transform.position = zone.transform.position;
    }

    public Vector2 GetCurrentZoneId()
    {
        return currentZone.Id;
    }
}