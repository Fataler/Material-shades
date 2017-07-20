using UnityEngine;
using System.Collections;


//[ProtoContract]
public class BlockData
{
    //[ProtoMember(1)]
    public int column
    {
        get;
        set;
    }

   // [ProtoMember(2)]
    public int row
    {
        get;
        set;
    }

    //[ProtoMember(3)]
    public int code
    {
        get;
        set;
    }
}
