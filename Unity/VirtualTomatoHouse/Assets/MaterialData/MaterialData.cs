using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "MaterialData", menuName = "ScriptableObjects/MaterialData", order = 1)]
public class MaterialData : ScriptableObject
{
    public enum TextureProperty
    {
        _MainTex,
        _Color,
        _BumpMap,
        _NormalMap,
        _OcclusionMap,
        _SpecGlossMap,
        _MetallicGlossMap,
        _ParallaxMap,
        _DetailMask,
        _DetailAlbedoMap,
        _DetailNormalMap,
        _EmissionMap,
        _Cube,
        _Cube_HDR,
        _ShadowTex,
        _ShadowMap,
        _LightTexture0,
        _LightTextureB0,
        _DetailNormalMapScale
    }


    [Serializable]
    public struct TextureRule
    {
        public string footer;
        public TextureProperty textureProperty;
    }

    public string Header;
    public Shader Shader;

    //他パラメータ必要あればここに追加

    public List<TextureRule> TextureRules = new List<TextureRule>();
}
