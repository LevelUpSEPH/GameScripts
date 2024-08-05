using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chameleon.Game.Scripts.Model
{
    public class SoundsHolder : Singleton<SoundsHolder>
    {
        [SerializeField] private AudioClip _correctFoodSound;
        [SerializeField] private AudioClip _wrongFoodSound;
        [SerializeField] private AudioClip _pullBackFoodSound;
        [SerializeField] private AudioClip _eatFoodSound;
        [SerializeField] private AudioClip _gameWonSound;
        [SerializeField] private AudioClip _gameLostSound;
        [SerializeField] private AudioClip _buttonClickSound;

        public AudioClip CorrectFoodSound => _correctFoodSound;
        public AudioClip WrongFoodSound => _wrongFoodSound;
        public AudioClip PullBackFoodSound => _pullBackFoodSound;
        public AudioClip EatFoodSound => _eatFoodSound;
        public AudioClip GameWonSound => _gameWonSound;
        public AudioClip GameLostSound => _gameLostSound;
        public AudioClip ButtonClickSound => _buttonClickSound;
    }
}