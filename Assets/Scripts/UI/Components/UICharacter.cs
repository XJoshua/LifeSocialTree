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

        NameText.text = character.Name;
        RelationShipText.text = $"{character.Relationship} · {character.Desc}";

        var showJob = !character.Job.IsNullOrEmpty();
        JobObj.SetActive(showJob);
        JobText.text = character.Job;
        
        Sprite.sprite = Service.Cfg.GetCharacterIcon(character.Id);
    }
}
