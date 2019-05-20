using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBody : MonoBehaviour
{
    Boss boss;
    private void Start()
    {
        boss = transform.parent.GetComponent<Boss>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            boss.Damage(other.transform.position);
            Destroy(other.gameObject);
        }
    }
}
