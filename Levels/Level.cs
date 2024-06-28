namespace EscapeRoomGame
{
    public class Level
    {
        private readonly LevelInfo levelInfo;

        public bool IsCompleted { get; private set; }

        public Level(LevelInfo levelInfo)
        {
            this.levelInfo = levelInfo;
        }

        public void Start()
        {
            IsCompleted = false;

            foreach (var puzzle in levelInfo.Puzzles)
            {
                bool running = true;

                while (running)
                {
                    Console.WriteLine(puzzle.prompt);
                    string answer = Console.ReadLine();

                    if (answer.ToLower() == puzzle.answer.ToLower())
                    {
                        Console.WriteLine("Correct!");
                        running = false;
                    }
                    else if (answer.ToLower() == "exit")
                    {
                        Console.WriteLine("Exiting the game...");
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Wrong answer. Try again.");
                    }
                }
            }

            IsCompleted = true;
        }
    }
}
