using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tes : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] float cadence,bulletForce;
    float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        #region Shoot
        if (Input.GetMouseButton(0) && timer >= cadence)
        {
            Vector2 shootDir;
            shootDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            GameObject b = Instantiate(bullet, transform);
            b.transform.position = (Vector2)transform.position + shootDir.normalized * 0.1f;
            b.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(shootDir.y, shootDir.x) - 90 * Mathf.Deg2Rad);
            b.GetComponent<Rigidbody2D>().AddForce(shootDir * bulletForce);
            timer = 0;
        }
        #endregion
    }
}
