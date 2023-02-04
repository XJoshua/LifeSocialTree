/// Let's draw trees with a recursive algorithm in Unity!
/// 2014 Aaron San Filippo (@AeornFlippout)
/// Let me know if you have fun with this or make any cool additions :)

//INSTRUCTIONS:
//1. attach this component to an object in your scene that the camera can see.
// (see further instructions below)

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEditor;
using UnityEngine.Serialization;

[CustomEditor(typeof(TreeGenerate))]
public class TreeEditor : Editor
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

public class TreeGenerate : MonoBehaviour
{
    public Transform TrunkRoot;

    //2. point this at a prefab that's a skinny quad or something with height of 1 unit, where the origin is at the base.
    //for instance - make an 'empty' object with a child 'quad' offset by .5 units in the Y direction.
    [FormerlySerializedAs("TrunkPrefab")] 
    public UIBranch UiBranchPrefab;

    public Vector3 TrunkTopOffset = new Vector3(0, 1, 0);

    // 3. Play around with these parameters.
    //Note: careful with "scaleChange" and especially "max depth" - it can make the algorithm take a VERY long time.
    [FormerlySerializedAs("parms")] public Game.TreeParms Parms;

    //4. push this checkbox to re-calculate in the Unity editor at runtime.
    public bool reCalulateNow = false;

    Transform root;
    List<UIBranch> trunks = new List<UIBranch>(1000000);

    // Use this for initialization
    void Start()
    {
        root = transform;
        MakeTree(root.position, 0, 1.0f, 0);
    }

    void Update()
    {
        if (reCalulateNow)
        {
            reCalulateNow = false;
            Clear();
            MakeTree(root.position, 0, 1.0f, 0);
        }
    }

    public void ClearInEditor()
    {
        if (trunks.Count > 0)
        {
            for (int i = 0; i < trunks.Count; i++)
            {
                DestroyImmediate(trunks[i].gameObject);
            }
            trunks.Clear();
        }

        if (TrunkRoot.childCount > 0)
        {
            var count = TrunkRoot.childCount;
            for (int i = count - 1; i >= 0; i--)
            {
                DestroyImmediate(TrunkRoot.GetChild(i).gameObject);
            }
            trunks.Clear();
        }
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
    
    public void RefreshGenerate()
    {
        Clear();
        root = transform;
        MakeTree(root.position, 0, 1.0f, 0);
    }

    void MakeTree(Vector3 startPos, float angle, float scale, int depth)
    {
        //create a quaternion specified with euler angles where we rotate around 'x'
        Quaternion rot = Quaternion.Euler(0, 0, angle); // *  Quaternion.AngleAxis( Random.Range(0,360), Vector3.up);

        UiBranchPrefab.gameObject.SetActive(true);
        
        //make a trunk
        UIBranch uiBranch = GameObject.Instantiate(UiBranchPrefab, startPos, rot);
        uiBranch.transform.SetParent(TrunkRoot);
        uiBranch.transform.localScale = new Vector3(scale, scale, scale);
        // trunk.transform.localScale = new Vector3(1, scale, scale);
        uiBranch.UpdateSize(1, 0.9f);
        
        trunks.Add(uiBranch);

        //are we at the minimum scale?
        if (scale < Parms.minScale || depth >= Parms.maxDepth)
            return; //done with this 'leaf'!

        Vector3 topPos = startPos + (rot * (TrunkTopOffset * scale));

        //make a left branch
        MakeTree(topPos,
            angle - Parms.branchAngle + Random.Range(-Parms.angleRandom, Parms.angleRandom),
            scale - (Parms.scaleChange + Random.Range(0, Parms.scaleRandom)),
            depth + 1);

        //make a right branch
        MakeTree(topPos,
            angle + Parms.branchAngle + Random.Range(-Parms.angleRandom, Parms.angleRandom),
            scale - (Parms.scaleChange + Random.Range(0, Parms.scaleRandom)),
            depth + 1);
        
        UiBranchPrefab.gameObject.SetActive(false);
    }
}