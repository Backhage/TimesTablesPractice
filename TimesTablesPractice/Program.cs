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
            Write("Skriv ditt namn: ");
            var player = ReadLine();

            var askFunc = par(AskUntilCorrect, q => Write(q), () => ReadLine());

            var result =
                from i in Range(1, 10)
                from j in Range(1, 10)
                select askFunc($"{i} * {j} = ", i * j);

            var evaluatedResult = result.ToList();

            WriteLine($"Bra jobbat {player}! Du klarade alla {evaluatedResult.Count} frågor på {evaluatedResult.Sum()} försök.");
        }

        private static Func<Action<string>, Func<string>, string, int, int> AskUntilCorrect
            => (queryFunc, readFunc, question, answer) =>
            {
                queryFunc(question);
                var response = readFunc();
                if (Correct(response, answer)) return 1;
                return 1 + AskUntilCorrect(queryFunc, readFunc, question, answer);
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
            static void queryFunc(string _) { }
            static string readFunc() => "2";

            var askFunc = par(AskUntilCorrect, queryFunc, readFunc);

            Assert.That(askFunc(string.Empty, 2), Is.EqualTo(1));
        }
        #endif
        #endregion
    }
}
