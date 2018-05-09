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
using SQLite;

namespace Hangman.Models
{
    public class Score
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; } = -1;
        public string Player { get; set; }
        public int Value { get; set; }
    }
}