using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [SerializeField] int life;
    int currentLife;
    [SerializeField] Slider sliderHealth;
    Animator anim;
    void Start()
    {
        currentLife = life;
        sliderHealth.value = 1;
        anim = GetComponent<Animator>();
    }
    public void Damage()
    {
        currentLife--;
        sliderHealth.value = currentLife / life;
        if (currentLife<= 0) {
            Die();
            return;
        }
    }
    void Die()
    {
        anim.SetTrigger("Die");
    }
}
