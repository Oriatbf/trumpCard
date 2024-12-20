using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GambleGauge : MonoBehaviour
{
    private Character character;
    [SerializeField] private Image gambleImage;
    private float gaugePerSec=1;
    public float _curGauge, maxGauge;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    void Start()
    {
        gambleImage.fillAmount = _curGauge / maxGauge;
    }

    // Update is called once per frame
    void Update()
    {
        _curGauge += gaugePerSec * Time.deltaTime;
        gambleImage.fillAmount = _curGauge / maxGauge;

        if (_curGauge >= maxGauge && !TimeManager.Inst.timeChanging)
        {
            character.Gambling();
            _curGauge = 0;
        }
    }

    public void IncreaseGambleGauge(float value)
    {
        _curGauge+= value;
        if(_curGauge > maxGauge )_curGauge = maxGauge;
        gambleImage.fillAmount = _curGauge / maxGauge;
    }
}
