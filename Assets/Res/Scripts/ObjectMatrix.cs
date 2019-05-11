using System;
using System.Collections.Generic;

public class ObjectMatrix<T>
{
    private T[,] matrix;
    private Random m_random;

    public ObjectMatrix(int rows, int columns)
    {
        Row = rows;
        Column = columns;
        matrix = new T[rows, columns];
        m_random = new Random();
    }

    public int Row { get; private set; }

    public int Column { get; private set; }

    public T this[int row, int column]
    {
        get
        {
            return matrix[row, column];
        }
        set
        {
            matrix[row, column] = value;
        }
    }

    // 获得一个随机空位
    public List<int[]> RandomEmptyIndex(int num)
    {
        List<int[]> emptyList = new List<int[]>();
        for (int i = 0; i < Row; i++)
        {
            for (int j = 0; j < Column; j++)
            {
                if (matrix[i, j] == null)
                    emptyList.Add(new int[] { i, j });
            }
        }
        List<int[]> resulst = new List<int[]>();
        for (int i = 0; emptyList.Count > 0 && i < num; i++)
        {
            int index = m_random.Next(0, emptyList.Count);
            resulst.Add(emptyList[index]);
            emptyList.RemoveAt(index);
        }
        return resulst;
    }

}
