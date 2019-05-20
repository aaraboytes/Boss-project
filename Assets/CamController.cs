using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public static CamController Instance;
    Animator anim;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void Shake() {
        anim.SetTrigger("Shake");
    }
}
