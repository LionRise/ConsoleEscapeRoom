using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace EscapeRoomGame
{
    public class Game
    {
        private List<DialogNode> dialogNodes;
        private List<LevelInfo> levelInfos;
        private string dialogPath = "dialog.json";
        private string levelsPath = "levels.json";
        private string saveFilePath = "savegame.json";
        private int currentLevelIndex;

        public Game()
        {
            LoadDialog();
            LoadLevels();
            currentLevelIndex = 0;
        }

        // Read the json files with LoadDialog and LoadLevels
        private void LoadDialog()
        {
            if (File.Exists(dialogPath))
            {
                var json = File.ReadAllText(dialogPath);
                dialogNodes = JsonConvert.DeserializeObject<List<DialogNode>>(json);
            }
            else
            {
                dialogNodes = new List<DialogNode>();
            }
        }

        private void LoadLevels()
        {
            if (File.Exists(levelsPath))
            {
                var json = File.ReadAllText(levelsPath);
                levelInfos = JsonConvert.DeserializeObject<List<LevelInfo>>(json);
            }
            else
            {
                levelInfos = new List<LevelInfo>();
            }
        }

        public void Start()
        {
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("ESCAPE ROOM GAME");
                Console.WriteLine("1. START");
                Console.WriteLine("2. LOAD");
                Console.WriteLine("3. SAVE");
                Console.WriteLine("4. EXIT");
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        PlayGame(showDialog: true);
                        break;
                    case "2":
                        LoadGame();
                        break;
                    case "3":
                        SaveGame();
                        break;
                    case "4":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice, try again.");
                        break;
                }
            }
        }

        private void PlayGame(bool showDialog)
        {
            Console.Clear();
            Console.WriteLine("You are in a room. Solve the puzzles to escape.");
            Console.WriteLine("Type 'exit' to go back to the main menu.");

            if (showDialog)
            {
                // Start dialog
                ShowDialog(1);
            }

            bool allLevelsCompleted = true;

            for (int i = currentLevelIndex; i < levelInfos.Count; i++)
            {
                var level = new Level(levelInfos[i]);
                level.Start();
                if (level.IsCompleted)
                {
                    Console.WriteLine("Level completed!");
                    currentLevelIndex++;
                }
                else
                {
                    Console.WriteLine("You exited the game.");
                    allLevelsCompleted = false;
                    break;
                }
            }

            if (allLevelsCompleted)
            {
                Console.WriteLine("Congratulations! You have escaped the room!");
                currentLevelIndex = 0; // Reset for a new game
            }

            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey();
        }

        private void LoadGame()
        {
            if (File.Exists(saveFilePath))
            {
                var json = File.ReadAllText(saveFilePath);
                var savedState = JsonConvert.DeserializeObject<GameState>(json);
                currentLevelIndex = savedState.CurrentLevelIndex;
                Console.WriteLine("Game loaded successfully.");
                PlayGame(showDialog: false); // Start the game after loading without showing the dialog
            }
            else
            {
                Console.WriteLine("No save game found.");
                Console.WriteLine("Press any key to return to the main menu...");
                Console.ReadKey();
            }
        }

        private void SaveGame()
        {
            var gameState = new GameState
            {
                CurrentLevelIndex = currentLevelIndex
            };
            var json = JsonConvert.SerializeObject(gameState);
            File.WriteAllText(saveFilePath, json);
            Console.WriteLine("Game saved successfully.");
            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey();
        }












        private void ShowDialog(int id)
        {
            var node = dialogNodes.FirstOrDefault(n => n.Id == id);
            if (node != null)
            {
                // Print the title before the options
                Console.WriteLine(node.Title);
                Console.WriteLine(); // Add an empty line for better readability

                // Use LINQ to retrieve and display options
                var options = dialogNodes.Where(n => node.Options.Contains(n.Id)).ToList();
                if (options.Any())
                {



                    Console.WriteLine(node.Text); 
                     

                    Console.WriteLine("Choose an option:");

                    for (int i = 0; i < options.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {options[i].Title}"); 
                    }
                    int choice;
                    bool validChoice = false;

                    while (!validChoice)
                    {
                        Console.Write("Enter the number of your choice: ");
                        if (int.TryParse(Console.ReadLine(), out choice) && choice > 0 && choice <= options.Count)
                        {
                            validChoice = true;
                            ShowDialog(options[choice - 1].Id);
                        }
                        else
                        {
                            Console.WriteLine("Invalid choice. Please try again.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine(node.Text); // Print the text if there are no options
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }

    }

    public class GameState
    {
        public int CurrentLevelIndex { get; set; }
    }
}
