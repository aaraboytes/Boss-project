using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudGenerator : MonoBehaviour
{
    [SerializeField] GameObject cloud;
    [SerializeField] float speed,spawnTime;
    float timer = 0;
    [SerializeField] Vector2 horizontalLimits;
    [SerializeField] Vector2 verticalLimits;
    [SerializeField] List<GameObject> clouds;

    void Update()
    {
        foreach(GameObject c in clouds)
        {
            c.transform.Translate(Vector2.left * speed * Time.deltaTime);
            if(c.transform.position.x < horizontalLimits.x)
            {
                clouds.Remove(c);
                Destroy(c);
                break;
            }
        }
        timer += Time.deltaTime;
        if (timer > spawnTime)
        {
            GameObject c = Instantiate(cloud, transform);
            c.transform.position = new Vector2(horizontalLimits.y, Random.Range(verticalLimits.x, verticalLimits.y));
            clouds.Add(c);
            timer = 0;
        }
    }
}
