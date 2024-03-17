using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>Avatar immortality effects setup.</para>
/// <param name="hpManager">the <see cref="HpManager"/></param>
/// <param name="immortalityMaterial">the immortality material</param>
/// <param name="blinkRate">the material blink rate</param>
/// </summary>
public sealed class ImmortalityEffectsSetup : AbstractAvatarSetup
{
    [SerializeField] private HpManager _hpManager;
    [SerializeField] private Material _immortalityMaterial;
    [Range(0.01f, 5)]
    [SerializeField] private float _blinkRate = 0.1f;

    private Dictionary<Renderer, Material> materials = new Dictionary<Renderer, Material>();
    private bool isPlayerImmortal = false;

    private void Start()
    {
        _hpManager.StartImmortality += ChangeMaterialToImmortality;
        _hpManager.StopImmortality += ChangeMaterialToDefault;
    }

    private void OnDestroy()
    {
        _hpManager.StartImmortality -= ChangeMaterialToImmortality;
        _hpManager.StopImmortality -= ChangeMaterialToDefault;
    }

    public override void SetUp(ref Animator spawnedAvatar)
    {
        MaterialsToDictionary(spawnedAvatar, ref materials);
    }

    private void MaterialsToDictionary(Animator spawnedAvatar, ref Dictionary<Renderer, Material> dictionary)
    {
        if (dictionary.Count != 0)
        {
            ChangeMaterialToDefault();
            dictionary.Clear();
        }

        Renderer[] renderers = spawnedAvatar.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            dictionary.Add(renderer, renderer.material);
        }
    }

    private void ChangeMaterialToImmortality()
    {
        isPlayerImmortal = true;
        TurnOnImmortalMaterial();
    }

    private void TurnOnImmortalMaterial()
    {
        if (!isPlayerImmortal)
        {
            return;
        }

        foreach (var material in materials)
        {
            material.Key.material = _immortalityMaterial;
        }

        Invoke("ResetMaterial", _blinkRate);
    }

    private void ResetMaterial()
    {
        TurnOnDefaultMaterial();
        Invoke("TurnOnImmortalMaterial", _blinkRate);
    }

    private void ChangeMaterialToDefault()
    {
        isPlayerImmortal = false;
        TurnOnDefaultMaterial();
    }

    private void TurnOnDefaultMaterial()
    {
        foreach (var material in materials)
        {
            material.Key.material = material.Value;
        }
    }
}