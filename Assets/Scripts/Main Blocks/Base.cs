/**********************************************/
/* Скрипт, блоки печатной платы, которую может 
 * кушать Мёрфи.             
 * Создан Белым Койотом             
/**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour {
    
    public bool canDestory;
    public Transform player;


    // Use this for initialization
    void Start () 
    {
	}
	
	// Update is called once per frame
	void Update () 
    {
        //если пришла команда разрушить
        if (canDestory)
            CompleteCompression();//разрушаем
	}

    public void CompleteCompression()
    {

        //ломаем коллайдер
        Destroy(GetComponent<BoxCollider>());
        if (transform.localScale.x > 0.1f)
        {
            //уменьшаем размер блока
            float speed = 2f;
            transform.localScale = new Vector3(transform.localScale.x - Time.deltaTime * speed,
                                               transform.localScale.y - Time.deltaTime * speed,
                                               transform.localScale.z - Time.deltaTime * speed);
        }
        else Destroy(this.gameObject);
    }

    int Direction()//функция расчета направления
    {
        //вычисляется разность координат по х и у данного блока и мёрфи
        //в зависимости от знака разности коориднат становится понятно,
        //с какой стороны к блоку движется мёрфи
        if (transform.position.y - player.position.y > 0)
            return 1;//если разность у-координат больше 0, то мёрфи идет снизу
        else if (transform.position.y - player.position.y < 0)
            return 2;//если разность у-координат меньше 0, то мёрфи идет сверху
        else if (transform.position.x - player.position.x < 0)
            return 3;//если разность x-координат меньше 0, то мёрфи идет справа
        else if (transform.position.x - player.position.x > 0)
            return 4;//если разность x-координат больше 0, то мёрфи идет слева
        else return 0;//иначе, если знак разности неопределен, то возвращаем 0
    }
}
