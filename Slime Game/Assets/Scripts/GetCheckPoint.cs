using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CheckPoint/checkpoint")]
public class GetCheckPoint : CheckPoint
{
    public GameObject checkpointLocation;
    public override void Apply(GameObject target)
    {
        target.GetComponent<Reset>().checkedpoint = checkpointLocation;
    }
}
