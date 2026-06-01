using UnityEngine;
using System.Collections;

public class SkyTransitionSystem : MonoBehaviour
{
    [System.Serializable]
    public class SkyZone
    {
        public string name;
        public float minHeight;
        public float maxHeight;
        public Sprite skySprite;
        public Color skyTint = Color.white;
        [Header("Cloud Visibility")]
        [Range(0, 1)] public float cloudFarAlpha = 1f;
        [Range(0, 1)] public float cloudMidAlpha = 1f;
        [Range(0, 1)] public float cloudNearAlpha = 1f;
        [Range(0, 1)] public float starsAlpha = 0f;
        [Range(0, 1)] public float nebulaAlpha = 0f;
        [Range(0, 1)] public float starsBigAlpha = 0f;
        [Range(0, 1)] public float moonAlpha = 0f;
    }

    [Header("References")]
    public Transform player;
    public SpriteRenderer skyRenderer;
    public SpriteRenderer cloudFar;
    public SpriteRenderer cloudMid;
    public SpriteRenderer cloudNear;

    [Header("Sky Zones")]
    public SkyZone[] zones;

    [Header("Transition")]
    public float transitionSpeed = 2f;

    [Header("Extra Layers")]
    public SpriteRenderer starsLayer;

    [Header("Space Layers")]
    public SpriteRenderer nebulaLayer;
    public SpriteRenderer nebula2Layer;
    public SpriteRenderer nebula3Layer;
    public SpriteRenderer starsBigLayer;

    [Header("Fade Overlay")]
    public UnityEngine.UI.Image fadeOverlay;

    [Header("Moon")]
    public SpriteRenderer moonLayer;
    private float currentMoonAlpha = 0f;

    private SkyZone currentZone;
    private SkyZone targetZone;

    // Simpan alpha saat ini untuk semua layer
    private float currentStarsAlpha = 0f;
    private float currentNebulaAlpha = 0f;
    private float currentStarsBigAlpha = 0f;

    void Start()
    {
        if (zones.Length > 0)
        {
            currentZone = zones[0];
            ApplyZoneImmediate(currentZone);
        }
    }

    void Update()
    {
        SkyZone zone = GetZoneAtHeight(player.position.y);
        if (zone != null && zone != targetZone)
        {
            targetZone = zone;
            StartCoroutine(TransitionToZone(targetZone));
        }
    }

    SkyZone GetZoneAtHeight(float y)
    {
        foreach (var zone in zones)
        {
            if (y >= zone.minHeight && y < zone.maxHeight)
                return zone;
        }
        return null;
    }

    IEnumerator TransitionToZone(SkyZone zone)
    {
        float t = 0;
        Color startTint = skyRenderer.color;
        float startStarsAlpha = currentStarsAlpha;
        float startNebulaAlpha = currentNebulaAlpha;
        float startStarsBigAlpha = currentStarsBigAlpha;
        float startMoonAlpha = currentMoonAlpha;

        // Ganti sprite LANGSUNG di awal (tidak keliatan karena tint sama)
        if (zone.skySprite != null)
            skyRenderer.sprite = zone.skySprite;

        while (t < 1f)
        {
            t += Time.deltaTime * (transitionSpeed * 0.5f); // lebih lambat

            skyRenderer.color = Color.Lerp(startTint, zone.skyTint, t);

            SetCloudAlpha(cloudFar, startTint.a, zone.cloudFarAlpha, t);
            SetCloudAlpha(cloudMid, startTint.a, zone.cloudMidAlpha, t);
            SetCloudAlpha(cloudNear, startTint.a, zone.cloudNearAlpha, t);

            if (starsLayer != null)
            {
                Color sc = starsLayer.color;
                sc.a = Mathf.Lerp(startStarsAlpha, zone.starsAlpha, t);
                starsLayer.color = sc;
            }
            if (nebulaLayer != null)
            {
                Color c = nebulaLayer.color;
                c.a = Mathf.Lerp(startNebulaAlpha, zone.nebulaAlpha, t);
                nebulaLayer.color = c;
            }
            if (starsBigLayer != null)
            {
                Color c = starsBigLayer.color;
                c.a = Mathf.Lerp(startStarsBigAlpha, zone.starsBigAlpha, t);
                starsBigLayer.color = c;
            }
            if (moonLayer != null)
            {
                Color c = moonLayer.color;
                c.a = Mathf.Lerp(startMoonAlpha, zone.moonAlpha, t);
                moonLayer.color = c;
            }

            yield return null;
        }

        currentStarsAlpha = zone.starsAlpha;
        currentNebulaAlpha = zone.nebulaAlpha;
        currentStarsBigAlpha = zone.starsBigAlpha;
        currentMoonAlpha = zone.moonAlpha;
        currentZone = zone;
    }

    void SetCloudAlpha(SpriteRenderer sr, float from, float to, float t)
    {
        Color c = sr.color;
        c.a = Mathf.Lerp(from, to, t);
        sr.color = c;
    }

    void ApplyZoneImmediate(SkyZone zone)
    {
        if (zone.skySprite != null)
            skyRenderer.sprite = zone.skySprite;
        if (zone.skyTint.a > 0)
            skyRenderer.color = zone.skyTint;

        SetAlpha(cloudFar, zone.cloudFarAlpha);
        SetAlpha(cloudMid, zone.cloudMidAlpha);
        SetAlpha(cloudNear, zone.cloudNearAlpha);

        if (starsLayer != null)
        {
            currentStarsAlpha = zone.starsAlpha;
            Color sc = starsLayer.color;
            sc.a = currentStarsAlpha;
            starsLayer.color = sc;
        }

        if (nebulaLayer != null)
        {
            currentNebulaAlpha = zone.nebulaAlpha;
            Color c = nebulaLayer.color;
            c.a = currentNebulaAlpha;
            nebulaLayer.color = c;
        }

        if (starsBigLayer != null)
        {
            currentStarsBigAlpha = zone.starsBigAlpha;
            Color c = starsBigLayer.color;
            c.a = currentStarsBigAlpha;
            starsBigLayer.color = c;
        }
        if (moonLayer != null)
        {
            currentMoonAlpha = zone.moonAlpha;
            Color c = moonLayer.color;
            c.a = currentMoonAlpha;
            moonLayer.color = c;
        }
    }

    void SetAlpha(SpriteRenderer sr, float alpha)
    {
        Color c = sr.color;
        c.a = alpha;
        sr.color = c;
    }
}