using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enums for state system
public enum GameStates { BeforeHand, Preflop, Flop, Turn, River, EndHand};

public enum PlayerStates { Bet, Raise, Fold, Check, Nothing};

public enum EnemyStates { Bet, Raise, Fold, Check, Nothing };