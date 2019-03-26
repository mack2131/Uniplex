/**********************************************/
/* Скрипт, отвечающий за инфотороны
 * 
 * Создан Белым Койотом                       
/**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infotron : MonoBehaviour {
    
    public bool canBeDestroy;

	// Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	void Update () 
    {
        //если пришла команда ломаться
        if (canBeDestroy)
            Destorying();//разрушаемся
	}

    void Destorying()
    {
        //ломаем коллайдер
        Destroy(GetComponent<BoxCollider>());
        if (transform.localScale.x > 0.1f)
        {
            //уменьшаем размер блока
            float speed = 1.8f;
            transform.localScale = new Vector3(transform.localScale.x - Time.deltaTime * speed/* * murphy.GetComponent<Murphy>().speed*/,
                                               transform.localScale.y - Time.deltaTime * speed/* * murphy.GetComponent<Murphy>().speed*/,
                                               transform.localScale.z - Time.deltaTime * speed/* * murphy.GetComponent<Murphy>().speed*/);
        }
        else Destroy(gameObject);
        
    }
}
