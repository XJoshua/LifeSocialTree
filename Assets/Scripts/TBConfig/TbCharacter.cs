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

public sealed partial class TbCharacter
{
    private readonly Dictionary<string, CharacterConfig> _dataMap;
    private readonly List<CharacterConfig> _dataList;
    
    public TbCharacter(JSONNode _json)
    {
        _dataMap = new Dictionary<string, CharacterConfig>();
        _dataList = new List<CharacterConfig>();
        
        foreach(JSONNode _row in _json.Children)
        {
            var _v = CharacterConfig.DeserializeCharacterConfig(_row);
            _dataList.Add(_v);
            _dataMap.Add(_v.Id, _v);
        }
        PostInit();
    }

    public Dictionary<string, CharacterConfig> DataMap => _dataMap;
    public List<CharacterConfig> DataList => _dataList;

    public CharacterConfig GetOrDefault(string key) => _dataMap.TryGetValue(key, out var v) ? v : null;
    public CharacterConfig Get(string key) => _dataMap[key];
    public CharacterConfig this[string key] => _dataMap[key];

    public void Resolve(Dictionary<string, object> _tables)
    {
        foreach(var v in _dataList)
        {
            v.Resolve(_tables);
        }
        PostResolve();
    }

    public void TranslateText(System.Func<string, string, string> translator)
    {
        foreach(var v in _dataList)
        {
            v.TranslateText(translator);
        }
    }
    
    
    partial void PostInit();
    partial void PostResolve();
}

}