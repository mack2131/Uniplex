/**********************************************/
/* Скрипт, отвечающий за сник-снэк
 *  
 * Создан Белым Койотом
/**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnikSnak : MonoBehaviour {
    
    public AnimationClip move;//анимация ножниц
    private Animation anim;//глобальный параметр анимации

	// Use this for initialization
	void Start () 
    {
        anim = transform.GetChild(0).GetComponent<Animation>();//берем все анимации с модели
	}

    void Update()
    {
        anim.CrossFade(move.name);
    }
}
