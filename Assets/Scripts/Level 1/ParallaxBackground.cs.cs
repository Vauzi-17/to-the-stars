using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ParallaxBackground : MonoBehaviour
{
    public ParallaxCamera parallaxCamera;
    List<ParallaxLayerLvl1> ParallaxLayerLvl1s = new List<ParallaxLayerLvl1>();

    void Start()
    {
        if (parallaxCamera == null)
            parallaxCamera = Camera.main.GetComponent<ParallaxCamera>();

        if (parallaxCamera != null)
            parallaxCamera.onCameraTranslate += Move;

        SetLayers();
    }

    void SetLayers()
    {
        ParallaxLayerLvl1s.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            ParallaxLayerLvl1 layer = transform.GetChild(i).GetComponent<ParallaxLayerLvl1>();

            if (layer != null)
            {
                layer.name = "Layer-" + i;
                ParallaxLayerLvl1s.Add(layer);
            }
        }
    }

    void Move(float delta)
    {
        foreach (ParallaxLayerLvl1 layer in ParallaxLayerLvl1s)
        {
            layer.Move(delta);
        }
    }
}