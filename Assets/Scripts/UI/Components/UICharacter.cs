using System.Collections;
using System.Collections.Generic;
using cfg;
using Game;
using QFramework;
using TMPro;
using UnityEngine;

public class UICharacter : MonoBehaviour
{
    public SpriteRenderer Sprite;

    public TextMeshPro NameText;
    
    public TextMeshPro RelationShipText;

    public GameObject JobObj;

    public TextMeshPro JobText;
    
    public void Setup(string id)
    {
        var character = Service.Cfg.GetCfgCharacter(id);

        transform.localScale = new Vector3(character.FrameSize, character.FrameSize, character.FrameSize);
        
        NameText.text = character.Name;

        var inter = character.Relationship.IsNullOrEmpty() ? "" : " · ";
        RelationShipText.text = $"{character.Relationship}{inter}{character.Desc}";

        var showJob = !character.Job.IsNullOrEmpty();
        JobObj.SetActive(showJob);
        JobText.text = character.Job;
        
        Sprite.sprite = Service.Cfg.GetCharacterIcon(character.Id);
    }
}
