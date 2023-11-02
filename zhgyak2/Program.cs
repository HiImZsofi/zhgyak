using System.Collections;
using System.Collections.Generic;

namespace zhgyak2
{
    class Game
    {
        public string title { get; set; }
        public int genreId { get; set; }
        public string publisher { get; set; }
        public DateTime stadiaReleaseDate { get; set; }
        public DateTime originalReleaseDate { get; set; }

        public Game(string title, int genreId, string publisher, DateTime stadiaReleaseDate, DateTime originalReleaseDate)
        {
            this.title = title;
            this.genreId = genreId;
            this.publisher = publisher;
            this.stadiaReleaseDate = stadiaReleaseDate;
            this.originalReleaseDate = originalReleaseDate;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            menu();
        }

        static Dictionary<int, string> readGenres()
        {
            Dictionary<int, string> genreIds = new Dictionary<int, string>();
            StreamReader sr = new StreamReader("genre.txt");
            string[] parts = new string[100];

            while (!sr.EndOfStream)
            {
                parts = sr.ReadLine().Split(',');
            }
            sr.Close();

            foreach (string part in parts)
            {
                string[] elements = part.Split("=");
                genreIds.Add(Convert.ToInt32(elements[1]), elements[0]);
            }

            /*foreach(var kvp in genreIds)
            {
                Console.WriteLine(kvp.Key + " " + kvp.Value);
            }*/

            return genreIds;
        }

        static List<Game> readGames()
        {
            List<Game> gameList = new List<Game>();
            StreamReader sr = new StreamReader("stadia_dataset.csv");
            string title = "";
            int genreId = 0;
            string publisher = "";
            DateTime stadiaRelease;
            DateTime originalRelease;
            string[] firstLine = sr.ReadLine().Split("\t");

            while (!sr.EndOfStream)
            {
                string[] readLines = sr.ReadLine().Split(';');
                title = readLines[0];
                genreId = Convert.ToInt32(readLines[1]);
                publisher = readLines[2];
                stadiaRelease = DateTime.Parse(readLines[3]);
                originalRelease = DateTime.Parse(readLines[4]);
                Game game = new Game(title, genreId, publisher, stadiaRelease, originalRelease);
                gameList.Add(game);
            }
            sr.Close();

            /*foreach(Game game in gameList)
            {
                Console.WriteLine(game);
            }*/

            return gameList;
        }

        static void gamesByPublisher()
        {
            Console.WriteLine("Adjon meg egy kiadó nevét");
            string givenName = Console.ReadLine();
            List<Game> gameList = readGames();
            int num = 0;

            foreach (Game game in gameList)
            {
                if (game.publisher == givenName)
                {
                    num++;
                }
            }

            if (num != 0)
            {
                Console.WriteLine($"A {givenName} által kiadott játékok száma: {num}");
            } else
            {
                Console.WriteLine("Ennek a kiadónak nincsenek kiadott játékai");
            }
        }

        static void releasedUnderOneYear()
        {
            List<Game> gameList = readGames();
            Dictionary<int, string> genres = readGenres();

            foreach (Game game in gameList)
            {
                if (game.stadiaReleaseDate.Year == game.originalReleaseDate.Year)
                {
                    string value = genres.FirstOrDefault(x => x.Key == game.genreId).Value;
                    Console.WriteLine($"{game.title} - {value}, {game.originalReleaseDate}");
                }
            }
        }

        static void numbersForGenres()
        {
            List<Game> gameList = readGames();
            Dictionary<int, string> genres = readGenres();

            foreach (var game in genres)
            {
                Console.WriteLine($"{game.Value}: {gameList.Count(x => x.genreId == game.Key)}");
            }
        }

        static void menu()
        {
            Console.WriteLine("Válasszon egy menüpontot: ");
            Console.WriteLine("Adott kiadóhoz tartozó játékok száma - 1, A megjelenés napjától elérhető játékok - 2, Játékok száma műfajonként - 3");
            int index = Convert.ToInt32( Console.ReadLine() );

            if(index == 1)
            {
                gamesByPublisher();
            } else if (index == 2)
            {
                releasedUnderOneYear();
            } else if (index == 3)
            {
                numbersForGenres();
            } else
            {
                Console.WriteLine("Nincs ilyen opció");
            }
        }
    }
}