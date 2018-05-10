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
using Hangman.Models;

namespace Hangman.Adapters
{
    class Scores : BaseAdapter<Score>
    {
        List<Score> scores;
        Activity context;

        public Scores(List<Score> scores, Activity context)
        {
            this.scores = scores;
            this.context = context;
        }

        public override Score this[int position] { get { return scores[position]; } }

        public override int Count { get { return scores.Count; } }

        public override long GetItemId(int position)
        {
            return position;
        }

        //Combine the object (Score) with the template and prepare it to add to ListView
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null) view = context.LayoutInflater.Inflate(Resource.Layout.score_dataTemplate, null);
            view.FindViewById<TextView>(Resource.Id.playerName_textView).Text = scores[position].Player;
            view.FindViewById<TextView>(Resource.Id.playerScore_textView).Text = scores[position].Value.ToString("0.##");
            return view;
        }
    }
}