using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using V7App = Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Hangman.Database;
using Hangman.Models;

namespace Hangman.Activities
{
    [Activity(Label = "@string/app_name")]
    public class Game : V7App.AppCompatActivity
    {
        private string _normalizedWord;
        private bool[] _correct;
        private string _word;
        private string _player;
        private string Word
        {
            get { return _word; }
            set
            {
                StringBuilder stringBuilder = new StringBuilder();
                var arrayText = value.Normalize(NormalizationForm.FormD).ToCharArray();
                foreach (char letter in arrayText)
                {
                    if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                        stringBuilder.Append(letter);
                }
                _normalizedWord = stringBuilder.ToString();
                _correct = new bool[value.Length];
                _word = value;
            }
        }
        private int Tries = -1;
        private int _fails = 0;
        private int Fails
        {
            get { return _fails; }
            set
            {
                switch (value)
                {
                    case 0:
                        FindViewById<ImageView>(Resource.Id.state_imageView).SetImageResource(Resource.Drawable.fail_0);
                        break;
                    case 1:
                        FindViewById<ImageView>(Resource.Id.state_imageView).SetImageResource(Resource.Drawable.fail_1);
                        break;
                    case 2:
                        FindViewById<ImageView>(Resource.Id.state_imageView).SetImageResource(Resource.Drawable.fail_2);
                        break;
                    case 3:
                        FindViewById<ImageView>(Resource.Id.state_imageView).SetImageResource(Resource.Drawable.fail_3);
                        break;
                    case 4:
                        FindViewById<ImageView>(Resource.Id.state_imageView).SetImageResource(Resource.Drawable.fail_4);
                        break;
                    case 5:
                        FindViewById<ImageView>(Resource.Id.state_imageView).SetImageResource(Resource.Drawable.fail_5);
                        break;
                    case 6:
                        FindViewById<ImageView>(Resource.Id.state_imageView).SetImageResource(Resource.Drawable.fail_6);
                        break;
                    case 7:
                        FindViewById<ImageView>(Resource.Id.state_imageView).SetImageResource(Resource.Drawable.fail_7);
                        break;
                    default:
                        FindViewById<ImageView>(Resource.Id.state_imageView).SetImageResource(Resource.Drawable.fail_7);
                        break;
                }
                _fails = value;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.game_activity);
            var wordArray = Resources.GetStringArray(Resource.Array.words_array);
            Word = wordArray[(new Random().Next() % wordArray.Length)];
            MakeTry('0');

            FindViewById<Button>(Resource.Id.a_button).Click += KeyboardButtonPressed;
            FindViewById<Button>(Resource.Id.b_button).Click += KeyboardButtonPressed;
            FindViewById<Button>(Resource.Id.c_button).Click += KeyboardButtonPressed;
            FindViewById<Button>(Resource.Id.d_button).Click += KeyboardButtonPressed;
            FindViewById<Button>(Resource.Id.e_button).Click += KeyboardButtonPressed;
            FindViewById<Button>(Resource.Id.f_button).Click += KeyboardButtonPressed;
            FindViewById<Button>(Resource.Id.g_button).Click += KeyboardButtonPressed;
            FindViewById<Button>(Resource.Id.h_button).Click += KeyboardButtonPressed;
            FindViewById<Button>(Resource.Id.i_button).Click += KeyboardButtonPressed;
            FindViewById<Button>(Resource.Id.j_button).Click += KeyboardButtonPressed;
            FindViewById<Button>(Resource.Id.k_button).Click += KeyboardButtonPressed;
            FindViewById<Button>(Resource.Id.l_button).Click += KeyboardButtonPressed;
            FindViewById<Button>(Resource.Id.m_button).Click += KeyboardButtonPressed;
            FindViewById<Button>(Resource.Id.n_button).Click += KeyboardButtonPressed;
            FindViewById<Button>(Resource.Id.o_button).Click += KeyboardButtonPressed;
            FindViewById<Button>(Resource.Id.p_button).Click += KeyboardButtonPressed;
            FindViewById<Button>(Resource.Id.q_button).Click += KeyboardButtonPressed;
            FindViewById<Button>(Resource.Id.r_button).Click += KeyboardButtonPressed;
            FindViewById<Button>(Resource.Id.s_button).Click += KeyboardButtonPressed;
            FindViewById<Button>(Resource.Id.t_button).Click += KeyboardButtonPressed;
            FindViewById<Button>(Resource.Id.u_button).Click += KeyboardButtonPressed;
            FindViewById<Button>(Resource.Id.v_button).Click += KeyboardButtonPressed;
            FindViewById<Button>(Resource.Id.w_button).Click += KeyboardButtonPressed;
            FindViewById<Button>(Resource.Id.x_button).Click += KeyboardButtonPressed;
            FindViewById<Button>(Resource.Id.y_button).Click += KeyboardButtonPressed;
            FindViewById<Button>(Resource.Id.z_button).Click += KeyboardButtonPressed;

            EditText playerName_editText = new EditText(this);
            playerName_editText.Hint = GetString(Resource.String.playerNameHint_editText);
            int _24dp = (int)(24 * ((float)Resources.DisplayMetrics.DensityDpi / 160F));
            playerName_editText.SetPadding(_24dp, _24dp >> 1, _24dp, _24dp >> 1);
            V7App.AlertDialog.Builder alert = new V7App.AlertDialog.Builder(this);
            alert.SetTitle(Resource.String.playerName_dialogTitle);
            alert.SetView(playerName_editText);
            alert.SetPositiveButton(Resource.String.ok_button,
                delegate
                {
                    _player = (string.IsNullOrWhiteSpace(playerName_editText.Text) ? $"Player {new Random().Next()}" : playerName_editText.Text);
                });
            alert.SetOnDismissListener(new OnDismissListener(
                delegate
                {
                    _player = (string.IsNullOrWhiteSpace(playerName_editText.Text) ? $"Player {new Random().Next()}" : playerName_editText.Text);
                }));
            alert.Show();
        }

