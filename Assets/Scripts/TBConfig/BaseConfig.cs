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

public sealed partial class BaseConfig :  Bright.Config.BeanBase 
{
    public BaseConfig(JSONNode _json) 
    {
        { if(!_json["id"].IsString) { throw new SerializationException(); }  Id = _json["id"]; }
        { if(!_json["overall_grow"].IsNumber) { throw new SerializationException(); }  OverallGrow = _json["overall_grow"]; }
        { if(!_json["round_time"].IsNumber) { throw new SerializationException(); }  RoundTime = _json["round_time"]; }
        { if(!_json["base_size"].IsNumber) { throw new SerializationException(); }  BaseSize = _json["base_size"]; }
        { if(!_json["base_life"].IsNumber) { throw new SerializationException(); }  BaseLife = _json["base_life"]; }
        { if(!_json["base_cost"].IsNumber) { throw new SerializationException(); }  BaseCost = _json["base_cost"]; }
        { if(!_json["branch_rate"].IsNumber) { throw new SerializationException(); }  BranchRate = _json["branch_rate"]; }
        { if(!_json["branch_rate_add"].IsNumber) { throw new SerializationException(); }  BranchRateAdd = _json["branch_rate_add"]; }
        { if(!_json["event_rate"].IsNumber) { throw new SerializationException(); }  EventRate = _json["event_rate"]; }
        { if(!_json["event_rate_add"].IsNumber) { throw new SerializationException(); }  EventRateAdd = _json["event_rate_add"]; }
        PostInit();
    }

    public BaseConfig(string id, float overall_grow, float round_time, float base_size, float base_life, float base_cost, float branch_rate, float branch_rate_add, float event_rate, float event_rate_add ) 
    {
        this.Id = id;
        this.OverallGrow = overall_grow;
        this.RoundTime = round_time;
        this.BaseSize = base_size;
        this.BaseLife = base_life;
        this.BaseCost = base_cost;
        this.BranchRate = branch_rate;
        this.BranchRateAdd = branch_rate_add;
        this.EventRate = event_rate;
        this.EventRateAdd = event_rate_add;
        PostInit();
    }

    public static BaseConfig DeserializeBaseConfig(JSONNode _json)
    {
        return new BaseConfig(_json);
    }

    /// <summary>
    /// 这是id
    /// </summary>
    public string Id { get; private set; }
    public float OverallGrow { get; private set; }
    public float RoundTime { get; private set; }
    public float BaseSize { get; private set; }
    public float BaseLife { get; private set; }
    public float BaseCost { get; private set; }
    public float BranchRate { get; private set; }
    public float BranchRateAdd { get; private set; }
    public float EventRate { get; private set; }
    public float EventRateAdd { get; private set; }

    public const int __ID__ = 712913299;
    public override int GetTypeId() => __ID__;

    public  void Resolve(Dictionary<string, object> _tables)
    {
        PostResolve();
    }

    public  void TranslateText(System.Func<string, string, string> translator)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "Id:" + Id + ","
        + "OverallGrow:" + OverallGrow + ","
        + "RoundTime:" + RoundTime + ","
        + "BaseSize:" + BaseSize + ","
        + "BaseLife:" + BaseLife + ","
        + "BaseCost:" + BaseCost + ","
        + "BranchRate:" + BranchRate + ","
        + "BranchRateAdd:" + BranchRateAdd + ","
        + "EventRate:" + EventRate + ","
        + "EventRateAdd:" + EventRateAdd + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}
}
