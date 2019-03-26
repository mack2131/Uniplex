/**********************************************/
/* Скрипт, отвечающий за электрон
 * 
 * Создан Белым Койотом
/**********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electron : MonoBehaviour {

	public GameObject levelManager;//менеджер уровня
    private Vector3 nextCell;//следующая ячейка, куда двигаться
    private int dir;//код напрпавления
    private float speed = 3;//скорость движения
    private bool moving;//находится ли объект в движении
    public Transform leftSideObject;//объект, с которого будет выпускаться луч для проверки свободы левой стороны
    private RaycastHit hit;//луч
    private Vector3 prevPos;//позиция, откуда объект начал движение
    private float timer;//счетчик, который отвечает за количетсво секунд между поворотом, когда нет пути
    public GameObject explosionBlock;//блок вызрыва

	// Use this for initialization
	void Start () 
    {
	}
	
	
}
