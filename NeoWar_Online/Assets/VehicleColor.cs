using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class VehicleColor : NetworkBehaviour
{
    [SerializeField] private SpriteRenderer[] spriteRenderers;

    [SerializeField] private Vehicle vehicle;
    private void Start()
    {
        var player = vehicle.owner.GetComponent<Player>().PlayerColor;
        foreach (var sprite in spriteRenderers)
        {
            sprite.color = player;
        }
    }
}
