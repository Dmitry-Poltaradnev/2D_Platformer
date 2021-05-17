using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] private GameObject massegeUI;
    [SerializeField] private GameObject levelCompleteCanvas;
    private bool _isActivated;// Переменная для обозначения активации рычага.

    public void Activate()
    {
        _isActivated = true;
        massegeUI.SetActive(false);
    }
    public void FinishLevel()
    {
        if (_isActivated)
        {
            levelCompleteCanvas.SetActive(true);
            gameObject.SetActive(false);
            Time.timeScale = 0f;
        }
        else
        {
            massegeUI.SetActive(true);
        }
    }
}
