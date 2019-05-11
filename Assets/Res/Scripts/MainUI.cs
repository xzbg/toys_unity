using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    public Button m_startBtn;
    public Grid grid;

    // Use this for initialization
    void Start()
    {
        m_startBtn.onClick.AddListener(StartAction);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DoMove(InputDirection input)
    {
        grid.DOMove(input);
    }

    public void StartAction()
    {
        GameManager.Instance.GameStart();
    }

    internal void GameStart()
    {
        m_startBtn.gameObject.SetActive(false);
        grid.Init();
    }

    internal void GameOver()
    {
        m_startBtn.gameObject.SetActive(true);
    }
}
