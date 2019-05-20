using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D body;
    [Header("Health")]
    [SerializeField] int life;
    int currentLife;
    [SerializeField]
    bool gameOver = false;

    [Header("Movement")]
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] float gravityMultiplier;
    [SerializeField] float throwForce;
    [SerializeField] LayerMask groundMask;
    [SerializeField]bool stunted = false;
    Vector2 movement = Vector2.zero;
    bool isGrounded = true;

    [Header("Shoot")]
    [SerializeField] float cadence, bulletForce;
    GameObject bullet_bucket;
    public GameObject bullet;

    [Header("Animation")]
    [SerializeField] bool useAnimations = false;
    Animator anim;

    [Header("Particles")]
    [SerializeField] bool useParticles = false;
    [SerializeField] GameObject grassParticle;
    GameObject grass_bucket;

    [Header("Sound")]
    [SerializeField] bool useSound = false;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip groundSound;
    [SerializeField] AudioClip shootSound;
    AudioSource audio;
    
    float timer = 0;
    void Start()
    {
        bullet_bucket = new GameObject("Bullet_Bucket");
        grass_bucket = new GameObject("Grass_Bucket");
        body = GetComponent<Rigidbody2D>();
        currentLife = life;
        if (useAnimations)
            anim = GetComponent<Animator>();
        if (useSound) audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        #region Movement
        bool lastGrounded = isGrounded;
        isGrounded = Grounded();
        if(lastGrounded == false && isGrounded == true && useAnimations)
        {
            if(anim.GetCurrentAnimatorStateInfo(0).IsName("jump"))
                anim.SetTrigger("Ground");
            GameObject gPart = Instantiate(grassParticle, grass_bucket.transform);
            gPart.transform.position = new Vector2(transform.position.x, transform.position.y - GetComponent<BoxCollider2D>().bounds.extents.y);
            Destroy(gPart, 3);
            if (useSound) audio.PlayOneShot(groundSound);
        }
        
        if (!stunted && !gameOver)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                if (useAnimations) anim.SetTrigger("Jump");
                if (useParticles) {
                    GameObject gPart = Instantiate(grassParticle, grass_bucket.transform);
                    gPart.transform.position = new Vector2(transform.position.x, transform.position.y - GetComponent<BoxCollider2D>().bounds.extents.y);
                    Destroy(gPart, 3);
                }
                if (useSound) audio.PlayOneShot(jumpSound);
            }
            if (body.velocity.y < 0 && !isGrounded)
                body.velocity += Vector2.up * Physics2D.gravity.y * (gravityMultiplier - 1) * Time.deltaTime;
        }
        else
        {
            movement.x = 0;
        }
        movement.y = body.velocity.y;
        movement.x *= speed;
        #endregion

        #region Shoot
        timer += Time.deltaTime;
        if (Input.GetMouseButton(0) && timer >= cadence)
        {
            Vector2 shootDir;
            shootDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            GameObject b = Instantiate(bullet,bullet_bucket.transform);
            b.transform.position = (Vector2)transform.position + shootDir.normalized * 0.1f;
            b.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(shootDir.y, shootDir.x)*Mathf.Rad2Deg - 90);
            b.GetComponent<Rigidbody2D>().AddForce( shootDir.normalized * bulletForce,ForceMode2D.Impulse);
            Destroy(b, 5);
            timer = 0;
            if (useSound) b.GetComponent<AudioSource>().PlayOneShot(shootSound);
        }
        #endregion
    }

    private void FixedUpdate()
    {
        if(!stunted)
            body.velocity = movement;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !stunted)
        {
            stunted = true;
            Vector2 throwDir = collision.transform.position - transform.position;
            if (Grounded())
            {
                if (throwDir.x > 0) throwDir = Vector2.left + Vector2.up * 0.3f;
                else if (throwDir.x < 0) throwDir = Vector2.right + Vector2.up * 0.3f;
            }
            else
            {
                if (throwDir.x > 0) throwDir = Vector2.left;
                else if (throwDir.x < 0) throwDir = Vector2.right;
            }
            print(throwDir);
            body.AddForce(throwDir * throwForce, ForceMode2D.Impulse);
            StartCoroutine(IEWaitForLand(throwDir));
            Damage();
        }
    }
    IEnumerator IEWaitForLand(Vector2 throwDir)
    {
        Vector2 rayDir;
        if (throwDir.x > 0) rayDir = Vector2.right;
        else rayDir = Vector2.left;
        yield return new WaitForSeconds(0.1f);
        while (!Grounded() && !Physics2D.Raycast((Vector2)transform.position + rayDir * 0.5f, rayDir, 0.1f, groundMask))
        {
            Debug.DrawLine((Vector2)transform.position + rayDir * 0.5f, (Vector2)transform.position + rayDir * 0.5f + rayDir * 0.1f);
            yield return null;
        }
        stunted = false;
    }
    bool Grounded()
    {
        if (Physics2D.OverlapCircle((Vector2)transform.position + Vector2.down, 0.001f,groundMask))
            return true;
        else
            return false;
    }

    void Damage()
    {
        currentLife--;
        if (currentLife <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        gameOver = true;
        if (useAnimations)
            anim.SetTrigger("Die");
    }
}
