/**********************************************/
/* Скрипт, отвечающий за главного персонажа - 
 * - Мёрфи.
 * Создан Белым Койотом
/**********************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Murphy : MonoBehaviour {

    [Tooltip("player movement speed")]
    public float speed;//скорость движения Мёерфи
    [Tooltip("if we can press move buttons")]
    public bool canMove;//можем ли нажимать кнопки на запрос движения
    [Tooltip("if we request for moving")]
    public bool moveRequest;//запрос на движение
    public int ammo;
    public Animation anim;
    public AnimationClip idle;
    public AnimationClip eat;

	// Use this for initialization
	void Start () 
    {
        /*
        canMove = true;//можно двигаться
        sounds = GetComponents<AudioSource>();//собираем все звуки с объекта
        anim = transform.GetChild(0).GetComponent<Animation>();//собираем всю анимацию с объекта модели мёрфи, который находится в детях у глобального объекта мёрфи
        ammo = 0;
        pCounter = 0;
        */
        anim = transform.GetChild(0).GetComponent<Animation>();
	}

    void Update()
    {
        if(GetComponent<Explosable>().canBeExploded)
        {
            GameObject.FindObjectOfType<LevelManager>().GameOver();
            Destroy(GameObject.FindObjectOfType<MurphyBehaviour>());
            Destroy(this.gameObject);
        }
    }

    public void AnimationIdle()
    {
        Debug.Log("idle");
        anim.CrossFade(idle.name);
    }

    public void AnimationEat()
    {
        Debug.Log("eat");
        anim.CrossFade(eat.name);
    }

}
