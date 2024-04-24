using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AfterAnimationFinished : MonoBehaviour
{
    public Animator animator;
    public UnityEvent onAnimationFinished;

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            onAnimationFinished.Invoke();
        }
    }


}
