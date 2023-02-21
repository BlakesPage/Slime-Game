using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWorldZones : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [Range(1, 2)] [SerializeField] private int activeZoneSpacing;

    private Zone currentZone;
    private Zone[,] zones = new Zone[WorldInfo.WorldWidth + 1, WorldInfo.WorldHeight + 1];

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
            }
        }
    }

    private void Start()
    {
       
    }

    public void UpdateZones(Zone zone)
    {
        if (zone == null || zones[zone.Id.x, zone.Id.y] == null) { return; }

        if (currentZone == null)
        {
            foreach (Zone z in zones)
            {
                if (z != null)
                {
                    z.gameObject.SetActive(false);
                }
            }
        }

        if (currentZone != zone) currentZone = zone;

        UpdateSurroundingZones(currentZone.Id);
        UpdateCamera(currentZone);
        currentZone.visited = true;
    }

    void UpdateSurroundingZones(Vector2Int id)
    {
        for (int x = id.x - activeZoneSpacing; x <= id.x + activeZoneSpacing; x++) // loops through the surrounding zones in the array
        {
            for (int y = id.y - activeZoneSpacing; y <= id.y + activeZoneSpacing; y++)
            {
                if (x < 0 || x > WorldInfo.WorldWidth || y < 0 || y > WorldInfo.WorldHeight) continue;
                if (zones[x, y] == null) continue;

                setZonesActive.Add(zones[x, y]); // zones to activate
            }
        }

        for(int i = 0; i < activeZones.Count; i++) // loop through active zones to check the current list of zones to set active remove the outliers 
        {
            Zone temp = activeZones[i];
            if(!setZonesActive.Contains(temp))
            {
                setZonesDeactive.Add(temp);
            }
        }

        SetZonesActive();
        SetZonesDeactive();
    }

    void SetZonesActive()
    {
        foreach (Zone z in setZonesActive)
        {
            z.gameObject.SetActive(true);
            activeZones.Add(z);
        }

        setZonesActive.Clear();
    }

    void SetZonesDeactive()
    {
        foreach (Zone z in setZonesDeactive)
        {
            z.gameObject.SetActive(false);
            activeZones.Remove(z);
        }

        setZonesDeactive.Clear();
    }

    public void UpdateCamera(Zone zone)
    {
        cam.transform.position = new Vector3(zone.transform.position.x, zone.transform.position.y, -10);
    }

    public Vector2Int GetCurrentZoneId()
    {
        return currentZone.Id;
    }
}