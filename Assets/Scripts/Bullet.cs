using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] bool useParticles;
    [SerializeField] GameObject grassParticle;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            if (useParticles)
            {
                GameObject particle = Instantiate(grassParticle, GameObject.Find("Grass_Bucket").transform);
                particle.transform.position = transform.position;
                Destroy(particle, 3);
            }
            Destroy(gameObject);
        }
    }
}
