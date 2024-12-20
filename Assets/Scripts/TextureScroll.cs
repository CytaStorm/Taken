using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScroll : MonoBehaviour
{
    public Vector2 scroll;
    public Vector2 scaleFactor;
    public Vector2 tiling;

    private Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();   
    }

    // Update is called once per frame
    void Update()
    {
        scaleFactor = new Vector2(1/transform.localScale.x, 1/transform.localScale.z);

        // Offset the texture
        renderer.material.mainTextureOffset 
            += scroll * Time.deltaTime * scaleFactor * renderer.material.mainTextureScale * 0.1f;
    }
}
