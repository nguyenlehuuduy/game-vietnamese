using System.Collections;
using UnityEngine;

public class welcome : MonoBehaviour
{
    public Animator maleAnimator;
    public Animator femaleAnimator;

    private void Start()
    {
        maleAnimator.Play("AnimNhay");
        femaleAnimator.Play("AnimNuNhay");
        StartCoroutine(ReturnToExitState(maleAnimator, 3f));
        StartCoroutine(ReturnToExitState(femaleAnimator, 3f));
    }

    private IEnumerator ReturnToExitState(Animator animator, float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.Play("Exit");
    }
}
