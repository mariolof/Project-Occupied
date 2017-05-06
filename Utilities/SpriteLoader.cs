using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLoader {

    public Dictionary<string, Sprite> tileSprite;
    public Dictionary<string, Sprite> humanSprite;
    public Dictionary<string, Sprite> archSprite;
    public Dictionary<string, Sprite> itemSprite;
    public Dictionary<string, Sprite> plantSprite;

    public Dictionary<string, Sprite> otherSprite;


    public void loadTileSprite()
    {
        tileSprite = new Dictionary<string, Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprite/Tile");

        foreach (Sprite s in sprites)
        {
            tileSprite[s.name] = s;
        }
    }


    public void loadHumanSprite()
    {
        humanSprite = new Dictionary<string, Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprite/Human");

        foreach (Sprite s in sprites)
        {
            humanSprite[s.name] = s;
        }
    }


    public void loadArchitectureSprite()
    {
        archSprite = new Dictionary<string, Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprite/Architecture");

        foreach (Sprite s in sprites)
        {
            archSprite[s.name] = s;
        }
    }


    public void loadItemSprite()
    {
        itemSprite = new Dictionary<string, Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprite/Item");

        foreach (Sprite s in sprites)
        {
            itemSprite[s.name] = s;
        }
    }


    public void loadPlantSprite()
    {
        plantSprite = new Dictionary<string, Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprite/Plant");

        foreach (Sprite s in sprites)
        {
            plantSprite[s.name] = s;
        }
    }


    public void loadOtherSprite()
    {
        otherSprite = new Dictionary<string, Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprite/UI");

        foreach (Sprite s in sprites)
        {
            otherSprite[s.name] = s;
        }
    }
}
