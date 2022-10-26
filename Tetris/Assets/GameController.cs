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
    [SerializeField]
    GameObject dropImage;
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
        
        if(Input.GetKeyDown(KeyCode.DownArrow)){
            dropGroup();
            fallTimePased = 0;
        }

        fallTimePased += Time.deltaTime;

        if(fallTimePased >= fallTime){
            fallTimePased = 0;

            dropGroup();
        }

        lockTimeUpdate(false);
    }

    public void setCurrentGroup(GameObject group){
        currentGroup = group;
        allBlocks = currentGroup.GetComponentsInChildren<Transform>();
    }

    #region Movement Methods
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
        
        makeDropImage();
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

        makeDropImage();
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

        makeDropImage();
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

        makeDropImage();
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

        makeDropImage();
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
        lockStart = true;
        lockTimeUpdate(true);
    }

    
    public void makeDropImage(){
        GameObject.Destroy(dropImage);

        int i = 100;
        foreach(Transform block in allBlocks){
            Vector2 currentPosition = block.position;

            int x = Mathf.RoundToInt((currentPosition.x+1.8f)/.4f);
            int y = Mathf.RoundToInt((currentPosition.y+3.8f)/.4f);

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

        
        Vector2 position = currentGroup.GetComponent<Transform>().position + new Vector3(0,-.4f*(i-2),0);

        dropImage = GameObject.Instantiate(currentGroup,position,Quaternion.identity);

        foreach(SpriteRenderer spriteRenderer in dropImage.GetComponentsInChildren<SpriteRenderer>()){
            spriteRenderer.color -= new Color32(0,0,0,110);
        }
    } 
    #endregion

    void lockTimeUpdate(bool instant){
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

        if(instant){
            gridUpdate();
            lockStart = false;
            lockTime = 0;
        }
    } 
     
    void gridUpdate(bool spawnNew = false){

        grid = new bool[10,20];

        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");

        foreach(GameObject block in blocks){
            Vector2 position = block.GetComponent<Transform>().position;
            int x = Mathf.RoundToInt((position.x+1.8f)/.4f);
            int y = Mathf.RoundToInt((position.y+3.8f)/.4f);
            
            try{
                grid[x,y] = true;
                //Debug.Log("Grid Update: " + x + "," + y);
            }catch(Exception){}
        }


            clearLines();

    }

    void clearLines(){
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");

        for(int i = 0; i < 20; i++){
            for(int j = 0; j < 10; j++){
                if(!grid[j,i])
                    break;

                if(j == 9){
                    foreach(GameObject block in blocks){
                        Vector2 position = block.GetComponent<Transform>().position;
                        int x = Mathf.RoundToInt((position.x+1.8f)/.4f);
                        int y = Mathf.RoundToInt((position.y+3.8f)/.4f);
                        if(y == i){
                            grid[x,y] = false;
                            GameObject.Destroy(block);
                        }   
                    }

                    foreach(GameObject block in blocks){
                        Vector2 position = block.GetComponent<Transform>().position;
                        int x = Mathf.RoundToInt((position.x+1.8f)/.4f);
                        int y = Mathf.RoundToInt((position.y+3.8f)/.4f);

                        if(y > i){
                            grid[x,y] = false;
                            block.transform.position += new Vector3(0,-.4f,0);
                            grid[x,y-1] = true;
                        }
                    }
                    i--;
                }
            }
        }
       
        spawner.MakeBlock();
        makeDropImage();
    }

}

    
     
