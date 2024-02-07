using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;
public class Destructible : NetworkBehaviour 
{
    public UnityAction<int> HitPointChange;

    [SerializeField] private int maxHitPoint;

    [SerializeField] private GameObject destroySfx;

    public int MaxHitPoint => maxHitPoint;

    [SyncVar(hook = nameof(ChangeHitPoint))]
    private int syncCurrentHitPoint;

    public int HitPoint => currentHitPoint;
    private int currentHitPoint;
    public override void OnStartServer()
    {
        base.OnStartServer();

        syncCurrentHitPoint = maxHitPoint;
        currentHitPoint = maxHitPoint;
    }

    [Server]
    public void SvApplyDamage(int damage)
    {
        syncCurrentHitPoint -= damage;

        if(syncCurrentHitPoint<=0)
        {
            if(destroySfx!=null)
            {
               GameObject sfx=Instantiate(destroySfx, transform.position, Quaternion.identity);
                NetworkServer.Spawn(sfx);
            }

            Destroy(gameObject);
        }
    }
    private void ChangeHitPoint(int oldValue, int newValue)
    {
        currentHitPoint = newValue;
        HitPointChange?.Invoke(newValue);
    }

    [SyncVar]
    public NetworkIdentity owner;
}
