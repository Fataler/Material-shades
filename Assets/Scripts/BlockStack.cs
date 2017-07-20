using System;
using System.Collections.Generic;
using System.Linq;

public class BlockStack 
{
    public List<List<MyBlock>> stack = new List<List<MyBlock>>();

    public BlockStack(int columns)
    {
        for (int i = 0; i < columns; i++)
        {
            this.stack.Add(new List<MyBlock>(20));
        }
    }

    public BlockStack(List<List<MyBlock>> stack)
    {
        this.stack = stack;
    }

    public void addBlock(MyBlock block)
    {
        List<MyBlock> list = this.stack[block.data.column];
        if (!list.Contains(block))
        {
            list.Add(block);
            this.updateRowsInColumn(list);
        }
    }

    public void removeBlock(MyBlock block)
    {
        List<MyBlock> list = this.stack[block.data.column];
        if (list != null && list.Contains(block))
        {
            list.Remove(block);
            this.updateRowsInColumn(list);
        }
    }

    public MyBlock blockInColumnAndRow(int columnNumber, int rowNumber)
    {
        if (columnNumber < 0 || rowNumber < 0 || columnNumber >= this.stack.Count)
        {
            return null;
        }
        List<MyBlock> list = this.stack[columnNumber];
        if (list.Count < rowNumber + 1)
        {
            return null;
        }
        return list[rowNumber];
    }

    public List<MyBlock> blocksAboveBlock(MyBlock block)
    {
        List<MyBlock> list = this.stack[block.data.column];
        List<MyBlock> list2 = new List<MyBlock>(list.Count);
        for (int i = block.data.row; i <= list.Count; i++)
        {
            MyBlock block2 = this.blockInColumnAndRow(block.data.column, i);
            if (block2 != null)
            {
                list2.Add(block2);
            }
        }
        return list2;
    }

    public List<MyBlock> blocksAboveRow(int rowNumber)
    {
        List<MyBlock> list = new List<MyBlock>();
        foreach (List<MyBlock> current in this.stack)
        {
            for (int i = rowNumber + 1; i < current.Count; i++)
            {
                MyBlock block = current[i];
                if (!(block != null))
                {
                    break;
                }
                list.Add(block);
            }
        }
        return list;
    }

    public void performOnEachBlock(Action<MyBlock> action)
    {
        foreach (List<MyBlock> current in this.stack)
        {
            for (int i = 0; i < current.Count; i++)
            {
                MyBlock block = current[i];
                if (block != null)
                {
                    action(block);
                }
            }
        }
    }

    public List<MyBlock> column(int columnNumber)
    {
        if (columnNumber < 0)
        {
            columnNumber = 0;
        }
        else if (columnNumber > this.stack.Count)
        {
            columnNumber = this.stack.Count;
        }
        return this.stack[columnNumber];
    }

    public bool isEmptyAboveRow(int rowNumber)
    {
        foreach (List<MyBlock> current in this.stack)
        {
            if (current.Count > rowNumber)
            {
                return false;
            }
        }
        return true;
    }

    public bool isEmpty()
    {
        return this.isEmptyAboveRow(0);
    }

    public int fullRows()
    {
        int[] array = new int[this.stack.Count];
        int num = 0;
        foreach (List<MyBlock> current in this.stack)
        {
            array[num] = current.Count;
            num++;
        }
        return array.Min();
    }

    private void updateRowsInColumn(List<MyBlock> column)
    {
        for (int i = 0; i < column.Count; i++)
        {
            MyBlock block = column[i];
            if (block.data != null)
            {
                block.data.row = i;
            }
        }
    }
}
