/**********************************************/
/* Скрипт, отвечающий за кнопку с титрами
 *  
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


public class CreditsButton : MonoBehaviour {

    public Image creditImage;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ShowCredit);//обрабатываем нажатия на кнопку
    }

	// Update is called once per frame
	void Update () 
    {
        if (creditImage.enabled == true && Input.anyKey)//если включен показ экрана титров и нажали любую кнопку
            creditImage.GetComponent<Image>().enabled = false;	//убираем экран титров
	}

    void ShowCredit()//показываем титры
    {
        creditImage.GetComponent<Image>().enabled = true;//активируем объект титров
    }
}
