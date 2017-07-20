using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Colors : Singleton<Colors>
{

    public int currentSet;

    public List<List<string>> sets = new List<List<string>>(5);

    public List<Color> colors;

    public Colors()
    {
        string[] collection = new string[]
        {
            "f1ffc7",
            "a1e877",
            "00ac00",
            "006500",
            "003c00"
        };
        this.sets.Add(new List<string>(collection));
        string[] collection2 = new string[]
        {
            "8bf4fd",
            "46abe0",
            "2a699f",
            "193e61",
            "082641"
        };
        this.sets.Add(new List<string>(collection2));
        string[] collection3 = new string[]
        {
            "fffedb",
            "faf189",
            "ff9200",
            "c54200",
            "730b01"
        };
        this.sets.Add(new List<string>(collection3));
        string[] collection4 = new string[]
        {
            "ffe9fc",
            "edace8",
            "bc67b4",
            "7b3772",
            "49183b"
        };
        this.sets.Add(new List<string>(collection4));
        string[] collection5 = new string[]
        {
            "ffbebe",
            "fe767a",
            "bd0d0d",
            "720a0a",
            "380909"
        };
        this.sets.Add(new List<string>(collection5));
        this.loadGreens();
        this.currentSet = 0;
    }

    public void loadRandomColorSet()
    {
        int num;
        for (num = this.currentSet; num == this.currentSet; num = UnityEngine.Random.Range(0, this.colors.Count))
        {
        }
        this.loadSet(num);
    }

    public void loadSet(int setIndex)
    {
        switch (setIndex)
        {
            case 1:
                this.loadBlues();
                break;
            case 2:
                this.loadOranges();
                break;
            case 3:
                this.loadPurples();
                break;
            case 4:
                this.loadReds();
                break;
            default:
                this.loadGreens();
                break;
        }
        this.currentSet = setIndex;
        //Singleton<GameData>.Instance.shadesSet = setIndex;
    }

    public void loadGreens()
    {
        this.loadColors(this.sets[0]);
    }

    public void loadBlues()
    {
        this.loadColors(this.sets[1]);
    }

    public void loadOranges()
    {
        this.loadColors(this.sets[2]);
    }

    public void loadPurples()
    {
        this.loadColors(this.sets[3]);
    }

    public void loadReds()
    {
        this.loadColors(this.sets[4]);
    }

    private void loadColors(List<string> colors)
    {
        this.colors = new List<Color>();
        foreach (string current in colors)
        {
            this.colors.Add(this.hexColor(current));
        }
    }

    public int randomColor()
    {
        int num = Singleton<GameData>.Instance.fallingShades;
        if (num > this.colors.Count)
        {
            num = this.colors.Count;
        }
        else if (num <= 0)
        {
            num = 1;
        }
        return UnityEngine.Random.Range(0, num);
    }

    public bool hasNextColor(int code)
    {
        return this.nextColor(code) > code;
    }

    public int nextColor(int code)
    {
        if (this.colors.Count >= code + 2 && code + 1 < this.colors.Count)
        {
            return code + 1;
        }
        return code;
    }

    public bool isTopColor(int code)
    {
        return code + 1 >= this.colors.Count;
    }

    public bool areTheSameColor(int code1, int code2)
    {
        return code1 == code2;
    }

    public Color shade(int code)
    {
        return this.colors[code];
    }

    public Color hexColor(string hex)
    {
        byte r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
        return new Color32(r, g, b, 255);
    }
}