        private void KeyboardButtonPressed(object sender, EventArgs e)
        {
            FindViewById<TextView>(Resource.Id.letter_textView).Text = (sender as Button).Text;
            if (MakeTry((sender as AppCompatButton).Text.ToCharArray()[0]))
            {
                (sender as AppCompatButton).Background = GetDrawable(Resource.Color.colorPrimary);
                (sender as AppCompatButton).Enabled = false;
                bool b = true;
                foreach (bool c in _correct) if (!c) b = false;
                if (b)
                {
                    int score = (int)((Word.Length * (Tries - Fails)) / (Fails == 0 ? 0.5 : Fails) * 10);
                    ScoreDB.Insert(new Score { Player = _player, Value = score });
                    V7App.AlertDialog.Builder alert = new V7App.AlertDialog.Builder(this);
                    alert.SetTitle(Resource.String.win_dialogTitle);
                    alert.SetMessage(Resources.GetQuantityString(Resource.Plurals.score_message, score, score));
                    alert.SetPositiveButton(Resource.String.ok_button, delegate { Finish(); });
                    alert.SetOnDismissListener(new OnDismissListener(delegate { Finish(); }));
                    alert.Show();
                }
            }
            else
            {
                (sender as AppCompatButton).Background = GetDrawable(Resource.Color.colorAccent);
                (sender as AppCompatButton).Enabled = false;
                Fails += 1;
                if (Fails >= 7)
                {
                    int score = (int)((Word.Length * (Tries - Fails)) / (Fails == 0 ? 0.5 : Fails) * 10);
                    ScoreDB.Insert(new Score { Player = _player, Value = score });
                    V7App.AlertDialog.Builder alert = new V7App.AlertDialog.Builder(this);
                    alert.SetTitle(Resource.String.lose_dialogTitle);
                    alert.SetMessage($"{GetString(Resource.String.wordAnswer_dialogMessage, Word)}, {Resources.GetQuantityString(Resource.Plurals.score_message, score, score).ToLower()}");
                    alert.SetPositiveButton(Resource.String.ok_button, delegate { Finish(); });
                    alert.SetOnDismissListener(new OnDismissListener(delegate { Finish(); }));
                    alert.Show();
                }
            }
            (sender as AppCompatButton).SetTextColor(Color.White);
        }

        private sealed class OnDismissListener : Java.Lang.Object, IDialogInterfaceOnDismissListener
        {
            private readonly Action action;

            public OnDismissListener(Action action)
            {
                this.action = action;
            }

            public void OnDismiss(IDialogInterface dialog)
            {
                this.action();
            }
        }

        private bool MakeTry(char letter)
        {
            bool r = false;
            Tries++;
            for (int i = 0; i < _normalizedWord.Length; i++)
            {
                if (_normalizedWord[i] == letter)
                {
                    _correct[i] = true;
                    r = true;
                }
            }
            TextView word_textView = FindViewById<TextView>(Resource.Id.word_textView);
            word_textView.Text = "";
            for (int i = 0; i < _correct.Length; i++) word_textView.Text += $"{(_correct[i] ? _word[i] : '_')} ";
            return r;
        }
    }
}