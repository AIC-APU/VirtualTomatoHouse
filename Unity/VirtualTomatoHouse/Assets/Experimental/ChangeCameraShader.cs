using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCameraShader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Camera>().SetReplacementShader(Shader.Find("IndexTextureLinear"), null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
