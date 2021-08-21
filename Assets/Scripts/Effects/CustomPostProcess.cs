using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[ExecuteInEditMode]
public class CustomPostProcess : MonoBehaviour
{
    [SerializeField] Material Mat;
    [SerializeField] float lensDistortion;
    [SerializeField] float lineDisplacement;
    [SerializeField] float sineLineThreshold;
    void Awake()
    {
        //Player.TakeDamage += TakeDamage;
    }
    void Start()
    {
        Mat.SetFloat("_LensDistortion", lensDistortion);
        Mat.SetFloat("_LinesDisplacement", lineDisplacement);
        Mat.SetFloat("_SineLinesThreshold", sineLineThreshold);
    }
    //void TakeDamage(Player player)
    //{
    //    StartCoroutine(changeFloat("_LensDistortion", lensDistortion + 0.2f, lensDistortion));
    //    StartCoroutine(changeFloat("_LinesDisplacement", 5.0f, lineDisplacement));
    //    StartCoroutine(changeFloat("_SineLinesThreshold", 1.0f, sineLineThreshold));
    //}
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(Mat!=null)
            Graphics.Blit(source, destination, Mat);
    }

    IEnumerator changeFloat(string name, float value, float originalValue)
    {
        Mat.SetFloat(name, value);
        yield return new WaitForSeconds(0.5f);
        Mat.SetFloat(name, originalValue);
        yield return null;
    }

    void OnDestroy()
    {
        //Player.TakeDamage -= TakeDamage;
    }
}
