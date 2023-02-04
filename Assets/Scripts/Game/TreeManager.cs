using System.Collections;
using System.Collections.Generic;
using Game;
using GameEvent;
using Mono.CSharp;
using QFramework;
using UnityEngine;

public class TreeManager
{
    public Branch ParentBranch;

    private float timer;

    public float TotalTime;
    
    public void CreateNewTree()
    {
        TotalTime = 0;
        ParentBranch = Branch.CreateRootBranch();
        TheGame.Get().UITreeMng.CreateRootBranch(ParentBranch);
    }

    // the game 调用
    public void Update()
    {
        if (!TheGame.Get().IsGameStart) return;
        
        if (CheckAllBranchDead())
        {
            TheGame.Get().GameEnd();
            return;
        }
        
        TotalTime += Time.deltaTime;
        timer += Time.deltaTime;

        if (timer >= Service.Cfg.BaseConfig.RoundTime)
        {
            timer = 0;
            Debug.Log($"time to glow");
            ParentBranch.Update(true);
        }
        else
        {
            ParentBranch.Update(false);
        }
        
        //TypeEventSystem.Global.Send<RefreshTreeEvent>();
        TheGame.Get().UITreeMng.Refresh();
    }

    public bool CheckAllBranchDead()
    {
        return ParentBranch.Dead;
    }
}
