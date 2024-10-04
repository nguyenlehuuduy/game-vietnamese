using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimation : MonoBehaviour
{
    public Animator maleAnimator;
    public Animator femaleAnimator;
    public Animator malebotAnimator;
    public Animator femalebotAnimator;

    private void Start()
    {
        maleAnimator.Play("NVNamChaonew");
        
        malebotAnimator.Play("AnimBotNam");
    }

    /*private IEnumerator ReturnToExitState(Animator animator, float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.Play("Exit");
    }*/
}
