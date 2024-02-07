using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(CircleCollider2D))]
public class DamageZone : NetworkBehaviour 
{
    [SerializeField] private int damage;

    CircleCollider2D colliderZone;
    float radius;

    public override void OnStartServer()
    {
        base.OnStartServer();
        colliderZone = GetComponent<CircleCollider2D>();
        radius = colliderZone.radius;

    }
    float time=1;
    private void Update()
    {
        time -= Time.deltaTime;

        if (time <= 0)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
            foreach (Collider2D collider in colliders)
            {
                Destructible dest = collider.transform.root.GetComponent<Destructible>();
                // Проверяем, является ли объект игроком
                if (dest != null)
                {
                    dest.SvApplyDamage(damage);

                }
            }
            time = 1;
        }
       
    }

   
   
}
