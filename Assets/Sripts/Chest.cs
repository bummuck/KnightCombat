using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public int score = 100;
    public bool isActive;

    private void Start()
    {
        isActive = true;
    }

    public void Open()
    {
        if (isActive)
        {
            animator.SetBool("IsOpen", true);
            isActive = false;
        }
    }
}