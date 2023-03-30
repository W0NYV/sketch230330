using System;
using UnityEngine;

public class RDSystem : MonoBehaviour
{
    [SerializeField] private ComputeShader _computeShader;
    [SerializeField] private Material _material;
    
    private RenderTexture _cardinalityTexture; //Rが濃度u、Gが濃度v

    [SerializeField] private int _width = 960;
    [SerializeField] private int _height = 540;

    private float _Du = 0.00002f; //濃度uの拡散係数
    private float _Dv = 0.00001f; //濃度vの拡散係数
    [SerializeField] private float _f = 0.037f; //補充
    [SerializeField] private float _k = 0.06f; //減量

    [SerializeField] private int _step = 1;
    
    private void Start()
    {
        _cardinalityTexture = new RenderTexture(_width, _height, 0, RenderTextureFormat.RG32);
        _cardinalityTexture.enableRandomWrite = true;
        _cardinalityTexture.Create();

        int kernelIdx_Init = _computeShader.FindKernel("Init");
        
        _computeShader.SetTexture(kernelIdx_Init, "_CardinalityTexture", _cardinalityTexture);
        _computeShader.SetInt("_Width", _width);
        _computeShader.SetInt("_Height", _height);

        _computeShader.Dispatch(kernelIdx_Init, _width / 8,_height / 8,1);
    }

    private void Update()
    {
        for (int i = 0; i < _step; i++)
        {
            int kernelIdx_Update = _computeShader.FindKernel("Update");

            _computeShader.SetTexture(kernelIdx_Update, "_CardinalityTexture", _cardinalityTexture);
            _computeShader.SetFloat("_Du", _Du);
            _computeShader.SetFloat("_Dv", _Dv);
            _computeShader.SetFloat("_f", _f);
            _computeShader.SetFloat("_k", _k);

            _computeShader.Dispatch(kernelIdx_Update, _width / 8, _height / 8, 1);

        }
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        _material.SetTexture("_CardinalityTexture", _cardinalityTexture);
        Graphics.Blit(src, dest, _material);
    }
}
