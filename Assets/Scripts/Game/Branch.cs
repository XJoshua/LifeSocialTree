using System.Collections.Generic;
using cfg;
using cfg.config;
using UnityEngine;

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

        public float Timer = 0; // 生长的时间 影响长度
        
        public List<Flag> FlagList = new List<Flag>();
        public List<string> CharacterList = new List<string>();
        
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
                    // 分支判断
                    var toBranch = UnityEngine.Random.Range(0, 1f) < BranchRate;

                    if (toBranch)
                    {
                        Debug.Log($"CreateBranch {this}");
                        
                        for (int i = 0; i < 2; i++)
                        {
                            CreateChildBranch(i);
                        }

                        Sleep = true;
                        BranchRate += Service.Cfg.BaseConfig.BranchRate;
                        Life = 0;
                        return;
                    }
                    else
                    {
                        BranchRate += Service.Cfg.BaseConfig.BranchRateAdd;
                    }

                    // 事件判断
                    if (!Sleep)
                    {
                        var toEvent = UnityEngine.Random.Range(0, 1f) < EventRate;

                        if (toEvent)
                        {
                            var character = TheGame.Get().TreeMng.AddCharacter(this);
                            
                            if (character != null)
                            {
                                Debug.Log($"{IndexPath} Event Happen {character.Id}");
                                CharacterList.Add(character.Id);
                                ActiveCharacterFlag(character, this);
                            }
                            else
                            {
                                Debug.Log($"{IndexPath} Event Happen no character got");
                            }
                            
                            EventRate = Service.Cfg.BaseConfig.EventRate;
                        }
                        else
                        {
                            EventRate += Service.Cfg.BaseConfig.EventRateAdd;
                        }
                    }
                }
            }
            
            // todo 正常生长 变粗？
            Timer += Time.deltaTime;
            
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

        public bool CheckAlive()
        {
            if (ChildBranches.Count > 0)
            {
                var alive = false;
                for (int i = 0; i < ChildBranches.Count; i++)
                {
                    alive = alive || ChildBranches[i].CheckAlive();
                }

                return alive;
            }
            else
            {
                return !Dead && !Sleep;
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

        public void TryClip()
        {
            var life = GetLife();
            
            ParentBranch.GetClipLife(life, this);
            
            Clip();
        }

        // 获得这个分支剩余的生命值
        public float GetLife()
        {
            float life = 0f;
            
            if (ChildBranches.Count > 0)
            {
                for (int i = 0; i < ChildBranches.Count ; i++)
                {
                    life += ChildBranches[i].GetLife();
                }
            }
            else
            {
                life = this.Life;
            }

            return life;
        }
        
        // 尝试获得生命值
        public void GetClipLife(float life, Branch clipedBranch)
        {
            var gotLife = life * GameConfig.ClipLifeTransferLeft;
            
            var list = new List<Branch>();

            if (ChildBranches.Count > 0)
            {
                for (int i = 0; i < ChildBranches.Count; i++)
                {
                    if (ChildBranches[i] != clipedBranch)
                    {
                        ChildBranches[i].GetAllAliveChildBranch(list);
                    }
                }
            }
            
            if (list.Count > 0)
            {
                var divLife = life / list.Count;
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Life += divLife;
                }
            }
            else
            {
                ParentBranch.GetClipLife(gotLife, this);
            }
        }

        public void GetAllAliveChildBranch(List<Branch> list)
        {
            if (ChildBranches.Count > 0)
            {
                for (int i = 0; i < ChildBranches.Count; i++)
                {
                    ChildBranches[i].GetAllAliveChildBranch(list);
                }
            }
            else
            {
                if (!Dead && !Sleep)
                {
                    list.Add(this);
                }
            }
        }

        public void Clip()
        {
            this.Dead = true;
            
            for (var i = 0; i < FlagList.Count; i++)
            {
                FlagList[i].Alive = false;
            }
            
            // 被剪掉的表现
            for (int i = 0; i < ChildBranches.Count; i++)
            {
                ChildBranches[i].Clip();
            }

            for (int i = 0; i < ParentBranch.ChildBranches.Count; i++)
            {
                if (ParentBranch.ChildBranches[i] == this)
                {
                    ParentBranch.ChildBranches.RemoveAt(i);
                    break;
                }
            }
        }

        // 只检查自己 id 存活 时间
        public bool CheckFlag(FlagCondition condition)
        {
            for (int i = 0; i < FlagList.Count; i++)
            {
                if (FlagList[i].Id == condition.Id)
                {
                    if (condition.HasTime > 0 && (Time.time - FlagList[i].AddTime) < condition.HasTime)
                    {
                        break;
                    }
                    
                    if (condition.Alive != FlagList[i].Alive)
                    {
                        break;
                    }

                    return true;
                }
            }

            return false;
        }
        
        // 激活 人物的flag
        public void ActiveCharacterFlag(CharacterConfig character, Branch branch)
        {
            for (var i = 0; i < character.ActiveFlag.Count; i++)
            {
                var flag = new Flag(character.ActiveFlag[i], Time.time);
                branch.FlagList.Add(flag);
                TheGame.Get().TreeMng.AddFlag(flag);
            }
        }
        
        public override string ToString()
        {
            return $"Path:{IndexPath} Life:{Life} LifeCost:{LifeCost} BranchRate:{BranchRate} EventRate:{EventRate}";
        }
    }
}