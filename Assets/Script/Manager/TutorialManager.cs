using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TutorialManager : SingletonDontDestroyOnLoad<TutorialManager>
{
    public bool isTutorialing;
    public PlayableDirector[] playableDirectors;

    protected override void Awake()
    {
        base.Awake();
    }


    public void ClickTutorial()
    {

        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                playableDirectors[0].Play(); break;
            case 1:
                playableDirectors[1].Play(); break;
          
        }
     isTutorialing= true;
    }

    public void EndTutorial()
    {
        isTutorialing= false;
    }
}
