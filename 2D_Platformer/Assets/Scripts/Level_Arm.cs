using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Arm : MonoBehaviour
{
    private Finish _finish;
    [SerializeField] private Animator animator;
    private void Start()
    {        
        _finish = GameObject.FindGameObjectWithTag("Finish").GetComponent<Finish>();//Передаём в новый экземпляр объекта ссылку на компонент с тэгом Finish.
    }
    public void ActivateLeverArm()
    {
        animator.SetTrigger("activate");
        _finish.Activate();
    }
}
