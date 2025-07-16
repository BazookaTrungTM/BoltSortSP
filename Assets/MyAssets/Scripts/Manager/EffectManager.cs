using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Do;

public class EffectManager : Singleton<EffectManager>
{
    [SerializeField] Canvas canvasTextEffect;
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera cameraUI;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    #region Fill
    [SerializeField] List<ParticleSystem> fillEffects = new List<ParticleSystem>();
    public void InitFillEffect(int index, Vector3 pos)
    {
        fillEffects[index].transform.localPosition = pos;
    }

    public void PlayFillEffect(int index, Color color)
    {
        color.a = 1;
        var maiN = fillEffects[index].main;
        maiN.startColor = color;
        fillEffects[index].Play();
        if (index == 0)
            AudioManager.instance.PlaySFX(SoundType.Complete_Holder_1);
        else if (index == 1)
            AudioManager.instance.PlaySFX(SoundType.Complete_Holder_2);
        else
            AudioManager.instance.PlaySFX(SoundType.Complete_Holder_3);
    }

    #endregion

    #region Text Effect
    public float speed;
    [SerializeField] Transform textHolder;
    [SerializeField] private TextEffect prefab;
    private readonly List<TextEffect> _effectTexts = new List<TextEffect>();
    public void PlayEffectText(string content, Vector3 position = default(Vector3), bool isLocal = false, int size = 50, Callback complete = null)
    {
        var effectText = GetEffectText();
        if (isLocal)
        {
            Vector3 screenPos = mainCamera.WorldToScreenPoint(position);
            Debug.Log(screenPos);

            Vector2 uiPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasTextEffect.GetComponent<RectTransform>(),
                screenPos,
                mainCamera,
                out uiPos);
            effectText.ShowEffectText(content, uiPos, size, complete, isLocal);
        }
        else
            effectText.ShowEffectText(content, position, size, complete, isLocal);
    }

    private TextEffect GetEffectText()
    {
        foreach (var effectText in _effectTexts)
        {
            if (!effectText.IsActive)
                return effectText;
        }

        var newEffectText = Instantiate(prefab, textHolder);
        newEffectText.name = "Effect Text";
        _effectTexts.Add(newEffectText);
        return newEffectText;
    }
    #endregion

    #region Nut Effect
    [SerializeField] private ParticleSystem nutEffPrefab;
    private readonly List<ParticleSystem> nutEffectList = new List<ParticleSystem>();
    public void PlayNutSpecialEffect(Vector3 pos)
    {
        var nutEffect = GetNutEffect();
        nutEffect.transform.position = pos;
        nutEffect.Play();
    }
    public void PlayAddColEffect(Vector3 pos)
    {
        var nutEffect = GetNutEffect();
        nutEffect.transform.localPosition = pos;
        nutEffect.Play();
    }
    private ParticleSystem GetNutEffect()
    {
        foreach (var effect in nutEffectList)
        {
            if (!effect.isPlaying)
                return effect;
        }

        var nutEff = Instantiate(nutEffPrefab, transform);
        nutEffectList.Add(nutEff);
        return nutEff;
    }
    #endregion

    #region Bomb Explore Effect
    [SerializeField] private ParticleSystem bombVFX;
    private readonly List<ParticleSystem> bombEffectList = new List<ParticleSystem>();
    public void PlayBombExploreEffect(Vector3 pos)
    {
        var nutEffect = GetBombExploreVFX();
        nutEffect.transform.position = pos;
        nutEffect.Play();
        AudioManager.instance.PlaySFX(SoundType.Bomb_Explore);
    }
    private ParticleSystem GetBombExploreVFX()
    {
        foreach (var effect in bombEffectList)
        {
            if (!effect.isPlaying)
                return effect;
        }

        var nutEff = Instantiate(bombVFX, transform);
        bombEffectList.Add(nutEff);
        return nutEff;
    }
    #endregion

    #region Cut Rope Effect
    [SerializeField] private ParticleSystem cutRopeVfx;
    private readonly List<ParticleSystem> cutRopeVfxList = new List<ParticleSystem>();
    public void PlayCutRopeVfx(Vector3 pos)
    {
        var nutEffect = GetCutRopeVfx();
        nutEffect.transform.position = pos;
        nutEffect.Play();
    }
    private ParticleSystem GetCutRopeVfx()
    {
        foreach (var effect in cutRopeVfxList)
        {
            if (!effect.isPlaying)
                return effect;
        }

        var nutEff = Instantiate(cutRopeVfx, transform);
        cutRopeVfxList.Add(nutEff);
        return nutEff;
    }
    #endregion






}
