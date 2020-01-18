namespace TimesTablesPractice
{
    using LanguageExt;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using static LanguageExt.Prelude;
    using static System.Console;

    internal class Program
    {
        public static void Main()
        {
            WriteLine($"Välkommen till gångertabellsspelet!");

            var player = QueryPlayer("Skriv ditt namn: ");
            var askFunc = par(AskUntilCorrect, QueryPlayer);

            var result =
                from i in Range(1, 10)
                from j in Range(1, 10)
                select askFunc($"{i} * {j} = ", i * j);

            var evaluatedResult = result.ToList();

            WriteLine($"Bra jobbat {player}! Du klarade alla {evaluatedResult.Count} frågor på {evaluatedResult.Sum()} försök.");
        }

        private static Func<string, string> QueryPlayer
            => query => 
            {
                Write(query);
                return ReadLine();
            };

        private static Func<Func<string, string>, string, int, int> AskUntilCorrect
            => (queryFunc, question, answer) =>
            {
                var response = queryFunc(question);
                if (Correct(response, answer)) return 1;
                return 1 + AskUntilCorrect(queryFunc, question, answer);
            };

        private static Func<string, int, bool> Correct => (input, answer)
            =>  parseInt(input).Match(
                    Some: i => i == answer,
                    None: () => false);

        #region UnitTests 
        #if DEBUG
        [Test]
        public static void TestCorrect()
        {
            Assert.That(Correct("1", 1), Is.True);
            Assert.That(Correct("1", 2), Is.False);
        }

        [Test]
        public static void TestReadUntilCorrect()
        {
            static string queryFunc(string _) => "2";

            var askFunc = par(AskUntilCorrect, queryFunc);

            Assert.That(askFunc(string.Empty, 2), Is.EqualTo(1));
        }
        #endif
        #endregion
    }
}
