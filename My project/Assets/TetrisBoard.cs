using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TetrisBoard : MonoBehaviour
{
    [SerializeField] GameObject border;
    [SerializeField] GameObject borderWhite;
    [SerializeField] GameObject parentObject;
    [SerializeField] int height, width;



    [ContextMenu("Build Border")]
    public void makeBorder(){

        GameObject[] currentBorder = GameObject.FindGameObjectsWithTag("Border");

        foreach(GameObject gameObject in currentBorder){
            GameObject.DestroyImmediate(gameObject);
        }

        GameObject childObject;
        //ew Vector3(0,i + .5f,0)
        for(int i = 0; i < height; i++){
            childObject = Instantiate(border, parentObject.transform.localPosition + new Vector3(0, i + .5f,0), Quaternion.identity);
            childObject.transform.parent = parentObject.transform;

            childObject = Instantiate(border, parentObject.transform.localPosition + new Vector3(width,i + .5f,0), Quaternion.identity);
            childObject.transform.parent = parentObject.transform;
        }

        for(int i = 0; i < width; i++){
            childObject = Instantiate(border, parentObject.transform.localPosition + new Vector3(i + .5f,0,0), Quaternion.Euler(0,0,90));
            childObject.transform.parent = parentObject.transform;

            childObject = Instantiate(border, parentObject.transform.localPosition + new Vector3(i + .5f,height,0), Quaternion.Euler(0,0,90));
            childObject.transform.parent = parentObject.transform;
        }

        for(int k = 1; k < 10; k++){
            for(int i = 0; i < height; i++){
                childObject = Instantiate(borderWhite, parentObject.transform.localPosition + new Vector3(k*.4f, i + .5f,0), Quaternion.identity);
                childObject.transform.parent = parentObject.transform;
            }
        }

        for(int k = 1; k < 20; k++){
            for(int i = 0; i < width; i++){
                childObject = Instantiate(borderWhite, parentObject.transform.localPosition + new Vector3(i + .5f,k*.4f,0), Quaternion.Euler(0,0,90));
                childObject.transform.parent = parentObject.transform;
            }
        }
        
    }
}
