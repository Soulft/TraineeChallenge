# TraineeChallenge
Based on UVA, this repository have the propose of test the ability of an trainee candidate

On this challenge, you need to create a Hangman game.

The rules:<br />
The game starts when the player type his name.<br />
A new word need to be randomly taken from a list of words.<br />
The player can guess only one letter at a time.<br />
The player win if has guessed all the characters.<br />
The player lose if has guessed wrong 7 times.<br />
Each time a guess is correct, all chatacters in the word that match the guess will be turned over.<br />
Each time a guess is wrong, the counter reduce by one and the letter can not be used anymore.<br />
When the game finish, the player is set on a Rank.<br />

The Rank is calculated by the amount of characters that the taken word have, multiplicated by 0.5 and then multiplicated by amount of chances the player still having at the end.

At the end, the game should show a Rank 

```
  ______ 
  |  | 
  |  O  
  | /|\ 
  |  | 
  | / \ 
__|_ 
| |_______ 
|________/ 

```


Good lucky 
