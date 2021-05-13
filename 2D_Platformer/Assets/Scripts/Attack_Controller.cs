using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Controller : MonoBehaviour
{
    [SerializeField] private AudioSource attackSound;//т.к  на player 2 AudioSource добавляем эффекты через поля. 
    [SerializeField] private Animator animator;//Добавим поле для компонента аниматор.    

    private bool _isAttack;
    public bool IsAttack { get => _isAttack; }//Передаём переменную через публичный метод.
   

    public void FinishAttak()//Метод для завершения атаки.
    {
        _isAttack = false;
    }



    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isAttack = true;
            animator.SetTrigger("Attack");
            attackSound.Play();
        }
    }

}
