/**********************************************/
/* Скрипт, отвечающий за кнопку показа все
 * действующих лиц в игре
 * Создан Белым Койотом
/**********************************************/

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class TutorButton : MonoBehaviour {

    public Image tutorImage;//изображение со всеми объектами
	
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ShowCast);//обрабатываем нажатие на кнопку
    }

	// Update is called once per frame
	void Update () 
    {
        //GetComponent<Button>().onClick.AddListener(ShowCast);//обрабатываем нажатие на кнопку
        if (tutorImage.enabled == true && Input.anyKey)//если включен показ и нажали любую кнопку
            tutorImage.GetComponent<Image>().enabled = false;//убираем показ
	}

    void ShowCast()
    {
        tutorImage.GetComponent<Image>().enabled = true;//включаем показ
    }
}
