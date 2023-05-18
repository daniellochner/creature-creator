using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class TestGameServices : MonoBehaviour
{
    private AchievementNames[] allAchievements;
    private LeaderboardNames[] allLeaderboards;
    private Vector2 scrollViewVector = Vector2.zero;
    private string scoreText = "0";
    private float buttonWidth;
    private float buttonHeight;
    private float padding;
    private long score = 0;
    private int indexNumberAchievements;
    private int indexNumberLeaderboards;
    private int nr = 0;
    private bool showAchievements = false;
    private bool showLeaderboards = false;

    private void Start()
    {
        //setup the screen buttons
        buttonWidth = Screen.width;
        buttonHeight = Screen.height / 15;
        padding = Screen.width / 6;

        //make a list of all achievements
        int nrOfAchievements = System.Enum.GetValues(typeof(AchievementNames)).Length;
        allAchievements = new AchievementNames[nrOfAchievements];
        for (int i = 0; i < nrOfAchievements; i++)
        {
            allAchievements[i] = ((AchievementNames)i);
        }

        //make a list of all leaderboards
        int nrOfLeaderboards = System.Enum.GetValues(typeof(LeaderboardNames)).Length;
        allLeaderboards = new LeaderboardNames[nrOfLeaderboards];
        for (int i = 0; i < nrOfLeaderboards; i++)
        {
            allLeaderboards[i] = ((LeaderboardNames)i);
        }
    }

    private void OnGUI()
    {
        //some display setup
        GUI.skin.button.fontSize = 30;
        GUI.skin.label.fontSize = 30;
        GUI.skin.label.alignment = TextAnchor.LowerCenter;
        GUI.skin.textField.fontSize = 30;
        GUI.skin.textField.alignment = TextAnchor.MiddleCenter;
        nr = 0;

        if (!GameServices.Instance.IsLoggedIn())
        {
            if (Button("Login"))
            {
                //Login to Game Servicies
                GameServices.Instance.LogIn(LoginResult);
            }
        }
        else
        {
            if (allAchievements.Length > 0)
            {
                Label("Achievements");

                Label("Select an achievement from drop down list below: ");

                #region DropDownAchievements
                //used to select a single achievement from the list
                if (Button("\\/   " + "Tap to select an achievement" + "   \\/"))
                {
                    if (!showAchievements)
                    {
                        showAchievements = true;
                    }
                    else
                    {
                        showAchievements = false;
                    }
                }
                if (showAchievements)
                {
                    DropDown(7, ref indexNumberAchievements, ref showAchievements, allAchievements);
                }
                #endregion

                Label("Selected Achievement: " + allAchievements[indexNumberAchievements]);
                if (Button("Submit Selected Achievement"))
                {
                    Debug.Log("Submit Achievement: " + allAchievements[indexNumberAchievements]);
                    GleyGameServices.ScreenWriter.Write("Submit Achievement: " + allAchievements[indexNumberAchievements]);

                    //submit an achievement
                    //it also can be done like this:
                    //GameServices.Instance.SubmitAchievement(AchievementNames.Achievement1);
                    GameServices.Instance.SubmitAchievement(allAchievements[indexNumberAchievements], AchievementSUbmitted);
                    //GameServices.Instance.IncrementAchievement(allAchievements[indexNumberAchievements],1, AchievementSUbmitted);
                }

                if (Button("Show Achievements UI"))
                {
                    //Show all game achievements
                    GameServices.Instance.ShowAchievementsUI();
                }
            }
            else
            {
                Label("No Achievements Configured");
            }

            if (allLeaderboards.Length > 0)
            {
                Label("Leaderboards");

                Label("Select a Leaderboard from drop down list below: ");

                #region DropDownLeaderboards
                //used to select a single leaderboard from the list
                if (Button("\\/   " + "Tap to select a leaderboard" + "   \\/"))
                {
                    if (!showLeaderboards)
                    {
                        showLeaderboards = true;
                    }
                    else
                    {
                        showLeaderboards = false;
                    }
                }

                if (showLeaderboards)
                {
                    DropDown(4, ref indexNumberLeaderboards, ref showLeaderboards, allLeaderboards);
                }
                #endregion
                Label("Selected Leaderboard: " + allLeaderboards[indexNumberLeaderboards]);
                score = ScoreTextField(score);

                if (Button("Submit Score"))
                {
                    Debug.Log("Submit Score: " + score + " to leaderboard " + allLeaderboards[indexNumberLeaderboards]);
                    GleyGameServices.ScreenWriter.Write("Submit Score: " + score + " to leaderboard " + allLeaderboards[indexNumberLeaderboards]);
                    //submit a score
                    //it also can be done like this:
                    //GameServices.Instance.SubmitScore(score, LeaderboardNames.Leaderboard1);
                    GameServices.Instance.SubmitScore(score, allLeaderboards[indexNumberLeaderboards], ScoreSubmitted);
                    //GameServices.Instance.GetPlayerScore(LeaderboardNames.Leaderboard1, ScoreLoaded);
                }

                if (Button("Show Leaderboards UI"))
                {
                    //Show all game leaderboards
                    GameServices.Instance.ShowLeaderboadsUI();
                    //GameServices.Instance.ShowSpecificLeaderboard(LeaderboardNames.Leaderboard1);
                }
            }
            else
            {
                Label("No Leaderboards Configured");
            }
        }
    }

    private void ScoreLoaded(long score)
    {
        GleyGameServices.ScreenWriter.Write("Score: " + score);
    }


    //Automatically called when Login is complete 
    private void LoginResult(bool success)
    {
        if (success == true)
        {
            //Login was successful
        }
        else
        {
            //Login failed
        }
        Debug.Log("Login success: " + success);
        GleyGameServices.ScreenWriter.Write("Login success: " + success);
    }


    //Automatically called when an achievement was submitted 
    private void AchievementSUbmitted(bool success, GameServicesError error)
    {
        if (success)
        {
            //achievement was submitted
        }
        else
        {
            //an error occurred
            Debug.LogError("Achivement failed to submit: " + error);

        }
        Debug.Log("Submit achievement result: " + success + " message:" + error);
        GleyGameServices.ScreenWriter.Write("Submit achievement result: " + success + " message:" + error);
    }


    //Automatically called when a score was submitted 
    private void ScoreSubmitted(bool success, GameServicesError error)
    {
        if (success)
        {
            //score successfully submitted
        }
        else
        {
            //an error occurred
            Debug.LogError("Score failed to submit: " + error);
        }
        Debug.Log("Submit score result: " + success + " message:" + error);
        GleyGameServices.ScreenWriter.Write("Submit score result: " + success + " message:" + error);
    }


    //custom GUI button
    private bool Button(string label)
    {
        nr++;
        return GUI.Button(new Rect(0, (nr - 1) * buttonHeight, buttonWidth, buttonHeight), label);
    }


    //custom label field
    private void Label(string label)
    {
        GUI.Label(new Rect(0, nr * buttonHeight, buttonWidth, buttonHeight), label);
        nr++;
    }


    //custom text field
    private long ScoreTextField(long score)
    {
        scoreText = GUI.TextField(new Rect(0, nr * buttonHeight, buttonWidth, buttonHeight), score.ToString());
        scoreText = Regex.Replace(scoreText, @"[^0-9]", "");
        long result;
        long.TryParse(scoreText, out result);
        nr++;
        return result;
    }


    //custom drop down
    private void DropDown<T>(int maxButtons, ref int indexNumber, ref bool showDropDown, IList<T> coll)
    {
        Rect dropDownRect = new Rect(0, 0 * buttonHeight, buttonWidth, maxButtons * buttonHeight);
        scrollViewVector = GUI.BeginScrollView(new Rect(dropDownRect.x, (dropDownRect.y + nr * buttonHeight), dropDownRect.width, dropDownRect.height), scrollViewVector, new Rect(0, 0, dropDownRect.width - 100, Mathf.Max(dropDownRect.height, (coll.Count * buttonHeight))));
        float min = Mathf.Min(dropDownRect.height, (coll.Count * buttonHeight));
        for (int index = 0; index < coll.Count; index++)
        {

            if (GUI.Button(new Rect(padding, (index * buttonHeight), buttonWidth - 2 * padding, buttonHeight), coll[index].ToString()))
            {
                showDropDown = false;
                indexNumber = index;
            }
            if (index < min / buttonHeight)
            {
                nr++;
            }
        }
        GUI.EndScrollView();
    }
}