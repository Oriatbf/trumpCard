using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEffect : MonoBehaviour
{
    bool isDash;
    [SerializeField] float effectSpawnCool;
    [SerializeField] SpriteRenderer ghost;
    float _curSpawnCool;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(_curSpawnCool >0) _curSpawnCool -= Time.deltaTime;

        if(isDash && _curSpawnCool <= 0)
        {
            _curSpawnCool = effectSpawnCool;
            SpriteRenderer currentEffect = Instantiate(ghost, transform.position, Quaternion.identity);
            currentEffect.DOFade(0, 0.5f);
            Destroy(currentEffect.gameObject, 0.501f);
        }
    }

    public void ActiveDashEffect(float time)
    {
        StartCoroutine(EffectActiveCool(time));
    }

    IEnumerator EffectActiveCool(float time)
    {
        isDash= true;
        yield return new WaitForSeconds(time);
        isDash= false;
    }


}
