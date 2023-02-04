
    
using UnityEngine;

public class GameConfig
{ 
    //public void 

    public static string BaseConfigId = "base_config";

    public static string IconAtlasPath = "IconsAtlas";

    public static string DefaultBtnSound = "";

    public static string DefaultBgm = "";

    public static float BranchAspect = 0.2f;
    
    public static float BranchGrow = 0.25f;

    public static Vector3 GetBranchScale(float timer, Vector3 scale)
    {
        return new Vector3(scale.x, BranchGrow * timer, 1);
    }
    
}
