using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBlock : MonoBehaviour {
    public Vector3 blockSize;
    public bool last;
    public bool curr;
    public float leftLimit;
    public float rightLimit;
    public int column;
    private bool isStopped;
    public int currColumn;
    public BlockData data;
	public Mesh mesh;
	private List<Vector3> newVertices = new List<Vector3>();
	private List<int> newTriangles = new List<int>();
	public Color color;
    public bool isStarted;
    public bool isMoving;
    public float paceScale;
    public bool isAccelerated;
    public bool shouldStop;
    public bool isBeingDragged;
    public bool shouldBeRemoved;
    public int connections;
    public Action connectAction;
    private float bounce = 0.2f;
    private Action bounceDownAction;
    private Action removeAction;

    public void initialize(Vector2 size, int code, int columnNumber)
    {
        this.data = new BlockData();
        this.data.column = columnNumber;
        this.currColumn = columnNumber;
        this.data.code = code;
        this.initialize();
    }

    private void initialize()
    {
        this.isStarted = false;
        this.isMoving = false;
        this.paceScale = 1f;
        this.shouldStop = false;
        
        this.isStopped = false;
        this.isBeingDragged = false;
        
        this.isAccelerated = false;
        this.shouldBeRemoved = false;
        
        this.connections = 0;
        this.start();
    }

    public void start()
    {
        this.isStarted = true;
        base.transform.position = new Vector2(MyBlock.xFromColumn(this.data.column), GameController.ScreenHeight / 2f - GameController.ScreenHeight * (GameController.ScreenWidth / GameController.ScreenHeight) * 0.025f);
        this.leftLimit = 0f;
        this.rightLimit = GameController.ScreenWidth;
    }

    // Use this for initialization
    void Start () {
        //brick = GameObject.CreatePrimitive(PrimitiveType.Cube);
		this.mesh = base.GetComponent<MeshFilter>().mesh;
		float z = 0f;
		this.newVertices.Add(new Vector3(0f, 0f, z));
		this.newVertices.Add(new Vector3(GameController.BlockSize.x, 0f, z));
		this.newVertices.Add(new Vector3(GameController.BlockSize.x, GameController.BlockSize.y, z));
		this.newVertices.Add(new Vector3(0f, GameController.BlockSize.y, z));
		this.newTriangles.Add(0);
		this.newTriangles.Add(1);
		this.newTriangles.Add(3);
		this.newTriangles.Add(1);
		this.newTriangles.Add(2);
		this.newTriangles.Add(3);
		this.mesh.Clear();
		this.mesh.vertices = this.newVertices.ToArray();
		this.mesh.triangles = this.newTriangles.ToArray();
		this.mesh.RecalculateNormals();
		this.setColor(Singleton<Colors>.Instance.shade(1));
    }
 	
	 
    // Update is called once per frame
    void Update () {
		
	}


    private bool isEmptyAtPosition(Vector2 position)
    {
        int columnNumber = MyBlock.columnFromX(position.x) + 1;
        int rowNumber = MyBlock.rowFromY(position.y) - 1;
        MyBlock x = Singleton<GameData>.Instance.stack.blockInColumnAndRow(columnNumber, rowNumber);
        return x == null;
    }

    public static int columnFromX(float x)
    {
        return Mathf.RoundToInt((x + GameController.ScreenWidth / 2f - GameController.BlockSize.x / 2f) / GameController.BlockSize.x);
    }

    public static int rowFromY(float y)
    {
        return Mathf.RoundToInt((y + GameController.ScreenHeight / 2f) / GameController.BlockSize.y);
    }

    public static float xFromColumn(int column)
    {
        return GameController.BlockSize.x * (float)column - GameController.ScreenWidth / 2f;
    }

    public static float yFromRow(int row)
    {
        float num = GameController.BlockSize.y * (float)row - GameController.ScreenHeight / 2f;
        return num + GameController.bottomSpacing;
    }

	public void setColor(Color color)
	{
		this.color = color;
		MeshRenderer component = base.GetComponent<MeshRenderer>();
		component.material.color = color;
	}

    public void stop()
    {
        this.isStopped = true;
    }
    public void moveLeft()
    {
        if (!this.isMoving)
        {
            this.setColumnTo(this.data.column - 1, true);
        }
    }
    public void moveRight()
    {
        if (!this.isMoving)
        {
            this.setColumnTo(this.data.column + 1, true);
        }
    }
    public void setColumnTo(int columnNumber, bool animated)
    {
        if (this.isStopped)
        {
            return;
        }
        if (columnNumber != this.data.column && columnNumber >= 0 && columnNumber < Singleton<GameData>.Instance.columns)
        {
            
            if (this.data.column < columnNumber)
            {
                for (int i = this.data.column + 1; i <= columnNumber; i++)
                {
                    if (!this.isEmptyAtPosition(new Vector2(MyBlock.xFromColumn(i), base.transform.position.y + GameController.BlockSize.y / 2f)))
                    {
                        columnNumber = i - 1;
                    }
                }
            }
            else
            {
                for (int j = this.data.column - 1; j >= columnNumber; j--)
                {
                    if (!this.isEmptyAtPosition(new Vector2(MyBlock.xFromColumn(j), base.transform.position.y + GameController.BlockSize.y / 2f)))
                    {
                        columnNumber = j + 1;
                    }
                }
            }
            if (columnNumber != this.data.column)
            {
                this.isMoving = true;
                this.removeAnimation();
                int num = Math.Abs(this.data.column - columnNumber);
                this.data.column = columnNumber;
                float num2 = MyBlock.xFromColumn(columnNumber);
                if (animated)
                {
                    this.isMoving = true;
                    iTween.MoveTo(base.gameObject, iTween.Hash(new object[]
                    {
                    "x",
                    num2,
                    "time",
                    (float)num * 0.04f,
                    "oncompletetarget",
                    base.gameObject,
                    "oncomplete",
                    "stoppedMoving"
                    }));
                }
                else
                {
                    base.transform.position = new Vector2(num2, base.transform.position.y);
                    this.stoppedMoving();
                }
            }

           
        }
       


    }
    private void stoppedMoving()
    {
        this.isMoving = false;

    }

    public void remove()
    {
        UnityEngine.Object.Destroy(base.gameObject);
    }

    public void connect(MyBlock otherBlock, Action completionHandler)
    {
        this.connectAction = completionHandler;
        this.removeAnimation();
        //this.sounds.playBlend(this.data.code);
        this.data.code = Singleton<Colors>.Instance.nextColor(this.data.code);
        Color color = Singleton<Colors>.Instance.shade(this.data.code);
        List<MyBlock> list = Singleton<GameData>.Instance.stack.blocksAboveBlock(this);
        float num = 0.4f;
        if (list.Count > 0)
        {
            foreach (MyBlock current in list)
            {
                iTween.MoveTo(current.gameObject, iTween.Hash(new object[]
                {
                "y",
                MyBlock.yFromRow(current.data.row),
                "time",
                num,
                "easeType",
                "easeInSine"
                }));
            }
        }
        iTween.MoveTo(base.gameObject, iTween.Hash(new object[]
        {
            "y",
            MyBlock.yFromRow(this.data.row),
            "time",
            num,
            "oncompletetarget",
            base.gameObject,
            "oncomplete",
            "runConnectAction",
            "easeType",
            "easeInSine"
        }));
        iTween.ColorTo(base.gameObject, iTween.Hash(new object[]
        {
            "color",
            color,
            "time",
            num,
            "easeType",
            "easeInSine"
        }));
        iTween.ColorTo(otherBlock.gameObject, iTween.Hash(new object[]
        {
            "color",
            color,
            "time",
            num,
            "easeType",
            "easeInSine"
        }));
    }
    public void removeAnimation()
    {
        iTween.Stop(base.gameObject);
    }
    public Action bounceDownWithCompletion(Action completionHandler)
    {
        this.bounceDownAction = completionHandler;
        return delegate
        {
            float num = 0.15f;
            iTween.MoveBy(base.gameObject, iTween.Hash(new object[]
            {
            "y",
            GameController.BlockSize.y * this.bounce,
            "time",
            num,
            "easeType",
            "easeOutSine",
            "oncompletetarget",
            base.gameObject,
            "oncomplete",
            "runBounceDown"
            }));
        };
    }

    public Action removeWithCompletion(Action completionHandler)
    {
        this.removeAction = completionHandler;
        return delegate
        {
            if (completionHandler != null)
            {
                //this.sounds.playRow(this.data.code);
            }
            float num = 0.15f;
            iTween.ScaleBy(this.gameObject, iTween.Hash(new object[]
            {
            "y",
            1f + this.bounce,
            "time",
            num,
            "easeType",
            "easeOutSine",
            "oncompletetarget",
            this.gameObject,
            "oncomplete",
            "runRemove"
            }));
        };
    }
    private void runRemove()
    {
        float num = 0.25f;
        iTween.MoveBy(base.gameObject, iTween.Hash(new object[]
        {
            "y",
            -GameController.BlockSize.y * (1f + this.bounce),
            "time",
            num,
            "easeType",
            "easeInSine",
            "oncompletetarget",
            base.gameObject,
            "oncomplete",
            "runRemoveAction"
        }));
    }
    private void runRemoveAction()
    {
        if (this.removeAction != null)
        {
            this.removeAction();
        }
    }

    private void runBounceDown()
    {
        float num = 0.25f;
        iTween.MoveBy(base.gameObject, iTween.Hash(new object[]
        {
            "y",
            -GameController.BlockSize.y * (1f + this.bounce),
            "time",
            num,
            "easeType",
            "easeInSine",
            "oncompletetarget",
            base.gameObject,
            "oncomplete",
            "runBounceDownAction"
        }));
    }

    private void runBounceDownAction()
    {
        if (this.bounceDownAction != null)
        {
            this.bounceDownAction();
        }
    }

}
