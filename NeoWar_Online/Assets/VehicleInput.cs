using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
   [RequireComponent(typeof(Player))]
    public class VehicleInput :NetworkBehaviour
    {
        public enum ControlMode
        {
            Keyboard,
            Mobile
        }
        
        private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if(player.hasAuthority && player.isLocalPlayer)
        UpdateControl();
    }

       

    private void UpdateControl()
    {
        if (player.ActiveVehicle == null) return;

        float thrust = 0;
        float torque = 0;
            
        if (Input.GetKey(KeyCode.W))
                thrust = 1.0f;

        if (Input.GetKey(KeyCode.S))
                thrust = -1.0f;

        if (Input.GetKey(KeyCode.A))
                torque = 1.0f;

        if (Input.GetKey(KeyCode.D))
                torque = -1.0f;

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            player.ActiveVehicle.Fire(true);
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) || Input.GetMouseButtonDown(1))
        {
            player.ActiveVehicle.Fire(false);
        }

        if(player.Type == Player.TypeVehicle.Tank && isLocalPlayer && Application.isFocused)
        {
            if (hasAuthority && isClient)
            {
                Vector2 mouseViewportPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);// Camera.main.ScreenToViewportPoint(Input.mousePosition);

                Vector3 mouseWorldPosition = new Vector3(mouseViewportPosition.x, mouseViewportPosition.y, 0); 
                
                Vector2 direction = (mouseWorldPosition - player.ActiveVehicle.turretTransform.position).normalized;

                if(player.ActiveVehicle.hasAuthority)
                player.ActiveVehicle.RotateTurret(direction.x,direction.y);
            }
        }
       
          
        player.ActiveVehicle.ThrustControl = thrust;
        player.ActiveVehicle.TorqueControl = torque;
    }

 }

