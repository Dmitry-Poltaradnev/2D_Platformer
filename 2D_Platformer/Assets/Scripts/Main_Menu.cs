using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//Библиотека для работы со сценами.

public class Main_Menu : MonoBehaviour
{
    public void StartHandler()//Start button.
    {
        SceneManager.LoadScene(1);//Загрузка сцены по названию(также можно добавить по индексу), также необходимо добавить в Build Setting данную сцену.
    }

    public void ExitHandler()//Exit button.
    {
        Application.Quit();//Метод отвечает за выход.
    }
}
