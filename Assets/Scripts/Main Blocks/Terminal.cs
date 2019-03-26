/**********************************************/
/* Скрипт, отвечающий за терминал
 *  
 * Создан Белым Койотом
/**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : MonoBehaviour {

    public GameObject yellowDiscStorage;//хранилище желтых дискет

    public void ExploseDiscs()//функция, которая провоцирует взрыв дискет
    {
        if (yellowDiscStorage.transform.childCount > 0)//если в хранилище есть дискеты
        {
            for (int i = 0; i < yellowDiscStorage.transform.childCount; i++)//идем по всем
                yellowDiscStorage.transform.GetChild(i).GetComponent<Disc>().SelfDestruction();//вызываем у дискет функцию взрыва
        }
    }
}
