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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            boss.Damage();
        }
    }
}
