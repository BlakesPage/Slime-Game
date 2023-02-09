using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ZoneData
{
    public struct rowData
    {
        public Zone[] row;
    }

    public rowData[] rows = new rowData[10];
}
