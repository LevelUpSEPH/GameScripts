using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameStateManager : Singleton<GameStateManager>
{
    public static event Action<GameState> GameStateChanged;

    public enum GameState{
        PlacementState,
        PlayingState
    }

    private GameState _gameState = GameState.PlayingState;

    private void Start(){

    }

    public GameState GetGameState(){
        return _gameState;
    }

    public void SwitchGameStateToPlacement(){
        if(_gameState != GameState.PlacementState)
            _gameState = GameState.PlacementState;

        GameStateChanged?.Invoke(_gameState);
    }

    public void SwitchGameStateToPlaying(){
        if(_gameState != GameState.PlayingState)
            _gameState = GameState.PlayingState;

        GameStateChanged?.Invoke(_gameState);
    }

}
