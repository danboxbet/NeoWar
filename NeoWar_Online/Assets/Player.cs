using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[System.Serializable]
public struct ColorPlayer
{
    public float R;
    public float G;
    public float B;
    public float A;
}
public class Player : NetworkBehaviour
{   
    public enum TypeVehicle
    {
        Space,
        Tank
    }
    [SerializeField] private Vehicle spaceVehiclePrefab;
    [SerializeField] private Vehicle tankVehiclePrefab;
    public Vehicle ActiveVehicle { get; set; }

    
    private TypeVehicle typeVehicle;
    public TypeVehicle Type => typeVehicle;

    [SyncVar]
    ColorPlayer colorPlayer=new ColorPlayer();

    
    public Color PlayerColor => new Color(colorPlayer.R,colorPlayer.G,colorPlayer.B,colorPlayer.A);

    private void Update()
     {

         if (ActiveVehicle != null) return;

         if(hasAuthority && Input.GetMouseButtonDown(0))
         {

             Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
             RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
             Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
              if (hit.collider!=null && hit.collider.GetComponent<DamageZone>()==null) return;


             CmdSpawnVehicle(mousePos.x,mousePos.y);
         }
     }

    public override void OnStartServer()
    {
        base.OnStartServer();
        Color color = PlayerColorPallete.Instance.TakeRandomColor();
        colorPlayer = new ColorPlayer { A = color.a, B = color.b, G = color.g, R = color.r }; 
    }
    public override void OnStopServer()
    {
        base.OnStopServer();

        Color color = new Color(colorPlayer.R, colorPlayer.G, colorPlayer.B, colorPlayer.A);
        PlayerColorPallete.Instance.PutColor(color);
    }
    [Command]
    private void CmdSpawnVehicle(float x,float y)
    {
        SvSpawnClientVehicle(x,y);
    }

    [Server]
    public void SvSpawnClientVehicle(float x, float y)
    {
        if (ActiveVehicle != null) return;

        if (SpawnZone.Instance.CheckPointInZone(new Vector2(x, y)) == false) return;

        float randomValue=Random.Range(0f, 1f);
        Vehicle selectedPrefab = (randomValue < 0.5f) ? spaceVehiclePrefab : tankVehiclePrefab;

       
        typeVehicle= (randomValue < 0.5f) ? TypeVehicle.Space : TypeVehicle.Tank;

        GameObject playerVehicle = Instantiate(selectedPrefab.gameObject, new Vector2(x,y), Quaternion.identity);
       
        NetworkServer.Spawn(playerVehicle, netIdentity.connectionToClient);

        ActiveVehicle = playerVehicle.GetComponent<Vehicle>();
        ActiveVehicle.owner = netIdentity;

        RpcSetVehicle(ActiveVehicle.netIdentity, netIdentity, randomValue);
    }

    [ClientRpc]
    private void RpcSetVehicle(NetworkIdentity vehicle, NetworkIdentity networkIdentity, float random)
    {
        ActiveVehicle = vehicle.GetComponent<Vehicle>();

        if (netIdentity == networkIdentity)
        {
            if (random > 0.5f) netIdentity.GetComponent<Player>().typeVehicle = TypeVehicle.Tank;
            else netIdentity.GetComponent<Player>().typeVehicle = TypeVehicle.Space;
        }

        if(ActiveVehicle!=null && ActiveVehicle.hasAuthority && VehicleCamera.Instance!=null)
        {
            VehicleCamera.Instance.SetTarget(ActiveVehicle.transform);
        }
    }
}
