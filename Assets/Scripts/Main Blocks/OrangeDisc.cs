/**********************************************/
/* Скрипт, отвечающий за поведение оранжевой
 * дискеты
 * Создан Белым Койотом
/**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeDisc : MonoBehaviour {

    public float speed = 3;//скорость движения зонка
    public GameObject explosionBlock;//блок взрыва

    private Vector3 nextCell;//следующая позиция для движения
    public bool moving;//движется ли зонк сейчас
    private RaycastHit hit;//луч
    public bool danger;//опасен ли в движении зонк
    private int dir;//направление движения

    public GameObject levelManager;//менеджер уровня

    private bool firstFall;
    public AudioSource sounds;//звуки

    public bool chained;
    private float chainedTime;

    // Use this for initialization
    void Start () 
    {
	}
	
	// Update is called once per frame
	void Update () 
    {
        ChainedExplosion();
        if (!chained)
        {
            if (!moving)//если не в движении
                CheckSpace();//то ищем, можно ли куда-то двигаться
            else //если есть, куда двигаться, то
                Fall();//двигаемся
        }
	}

    void CheckSpace()//функция которая проверяет в какую сторону можно двигаться
    {
        //сначала зонк падает, если под ним есть свободное место
        //т.е. в массиве уровня смотрим, если под ячейкой зонка -1 (пусто), то можно падать
        if (levelManager.GetComponent<LevelManager>().levelState[(int)transform.position.x, (int)transform.position.y - 1] == -1)
        {
            //говорим менджеру уровня, что зонк движется на новую ячейку и записываем туда 3- код зонка
            levelManager.GetComponent<LevelManager>().levelState[(int)transform.position.x, (int)transform.position.y - 1] = 3;
            //говорим, что направление движения вниз
            dir = 0;
            //записываем позицию ячейки, куда движется зонк
            nextCell = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
            //говорим, что зонк движется
            moving = true;
            //говорим, что зонк опасен в движении
            danger = true;
            firstFall = true;
        }
        else if (firstFall)
            SelfDestruction();
    }

    void Fall()//функция, которая отвечает за движение в выбранном направлении
    {
        //если расстояние до ячейки, где должен оказаться зонк больше 0.1
        if (Vector3.Distance(transform.position, nextCell) > 0.1f)
            transform.Translate(-Vector3.up * Time.deltaTime * speed);//то движемся к ней
        else //как только приблизились к ней
        {
            if (FreeFall())//если падаем долго
                danger = true;//то опасны в движении
            else danger = false;//иначе нет
            transform.position = nextCell;//ставим зонку конечную позицию для движения
            //записываем в массив уровня то, что зонк занимает новую и освободил старую ячейку
            levelManager.GetComponent<LevelManager>().levelState[(int)transform.position.x, (int)transform.position.y + 1] = -1;
            levelManager.GetComponent<LevelManager>().levelState[(int)transform.position.x, (int)transform.position.y] = 3;
            moving = false;//не движемся
            dir = -1;//сбрасываем направление движения
        }
    }

    bool FreeFall()
    {
        if (Physics.Raycast(transform.position, -transform.up, out hit, 1))
        {
            if (hit.collider.GetComponent<Murphy>() != null)
                return true;
            else return false;
        }
        else return true;
    }

    public void SelfDestruction()
    {
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
            if (chainedTime >= 0.2f)
                SelfDestruction();
        }
    }
}
