using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public int columns = 4;
    public int currColumn;
     
    public static float ScreenWidth;
    public static float ScreenHeight;
    public static Vector3 BlockSize;
    public static int rows;
    public static float bottomSpacing=1f;
	public MyBlock block;
    public GameData data;
    public bool isPaused=false;
    private int concurrentBlocksCount;
    private bool isPlaying=true;
    private MyBlock lastBlock;

    public void calculateParams()
    {
        ScreenHeight = 2f * Camera.main.orthographicSize;
        ScreenWidth = ScreenHeight * Camera.main.aspect;
        float num = ScreenWidth / columns;
        BlockSize = new Vector3(num, num / ((1f + Mathf.Sqrt(5f)) / 2f), 0.2f);
        rows = Mathf.RoundToInt(ScreenHeight / BlockSize.y);
    }

    // Use this for initialization
    void Start () {

        calculateParams();
        /*GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.position = new Vector3(0, 0);
        plane.transform.localScale = new Vector3(ScreenWidth / 2, 1f, 0.2f);*/
        this.data = Singleton<GameData>.Instance.initialize(4);
    }



    public void SpawnBlock()
    {
        GameObject brick = GameObject.CreatePrimitive(PrimitiveType.Cube);
        brick.name = "block";
        //brick.AddComponent<Rigidbody>();
        /*brick.GetComponent<Rigidbody>().constraints =   RigidbodyConstraints.FreezePositionZ|
                                                        RigidbodyConstraints.FreezeRotationX| 
                                                        RigidbodyConstraints.FreezeRotationY| 
                                                        RigidbodyConstraints.FreezeRotationZ;*/
        brick.transform.position = new Vector3((BlockSize.x/2), ScreenHeight, 0);
        brick.transform.localScale = BlockSize;
        brick.GetComponent<MeshRenderer>().material.color = UnityEngine.Random.ColorHSV();
         

         
    }


    public void addBlock(int rowNumber, int columnNumber, int colorIndex)
	{
		MyBlock block = UnityEngine.Object.Instantiate<MyBlock>(this.block);
		block.initialize(GameController.BlockSize, colorIndex, columnNumber);
		block.data.row = rowNumber;
		block.transform.position = new Vector2(MyBlock.xFromColumn(columnNumber), MyBlock.yFromRow(rowNumber));
        block.transform.Rotate(new Vector3(180f, 0f));
		block.transform.parent = base.transform;
		//this.data.stack.addBlock(block);
        data.currentBlock = block;
    }

    private void Awake()
    {
        

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //SpawnBlock();
            //SpawnBlock2d ();
            
            addBlock(rows, 1, 0);
            
        }
        if (data.currentBlock != null)
        {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        { 
            this.data.currentBlock.moveLeft();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            
            this.data.currentBlock.moveRight();
        }
        }
        if (this.data.currentBlock!=null)
        {
            float num = (!this.data.currentBlock.isAccelerated) ? this.data.currentPace : (this.data.currentPace * this.data.currentBlock.paceScale);
            float num2 = GameController.BlockSize.y / num;
            float num3 = this.data.currentBlock.transform.position.y - GameController.bottomSpacing - GameController.BlockSize.y / 2f - num2;
            int rowNumber = MyBlock.rowFromY(num3);
            MyBlock x = this.data.stack.blockInColumnAndRow(this.data.currentBlock.data.column, rowNumber);
            if (num3 > -GameController.ScreenHeight / 2f - GameController.BlockSize.y / 2f && x == null)
            {
                this.data.currentBlock.transform.position = new Vector2(this.data.currentBlock.transform.position.x, this.data.currentBlock.transform.position.y - num2);
            }
            else
            {
                int row = this.availableRowInColumn(this.data.currentBlock.data.column) + 1;
                this.data.currentBlock.transform.position = new Vector2(this.data.currentBlock.transform.position.x, MyBlock.yFromRow(row));
                this.stoppedBlock(this.data.currentBlock);
            }
        }
    }

    private int availableRowInColumn(int columnNumber)
    {
        List<MyBlock> list = this.data.stack.column(columnNumber);
        return list.Count - 1;
    }

    private void stoppedBlock(MyBlock block)
    {
        //this.showHighlight(false, 0f);
        block.stop();
        this.data.stack.addBlock(block);
        //this.addPoints(this.data.pointsPerBlock);
        this.data.currentBlock = null;
        
        if (block.data.row > 0)
        {
            MyBlock block2 = this.data.stack.blockInColumnAndRow(block.data.column, block.data.row - 1);
            int code = block2.data.code;
            int code2 = block.data.code;
            
        }
        if (block.data.row < rows)
        {
            addBlock(rows, 1, 1);

        }
        this.checkForMatchesBelow(block, true, false);
    }

    private Action checkForMatchesBelow(MyBlock block, bool connect, bool check)
    {
        MyBlock blockBelow = this.data.stack.blockInColumnAndRow(block.data.column, block.data.row - 1);
        if (blockBelow == null)
        {
            if (!this.isPaused && connect)
            {
                this.checkForFullRow();
            }
            return null;
        }
        bool flag = Singleton<Colors>.Instance.areTheSameColor(block.data.code, blockBelow.data.code);
        bool flag2 = !Singleton<Colors>.Instance.hasNextColor(block.data.code);
        if (block.data.row > 0 && blockBelow != null && flag && !flag2)
        {
            Action action = delegate
            {
                this.data.stack.removeBlock(blockBelow);
                blockBelow.shouldBeRemoved = true;
                Action completionHandler = delegate
                {
                    blockBelow.remove();
                    int points = Mathf.NextPowerOfTwo(block.connections * this.data.pointsPerBlock);
                    //this.addPoints(points);
                    //this.combo(block.connections);
                };
                if (check)
                {
                    completionHandler = delegate
                    {
                        blockBelow.remove();
                        int points = Mathf.NextPowerOfTwo(block.connections * this.data.pointsPerBlock);
                        //this.addPoints(points);
                        //this.combo(block.connections);
                        this.concurrentBlocksCount--;
                        if (this.concurrentBlocksCount == 0)
                        {
                            this.checkForAnyMatches();
                        }
                    };
                }
                else if (connect)
                {
                    completionHandler = delegate
                    {
                        blockBelow.remove();
                        int points = Mathf.NextPowerOfTwo(block.connections * this.data.pointsPerBlock);
                        //this.addPoints(points);
                        //this.combo(block.connections);
                        this.checkForMatchesBelow(block, true, false);
                    };
                }
                block.connect(blockBelow, completionHandler);
            };
            if (!this.isPaused && connect)
            {
                action();
                return null;
            }
            return action;
        }
        else
        {
            if (!this.isPaused && connect)
            {
                this.checkForFullRow();
                return null;
            }
            return null;
        }
    }


    private void checkForFullRow()
    {
        int num = 0;
        for (int i = 0; i < GameController.rows; i++)
        {
            int num2 = 0;
            int num3 = 0;
            for (int j = 0; j < this.data.columns; j++)
            {
                MyBlock block = this.data.stack.blockInColumnAndRow(j, i);
                if (block != null)
                {
                    if (j == 0)
                    {
                        num3 = block.data.code;
                    }
                    if (block.data.code == num3)
                    {
                        num2++;
                    }
                }
            }
            if (num2 == this.data.columns)
            {
                num++;
                this.removeRow(i);
                //this.addPoints(this.data.pointsPerRow);
                break;
            }
        }
        if (num == 0)
        {
            if (this.lastBlock != null && this.lastBlock.transform.position.y + GameController.BlockSize.y > GameController.ScreenHeight / 2f)
            {
                this.finish();
            }
            else
            {
                //this.spawnBlock();
                //addBlock(rows, 1, 0);
            }
        }
    }
    public void finish()
    {
        //this.hideIndicator(true);
        //this.showHighlight(false, 0f);
        this.data.saveable = false;
        //Singleton<User>.Instance.gamesPlayed++;
        //Singleton<User>.Instance.save();
        this.isPlaying = false;
        //this.hud.gameOver();
        
    }

    private void removeRow(int rowNumber)
    {
        if (!this.isPaused)
        {
            List<Action> list = new List<Action>(this.data.columns);
            List<MyBlock> list2 = this.data.stack.blocksAboveRow(rowNumber);
            for (int i = 0; i < this.data.columns; i++)
            {
                MyBlock block = this.data.stack.blockInColumnAndRow(i, rowNumber);
                this.data.stack.removeBlock(block);
                Action item = block.removeWithCompletion(delegate
                {
                    block.remove();
                });
                if (i == this.data.columns - 1 && list2.Count == 0)
                {
                    item = block.removeWithCompletion(delegate
                    {
                        block.remove();
                        this.checkForAnyMatches();

                    });
                }
                list.Add(item);
            }
            if (list2.Count > 0)
            {
                int num = 0;
                foreach (MyBlock current in list2)
                {
                    num++;
                    if (num == list2.Count)
                    {
                        Action item2 = current.bounceDownWithCompletion(delegate
                        {
                            this.checkForAnyMatches();
                        });
                        list.Add(item2);
                    }
                    else
                    {
                        Action item3 = current.bounceDownWithCompletion(null);
                        list.Add(item3);
                    }
                }
            }
            foreach (Action current2 in list)
            {
                current2();
            }

            if (this.data.stack.isEmpty())
            {
                this.data.countClearScreen++;

                this.data.countRowsFromClearScreen = 0;
            }
            this.data.countRowsWithOneBlock++;

        }
    }

    private void checkForAnyMatches()
    {
        this.concurrentBlocksCount = 0;
        List<Action> list = new List<Action>(this.data.columns);
        for (int i = 0; i < GameController.rows; i++)
        {
            for (int j = 0; j < this.data.columns; j++)
            {
                MyBlock block = this.data.stack.blockInColumnAndRow(j, i);
                if (block != null)
                {
                    Action action = this.checkForMatchesBelow(block, false, true);
                    if (action != null)
                    {
                        this.concurrentBlocksCount++;
                        list.Add(action);
                    }
                }
            }
            if (list.Count > 0)
            {
                foreach (Action current in list)
                {
                    current();
                }
                return;
            }
        }
       
    }

    private void  startBlock()
    {
        //addBlock(rows, 3, 0);
    }

}
