using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using QFramework;
using UnityEngine;

public class TheGame : MonoSingleton<TheGame>
{
    public static TheGame Ins { get; private set; }

    public static TheGame Get()
    {
        return Ins;
    }
    
    // private GameData data;
    // public GameData Data => data;
    
    public TreeManager TreeMng;

    public UITreeManager UITreeMng;

    public bool IsGameStart = false;
    
    void Awake()
    {
        Ins = this;
    }

    // void Start()
    // {
    //     Ins = this;
    // }

    void Update()
    {
        if (TreeMng != null)
        {
            TreeMng.Update();
        }
    }

    public void NewGame()
    {
        TreeMng = new TreeManager();
        TreeMng.CreateNewTree();
        IsGameStart = true;
    }

    public void GameEnd()
    {
        Debug.LogWarning("Game End");
        
        TreeMng = null;
        IsGameStart = false;
    }
    
    
}
