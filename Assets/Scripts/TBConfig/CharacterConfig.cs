//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;
using SimpleJSON;



namespace cfg
{ 

public sealed partial class CharacterConfig :  Bright.Config.BeanBase 
{
    public CharacterConfig(JSONNode _json) 
    {
        { if(!_json["id"].IsString) { throw new SerializationException(); }  Id = _json["id"]; }
        { if(!_json["name"].IsString) { throw new SerializationException(); }  Name = _json["name"]; }
        { if(!_json["image"].IsString) { throw new SerializationException(); }  Image = _json["image"]; }
        { if(!_json["priority"].IsNumber) { throw new SerializationException(); }  Priority = _json["priority"]; }
        { if(!_json["weight"].IsNumber) { throw new SerializationException(); }  Weight = _json["weight"]; }
        { if(!_json["relationship"].IsString) { throw new SerializationException(); }  Relationship = _json["relationship"]; }
        { if(!_json["desc"].IsString) { throw new SerializationException(); }  Desc = _json["desc"]; }
        { if(!_json["job"].IsString) { throw new SerializationException(); }  Job = _json["job"]; }
        { if(!_json["effect"].IsString) { throw new SerializationException(); }  Effect = _json["effect"]; }
        { if(!_json["frame_size"].IsNumber) { throw new SerializationException(); }  FrameSize = _json["frame_size"]; }
        { if(!_json["min_tree_time"].IsNumber) { throw new SerializationException(); }  MinTreeTime = _json["min_tree_time"]; }
        { if(!_json["max_tree_time"].IsNumber) { throw new SerializationException(); }  MaxTreeTime = _json["max_tree_time"]; }
        { var __json0 = _json["flag_conditions"]; if(!__json0.IsArray) { throw new SerializationException(); } FlagConditions = new System.Collections.Generic.List<config.FlagCondition>(__json0.Count); foreach(JSONNode __e0 in __json0.Children) { config.FlagCondition __v0;  { if(!__e0.IsObject) { throw new SerializationException(); }  __v0 = config.FlagCondition.DeserializeFlagCondition(__e0);  }  FlagConditions.Add(__v0); }   }
        { var __json0 = _json["active_flag"]; if(!__json0.IsArray) { throw new SerializationException(); } ActiveFlag = new System.Collections.Generic.List<string>(__json0.Count); foreach(JSONNode __e0 in __json0.Children) { string __v0;  { if(!__e0.IsString) { throw new SerializationException(); }  __v0 = __e0; }  ActiveFlag.Add(__v0); }   }
        PostInit();
    }

    public CharacterConfig(string id, string name, string image, int priority, int weight, string relationship, string desc, string job, string effect, float frame_size, float min_tree_time, float max_tree_time, System.Collections.Generic.List<config.FlagCondition> flag_conditions, System.Collections.Generic.List<string> active_flag ) 
    {
        this.Id = id;
        this.Name = name;
        this.Image = image;
        this.Priority = priority;
        this.Weight = weight;
        this.Relationship = relationship;
        this.Desc = desc;
        this.Job = job;
        this.Effect = effect;
        this.FrameSize = frame_size;
        this.MinTreeTime = min_tree_time;
        this.MaxTreeTime = max_tree_time;
        this.FlagConditions = flag_conditions;
        this.ActiveFlag = active_flag;
        PostInit();
    }

    public static CharacterConfig DeserializeCharacterConfig(JSONNode _json)
    {
        return new CharacterConfig(_json);
    }

    /// <summary>
    /// 这是id
    /// </summary>
    public string Id { get; private set; }
    /// <summary>
    /// 名字
    /// </summary>
    public string Name { get; private set; }
    public string Image { get; private set; }
    public int Priority { get; private set; }
    public int Weight { get; private set; }
    public string Relationship { get; private set; }
    public string Desc { get; private set; }
    public string Job { get; private set; }
    public string Effect { get; private set; }
    /// <summary>
    /// 框的大小
    /// </summary>
    public float FrameSize { get; private set; }
    public float MinTreeTime { get; private set; }
    public float MaxTreeTime { get; private set; }
    public System.Collections.Generic.List<config.FlagCondition> FlagConditions { get; private set; }
    /// <summary>
    /// 激活的flag
    /// </summary>
    public System.Collections.Generic.List<string> ActiveFlag { get; private set; }

    public const int __ID__ = 676994987;
    public override int GetTypeId() => __ID__;

    public  void Resolve(Dictionary<string, object> _tables)
    {
        foreach(var _e in FlagConditions) { _e?.Resolve(_tables); }
        PostResolve();
    }

    public  void TranslateText(System.Func<string, string, string> translator)
    {
        foreach(var _e in FlagConditions) { _e?.TranslateText(translator); }
    }

    public override string ToString()
    {
        return "{ "
        + "Id:" + Id + ","
        + "Name:" + Name + ","
        + "Image:" + Image + ","
        + "Priority:" + Priority + ","
        + "Weight:" + Weight + ","
        + "Relationship:" + Relationship + ","
        + "Desc:" + Desc + ","
        + "Job:" + Job + ","
        + "Effect:" + Effect + ","
        + "FrameSize:" + FrameSize + ","
        + "MinTreeTime:" + MinTreeTime + ","
        + "MaxTreeTime:" + MaxTreeTime + ","
        + "FlagConditions:" + Bright.Common.StringUtil.CollectionToString(FlagConditions) + ","
        + "ActiveFlag:" + Bright.Common.StringUtil.CollectionToString(ActiveFlag) + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}
}
