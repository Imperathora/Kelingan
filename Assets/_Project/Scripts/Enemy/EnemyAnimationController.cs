using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OOB.Enemy;

public class EnemyAnimationController : MonoBehaviour
{

    private Animator m_animator;
    private MobileEnemy m_mobileEnemy;
    private void OnEnable()
    {
        m_mobileEnemy = GetComponent<MobileEnemy>();
        m_animator = GetComponent<Animator>();
        m_mobileEnemy.OnStartAttacking += PlayAttackAnimation;
        
    }

    private void OnDisable()
    {
        m_mobileEnemy.OnStartAttacking -= PlayAttackAnimation;
    }

    private void OnDestroy()
    {
        m_animator.ResetTrigger("isAttacking");
    }


    private void PlayAttackAnimation()
    {
        m_animator.SetTrigger("isAttacking");
    }
}
