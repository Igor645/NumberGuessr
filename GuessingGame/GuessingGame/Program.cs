using System.Media;

string Gamemode = "";
string Answer = "";

Random random = new Random();
int WhoStarts = random.Next(1, 3);
int CorrectNumber = random.Next(1, 100);
int UserGuess = 0;
int AmountOfGuesses = 1;
int HighScore = 9999999;
int minValue = 1;
int maxValue = 100;

bool ProgramRunning = true;
bool VersusActive = false;

SoundPlayer sound1 = new SoundPlayer("sound1.wav");

sound1.PlayLooping();

Intro();

while (ProgramRunning)
{
    Answer = "";
    Gamemode = "";

    AmountOfGuesses = 0;
    minValue = 1;
    maxValue = 100;

    while (!(Gamemode == "u" || Gamemode == "c" || Gamemode == "v"))
    {
        Console.WriteLine("What game-mode do you want to play?\n[u = User is guessing | c = Computer is guessing | v = Compete against the Computer]");
        Gamemode = Console.ReadLine().ToLower();

        if(!(Gamemode == "u" || Gamemode == "c" || Gamemode == "v"))
        {
            ErrorMessage();
        }
    }

    if (Gamemode == "c")
    {
        Clue("The Computer will be guessing your chosen number.");
        CorrectNumber = NumberValidator(CorrectNumber, "Enter the number the computer should guess: ");
        UserGuess = random.Next(1, 100);
    }
    else if (Gamemode == "u")
    {
        Clue("You will be guessing a random number.");
        UserGuess = NumberValidator(UserGuess, "Enter your guess: ");
    }
    else if(Gamemode == "v")
    {
        VersusActive = true;

        Clue("You will be competing against the computer");
        if(WhoStarts == 1)
        {
            Gamemode = "u";
            TurnIndicator(Gamemode, VersusActive);
            UserGuess = NumberValidator(UserGuess, "Enter your guess: ");
            Gamemode = "c";
        }
        else
        {
            Gamemode = "c";
            TurnIndicator(Gamemode, VersusActive);
            UserGuess = GuessFromUserOrComputer(Gamemode, UserGuess, minValue, maxValue);
            Gamemode = "u";
        }
    }

    while (true)
    {
        AmountOfGuesses++;

        if (UserGuess < CorrectNumber)
        {
            Clue($"The correct number is higher than {UserGuess}.");
            TurnIndicator(Gamemode, VersusActive);
            if(minValue < UserGuess)
            {
            minValue = UserGuess;
            }
            UserGuess = GuessFromUserOrComputer(Gamemode, UserGuess, minValue, maxValue);
        }
        else if (UserGuess > CorrectNumber)
        {
            Clue($"The correct number is less than {UserGuess}.");
            TurnIndicator(Gamemode, VersusActive);
            if(maxValue > UserGuess)
            {
            maxValue = UserGuess;
            }
            UserGuess = GuessFromUserOrComputer(Gamemode, UserGuess, minValue, maxValue);
        }
        else if (UserGuess == CorrectNumber)
        {
            if (!VersusActive)
            {
                Console.WriteLine("---------------------------------------------------------");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Correct!!! The number was {CorrectNumber}.");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Score("It took ", AmountOfGuesses, " guesses.");
                if (Gamemode == "u")
                {
                    if (AmountOfGuesses < HighScore)
                    {
                        HighScore = AmountOfGuesses;
                    }
                    Score("Highscore: ", HighScore, " guesses.");
                }
            }
            else
            {
                if (Gamemode == "u")
                {
                    Console.WriteLine("---------------------------------------------------------");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"You won against the Computer!!! \nThe number was {CorrectNumber}.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (Gamemode == "c")
                {
                    Console.WriteLine("---------------------------------------------------------");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"The computer won. \nThe number was {CorrectNumber}.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            Console.WriteLine("---------------------------------------------------------");
            break;
        }

        if (VersusActive && UserGuess != CorrectNumber)
        {
            if (Gamemode == "u")
            {
                Gamemode = "c";
            }
            else if (Gamemode == "c")
            {
                Gamemode = "u";
            }
        }
    } 


    while (!(Answer == "y" || Answer == "n"))
    {
        Console.WriteLine("Do you want to guess another number? [y|n]");
        Answer = Console.ReadLine();

        switch (Answer)
        {
            case "y":
                CorrectNumber = random.Next(1, 100);
                WhoStarts = random.Next(1, 2);
                VersusActive = false;
                Console.Clear();
                Intro();
                break;

            case "n":
                ProgramRunning = false;
                Clue("Press any key to quit Programm.");
                Console.ReadKey();
                break;

            default:
                ErrorMessage();
                break;
        }
    }
}

static void Intro()
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine(" ____   __ __  ___ ___  ____     ___  ____    ____  __ __    ___  _____ _____ ____  \r\n" +
        "|    \\ |  |  ||   |   ||    \\   /  _]|    \\  /    ||  |  |  /  _]/ ___// ___/|    \\ \r\n" +
        "|  _  ||  |  || _   _ ||  o  ) /  [_ |  D  )|   __||  |  | /  [_(   \\_(   \\_ |  D  )\r\n" +
        "|  |  ||  |  ||  \\_/  ||     ||    _]|    / |  |  ||  |  ||    _]\\__  |\\__  ||    / \r\n" +
        "|  |  ||  :  ||   |   ||  O  ||   [_ |    \\ |  |_ ||  :  ||   [_ /  \\ |/  \\ ||    \\ \r\n" +
        "|  |  ||     ||   |   ||     ||     ||  .  \\|     ||     ||     |\\    |\\    ||  .  \\\r\n" +
        "|__|__| \\__,_||___|___||_____||_____||__|\\_||___,_| \\__,_||_____| \\___| \\___||__|\\_|\r\n                                                                                    ");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("In this game you:");
    Console.WriteLine("- guess a number from 1 to 100");
    Console.WriteLine("- make the computer guess your chosen number");
    Console.WriteLine("- Compete against the Computer and try to guess the number before it.");
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
    Console.WriteLine("Made by: Igor Martic");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
}

static int NumberValidator(int ToCheck, string instruction )
{
    bool Loop = true;

    while (Loop)
    {
        try
        {
            Console.WriteLine(instruction);
            ToCheck = int.Parse(Console.ReadLine());
            if (ToCheck > 100 || ToCheck < 0)
            {
                throw new Exception();
            }
            Loop = false;
        }
        catch
        {
            ErrorMessage();
        }
    }
    return ToCheck;
}

static void ErrorMessage()
{
    Console.WriteLine("---------------------------------------------------------");
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Invalid Input");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("---------------------------------------------------------");
}

static void Clue(string ClueText)
{
    Console.WriteLine("---------------------------------------------------------");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine(ClueText);
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("---------------------------------------------------------");
}

static void TurnText(string Turn)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine(Turn);
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("---------------------------------------------------------");
}

static void TurnIndicator(string Gamemode, bool VersusActive)
{
    if (VersusActive)
    {

        if (Gamemode == "u")
        {
            TurnText("It's your turn");
        }
        else if (Gamemode == "c")
        {
            Thread.Sleep(800);
            TurnText("It's the computer's turn");
        }
    }
}

static void Score(string Beginning, int Score, string End)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write(Beginning);
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write(Score);
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine(End);
    Console.ForegroundColor = ConsoleColor.White; 
}

static int GuessFromUserOrComputer(string Gamemode, int UserGuess, int minValue, int maxValue)
{
    Random random = new Random();

    if (Gamemode == "u")
    {
        UserGuess = NumberValidator(UserGuess, "Enter your guess: ");
    }
    else if(Gamemode == "c")
    {

        if (minValue != 1)
        {
            minValue++;
        }
        if(maxValue != 100)
        {
            maxValue--;
        }

        UserGuess = random.Next(minValue, maxValue);
        Console.WriteLine(UserGuess);
    }

    return UserGuess;
}