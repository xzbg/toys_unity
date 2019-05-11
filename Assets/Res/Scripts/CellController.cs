using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

public class CellController : MonoBehaviour
{
    private ObjectMatrix<Cell> m_cellMatrix = null;
    private ObjectMatrix<Item> m_itemMatrix = null;
    private List<Item> m_operateList = null;

    public Item m_item_prefab;
    public Cell m_cell_prefab;


    // Use this for initialization
    void Start()
    {
        m_cellMatrix = new ObjectMatrix<Cell>(AppConst.Rows, AppConst.Columns);
        m_itemMatrix = new ObjectMatrix<Item>(AppConst.Rows, AppConst.Columns);
        m_operateList = new List<Item>();
    }

    // 初始化格子
    public void Init()
    {
        m_operateList.Clear();
        for (int i = 0; i < m_cellMatrix.Row; i++)
        {
            for (int j = 0; j < m_cellMatrix.Column; j++)
            {
                Cell cell = m_cellMatrix[i, j];
                if (cell == null)
                {
                    cell = Instantiate<Cell>(m_cell_prefab, this.transform);
                    cell.gameObject.name = String.Format("Cell_{0}_{1}", i, j);
                }
                else
                {
                    if (cell.item != null)
                    {
                        Destroy(cell.item.gameObject);
                        cell.item = null;
                    }
                }
                m_cellMatrix[i, j] = cell;
            }
        }
        RandomAdd(2);
    }

    // 需要优化获取随机数的方式，一次可以获取多个随机数
    public void RandomAdd(int num)
    {

        List<int[]> resulst = m_itemMatrix.RandomEmptyIndex(num);
        if (resulst == null || resulst.Count <= 0)
        {
            GameManager.Instance.GameOver();
            return;
        }
        for (int i = 0; i < resulst.Count; i++)
        {
            int row = resulst[i][0];
            int column = resulst[i][1];
            Cell cell = m_cellMatrix[row, column];
            cell.item = Instantiate<Item>(m_item_prefab, cell.transform);
            cell.item.Raw = row;
            cell.item.Column = column;
            cell.item.InitRandomValue();
            m_itemMatrix[row, column] = cell.item;
        }
    }

    public void DOMove(InputDirection input)
    {
        if (input == InputDirection.Left || input == InputDirection.Right)
            MoveHorizontal(input);
        if (input == InputDirection.Top || input == InputDirection.Bottom)
            MoveVertical(input);
    }

    // 水平移动
    public void MoveHorizontal(InputDirection horizontal)
    {
        int startIndex = 0;
        int endIndex = m_itemMatrix.Column - 1;
        int split = 1;
        if (horizontal == InputDirection.Right)
        {
            startIndex = m_itemMatrix.Column - 1;
            endIndex = 0;
            split = -1;
        }
        Item pre = null;
        Item current = null;
        m_operateList.Clear();
        for (int i = 0; i < m_itemMatrix.Row; i++)
        {
            current = null;
            pre = null;
            int index = startIndex;
            for (int j = startIndex; split == 1 ? j <= endIndex : j >= endIndex; j += split)
            {
                current = m_itemMatrix[i, j];
                if (current == null) continue;
                if (pre != null && pre.IsOverdue && pre.Value == current.Value)
                {
                    pre.IsOverdue = false;
                    current.IsOverdue = false;
                    pre.Value = pre.Value + current.Value;
                    current.IsRemove = true;
                    m_operateList.Add(current);
                }
                if (!current.IsRemove && index != j)
                {
                    current.NextRaw = i;
                    current.NextColumn = index;
                    m_operateList.Add(current);
                }
                if (!current.IsRemove)
                    index += split;
                pre = current;
            }
        }
        DOResult();
    }

    // 垂直移动
    public void MoveVertical(InputDirection vertical)
    {
        int startIndex = 0;
        int endIndex = m_itemMatrix.Row - 1;
        int split = 1;
        if (vertical == InputDirection.Bottom)
        {
            startIndex = m_itemMatrix.Row - 1;
            endIndex = 0;
            split = -1;
        }
        Item pre = null;
        Item current = null;
        m_operateList.Clear();
        for (int i = 0; i < m_itemMatrix.Column; i++)
        {
            current = null;
            pre = null;
            int index = startIndex;
            for (int j = startIndex; split == 1 ? j <= endIndex : j >= endIndex; j += split)
            {
                current = m_itemMatrix[j, i];
                if (current == null) continue;
                if (pre != null && pre.IsOverdue && pre.Value == current.Value)
                {
                    pre.IsOverdue = false;
                    current.IsOverdue = false;
                    pre.Value = pre.Value + current.Value;
                    current.IsRemove = true;
                    m_operateList.Add(current);
                }
                if (!current.IsRemove && index != j)
                {
                    current.NextRaw = index;
                    current.NextColumn = i;
                    m_operateList.Add(current);
                }
                if (!current.IsRemove)
                    index += split;
                pre = current;
            }
        }
        DOResult();
    }

    // 切换格子
    private void SwapCell(Item item)
    {
        if (item.NextRaw != -1 && item.NextColumn != -1)
        {
            Item nextItem = m_itemMatrix[item.NextRaw, item.NextColumn];
            if (nextItem != null)
            {
                SwapCell(nextItem);
            }
            Cell cell = m_cellMatrix[item.Raw, item.Column];
            Cell nextCell = m_cellMatrix[item.NextRaw, item.NextColumn];
            cell.item = null;
            nextCell.item = item;
            item.transform.SetParent(nextCell.transform, false);
            m_itemMatrix[item.Raw, item.Column] = null;
            m_itemMatrix[item.NextRaw, item.NextColumn] = item;
            item.Raw = item.NextRaw;
            item.Column = item.NextColumn;
            item.NextRaw = -1;
            item.NextColumn = -1;
        }
    }

    // 执行结果
    private void DOResult()
    {
        for (int i = 0; i < m_operateList.Count; i++)
        {
            if (m_operateList[i] == null) continue;
            Item item = m_operateList[i];
            if (item.IsRemove)
            {
                Cell cell = m_cellMatrix[item.Raw, item.Column];
                cell.item = null;
                item.transform.SetParent(null);
                m_itemMatrix[item.Raw, item.Column] = null;
                Destroy(item.gameObject);
                continue;
            }
            if (item.NextRaw != -1 && item.NextColumn != -1)
            {
                SwapCell(item);
            }
        }
        if (m_operateList.Count > 0)
        {
            for (int i = 0; i < m_itemMatrix.Row; i++)
            {
                for (int j = 0; j < m_itemMatrix.Column; j++)
                {
                    if (m_itemMatrix[i, j] == null) continue;
                    Item item = m_itemMatrix[i, j];
                    item.IsOverdue = true;
                    item.IsRemove = false;
                    item.NextRaw = -1;
                    item.NextColumn = -1;
                }
            }
            RandomAdd(1);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
