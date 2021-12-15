using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeManager : MonoBehaviour
{
    private Animator animator;
    private bool isShowingTodaySumary;
    public RuntimeAnimatorController[] runtimeAnimatorCtrls;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void _ToggleTodaySumary()
    {
        TodaySumary.instance.SetContent();
        animator.runtimeAnimatorController = runtimeAnimatorCtrls[0];
        isShowingTodaySumary = !isShowingTodaySumary;
        animator.SetBool("show", isShowingTodaySumary);
    }


}
