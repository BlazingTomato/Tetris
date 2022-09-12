using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
   
    [SerializeField]
    bool[] gridX;

    [SerializeField]
    bool[] gridY;

    bool[,] grid = new bool[10,20];

    [SerializeField]
    GameObject currentGroup;
    public Spawner spawner;

    [SerializeField]
    float fallTime;
    float fallTimePased;

    Transform[] allBlocks;

    float lockTime;
    bool lockStart;



   
    private void Start() {
        fallTime = 1;
        fallTimePased = 0;

        lockTime = -1;
        lockStart = false;

        currentGroup = null;
    
    }


    void Update()
    {

        if(currentGroup == null)
            return;
    
    
        if(Input.GetKeyDown(KeyCode.RightArrow))
            tryMoveRight();
        
        if(Input.GetKeyDown(KeyCode.LeftArrow))
            tryMoveLeft();

        if(Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.UpArrow))
            tryCW();
        
        if(Input.GetKeyDown(KeyCode.Z))
            tryCCW();

        if(Input.GetKeyDown(KeyCode.Space))
            dropEntirely();

        fallTimePased += Time.deltaTime;

        if(fallTimePased >= fallTime){
            fallTimePased = 0;

            dropGroup();
        }

        lockTimeUpdate();
    }

    public void setCurrentGroup(GameObject group){
        currentGroup = group;
        allBlocks = currentGroup.GetComponentsInChildren<Transform>();
    }

    //get lowest blocks for each x value
    void dropGroup(){
        bool canMove = true;
        
        foreach(Transform block in allBlocks){
            Vector2 position = block.position;

            int x = Mathf.RoundToInt((position.x+1.8f)/.4f);
            int y = Mathf.RoundToInt((position.y+3.8f)/.4f);

            try{
                if(grid[x,y-1]) canMove = false;
            }catch(Exception){
                canMove = false;
            }
            
        }

        if(canMove)
            currentGroup.GetComponent<Transform>().position += new Vector3(0,-.4f,0);
        else
            lockStart = true;
    }
    
    void tryMoveLeft(){
        bool canMove = true;
        
        foreach(Transform block in allBlocks){
            Vector2 position = block.position;

            int x = Mathf.RoundToInt((position.x+1.8f)/.4f);
            int y = Mathf.RoundToInt((position.y+3.8f)/.4f);

            try{
                if(grid[x-1,y]) canMove = false;
            }catch(Exception){
                canMove = false;
            }
            
        }

        if(canMove){
           currentGroup.GetComponent<Transform>().position += new Vector3(-.4f,0f,0);
        }

    }

    void tryMoveRight(){
        bool canMove = true;
        
        foreach(Transform block in allBlocks){
            Vector2 position = block.position;

            int x = Mathf.RoundToInt((position.x+1.8f)/.4f);
            int y = Mathf.RoundToInt((position.y+3.8f)/.4f);

            try{
                if(grid[x+1,y]) canMove = false;
            }catch(Exception){
                canMove = false;
            }
            
        }

        if(canMove){
            currentGroup.GetComponent<Transform>().position += new Vector3(.4f,0f,0);
        }

    }

    void tryCCW(){
        bool canMove = true;

        foreach(Transform block in allBlocks){
            Vector3 rotationPoint = block.position - currentGroup.GetComponent<Transform>().position;
            Vector3 newPosition = new Vector3(-rotationPoint.y, rotationPoint.x);
            newPosition += currentGroup.GetComponent<Transform>().position;
            

            int x = Mathf.RoundToInt((newPosition.x+1.8f)/.4f);
            int y = Mathf.RoundToInt((newPosition.y+3.8f)/.4f);

            try{
                if(grid[x,y]) canMove = false;
            }catch(Exception){
                canMove = false;
            }
            
        }

        if(canMove){
            foreach(Transform block in allBlocks){
                Vector3 rotationPoint = block.position - currentGroup.GetComponent<Transform>().position;
                Vector3 newPosition = new Vector3(-rotationPoint.y, rotationPoint.x);
                newPosition += currentGroup.GetComponent<Transform>().position;
                block.GetComponent<Transform>().position = newPosition;
            }
        }
    }

    void tryCW(){
        bool canMove = true;

        foreach(Transform block in allBlocks){
            Vector3 rotationPoint = block.position - currentGroup.GetComponent<Transform>().position;
            Vector3 newPosition = new Vector3(rotationPoint.y, -rotationPoint.x);
            newPosition += currentGroup.GetComponent<Transform>().position;
            

            int x = Mathf.RoundToInt((newPosition.x+1.8f)/.4f);
            int y = Mathf.RoundToInt((newPosition.y+3.8f)/.4f);

            try{
                if(grid[x,y]) canMove = false;
            }catch(Exception){
                canMove = false;
            }
            
        }

        if(canMove){
            foreach(Transform block in allBlocks){
                Vector3 rotationPoint = block.position - currentGroup.GetComponent<Transform>().position;
                Vector3 newPosition = new Vector3(rotationPoint.y, -rotationPoint.x);
                newPosition += currentGroup.GetComponent<Transform>().position;
                block.GetComponent<Transform>().position = newPosition;
            }
        }
    }
   
    void dropEntirely(){
        int i = 100;

        foreach(Transform block in allBlocks){
            Vector2 position = block.position;

            int x = Mathf.RoundToInt((position.x+1.8f)/.4f);
            int y = Mathf.RoundToInt((position.y+3.8f)/.4f);

            bool canMove = true;
            int j = 0;

            while(canMove){
                try{
                    if(grid[x,y-j]) canMove = false;
                }catch(Exception){
                    canMove = false;
                }
                j++;
            }
            
            i = Math.Min(i,j);
        }

        currentGroup.GetComponent<Transform>().position += new Vector3(0,-.4f*(i-2),0);
        gridUpdate();
    }
    
    void lockTimeUpdate(){
        if(!lockStart)
            return;

        if(lockTime < .5){
            lockTime += Time.deltaTime;
        }
        else{
            gridUpdate();
            lockStart = false;
            lockTime = 0;
        }
    } 
     
    void gridUpdate(){
        foreach(Transform block in allBlocks){
            Vector2 position = block.position;

            int x = Mathf.RoundToInt((position.x+1.8f)/.4f);
            int y = Mathf.RoundToInt((position.y+3.8f)/.4f);
            
            try{
                grid[x,y] = true;
            }catch(Exception){}
        }

        clearLines();
        spawner.MakeBlock();
    }

    void clearLines(){
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");

        for(int i = 0; i < 20; i++){
            int j = 0;

            for(j = 0; j < 10; j++){
                if(!grid[j,i]) break;
            }
            if(j == 9){
                foreach(GameObject position in blocks){
                    int y = Mathf.RoundToInt((position.GetComponent<Transform>().position.y+3.8f)/.4f);
                    if(y == i){
                        GameObject.Destroy(position);
                    }
                }
            }
            


        }
    }
}

    
     
     
     /*
    Spawn: only if isPlaced = true -> isPlaced = false & spawn block
    Fall: Every Delta Seconds, Drop group by 1 if there are no blocks below it

    Grid: a 10x20 array, 
        array -> position: x is dilated by .4 + (-1.8), y is dilated by .4 + 3.8

    Update every delta Seconds, drop each block by 1 only if there are no blocks below it

    */
