using System;
using UnityEngine;

public class Dissolve_Shader_Values : MonoBehaviour
{
  public float intensity=10; 
  [SerializeField] private float red; 
  [SerializeField] private float green; 
  [SerializeField] private float blue; 
  private MaterialPropertyBlock _block;
  private SpriteRenderer _renderer; // This is your sprite renderer or mesh renderer
  [SerializeField] private Texture2D sprite;
  public bool tryNewColors;

  private void Awake()
  {
    _block = new MaterialPropertyBlock();
    _renderer = GetComponent<SpriteRenderer>();
  }

  private void Start()
  {
    setColor();
  }

  public void setColor()
  {
    double factor= Math.Pow(2, intensity);
    Color color = new Color((float) (red*factor),(float) (green*factor),(float) (blue*factor));
//  Debug.Log(color);
    _block.SetTexture("_MainTex",sprite); // _Texture or whatever variable you kept for the material to change
    _block.SetColor("_Color",color); // _Color or whatever variable you kept for the material to change
    _renderer.SetPropertyBlock(_block);
    //_renderer.sprite = sprite;
  }

  private void Update()
  {
    if (tryNewColors)
    {
      setColor();
    }
  }
}
