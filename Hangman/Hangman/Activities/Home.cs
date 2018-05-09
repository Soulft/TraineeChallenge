using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Hangman.Database;
using Hangman.Models;
using Hangman.Adapters;

namespace Hangman.Activities
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class Home : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.home_activity);

            ScoreDB.CreateTable();

            Button newGame_button = FindViewById<Button>(Resource.Id.newGame_button);
            newGame_button.Click += (sender, e) =>
            {
                StartActivity(new Intent(this, typeof(Game)));
            };
        }

        protected override void OnStart()
        {
            base.OnStart();
            List<Score> scores = ScoreDB.Select($"SELECT * FROM {nameof(Score)} ORDER BY {nameof(Score.Value)} DESC");

            ListView ranking_listView = FindViewById<ListView>(Resource.Id.ranking_listView);
            ranking_listView.Adapter = new Scores(scores, this);
        }
    }
}