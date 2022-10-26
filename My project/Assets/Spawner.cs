using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    GameObject[] groups;
    
    public GameObject SpawnPosition;
    public GameController gameController;

    

    //spawn position in grid = 4,18
    public void MakeBlock(){
        GameObject gameObject = Instantiate(groups[Random.Range(0,7)], SpawnPosition.transform.position, Quaternion.identity);
        gameObject.transform.parent = SpawnPosition.transform;

        gameController.setCurrentGroup(gameObject);
    }
    
    private void Update() {
        
    }




}
