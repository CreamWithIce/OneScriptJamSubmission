using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class win{
    public Vector2 size;
    public LayerMask companionMask;
    public Transform winpos;
    public void winCheck(GameObject winUI,bool won){
        RaycastHit2D hit = Physics2D.BoxCast(winpos.position,size,0f,new Vector2(0f,0f),Mathf.Infinity,companionMask);
        if(hit.collider != null){
            Time.timeScale = 0f;
            winUI.SetActive(true);
            won = true;
        }

    }

}

public class pauseGame{
    public bool pauseNunpause(bool pause){
        
        if(!pause){
            return true;
        }
        else{   
            return false;
        }
            
    }
}



public class moveCamera{
    public Transform PastDoor;
    public Transform companion;

    public Vector2 boxSize;
    public LayerMask mask;

    public Transform Camera;


    public float MoveCameraAmount;
    public Transform OriginalDoorPosition;

    public Vector2 newCompPos;

    bool isFirstTime = false;

    public void isPassedDoor(GameObject Door){
        RaycastHit2D hit = Physics2D.BoxCast(PastDoor.position,boxSize,0f,new Vector2(0f,0f),Mathf.Infinity,mask);
        if(hit.collider != null&&!isFirstTime){
            Door.SetActive(true);
            Camera.position = new Vector3(MoveCameraAmount,Camera.position.y,Camera.position.z);
            companion.position = newCompPos;
            isFirstTime = true;

        }
    }

}

public class changeRoom{
    public LayerMask compLayer;

    public Vector2 boxSize = new Vector2(0.5f,0.5f);

    public Transform depositBox;
    public Transform Camera;
    

    private bool doorMoved = false;

    // Rest if changeRoom class. Check for companion cube layer using physics
    public void changeRoomCheck(GameObject door){
        RaycastHit2D hit = Physics2D.BoxCast(depositBox.position,boxSize,0f,new Vector2(0f,0f),Mathf.Infinity,compLayer);
        if(hit.collider != null && doorMoved == false){
            door.SetActive(false);
            doorMoved = true;
        }
    }
}


public class pickUp{
    public GameObject player,companion;
    public LayerMask playerMask;
    public float maxDist = 10f;
    bool pickupBool = false;

    public Rigidbody2D compRb;
    public void Grab(){
        RaycastHit2D hit = Physics2D.Raycast(companion.transform.position, Vector2.up, maxDist,playerMask);
        if(pickupBool){
            companion.transform.position = new Vector3(player.transform.position.x,player.transform.position.y - player.transform.localScale.y,player.transform.position.z);
        }
        if(Input.GetAxis("PickUp")>=0.5f&&hit.collider!=null){
            switch(pickupBool){
                case true:
                    pickupBool = false;
                    break;
                case false:
                    pickupBool = true;
                    break;
            }
            //compRb.velocity = new Vector2(0f,0f);
        }
    }

}

public class movement{
    public Rigidbody2D rb;
    
    public float speed;
    public float speedCap =4f;
    // Update is called once per frame
    
    private float timeAtStart;
    private float xMove, yMove;

    public void move(){
        xMove = Input.GetAxisRaw("Horizontal");
        yMove = Input.GetAxisRaw("Vertical");
        
        Vector2 movement = new Vector2(xMove*speed,yMove*speed);
        if(rb.velocity.x >= speedCap && movement.x > 0f || rb.velocity.x <= -speedCap && movement.x < 0f){
            movement.x = 0f;
        }
        if(rb.velocity.y >= speedCap && movement.y > 0f || rb.velocity.y <= - speedCap && movement.y < 0f){
            movement.y = 0f;
        }
        
        rb.AddForce(movement,ForceMode2D.Impulse);
    }
}


public class cameraFollow{

    public Transform camera;
    Vector3 pos;

    public void followPlayer(Transform playerPos){
        pos = new Vector3(camera.position.x,playerPos.position.y,camera.position.z);
        
        camera.position = pos;
    }

}

public class Main : MonoBehaviour
{
    [Header("PauseMenu")]
    public bool paused = false;
    public GameObject pauseMenu;
    pauseGame pauseGame = new pauseGame();

    [Header("Movement")]
    public Rigidbody2D rb;
    public float speed;
    public float speedCap = 4f;
    movement move = new movement();

    [Header("Camera\n")]
    cameraFollow cam = new cameraFollow();
    public Transform camera;
    public Transform playerPos;

    [Header("Pickup")]
    public Rigidbody2D compRb;
    public GameObject player,companion;
    public float maxDist;
    public LayerMask mask;
    pickUp grab = new pickUp();


    [Header("ChangeRoom")]
    public LayerMask compLayer;
    public Vector2 boxSize = new Vector2(0.5f,0.5f);
    public Transform depositBox;
    public GameObject door;
    changeRoom changeRoomClass = new changeRoom();

    [Header("MoveCamera")]
    public Transform PastDoor;
    public Transform companionPos;
    public Vector2 doorCheckSize;
    public LayerMask playerMask;
    public Transform Camera;
    public GameObject Door;
    public float MoveCameraAmount;
    public Transform OriginalDoorPosition;
    public Vector2 newCompPos;
    moveCamera camMove = new moveCamera();

    [Header("Spikes")]
    public LayerMask spikeLayer;

    [Header("Win")]
    public Transform winpos;
    public LayerMask companionMask;
    public Vector2 checkSize;

    public GameObject winUI;
    bool won = false;
    win wonGame = new win();


    public void restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void quit(){
        Application.Quit();
    }

    public void resume(){
        paused = false;
    }

    void spikeCollision(){
        if(rb.IsTouchingLayers(spikeLayer) || compRb.IsTouchingLayers(spikeLayer)){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void Start(){
        move.rb = rb;
        move.speed = speed;
        move.speedCap = speedCap;

        cam.camera = camera;

        grab.player = player;
        grab.companion = companion;
        grab.maxDist = maxDist;
        grab.playerMask = mask;
        grab.compRb = compRb;


        changeRoomClass.compLayer = compLayer;
        changeRoomClass.boxSize = boxSize;
        changeRoomClass.depositBox = depositBox;
        

        camMove.PastDoor = PastDoor;
        camMove.companion = companionPos;
        camMove.boxSize = doorCheckSize;
        camMove.mask = playerMask;
        camMove.Camera = Camera;
        camMove.MoveCameraAmount = MoveCameraAmount;
        camMove.OriginalDoorPosition = OriginalDoorPosition;
        camMove.newCompPos=newCompPos;

        wonGame.size = checkSize;
        wonGame.winpos = winpos;
        wonGame.companionMask = companionMask;
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            paused = pauseGame.pauseNunpause(paused);
        if(won){
            paused = false;
        }
        if(!paused){
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
            move.move();
            cam.followPlayer(playerPos);

            grab.companion = companion;
            grab.Grab();


            changeRoomClass.changeRoomCheck(door);

            camMove.isPassedDoor(Door);

            wonGame.winCheck(winUI,won);
        }
        else if(paused){
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        }
        spikeCollision();
    }
}
