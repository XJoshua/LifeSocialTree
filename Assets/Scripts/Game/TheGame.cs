using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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

    public Camera Camera;

    private bool cameraIsMoving = false;
    
    public bool IsGameStart = false;
    
    void Awake()
    {
        Ins = this;
        Camera = Camera.main;
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
        ResetCamera();
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

    public void ResetCamera()
    {
        Camera.transform.position = new Vector3(0, 4, -10);
        Camera.orthographicSize = 5;
    }

    public void CheckMoveCamera(Vector3 pos)
    {
        if (cameraIsMoving) return;
        
        if (pos.y > Camera.orthographicSize + Camera.transform.position.y)
        {
            UpdateCamera();
        }
    }
    
    public void UpdateCamera()
    {
        Camera.DOKill();
        Camera.transform.DOKill();
        
        cameraIsMoving = true;
        Camera.DOOrthoSize(Camera.orthographicSize + 2, 2f);
        Camera.transform.DOMoveY(Camera.transform.position.y + 2, 2f).OnComplete(()=>
            cameraIsMoving = false);
    }
    
}
