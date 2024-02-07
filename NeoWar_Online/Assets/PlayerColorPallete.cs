using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PlayerColorPallete : NetworkBehaviour
{
    public static PlayerColorPallete Instance;

    [SerializeField] private List<Color> allColors;
    private List<Color> availableColor;
    private void Awake()
    {
        if(Instance !=null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        availableColor = new List<Color>();
        allColors.CopyTo(availableColor);
    }

    public Color TakeRandomColor()
    {
        int index = Random.Range(0, availableColor.Count);
        Color color = availableColor[index];

        availableColor.RemoveAt(index);

        return color;
    }

    public void PutColor(Color color)
    {
        if(allColors.Contains(color)==true)
        {
            if(availableColor.Contains(color)==false)
            {
                availableColor.Add(color);
            }
        }
    }
}
