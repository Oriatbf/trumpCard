using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GambleGauge : MonoBehaviour
{
    [SerializeField] private Image gambleImage;
    [SerializeField] private float maxGauge,gaugePerSec;
    [SerializeField] private float _curGauge;
    // Start is called before the first frame update
    void Start()
    {
        gambleImage.fillAmount = _curGauge / maxGauge;
    }

    // Update is called once per frame
    void Update()
    {
        _curGauge += gaugePerSec * Time.deltaTime;
        gambleImage.fillAmount = _curGauge / maxGauge;
    }

    public void IncreaseGambleGauge(float value)
    {
        _curGauge+= value;
        if(_curGauge > maxGauge )_curGauge = maxGauge;
        gambleImage.fillAmount = _curGauge / maxGauge;
    }
}
