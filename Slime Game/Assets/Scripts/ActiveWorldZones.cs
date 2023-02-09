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

        if (currentZone != zone)
        {
            currentZone = zone;
            currentZone.gameObject.SetActive(true);
        }

        UpdateSurroundingZones(currentZone.Id);
        UpdateCamera(zone);
    }

    void UpdateSurroundingZones(Vector2Int id)
    {
        for (int x = id.x - 1; x <= id.x + 1; x++) // loops through the surrounding zones in the array
        {
            for (int y = id.y - 1; y <= id.y + 1; y++)
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

        setZonesActive.Clear();
        setZonesDeactive.Clear();
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