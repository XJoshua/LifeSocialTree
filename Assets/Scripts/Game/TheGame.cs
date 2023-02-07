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


    private float ZoomSpeed = 5;
    private float MaxScale = 20;
    private float MinScale = 1;
    private float NowSize;


    private float moveSpeed =20f;


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

        ZoomCamera();

        float speed = Camera.orthographicSize / moveSpeed;
        if (Input.GetKey(KeyCode.W))
        {
            Camera.transform.Translate(Vector3.up  * speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Camera.transform.Translate(Vector3.down  * speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            Camera.transform.Translate(Vector3.left  * speed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Camera.transform.Translate(Vector3.right * speed);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            Time.timeScale = 2;
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    public void NewGame()
    {
        ResetCamera();
        UITreeMng.Clear();
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

    private void ZoomCamera()
    {
        float zoomValue = Input.GetAxis("Mouse ScrollWheel");
        if (zoomValue != 0)
        {
            NowSize = Camera.main.orthographicSize + -zoomValue * ZoomSpeed;
            NowSize = Mathf.Min(NowSize, MaxScale);
            NowSize = Mathf.Max(NowSize, MinScale);

            Camera.main.orthographicSize = NowSize;
        }
    }

    private void MoveCamera()
    { 
        Vector2 screenpos = Camera.main.WorldToScreenPoint(Input.mousePosition);
        Vector3 targetPos =new Vector3(screenpos.x, screenpos.y, Camera.transform.position.z);
        Debug.Log(targetPos);
        Camera.transform.position = Vector3.MoveTowards(transform.position, targetPos, 0.1f * Time.deltaTime);
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
