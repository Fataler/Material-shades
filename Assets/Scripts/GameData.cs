using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : Singleton<GameData>
{
    public bool resumedGame;

    public bool saveable = true;

    public int score;

    public int rows;

    public int pointLevel;

    public int colorLevel;

    public int pointsPerRow = 200;

    public int pointsPerBlock = 2;

    public int columns = 4;

    public int fallingShades;

    public int shadesSet;

    public float pace;

    public float currentPace = 10f;

    public float paceIncrease;

    public float pointsInterval;

    public int nextColor;

    public MyBlock currentBlock;

    public BlockStack stack;

    //public Save savedGame;

    public BlockData currentBlockData;

    public List<List<BlockData>> stackData;

    public int countClearScreen;

    public int countRowsFromClearScreen;

    public int countCombos;

    public int countRowsWithOneBlock;

    public GameData(){
    }

    public GameData(bool newGame)
    {
        if (newGame) //|| !this.initializeSavedGame()
        {
            this.resumedGame = false;
            this.score = 0;
            this.rows = 0;
            this.pointLevel = 1;
            this.colorLevel = 1;

            this.columns = 3; //Singleton<User>.Instance.columnsNumber;
            this.fallingShades = 2;
            this.currentBlock = null;
            this.pointsInterval = 500f;
            //this.updatePaceWithLevel();
            this.stack = new BlockStack(this.columns);
            this.countClearScreen = 0;
            this.countRowsFromClearScreen = 0;
            this.countCombos = 0;
            this.countRowsWithOneBlock = 0;
        }
        else
        {
            this.resumedGame = true;
        }
    }

    public GameData initialize()
    {
        /*if (!this.initializeSavedGame())
        {
            return this.initialize(Singleton<User>.Instance.columnsNumber);
        }*/
        this.resumedGame = true;
        return this;
    }

    public GameData initialize(int columns)
    {
        this.resumedGame = false;
        this.score = 0;
        this.rows = 0;
        this.pointLevel = 2;
        this.colorLevel = 2;
        this.columns = columns;
        this.fallingShades = 2;
        this.currentBlock = null;
        this.pointsInterval = 500f;
        this.updatePaceWithLevel();
        this.stack = null;
        this.stack = new BlockStack(this.columns);
        this.countClearScreen = 0;
        this.countRowsFromClearScreen = 0;
        this.countCombos = 0;
        this.countRowsWithOneBlock = 0;
        return this;
    }

    public void updatePaceWithLevel()
    {
        //this.updatePaceWithLevel(Singleton<User>.Instance.difficultyLevel);
    }

    public void updatePaceWithLevel(int level)
    {
        float num = 18.52f;
        this.paceIncrease = 0.025f * this.pointsInterval / 100f;
        switch (level)
        {
            case 0:
                this.paceIncrease = 0.012f * this.pointsInterval / 100f;
                break;
            case 1:
                num /= (float)Math.Pow((double)(1f + this.paceIncrease), 3.0);
                this.paceIncrease = 0.021f * this.pointsInterval / 100f;
                break;
            case 2:
                num /= (float)Math.Pow((double)(1f + this.paceIncrease), 5.0);
                break;
        }
        this.currentPace = num;
    }
}
