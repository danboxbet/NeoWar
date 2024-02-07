using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Turret : NetworkBehaviour
{
    public enum TypeProjectile
    {
        Rocket=0,
        Default=1
    }
    [SerializeField] private GameObject projectile;

    [SerializeField] private float fireRate;
    [SerializeField] private float fireRocketRate;

    [SerializeField] private GameObject rocket;
    [SerializeField] private Transform ownerTransform;
    [SerializeField] private Transform turretTransform;
    
    private float currentTimeToDefault;
    private float currentTimeToRocket;
   

    private void Update()
    {
        if(isServer)
        {
            currentTimeToDefault += Time.deltaTime;
            currentTimeToRocket += Time.deltaTime;
        }
    }

    [Command]
    public void CmdFire(bool isDefaultProjectile)
    {
       
        SvFire(isDefaultProjectile);
    }

    [Server]
    private void SvFire(bool isDefaultProjectile)
    {
        

        GameObject pref;
        if (isDefaultProjectile)
        {
            pref = this.projectile;
            if (currentTimeToDefault < fireRate) return;
            else currentTimeToDefault = 0;
        }
        else
        {
            pref = this.rocket;
            if (currentTimeToRocket < fireRocketRate) return;
            else currentTimeToRocket = 0;
        }
        GameObject projectile = Instantiate(pref, ownerTransform.position, ownerTransform.rotation);
        projectile.GetComponent<Projectile>().SetParent(transform);

        RpcFire(isDefaultProjectile,netIdentity);
    }

    [ClientRpc]
    private void RpcFire(bool isDefaultProjectile, NetworkIdentity networkIdentity)
    {
        GameObject pref;
        if (isDefaultProjectile)
        {
           pref=this.projectile;
        }
        else
        {
            pref = this.rocket;
        }
        GameObject projectile = Instantiate(pref, ownerTransform.position, ownerTransform.rotation);
        projectile.GetComponent<Projectile>().SetParent(transform);
    }

  
    [Command]
    public void CmdSynhronize(float x, float y, NetworkIdentity networkIdentity)
    {
      
        SvSynhronize(x,y,networkIdentity);
    }
   
    [Server]
   private void SvSynhronize(float x, float y,NetworkIdentity networkIdentity)
    {
        RpcSynhronize(x,y, networkIdentity, netIdentity);
    }
    [ClientRpc]
    private void RpcSynhronize(float x, float y, NetworkIdentity networkIdentity, NetworkIdentity networkIdentity1)
    {
      
       if(GetComponent<Vehicle>().netIdentity==networkIdentity)
        turretTransform.up = new Vector2(x,y);
    }
}
