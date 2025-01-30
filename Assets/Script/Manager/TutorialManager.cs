using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private MapTutorial mapTutorial;
    public void ClickTutorial()
    {

        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
               mapTutorial.Open();
                break;
            case 1:
               mapTutorial.Open();
               break;
          
        }
    }


}
