using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private PlayerLifeController playerLifeController;
    void Start()
    {
        //animator = GetComponent<Animator>();
        //playerLifeController = GetComponent<PlayerLifeController>();
        //playerLifeController.OnDeath.AddListener(DeathAnimation);
    }

    //void DeathAnimation()
    //{
    //    animator.SetTrigger("death");
    //}
}
