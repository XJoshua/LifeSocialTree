using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using cfg;
using QFramework;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game
{
    [System.Serializable]
    public class TreeParms
    {
        public float branchAngle = 15.0f; //how far left/right each branch goes from its parent.
        public float minScale = 0.05f; //what's the minimum scale of each branch?
        public float scaleChange = 0.05f; //how much smaller than its parent is each branch?
        public float angleRandom = 5.0f; //how much random variation is there in the angle for each branch?
        public float scaleRandom = 0.1f; // how much random variation is there in the scale for each branch?

        public int maxDepth = 15; //what's the maximum "depth" of the recursive algorithm?
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(UITreeManager))]
    public class UITreeManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            //生成一个按钮 在Inspector
            if (GUILayout.Button("Regenerate"))
            {
                ((TreeGenerate) target).RefreshGenerate();
            }

            if (GUILayout.Button("Clear"))
            {
                ((TreeGenerate) target).ClearInEditor();
            }
        }
    }
#endif
    
    public class UITreeManager : MonoBehaviour
    {
        public Transform TrunkRoot;
        public UIBranch UiBranchPrefab;

        public Transform CharacterRoot;
        public UICharacter UiCharacterPrefab;
        
        public Vector3 TrunkTopOffset = new Vector3(0, 1, 0);

        //Note: careful with "scaleChange" and especially "max depth" - it can make the algorithm take a VERY long time.
        [FormerlySerializedAs("parms")] public TreeParms Parms;

        // public bool reCalulateNow = false;

        Transform root;
        List<UIBranch> trunks = new List<UIBranch>(1000000);

        private UIBranch RootBranch;

        private TreeManager TreeMng => TheGame.Get().TreeMng;
        
        void Start()
        {
            root = transform;
        }

        public void CreateRootBranch(Branch branch)
        {
            RootBranch = CreateBranch(root.position, 0, 1.0f, 0, branch, null);
        }
        
        public void Clear()
        {
            if (trunks.Count > 0)
            {
                for (int i = 0; i < trunks.Count; i++)
                {
                    GameObject.Destroy(trunks[i].gameObject);
                }

                trunks.Clear();
            }
        }

        // 按照 tree manager 的数据更新
        public void Refresh()
        {
            var branch = TreeMng.ParentBranch;
            var uiBranch = RootBranch;
            
            uiBranch.Refresh(branch);

            if (!TreeMng.CheckAllBranchDead())
            {
                var scale = 1 + TreeMng.TotalTime * Service.Cfg.BaseConfig.OverallGrow;
                root.transform.localScale = new Vector3(scale, scale, scale);
            }
        }

        public void CreateBranch(UIBranch parent)
        {
            // //are we at the minimum scale?
            // if (scale < Parms.minScale || depth >= Parms.maxDepth)
            //     return; //done with this 'leaf'!

            Vector3 topPos = parent.EndPos(); 
            // startPos + (rot * (TrunkTopOffset * scale));

            //make a left branch
            var child1 = CreateBranch(topPos,
                parent.angle - Parms.branchAngle + Random.Range(-Parms.angleRandom, Parms.angleRandom),
                parent.scale - (Parms.scaleChange + Random.Range(0, Parms.scaleRandom)),
                parent.depth + 1, parent.GetBranchData().ChildBranches[0], parent);
            
            //make a right branch
            var child2 = CreateBranch(topPos,
                parent.angle + Parms.branchAngle + Random.Range(-Parms.angleRandom, Parms.angleRandom),
                parent.scale - (Parms.scaleChange + Random.Range(0, Parms.scaleRandom)),
                parent.depth + 1,  parent.GetBranchData().ChildBranches[1], parent);

            parent.AddBranchChild(child1, child2);
        }
        
        UIBranch CreateBranch(Vector3 startPos, float angle, float scale, int depth, Branch data, UIBranch parent)
        {
            // Debug.Log("Create UI branch");
            
            Quaternion rot = Quaternion.Euler(0, 0, angle);

            //make a trunk
            UIBranch uiBranch = GameObject.Instantiate(UiBranchPrefab, startPos, rot);
            uiBranch.transform.SetParent(TrunkRoot);
            uiBranch.gameObject.SetActive(true);
            scale = Math.Max(0.05f, scale);
            uiBranch.transform.localScale = new Vector3(scale, 0, 1);

            uiBranch.CreateInfo(startPos, angle, scale, depth, data, parent);
            
            trunks.Add(uiBranch);
            return uiBranch;
        }

        public UICharacter CreateUiCharacter(Vector3 pos, string id)
        {
            UICharacter uiCharacter = GameObject.Instantiate(UiCharacterPrefab, CharacterRoot, true);
            uiCharacter.transform.position = pos;
            
            uiCharacter.Setup(id);
            
            uiCharacter.gameObject.SetActive(true);
            return uiCharacter;
        }
        
        void UpdateBranch()
        {
            
        }
    }
}