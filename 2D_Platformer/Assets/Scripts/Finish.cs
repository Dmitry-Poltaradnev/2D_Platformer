using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] private GameObject levelCompleteCanvas;
    private bool _isActivated;// Переменная для обозначения активации рычага.

    public void Activate()
    {
        _isActivated = true;
    }
    public void FinishLevel()
    {
        if (_isActivated)
        {
            levelCompleteCanvas.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
