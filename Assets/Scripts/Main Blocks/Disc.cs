/**********************************************/
/* Скрипт, отвечающий за желтые дискеты
 *  
 * Создан Белым Койотом
/**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disc : MonoBehaviour {

    public GameObject explosionBlock;//блок взрыва
    public AudioSource sounds;//звуки

    public bool chained;
    private float chainedTime;

    // Use this for initialization
    void Start () 
    {
        sounds = GetComponent<AudioSource>();//собираем все звуки с объекта
	}

    void Update()
    {
        ChainedExplosion();
    }

    public void SelfDestruction()//функция саморазрушения
    {
        sounds.Play();//играем звук взрыва
        //создаются 9 блоков взрыва в токой очередности
        // ***
        // *0*
        // ***
        // где * - блок взрыва, а 0 - дискета
        /* up */
        GameObject ex1 = Instantiate(explosionBlock);
        ex1.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        /* down */
        GameObject ex2 = Instantiate(explosionBlock);
        ex2.transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        /* left */
        GameObject ex3 = Instantiate(explosionBlock);
        ex3.transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
        /* right */
        GameObject ex4 = Instantiate(explosionBlock);
        ex4.transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
        /* up-left */
        GameObject ex5 = Instantiate(explosionBlock);
        ex5.transform.position = new Vector3(transform.position.x - 1, transform.position.y + 1, transform.position.z);
        /* up-right */
        GameObject ex6 = Instantiate(explosionBlock);
        ex6.transform.position = new Vector3(transform.position.x + 1, transform.position.y + 1, transform.position.z);
        /* down-left */
        GameObject ex7 = Instantiate(explosionBlock);
        ex7.transform.position = new Vector3(transform.position.x - 1, transform.position.y - 1, transform.position.z);
        /* down-right */
        GameObject ex8 = Instantiate(explosionBlock);
        ex8.transform.position = new Vector3(transform.position.x + 1, transform.position.y - 1, transform.position.z);
        /* myself */
        GameObject ex9 = Instantiate(explosionBlock);
        ex9.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Destroy(gameObject);
    }

    void ChainedExplosion()
    {
        if (chained)
        {
            chainedTime += Time.deltaTime;
            if (chainedTime >= 1.0f)
                SelfDestruction();
        }
    }
}
