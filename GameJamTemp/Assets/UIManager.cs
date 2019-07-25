using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image _Sun;
    public Transform[] _Positions;

    public Image _FadeInOut;

    public bool _FadeIn = false;
    public float _FadeTime = 0;

    public bool _FadeOut = false;


    LightSource _LightSource;
    LightSource.LightSourcePosition _LightPosition;

    // Start is called before the first frame update
    void Start()
    {
        _LightSource = GameObject.FindGameObjectWithTag("LightSource").GetComponent<LightSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_FadeIn)
        {
            FadeIn();
        }

        if (_FadeOut)
        {
            FadeOut();
        }

        _LightPosition = _LightSource._LightPosition;
        if(_LightPosition == LightSource.LightSourcePosition.Center)
        {
            _Sun.rectTransform.position = _Positions[0].position;
        }
        if (_LightPosition == LightSource.LightSourcePosition.Left)
        {
            _Sun.rectTransform.position = _Positions[1].position;
        }
        if (_LightPosition == LightSource.LightSourcePosition.Right)
        {
            _Sun.rectTransform.position = _Positions[2].position;
        }
        if (_LightPosition == LightSource.LightSourcePosition.Down)
        {
            _Sun.rectTransform.position = _Positions[3].position;
        }
        if (_LightPosition == LightSource.LightSourcePosition.Up)
        {
            _Sun.rectTransform.position = _Positions[4].position;
        }
    }

    void FadeIn()
    {
        if (_FadeTime < 0.75f)
        {
            _FadeTime += Time.deltaTime;
            Color a = Color.black;
            a.a = 1.0f - (_FadeTime / 0.75f);
            _FadeInOut.color = a;
        }
        if (_FadeTime >= 0.75f)
        {
            _FadeIn = true;
        }
    }

    void FadeOut()
    {
        if (_FadeTime > 0.0f)
        {
            _FadeTime -= Time.deltaTime;
            Color a = Color.black;
            a.a = 1.0f - (_FadeTime / 0.75f);
            _FadeInOut.color = a;
        }
        if (_FadeTime <= 0.0f)
        {
            _FadeOut = false;
            GameLibrary.GameManager.GameSceneChange();
        }
    }
}
