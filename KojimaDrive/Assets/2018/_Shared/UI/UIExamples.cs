using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine;

//===================== Kojima Party - Team Lunatics 2018 ====================//
//
// Author:		Daniel Donaldson
// Purpose:		Info and Examples for UI System
// Namespace:	LT
//
//===============================================================================//

/// ********** <INFO> ********** \\\
/// 
/// ********** <SETTING_UP_UI_MANAGER> ********** \\\
/// 
/// To set up the UI manager, drag the UI manager prefab onto your player or player prefab object
/// 
/// 
/// ********** <ADDING_AN_ELEMENT> ********** \\\
///
/// NOTE: All elements have default positions in the UI, based on feedback from groups
/// 
/// To add an elemnt to the canvas, select the relevant button to activate it
///
/// 
/// ********** <SINGLE_CAMERA_MODE> ********** \\\
/// 
/// Pressing Single Camera mode generates a scoreboard for games with only one perspective
/// This element assumes that default health bars/lives/ammo and power bars are not being used
/// 
/// 
/// ********** <TEST_MODE> ********** \\\
/// 
/// Presseing Test mode applies test default values to the elements making it easier to see their layout on screen
/// 
/// 
/// ********** <ARCADE_MODE> ********** \\\
/// 
/// ***** This Element is stil in development *****
/// Pressing the arcade mode feature changes some of the elements such as the timer to a style similar to that of an arcade machine
///
/// 
/// ********** <END> ********** \\\

class UIExamples {

    private int testInt;
    private string testString;
    private int testID;

    private Canvas testPlayer1Canvas;
    private Canvas testPlayer2Canvas;
    private Canvas testPlayer3Canvas;
    private Canvas testPlayer4Canvas;

    private AudioClip testAudioclip;
    private AudioSource testAudioSource;

    // Use this for initialization
    private void Start() { 
    }

    // Update is called once per frame
    private void Update() {
    }

    private void PowerBarExample()
    {
        // Pass in value to change power bar value by - e.g increase powerbar value over time (testInt = 1)
        LT.PowerBar.instance.ChangePowerValueBy(testInt);

        // Set power bar value to specific value - e.g current powerbar value = zero (testInt = 0)
        LT.PowerBar.instance.SetPowerValue(testInt);

        // Get current vaule of the power bar
        LT.HealthBar.instance.GetCurrentHealth();
    }

    private void HealthBarExample()
    {
        // Pass in value to change health bar value by - e.g increase powerbar value over time (testInt = 1)
        LT.HealthBar.instance.ChangeHealthValueBy(testInt);

        // Set health bar value to specific value - e.g current powerbar value = zero
        LT.HealthBar.instance.SetHealthValue(testInt);

        // Get current vaule of the health bar
        LT.HealthBar.instance.GetCurrentHealth();
    }

    private void LivesAmmoExample()
    {
        // Retrieve current number of lives/ammo 
        LT.LivesOrAmmo.instance.GetLives();

        // Pass in the number of lives/ammo to add or deduct - e.g if player takes damage -1 life (testInt = -1)
        LT.LivesOrAmmo.instance.ChangeLives(testInt);
    }

    private void TimerExample()
    {
        // Retrieve current timer value
        //LT.Timer.instance.GetTimerValue();

        // Set current timer value - e.g reset value (testInt = 0)
        //LT.Timer.instance.SetTimerValue(testInt);
    }

    private void CountdownExample()
    {
        // Call this when you want to trigger the countdown
        LT.Countdown.instance.ActivateCountdown();

        // Call this when you want the countdown to activate a game function, e.g a respawn - returns a Bool - 
        LT.Countdown.instance.BeginGame();

        // To set custom audio clip sounds call this function activating countdown
        LT.Countdown.instance.SetCustomSounds(testAudioclip, testAudioclip);
    }

    private void WarningExample()
    {
        // Set the flash inerval of the warning - e.g lower flash intervals will make the text flash faster (testInt = 0.5f)
        LT.WarningText.instance.SetFlashInterval(testInt);

        // Set a custom warning text - e.g (testString = "Turn Back")
        LT.WarningText.instance.SetWarningText(testString);

        // Get current warning text string
        LT.WarningText.instance.GetWarningText();

        // Call this to activate the warning text
        LT.WarningText.instance.ActivateWarning();

        // Call this to deactivate the warning text
        LT.WarningText.instance.DeactivateWarning();
    }

    private void ScoreExample()
    {
        // Retrieve current score
        LT.Score.instance.GetScore();

        // Pass in value to change Score value by - e.g increase socre by 10 points (testInt = 10)
        LT.Score.instance.ChangeScore(testInt, testID);
    }

    private void ScoreboardExample()
    {
        ///////// This element requires the player to have a score element \\\\\\\

        // Retrieve current placement of the player noted by it's id. e-g player threes's position (testID = 3)
        LT.PlacementText.instance.GetCurrentPosition(testID);
    }

    private void PlacementTextExample()
    {
        ///////// This element requires the player to have a score element \\\\\\\

        // Retrieve current placement of the player noted by it's id. e-g player threes's position (testID = 3)
        LT.PlacementText.instance.GetCurrentPosition(testID);
    }
}