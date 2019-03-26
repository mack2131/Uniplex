/**********************************************/
/* Скрипт, отвечающий за зонки
 * 
 * Создан Белым Койотом                       
/**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zonk : MonoBehaviour {

    public float speed = 3;//скорость движения зонка
    public GameObject explosionBlock;//блок взрыва

    private Vector3 nextCell;//следующая позиция для движения
    public bool moving;//движется ли зонк сейчас
    private RaycastHit hit;//луч
    public bool danger;//опасен ли в движении зонк
    private int dir;//направление движения

    public GameObject levelManager;//менеджер уровня

    public AudioSource sounds;//звуки

    public bool dangerous;
    public bool canBeDestroy;
    public bool pushL;
    public bool pushR;

	// Use this for initialization
	void Start ()
    {
        sounds = GetComponent<AudioSource>();//берем все звуки с объекта
        dir = -1;//говорим, что никуда не двигаемся
	}
	
	// Update is called once per frame
	void Update () 
    {
        /*
        if (!moving)//если не в движении
            CheckSpace();//то ищем, можно ли куда-то двигаться
        else //если есть, куда двигаться, то
            Fall();//двигаемся
        */
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
        }
        //иначе, сели мы будем падать дальше и под нами есть игрок, то мы должны говорить, что мы опасны danger = true;
        else if (levelManager.GetComponent<LevelManager>().levelState[(int)transform.position.x, (int)transform.position.y - 1] == -1 ||
                 levelManager.GetComponent<LevelManager>().levelState[(int)transform.position.x, (int)transform.position.y - 1] == -99 &&
                 danger)
        {
            //говорим менджеру уровня, что зонк движется на новую ячейку и записываем туда 3 - код зонка
            levelManager.GetComponent<LevelManager>().levelState[(int)transform.position.x, (int)transform.position.y - 1] = 3;
            //говорим, что направление движения вниз
            dir = 0;
            //записываем позицию ячейки, куда движется зонк
            nextCell = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
            //говорим, что зонк движется
            moving = true;
            //говорим, что зонк опасен в движении
            danger = true;
        }
        // иначе, если вниз двигататься нельзя, мы смотри, стоит ли зонк на Зонке, инфортроне или блоке ram памяти, код инфотрона и зонка в массиве уровня равен 3
        else if (levelManager.GetComponent<LevelManager>().levelState[(int)transform.position.x, (int)transform.position.y - 1] == 3 ||
                 levelManager.GetComponent<LevelManager>().levelState[(int)transform.position.x, (int)transform.position.y - 1] == 4)
        {
            //если ячейка справа и справа-снизу от зонка свободна, т.е. он может двигаться право и вниз
            if (levelManager.GetComponent<LevelManager>().levelState[(int)transform.position.x + 1, (int)transform.position.y] == -1 &&
                levelManager.GetComponent<LevelManager>().levelState[(int)transform.position.x + 1, (int)transform.position.y - 1] == -1)
            {
                //говорим менджеру уровня, что зонк движется на новую ячейку и записываем туда 3- код зонка
                levelManager.GetComponent<LevelManager>().levelState[(int)transform.position.x + 1, (int)transform.position.y] = 3;
                //говорим, что направление движения вправо
                dir = 1;
                //записываем позицию ячейки, куда движется зонк
                nextCell = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
                //говорим, что зонк движется
                moving = true;
                //говорим, что зонк опасен в движении
                danger = true;
            }
            //если справа некуда двигаться, то смотрим, свободно ли движение влево и вниз?
            else if (levelManager.GetComponent<LevelManager>().levelState[(int)transform.position.x - 1, (int)transform.position.y] == -1 &&
                levelManager.GetComponent<LevelManager>().levelState[(int)transform.position.x - 1, (int)transform.position.y - 1] == -1)
            {
                //если свободно, то
                //говорим менджеру уровня, что зонк движется на новую ячейку и записываем туда 3- код зонка
                levelManager.GetComponent<LevelManager>().levelState[(int)transform.position.x - 1, (int)transform.position.y] = 3;
                //говорим, что направление движения влево
                dir = 2;
                //записываем позицию ячейки, куда движется зонк
                nextCell = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
                //говорим, что зонк движется
                moving = true;
                //говорим, что зонк опасен в движении
                danger = true;
            }
        }
        else danger = false;//если нам некуда двигаться, то говорим, что мы не опасны
    }

    void Fall()//функция, которая отвечает за движение в выбранном направлении
    {
        switch (dir)//смотрим, чему равно направление
        {
            case 0:/* down */
                {
                    //если расстояние до ячейки, где должен оказаться зонк больше 0.1
                    if (Vector3.Distance(transform.position, nextCell) > 0.1f)
                        transform.Translate(-Vector3.up * Time.deltaTime * speed);//то движемся к ней
                    else //как только приблизились к ней
                    {
                        //transform.GetChild(0).transform.localRotation = new Quaternion(0,0,0,0);
                        sounds.Play();//играем звук движения зонка
                        transform.position = nextCell;//ставим зонку конечную позицию для движения
                        //записываем в массив уровня то, что зонк занимает новую и освободил старую ячейку
                        levelManager.GetComponent<LevelManager>().levelState[(int)transform.position.x, (int)transform.position.y + 1] = -1;
                        levelManager.GetComponent<LevelManager>().levelState[(int)transform.position.x, (int)transform.position.y] = 3;
                        moving = false;//не движемся
                        dir = -1;//сбрасываем направление движения
                    }
                    break;
                }
            case 1:/* right */
                {
                    //если расстояние до ячейки, где должен оказаться зонк больше 0.1
                    if (Vector3.Distance(transform.position, nextCell) > 0.1f)
                    {
                        transform.Translate(Vector3.right * Time.deltaTime * speed);//то движемся к ней
                        transform.GetChild(0).transform.RotateAround(Vector3.back, 20.5f * Time.deltaTime);//крутим зонк для визуализации движения
                    }
                    else
                    {
                        sounds.Play();//играем звук движения зонка
                        transform.position = nextCell;//ставим зонку конечную позицию для движения
                        //записываем в массив уровня то, что зонк занимает новую и освободил старую ячейку
                        levelManager.GetComponent<LevelManager>().levelState[(int)transform.position.x - 1, (int)transform.position.y] = -1;
                        levelManager.GetComponent<LevelManager>().levelState[(int)transform.position.x, (int)transform.position.y] = 3;
                        moving = false;//не движемся
                        dir = -1;//сбрасываем направление движения
                    }
                    break;
                }
            case 2:/* left */
                {
                    //если расстояние до ячейки, где должен оказаться зонк больше 0.1
                    if (Vector3.Distance(transform.position, nextCell) > 0.1f)
                    {
                        transform.Translate(-Vector3.right * Time.deltaTime * speed);//то движемся к ней
                        transform.GetChild(0).transform.RotateAround(-Vector3.back, 20.5f * Time.deltaTime);//крутим зонк для визуализации движения
                    }
                    else
                    {
                        sounds.Play();//играем звук движения зонка
                        transform.position = nextCell;//ставим зонку конечную позицию для движения
                        //записываем в массив уровня то, что зонк занимает новую и освободил старую ячейку
                        levelManager.GetComponent<LevelManager>().levelState[(int)transform.position.x + 1, (int)transform.position.y] = -1;
                        levelManager.GetComponent<LevelManager>().levelState[(int)transform.position.x, (int)transform.position.y] = 3;
                        moving = false;//не движемся
                        dir = -1;//сбрасываем направление движения
                    }
                    break;
                }
        }
    }

    void OnCollisionEnter(Collision coll)//если зонк столкнулся с кем-то
    {
        if (danger || moving)//если в движении и опасен
        {
            if (coll.collider.GetComponent<Murphy>() != null)//и столкнулись с игроком
                GameOver(coll.collider.gameObject);//конец игры
        }
    }

    void GameOver(GameObject player)//фукнция конца игры
    {
        Debug.Log("YOU DEAD");//отладка
        //GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).parent = null;//убираем камеру из иерархии мерфи
        Destroy(this.gameObject);
        SelfDestruction();//саморазрушаемся
    }

    void SelfDestruction()//функция саморазрушения, вызывается, когда игрок встретился с зонком в движении зонка
    {
        //создаются 9 блоков взрыва в токой очередности
        // ***
        // *0*
        // ***
        // где * - блок взрыва, а 0 - зонк
        /* up */
        GameObject ex1 = Instantiate(explosionBlock);
        ex1.transform.position = new Vector3(nextCell.x, nextCell.y + 1, 0);
        /* down */
        GameObject ex2 = Instantiate(explosionBlock);
        ex2.transform.position = new Vector3(nextCell.x, nextCell.y - 1, 0);
        /* left */
        GameObject ex3 = Instantiate(explosionBlock);
        ex3.transform.position = new Vector3(nextCell.x - 1, nextCell.y, 0);
        /* right */
        GameObject ex4 = Instantiate(explosionBlock);
        ex4.transform.position = new Vector3(nextCell.x + 1, nextCell.y, 0);
        /* up-left */
        GameObject ex5 = Instantiate(explosionBlock);
        ex5.transform.position = new Vector3(nextCell.x - 1, nextCell.y + 1, 0);
        /* up-right */
        GameObject ex6 = Instantiate(explosionBlock);
        ex6.transform.position = new Vector3(nextCell.x + 1, nextCell.y + 1, 0);
        /* down-left */
        GameObject ex7 = Instantiate(explosionBlock);
        ex7.transform.position = new Vector3(nextCell.x - 1, nextCell.y - 1, 0);
        /* down-right */
        GameObject ex8 = Instantiate(explosionBlock);
        ex8.transform.position = new Vector3(nextCell.x + 1, nextCell.y - 1, 0);
        /* myself */
        GameObject ex9 = Instantiate(explosionBlock);
        ex9.transform.position = new Vector3(nextCell.x, nextCell.y, 0);
        Destroy(gameObject);
    }
}
