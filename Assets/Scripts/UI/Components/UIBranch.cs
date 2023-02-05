using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.U2D;

public class UIBranch : MonoBehaviour
{
    public List<UIBranch> ChildBranches;

    public UIBranch ParentBranch;
    
    public SpriteShapeController ShapeCtrl;
    
    // public Material Mat;
    private static readonly int StartSize = Shader.PropertyToID("_StartSize");
    private static readonly int EndSize = Shader.PropertyToID("_EndSize");

    // public Vector3 endPos;
    
    public static Vector3 TrunkTopOffset = new Vector3(0, 1f, 0);

    public float angle;
    public float scale;
    public int depth;

    public UITreeManager UITreeMng => TheGame.Get().UITreeMng;

    public Transform EndPosTrans;

    private Branch branchData;
    
    public List<UICharacter> Characters = new List<UICharacter>();
    
    public void UpdateSize(float startSize, float endSize)
    {
        // Mat.SetFloat(StartSize, startSize);
        // Mat.SetFloat(EndSize, endSize);
        return;
        
        var oriPos0 = ShapeCtrl.spline.GetPosition(0);
        var oriPos1 = ShapeCtrl.spline.GetPosition(1);
        var oriPos2 = ShapeCtrl.spline.GetPosition(2);
        var oriPos3 = ShapeCtrl.spline.GetPosition(3);

        var endPos = oriPos2 + oriPos3 / 2;
        var startPos = oriPos0 + oriPos1 / 2;

        var pos = new Vector3[4];
        
        pos[0] = endPos + (oriPos0 - endPos) * startSize;
        pos[1] = endPos + (oriPos1 - endPos) * startSize;
        pos[2] = startPos + (oriPos2 - endPos) * endSize;
        pos[3] = startPos + (oriPos3 - endPos) * endSize;
        
        Debug.Log($"startPos: {startPos} endPos: {endPos} {pos[0]} {pos[1]} {pos[2]} {pos[3]}");
        
        ShapeCtrl.spline.Clear();
        for (int i = 0; i < pos.Length; i++)
        {
            ShapeCtrl.spline.InsertPointAt(i, pos[i]);
        }
    }

    private void OnMouseDown()
    {
        if (ParentBranch == null) return;
        
        Debug.Log("try clip branch " + branchData);
        
        branchData.TryClip();
        
        // 被剪掉的表现
        Clip();
    }

    public void Clip()
    {
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
        
        Destroy(this.gameObject);
    }
    
    public void Refresh(Branch branchData)
    {
        // todo update branch info

        if (branchData.Dead) return;

        if (!branchData.Sleep)
        {
            transform.localScale = GameConfig.GetBranchScale(branchData.Timer, transform.localScale);
            TheGame.Get().CheckMoveCamera(EndPosTrans.position);
        }
        
        // 创建人物头像
        if (branchData.CharacterList.Count > Characters.Count)
        {
            var characterId = branchData.CharacterList[branchData.CharacterList.Count - 1];
            var uiCharacter = UITreeMng.CreateUiCharacter(EndPosTrans.position, characterId);
            Characters.Add(uiCharacter);
        }
        
        if (branchData.ChildBranches.Count > 0)
        {
            // 创建分支
            if (ChildBranches.Count == 0)
            {
                UITreeMng.CreateBranch(this);
            }
            
            for (var i = 0; i < ChildBranches.Count; i++)
            {
                ChildBranches[i].Refresh(branchData.ChildBranches[i]);
            }
        }
    }
    
    public void CreateInfo(Vector3 startPos, float angle, float scale, int depth, Branch data, UIBranch parent)
    {
        branchData = data;
        
        Quaternion rot = Quaternion.Euler(0, 0, angle);
        //EndPosTrans.position = startPos + (rot * (TrunkTopOffset * scale));
        this.angle = angle;
        this.scale = scale;
        this.depth = depth;

        ParentBranch = parent;
    }

    public List<UIBranch> GetChild()
    {
        return ChildBranches;
    }

    public void AddBranchChild(UIBranch child1, UIBranch child2)
    {
        ChildBranches = new List<UIBranch>();
        
        ChildBranches.Add(child1);
        ChildBranches.Add(child2);
    }
    
    public Vector3 EndPos()
    {
        return EndPosTrans.position;
    }

    public Branch GetBranchData()
    {
        return branchData;
    }
}
