/**********************************************/
/* Скрипт, отвечающий за кнопку выход главного
 * меню 
 * Создан Белым Койотом
/**********************************************/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class ExitButton : MonoBehaviour {

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ExitGame);//вызываем обработчик кнопки Выход
    }

    void ExitGame()//функция, которая вызывается при нажитии кнопки выход
    {
        //ищем объект с главной информацие в главном меню и вызываем функцию подготовки данных к сохранению
        GameObject.FindObjectOfType<MainInformation>().gameObject.GetComponent<MainInformation>().PrepareDataToSave();
        //сохраняем игру
        SaveGame.Save();
        //выходим из игры
        Application.Quit();
    }
}
