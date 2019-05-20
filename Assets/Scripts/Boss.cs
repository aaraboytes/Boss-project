using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [SerializeField] int life;
    int currentLife;
    [SerializeField] Slider sliderHealth;
    [SerializeField] GameObject bullet;
    [SerializeField] float shootForce;

    [Header("Particles")]
    [SerializeField] bool useParticles;
    [SerializeField] GameObject bloodParticle;
    [SerializeField] GameObject grassParticle;
    GameObject blood_bucket;

    [Header("Sound")]
    [SerializeField] bool useSound;
    [SerializeField] AudioClip damageSound;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip landSound;
    AudioSource audio;
    Animator anim;
    bool alive = true;
    void Start()
    {
        currentLife = life;
        sliderHealth.value = 1;
        anim = GetComponent<Animator>();
        blood_bucket = new GameObject("Blood_Bucket");
        if (useSound) audio = GetComponent<AudioSource>();
    }
    public void Damage(Vector2 hitPoint)
    {
        if (alive)
        {
            currentLife--;
            if (useParticles)
            {
                GameObject particle = Instantiate(bloodParticle, blood_bucket.transform);
                particle.transform.position = hitPoint;
                Destroy(particle, 2);
                
            }
            sliderHealth.value = (float)currentLife / (float)life;
            if (currentLife <= 0)
            {
                Die();
                alive = false;
            }
            if (useSound && Random.RandomRange(0,2)>0.8f) audio.PlayOneShot(damageSound);
        }
    }
    void Die()
    {
        anim.SetTrigger("Die");
    }
    public void ShootToBothSides()
    {
        #region Bullets
        GameObject bullet1 = Instantiate(bullet, transform);
        GameObject bullet2 = Instantiate(bullet, transform);
        bullet1.transform.position = (Vector2)transform.GetChild(0).transform.position + Vector2.right * 0.5f;
        bullet2.transform.position = (Vector2)transform.GetChild(0).transform.position + Vector2.left * 0.5f;
        bullet1.GetComponent<Rigidbody2D>().AddForce(Vector2.right * shootForce);
        bullet2.GetComponent<Rigidbody2D>().AddForce(Vector2.left * shootForce);
        Destroy(bullet1, 2f);
        Destroy(bullet2, 2f);
        #endregion

        #region Particles
        if (useParticles)
        {
            GameObject part1 = Instantiate(grassParticle, GameObject.Find("Grass_Bucket").transform);
            GameObject part2 = Instantiate(grassParticle, GameObject.Find("Grass_Bucket").transform);
            part1.transform.position = new Vector2(transform.position.x - transform.GetChild(0).GetComponent<BoxCollider2D>().bounds.extents.x, transform.GetChild(0).position.y);
            part2.transform.position = new Vector2(transform.position.x + transform.GetChild(0).GetComponent<BoxCollider2D>().bounds.extents.x, transform.GetChild(0).position.y);
            part1.transform.rotation = Quaternion.Euler(0, 0, 45);
            part2.transform.rotation = Quaternion.Euler(0, 0, -45);
            Destroy(part1, 3);
            Destroy(part2, 3);
        }
        #endregion

        if (FindObjectOfType<CamController>())
        {
            CamController.Instance.Shake();
        }
    }

    void PlayJumpSound() { audio.PlayOneShot(jumpSound); }
    void PlayLandSound() { audio.PlayOneShot(landSound); }
    void PlayBigJumpSound() {
        audio.pitch = -1;
        audio.PlayOneShot(jumpSound);
        audio.pitch = 1;
    }
    void PlayBigLandSound() {
        audio.pitch = -1;
        audio.PlayOneShot(landSound);
        audio.pitch = 1;
    }
}
