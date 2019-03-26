/**********************************************/
/* Скрипт, отвечающий правльное отображение кур-
 * сора в списке уровней в главном меню и реша-
 * ющий проблему с объектом Blocker
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

public class DropdownLayerSortSolver : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Solver();//функция решения проблемы
	}

    void Solver()
    {
        if (transform.childCount == 4)//если в объекте Dropdown больше 3 детей, т.е мы нажали на объект и появился список
        {
            if (GameObject.Find("Blocker") != null)//если появился объект Blocker
            {
                GameObject.Find("Blocker").GetComponent<Canvas>().sortingOrder = 1;
                //Destroy(GameObject.Find("Blocker").gameObject);//то удаляем его
            }
            transform.GetChild(3).GetComponent<Canvas>().sortingOrder = 1;//ставим уровень сортировки Canvas на 1, чтобы изображение, следующее за курсором не исчезал за списком
        }
    }
}
