using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game
{
    public class Branch
    {
        public float Size = 0.2f;

        public float Life = 1000;

        public float LifeCost = 5;

        public float BranchRate = 0.05f;

        public float EventRate = 0.2f;

        public bool Sleep = false;

        public bool Dead = false;

        public Branch ParentBranch;

        public List<Branch> ChildBranches = new List<Branch>();

        public int Index = 0;

        public string IndexPath = string.Empty;

        public Branch()
        {
            Size = Service.Cfg.BaseConfig.BaseSize;
            Life = Service.Cfg.BaseConfig.BaseLife;
            LifeCost = Service.Cfg.BaseConfig.BaseCost;
            BranchRate = Service.Cfg.BaseConfig.BranchRate;
            EventRate = Service.Cfg.BaseConfig.EventRate;
        }

        public static Branch CreateRootBranch()
        {
            return new Branch();
        }

        public static Branch CreateBranch(Branch parentBranch, int index)
        {
            var branch = new Branch()
            {
                Index = index,

                Size = parentBranch.Size * 0.5f,
                Life = parentBranch.Life * 0.5f,
                LifeCost = parentBranch.LifeCost,
                BranchRate = parentBranch.BranchRate,
                EventRate = parentBranch.EventRate,

                ParentBranch = parentBranch,
                IndexPath = parentBranch.IndexPath + $"{index}",
            };
            
            // Debug.Log($"CreateBranch {branch}");
            return branch;
        }

        public bool IsAlive()
        {
            return !Dead && !Sleep;
        }

        public void Update(bool grow)
        {
            if (Dead) return;

            if (!Sleep)
            {
                Life = Life - LifeCost * Time.deltaTime;

                if (Life <= 0 || Life < LifeCost)
                {
                    SetDead();

                }

                if (grow)
                {
                    // 分支 事件判断
                    var toBranch = UnityEngine.Random.Range(0, 1f) < BranchRate;

                    if (toBranch)
                    {
                        Debug.Log($"CreateBranch {this}");
                        
                        for (int i = 0; i < 2; i++)
                        {
                            CreateChildBranch(i);
                        }

                        Sleep = true;
                        return;
                    }

                    if (!Sleep)
                    {
                        var toEvent = UnityEngine.Random.Range(0, 1f) < EventRate;

                        if (toEvent)
                        {
                            // todo 
                            Debug.Log($"{IndexPath} Event Happen!");

                            EventRate = Service.Cfg.BaseConfig.EventRate;
                        }
                    }
                }
            }
            
            // todo 正常生长 变粗？

            for (var i = 0; i < ChildBranches.Count; i++)
            {
                ChildBranches[i].Update(grow);
            }
        }

        public void CreateChildBranch(int index)
        {
            ChildBranches.Add(Branch.CreateBranch(this, index));
        }

        // 有子分支的时候调用，检查是否所有的分支都死了
        public void CheckDead()
        {
            if (ChildBranches.Count > 0)
            {
                var dead = true;
                for (int i = 0; i < ChildBranches.Count; i++)
                {
                    dead = dead && ChildBranches[i].Dead;
                }

                if (dead)
                {
                    SetDead();
                }
            }
        }

        public void SetDead()
        {
            Dead = true;
            if (ParentBranch != null)
                ParentBranch.CheckDead();
        }
        
        // // 获得 branch 的分支路径
        // public string GetIndexPath()
        // {
        //     var list = new List<int>();
        //
        //     var branch = this;
        //
        //     while (branch.ParentBranch != null)
        //     {
        //         list.Add(branch.Index);
        //         branch = branch.ParentBranch;
        //     }
        //
        //     var result = string.Empty;
        //     for (int i = list.Count - 1; i >= 0; i--)
        //     {
        //         result = $"{result}{list[i]}";
        //     }
        //
        //     return result;
        // }

        public override string ToString()
        {
            return $"Path:{IndexPath} Life:{Life} LifeCost:{LifeCost} BranchRate:{BranchRate} EventRate:{EventRate}";
        }
    }
}