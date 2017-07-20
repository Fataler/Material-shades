using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBounds : MonoBehaviour {
    public static float ScreenWidth;
    public static float ScreenHeight;
    public static Vector3 blockSize;
    public float rows;
    public Colors color;
    GameObject brick;
    public bool isMoving;
    public bool ifFalling;
  
    //public GameObject brick;


    
    //public GameObject camera;
    // Use this for initialization
    void Start () {
       

        //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cube.AddComponent<Rigidbody>();
        
        
    }

    
	
	// Update is called once per frame
	void Update () {
        
         /*if (Input.GetKeyDown(KeyCode.RightArrow)&&!isMoving){
            isMoving = true;
            iTween.MoveTo(brick, iTween.Hash(new object[]
             {
                "x",
                brick.transform.position.x+blockSize.x,
                "time",
                0.45f,
                "easeType",
                "easeOutSine"
             }));
            isMoving = false;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)&&!isMoving)
        {
            isMoving = true;
            iTween.MoveTo(brick, iTween.Hash(new object[]
            {
                "x",
                brick.transform.position.x-blockSize.x,
                "time",
                0.45f,
                "easeType",
                "easeOutSine"
            }));
            isMoving = false;
        }*/
    }
}
