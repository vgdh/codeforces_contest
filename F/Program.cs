using System.Text;
using static MyApp.TestSamples;

namespace MyApp 
{
    internal class Program
    {
        static void Main(string[] args)
        {

            List<Sample> samples = TestSamples.GetSamples("tests");

            foreach (var sample in samples)
            {
                string answer = DoTheMath(sample.Question);

                var trueOrNot = sample.Answer == answer;
                var que = sample.Question.Split("\r\n");
                var ansR = sample.Answer.Split("\r\n");
                var ansB = answer.Split("\r\n");

                for (int i = 0; i < ansR.Length; i++)
                {
                    if (ansR[i] != ansB[i])
                    {
                        var right = ansR[i];
                        var wrong = ansB[i];
                        System.Console.WriteLine($"NUM:{i} - {right} != {wrong} || Question: {que[i + 3]}");
                        throw new Exception();
                    }
                }

                if (sample.Answer != answer)
                    throw new Exception();
                else
                {
                    System.Console.WriteLine($"{samples.IndexOf(sample)} SUCCESS");
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
                var rl = Console.ReadLine().Split(' ').Select(it => int.Parse(it)).ToArray();

                var coupeCount = rl[0];
                var requestCount = rl[1];

                SortedSet<int> freeCouple = new SortedSet<int>(Enumerable.Range(1, coupeCount));

                bool[] soldenPlace = new bool[coupeCount * 2 + 1];
            
                for (int r = 0; r < requestCount; r++)
                {
                    rl = Console.ReadLine().Split(' ').Select(it => int.Parse(it)).ToArray();

                    //System.Console.WriteLine(String.Join(" ", rl));

                    OperationType operation = (OperationType)rl[0];

                    int place = 0;
                    if (operation != OperationType.BuyWholeFirstCoupe)
                    {
                        place = rl[1];
                    }

                    if (operation == OperationType.Buy)
                    {
                        if (soldenPlace[place])
                        {
                            Console.WriteLine("FAIL");
                        }
                        else
                        {
                            soldenPlace[place] = true;
                            freeCouple.Remove(calcCoupeNum(place));
                            Console.WriteLine("SUCCESS");
                        }
                    }
                    else if (operation == OperationType.Return)
                    {
                        if (soldenPlace[place])
                        {
                            Console.WriteLine("SUCCESS");
                            soldenPlace[place] = false;

                            if (place % 2 == 0)
                            {
                                if (soldenPlace[place - 1] == false)
                                {
                                    freeCouple.Add(calcCoupeNum(place));
                                }
                            }
                            else
                            {
                                if (soldenPlace[place + 1] == false)
                                {
                                    freeCouple.Add(calcCoupeNum(place));
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("FAIL");
                        }
                    }
                    else
                    {
                        var firstCouple = freeCouple.FirstOrDefault();

                        if (firstCouple != 0)
                        {
                            Console.WriteLine($"SUCCESS {firstCouple * 2 - 1}-{firstCouple * 2}");
                            freeCouple.Remove(firstCouple);

                            soldenPlace[firstCouple * 2 - 1] = true;
                            soldenPlace[firstCouple * 2] = true;
                        }
                        else
                        {
                            Console.WriteLine("FAIL");
                        }
                    }
                }
                Console.WriteLine();
            }
            //////////////////////////////////////////////////////////////////
            return Console.OutString;
        }

        private static int calcCoupeNum(int place)
        {
            if (place % 2 == 0)
            {
                return place / 2;
            }
            else
            {
                return (place + 1) / 2;
            }
        }
        enum OperationType
        {
            Buy = 1,
            Return = 2,
            BuyWholeFirstCoupe = 3
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