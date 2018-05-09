using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Hangman.Models;
using SQLite;

namespace Hangman.Database
{
    public static class ScoreDB
    {
        private static SQLiteConnection database;
        private static SQLitePCL.sqlite3 db;

        private static string dbPath
        {
            get
            {
                string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                return Path.Combine(path, "score_sqlite.db3");
            }
        }

        public static void CreateTable()
        {
            database = new SQLiteConnection(dbPath);
            database.CreateTable<Score>();
            database.Close();
        }

        public static void Insert(Score obj)
        {
            database = new SQLiteConnection(dbPath);
            if (obj.ID == -1) database.Insert(obj);
            else database.Update(obj);
            database.Close();
        }

        public static List<Score> Select(string query)
        {
            database = new SQLiteConnection(dbPath);
            List<Score> scores = database.Query<Score>(query);
            database.Close();
            return scores;
        }
    }
}