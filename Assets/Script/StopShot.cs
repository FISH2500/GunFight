using UnityEngine;

public class StopShot : MonoBehaviour
{
    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopShotGun() 
    {
        animator.SetBool("isForwardShot", false);
    }
}
