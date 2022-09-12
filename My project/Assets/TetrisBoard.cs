using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TetrisBoard : MonoBehaviour
{
    [SerializeField] GameObject border;
    [SerializeField] GameObject parentObject;
    [SerializeField] int length, width;


    [ContextMenu("Build Border")]
    public void makeBorder(){

        GameObject[] currentBorder = GameObject.FindGameObjectsWithTag("Border");

        foreach(GameObject gameObject in currentBorder){
            GameObject.DestroyImmediate(gameObject);
        }

        GameObject childObject;
        //ew Vector3(0,i + .5f,0)
        for(int i = 0; i < length; i++){
            childObject = Instantiate(border, parentObject.transform.localPosition + new Vector3(0, i + .5f,0), Quaternion.identity);
            childObject.transform.parent = parentObject.transform;

            childObject = Instantiate(border, parentObject.transform.localPosition + new Vector3(width,i + .5f,0), Quaternion.identity);
            childObject.transform.parent = parentObject.transform;
        }

        for(int i = 0; i < width; i++){
            childObject = Instantiate(border, parentObject.transform.localPosition + new Vector3(i + .5f,0,0), Quaternion.Euler(0,0,90));
            childObject.transform.parent = parentObject.transform;

            childObject = Instantiate(border, parentObject.transform.localPosition + new Vector3(i + .5f,length,0), Quaternion.Euler(0,0,90));
            childObject.transform.parent = parentObject.transform;
        }
    }

    void Update()
    {
        
    }
}
