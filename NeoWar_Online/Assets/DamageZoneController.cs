using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DamageZoneController : NetworkBehaviour
{
    public static DamageZoneController Instance;
    [SerializeField] private GameObject prefabArea;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float squereSize;

    private GameObject area;
    private Vector2 currentTarget;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    public override void OnStartServer()
    {
        base.OnStartServer();
        SpawnArea();
    }

    [Server]
    private void SpawnArea()
    {
        area = Instantiate(prefabArea, transform.position, Quaternion.identity);
        NetworkServer.Spawn(area);
    }
    [Server]
    private void MoveZone()
    {
        if(area!=null)
        {
            Vector3 direction = (new Vector3(currentTarget.x, currentTarget.y, 0) - area.transform.position).normalized;
            area.transform.Translate(direction * movementSpeed*Time.deltaTime);

           if (Vector3.Distance(area.transform.position, new Vector3(currentTarget.x,currentTarget.y,0)) < 1f)
                ChangePoint();
        }
    }

    private void Update()
    {
        if(isServer)
        {
            MoveZone();
        }
    }

    private void ChangePoint()
    {
        float newPointX = Random.Range(-squereSize / 2, squereSize/2);
        float newPointY = Random.Range(-squereSize / 2, squereSize/2);

        currentTarget = new Vector2(newPointX, newPointY);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(transform.position, new Vector3(squereSize, squereSize, 0));

        if(area!=null)
        Gizmos.DrawWireSphere(transform.position, area.transform.localScale.x*15);
    }
#endif
}
