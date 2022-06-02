using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Tile : MonoBehaviour {
    
    [SerializeField] private Color _baseColor, _offsetColor,_wallColor;
    
    [SerializeField] private SpriteRenderer _renderer;
   
    [SerializeField] private GameObject _highlight;

    public bool isWall;
    
    public void Init(bool isOffset) {
       
            isWall = false;
            _renderer.color = isOffset ? _offsetColor : _baseColor;   
    }

    public void makeWall(){
        isWall = true;
        _renderer.color = _wallColor;
       
    }
 
     void OnMouseEnter() {
        _highlight.SetActive(true);
       
    }
 
    void OnMouseExit()
    {
        _highlight.SetActive(false);
       
    }
}