using System.Collections;
using System.Collections.Generic;
using cfg;
using cfg.config;
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
    
    public Dictionary<string, List<Flag>> FlagDict = new Dictionary<string, List<Flag>>();
    
    public Dictionary<string, bool> CharacterDict = new Dictionary<string, bool>();
    
    public void CreateNewTree()
    {
        FlagDict.Clear();
        CharacterDict.Clear();
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

    public CharacterConfig AddCharacter(Branch branch)
    {
        var character = GetCharacterEvent(branch);

        if (character == null) return null;
        
        if (!CharacterDict.ContainsKey(character.Id))
        {
            CharacterDict.Add(character.Id, true);
        }

        return character;
    }
    
    // 触发事件 相遇人物
    public CharacterConfig GetCharacterEvent(Branch branch)
    {
        var list = new List<CharacterConfig>();
        
        var allCharList = Service.Cfg.GetAllCfgCharacters();
        
        for (var i = 0; i < allCharList.Count; i++)
        {
            if (CharacterDict.ContainsKey(allCharList[i].Id))
                continue;
            
            if (!CheckTreeLifeTime(allCharList[i].MinTreeTime, allCharList[i].MaxTreeTime))
                continue;
            
            bool can = true;
            for (var j = 0; j < allCharList[i].FlagConditions.Count; j++)
            {
                if (!CheckFlagCondition(allCharList[i].FlagConditions[j], branch))
                {
                    can = false;
                    break;
                }
            }

            if (can)
            {
                list.Add(allCharList[i]);
            }
        }
        
        // 按照 优先级 权重 随机选择
        int priority = 0;
        int allWeight = 0;
        var finalList = new List<CharacterConfig>();
        
        for (var i = 0; i < list.Count; i++)
        {
            if (list[i].Priority > priority)
            {
                allWeight = 0;
                finalList.Clear();
                priority = list[i].Priority;
                allWeight += list[i].Weight;
                finalList.Add(list[i]);
            }
            else if (list[i].Priority == priority)
            {
                finalList.Add(list[i]);
                allWeight += list[i].Weight;
            }
        }

        for (var i = 0; i < finalList.Count; i++)
        {
            var rand = UnityEngine.Random.Range(0, allWeight);
            if (rand <= finalList[i].Weight)
            {
                return finalList[i];
            }
                
            allWeight -= finalList[i].Weight;
        }

        return null;
    }

    public bool CheckTreeLifeTime(float min, float max)
    {
        if (min > 0 && TotalTime < min)
        {
            return false;
        }

        if (max > 0 && TotalTime > max)
        {
            return false;
        }

        return true;
    }
    
    public bool CheckFlagCondition(FlagCondition condition, Branch branch)
    {
        // 不存在
        if (!condition.Has)
        {
            if (FlagDict.ContainsKey(condition.Id))
            {
                return false;
            }
            
            // // 判断是否在父根中不存在
            // if (condition.InParent)
            // {
            //     var tempBranch = branch;
            //     if (tempBranch.CheckFlag(condition))
            //     {
            //         return false;
            //     }
            //     
            //     while (tempBranch.ParentBranch != null)
            //     {
            //         tempBranch = tempBranch.ParentBranch;
            //         if (tempBranch.CheckFlag(condition))
            //         {
            //             return false;
            //         }
            //     }
            // }

            return true;
        }
        
        if (condition.Has)
        {
            // 有 且在 parent 分支中
            if (condition.InParent)
            {
                var tempBranch = branch;
                if (tempBranch.CheckFlag(condition))
                {
                    return true;
                }
                
                while (tempBranch.ParentBranch != null)
                {
                    tempBranch = tempBranch.ParentBranch;
                    if (tempBranch.CheckFlag(condition))
                    {
                        return true;
                    }
                }

                return false;
            }
            else
            {
                // 有 不需要在 parent 分支中
                
                if (!FlagDict.ContainsKey(condition.Id))
                {
                    return false;
                }
                
                var list = FlagDict[condition.Id];
                
                for (int i = 0; i < list.Count; i++)
                {
                    if (condition.HasTime > 0 && (Time.time - list[i].AddTime) < condition.HasTime)
                    {
                        continue;
                    }
                    
                    if (condition.Alive != list[i].Alive)
                    {
                        continue;
                    }
                
                    return true;
                }
                
                return false;
            }
        }
        
        return true;
    }

    public void AddFlag(Flag flag)
    {
        if (!FlagDict.ContainsKey(flag.Id))
        {
            FlagDict.Add(flag.Id, new List<Flag>());
        }
        
        FlagDict[flag.Id].Add(flag);
    }

}
