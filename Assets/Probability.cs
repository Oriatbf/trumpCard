using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public enum ProbabilityState
{
    Nothing,Win,Fail,Common,Rare,Epic
}

[Serializable]
public class RandomPercent
{
    [Tooltip("Float가 아닌 String형 100.00 이런 방식으로 작성할 것")]
    public string percentage;
    public List<Percent> percents = new List<Percent>();

}

[Serializable]
public class Percent
{
    public ProbabilityState probabilityState;
    public float percent;
    public UnityEvent onEvent;

}


public static class Probability 
{
    public static Percent RandomProbability(RandomPercent _randomPercent)
    {
        int _totalPercentage = 100;
        int _decimalCount = 0;

        #region 소수점 자릿수 구하기
        string percentageStr = _randomPercent.percentage;
        if (percentageStr.Contains("."))
        {
            string[] parts = percentageStr.Split('.');
            _decimalCount = parts[1].Length;
            if (_decimalCount > 0)
            {
                _totalPercentage *= (int)Mathf.Pow(10, _decimalCount);
            }
        }
        Debug.Log(_totalPercentage);

        ProbabilityState resultState  = ProbabilityState.Nothing;

        Dictionary<ProbabilityState, int> dict = new Dictionary<ProbabilityState, int>();
        int sum = 0;
        foreach (var _percent in _randomPercent.percents)
        {
            sum += (int)(_percent.percent * Mathf.Pow(10, _decimalCount));
            dict.Add(_percent.probabilityState,sum);
            Debug.Log(sum);
        }

        if (sum != _totalPercentage)
        {
            Debug.Log(sum);
            Debug.LogError("확률이 정확하지 않음");
        }


        #endregion
 
        
        int random = Random.Range(1, _totalPercentage + 1);
        foreach (var _dict in dict)
        {
            if (random <= _dict.Value)
            {
                resultState = _dict.Key;
                break;
            }
        }
        
        Debug.Log(resultState);
        return _randomPercent.percents.FirstOrDefault(p=>p.probabilityState == resultState);
    }
}
