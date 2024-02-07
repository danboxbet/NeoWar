using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class SpawnZone : NetworkBehaviour
{
    public static SpawnZone Instance;

    [SerializeField] private float radiusZone;
    [SerializeField] private Transform ownerTransform;

    public override void OnStartServer()
    {
        base.OnStartServer();

        Instance = this;
    }

    public bool CheckPointInZone(Vector2 positionPoint)
    {
        Vector2 ownerPosition = ownerTransform.position;

        if (Vector2.Distance(ownerPosition, positionPoint) < radiusZone) return true;
        else return false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(ownerTransform.position, radiusZone);
    }
#endif
}
