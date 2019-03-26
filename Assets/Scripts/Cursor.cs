using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Cursor : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        //transform.position = Input.mousePosition;
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.transform as RectTransform, Input.mousePosition, transform.parent.GetComponent<Canvas>().worldCamera, out pos);
        transform.position = transform.parent.TransformPoint(pos);
        transform.position = new Vector3(transform.position.x + 7.5f, transform.position.y - 7.5f, 10);

    }
}
