using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private int damage;
    [SerializeField] private float lifeTime;
    [SerializeField] private GameObject destroySfx;

    private Transform parent;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
        

    }

    private void Update()
    {
      //  if (parent == null) return;

        float stepLength = Time.deltaTime * movementSpeed;

        Vector2 step = transform.up * stepLength;

        transform.position += new Vector3(step.x, step.y, 0);


        RaycastHit2D[] hits=Physics2D.RaycastAll(transform.position, transform.up, Time.deltaTime * movementSpeed);

        foreach(var hit in hits)
        {
            if (hit == true)
            {
                if (hit.collider.transform.root != parent && hit.collider.isTrigger == false)
                {
                    if (NetworkSeesionManager.Instance.IsServer)
                    {
                        Destructible destructible = hit.collider.transform.root.GetComponent<Destructible>();

                        if (destructible != null)
                        {
                            destructible.SvApplyDamage(damage);
                        }
                    }

                    if (NetworkSeesionManager.Instance.IsClient)
                    {
                        Instantiate(destroySfx, transform.position, Quaternion.identity);
                    }

                    Destroy(gameObject);
                }
            }
        }
    }
    public void SetParent(Transform transform)
    {
        parent = transform;
    }
}
