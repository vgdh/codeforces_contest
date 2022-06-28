using System.Text;
using static MyApp.TestSamples;
using System.Linq;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {

            List<Sample> samples = TestSamples.GetSamples("tests");

            foreach (var sample in samples)
            {
                string answer = DoTheMath(sample.Question);

                var t = sample.Answer == answer;
                if (sample.Answer != answer)
                    throw new Exception();
                else
                {
                    System.Console.WriteLine("SUCCESS");
                }
            }
        }

        private static string DoTheMath(string question)
        {
            MockConsole Console = new MockConsole(question);
            ///////////////////////////////////////////////////////////////////

            var testCaseCount = int.Parse(Console.ReadLine());

            for (var i = 0; i < testCaseCount; i++)
            {
                _ = Console.ReadLine();
                List<int> collection = Console.ReadLine().Split(' ').Select(it => int.Parse(it)).ToList();

                int rowsCount = collection[0];
                int columnCount = collection[1];

                List<int[]> table = new();

                for (int rowNum = 0; rowNum < rowsCount; rowNum++)
                {
                    int[] parseArr = Console.ReadLine().Split(' ').Select(it => int.Parse(it)).ToArray();
                    table.Add(parseArr);
                }

                int clickCount = int.Parse(Console.ReadLine());
                List<int> columnNumberForClick = Console.ReadLine().Split(' ').Select(it => int.Parse(it)).ToList();


                foreach (var columnNumForSorting in columnNumberForClick)
                {
                    table = table.OrderBy(x => x[columnNumForSorting-1]).ToList();
                }

                foreach (var item in table)
                {
                    Console.WriteLine(String.Join(" ", item));
                }
                Console.WriteLine();


            }

            //////////////////////////////////////////////////////////////////
            return Console.OutString;
        }
    }


    public class MockConsole
    {
        public string InputString { get; set; }
        public string OutString
        {
            get
            {
                return stringBuilder.ToString();
            }
        }
        public StringReader StringReader { get; set; }
        public StringBuilder stringBuilder { get; set; }
        public MockConsole(string inp)
        {
            InputString = inp;
            StringReader = new StringReader(inp);
            stringBuilder = new();
        }
        public string ReadLine()
        {
            return StringReader.ReadLine();
        }

        public void WriteLine(string total)
        {
            stringBuilder.AppendLine(total);
        }
        public void WriteLine()
        {
            stringBuilder.AppendLine();
        }
    }

    public static class TestSamples
    {
        public static List<Sample> GetSamples(string path)
        {
            string[] paths = Directory.GetFiles(path);
            List<string> onlyQuestionFilesPath = paths.ToList().FindAll(x => x.EndsWith(".a") != true);

            List<Sample> testSamples = new();
            foreach (var item in onlyQuestionFilesPath)
            {
                string question = File.ReadAllText(item);
                string answer = File.ReadAllText($"{item}.a");
                testSamples.Add(new Sample(question, answer));
            }
            return testSamples;
        }
        public class Sample
        {
            public Sample(string question, string answer)
            {
                Question = question;
                Answer = answer;
            }
            public string Question { get; set; }
            public string Answer { get; set; }
        }
    }



}