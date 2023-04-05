using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldZones : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject player;
    [Range(1, 5)] [SerializeField] private int activeZoneSpacing;

    [SerializeField] private Zone startingZone;
    private Zone currentZone;
    private Zone[,] zones = new Zone[WorldInfo.WorldWidth + 1, WorldInfo.WorldHeight + 1];

    public Vector2Int CurrentCheckPoint;

    private List<Zone> activeZones = new List<Zone>();
    private List<Zone> setZonesDeactive = new List<Zone>();
    private List<Zone> setZonesActive = new List<Zone>();
    private List<Zone> checkPoints = new List<Zone>();
    

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
        if (currentZone == null)
        {
            foreach (Zone z in zones)
            {
                if (z != null && z.Id != startingZone.Id)
                {
                    z.gameObject.SetActive(false);
                }
            }
        }
        currentZone = startingZone;

        CurrentCheckPoint = currentZone.Id;
    }

    public void UpdateZones(Zone zone)
    {
        if (zone == null || zones[zone.Id.x, zone.Id.y] == null) { return; }

        UpdateCurrentCheckPoint();

        if (currentZone != zone) currentZone = zone;

        UpdateSurroundingZones(currentZone.Id);
        UpdateCamera(currentZone);
        currentZone.visited = true;
    }

    #region CheckPoints
    void UpdateCurrentCheckPoint()
    {
        if(currentZone.isCheckPoint)
        {
            if(!checkPoints.Contains(currentZone))
            {
                checkPoints.Add(currentZone);
                CurrentCheckPoint = checkPoints[checkPoints.Count - 1].Id;
            }
        }
    }

    public void GoToCurrentCheckPoint()
    {
        foreach (Zone z in zones)
        {
            if(z != null)
            {
                if (z.Id != CurrentCheckPoint || !z.isCheckPoint) continue;

                UpdateSurroundingZones(z.Id);
                UpdateCamera(z);
                UpdatePlayerPosCheckPoint(z);

                break;
            }
        }
    }

    #endregion

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

        for (int i = 0; i < activeZones.Count; i++) // loop through active zones to check the current list of zones to set active remove the outliers 
        {
            Zone temp = activeZones[i];
            if (!setZonesActive.Contains(temp))
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

    void UpdatePlayerPosCheckPoint(Zone z)
    {
        player.transform.position = z.GetZoneCheckPoint;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    Vector2Int GetCurrentZoneId()
    {
        return currentZone.Id;
    }
}
