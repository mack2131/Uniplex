/**********************************************/
/* Скрипт, отвечающий за красную дискету
 *  
 * Создан Белым Койотом
/**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedDisc : MonoBehaviour {

    private bool startEating;//начали кушать
    private GameObject murphy;//объект мёрфи
    private int dir;//направление
    public GameObject levelManager;//менеджер уровня
    Vector3 mPrevPos;//позиция мёрфи до того, как он начал кушать этот блок
    public bool placedByMurphy;
    private float counter;
    public float detonationTime;
    public GameObject explosionBlock;
    public AudioSource sounds;//звуки
    public bool chained;
    private float chainedTime;


	// Use this for initialization
	void Start () 
    {
        sounds = GetComponent<AudioSource>();//собираем все звуки с объекта
	}
	
	// Update is called once per frame
	void Update () 
    {

        /*ChainedExplosion();
        if (!placedByMurphy)
            DestroyBlock();//функция, которая отвечает за поедание блока
        else ItIsABomb();*/
	}
}
