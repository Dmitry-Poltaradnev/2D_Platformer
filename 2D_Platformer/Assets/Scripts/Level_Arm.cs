using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Arm : MonoBehaviour
{
    private Finish finish;
    private void Start()
    {
        finish = GameObject.FindGameObjectWithTag("Finish").GetComponent<Finish>();//Передаём в новый экземпляр объекта ссылку на компонент с тэгом Finish.
    }
    public void ActivateLeverArm()
    {
        finish.Activate();
    }
}
