using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using URandom = UnityEngine.Random;

public class Item : MonoBehaviour
{
    private int m_raw;
    private int m_column;
    private int m_nextRaw = -1;
    private int m_nextColumn = -1;
    private int m_num;
    private bool m_isRemove = false;
    private bool m_isOverdue = true;

    public Text m_value;

    public int Value
    {
        get
        {
            return m_num;
        }

        set
        {
            this.m_num = value;
            m_value.text = Convert.ToString(value);
        }
    }

    public int Raw
    {
        get
        {
            return m_raw;
        }

        set
        {
            m_raw = value;
        }
    }

    public int Column
    {
        get
        {
            return m_column;
        }

        set
        {
            m_column = value;
        }
    }

    public int NextRaw
    {
        get
        {
            return m_nextRaw;
        }

        set
        {
            m_nextRaw = value;
        }
    }

    public int NextColumn
    {
        get
        {
            return m_nextColumn;
        }

        set
        {
            m_nextColumn = value;
        }
    }

    public bool IsRemove
    {
        get
        {
            return m_isRemove;
        }

        set
        {
            m_isRemove = value;
        }
    }

    public bool IsOverdue
    {
        get
        {
            return m_isOverdue;
        }

        set
        {
            m_isOverdue = value;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitRandomValue()
    {
        float rate = URandom.Range(0f, 100f);
        m_num = rate <= 50 ? 2 : 4;
        m_value.text = Convert.ToString(m_num);
    }
}
