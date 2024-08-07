using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBase : MonoBehaviour
{
    [SerializeField] protected TutorialBase _nextTutorial;
    private bool _isActive = false;

    protected virtual void Start(){
        if(gameObject.activeSelf)
            _isActive = true;
    }

    protected void Update(){
        if(!_isActive)
            return;
        if(HasMetTutorialCondition())
            GotoNextTutorial();
    }

    public void GotoNextTutorial(){
        _isActive = false;
        if(_nextTutorial != null){
            _nextTutorial.gameObject.SetActive(true);
            _nextTutorial.ActivateTutorial();
        }
            
        gameObject.SetActive(false);
    }

    protected virtual bool HasMetTutorialCondition(){
        return false;
    }

    public void ActivateTutorial(){
        _isActive = true;
    }
}
