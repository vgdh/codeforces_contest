using System.Text;
using static MyApp.TestSamples;

// https://route256.contest.codeforces.com/group/iQpLfHp2Pl/contest/386325/problem/B
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
            }


        }

        private static string DoTheMath(string question)
        {
            MockConsole Console = new MockConsole(question);
            ///////////////////////////////////////////////////////////////////
            
            var testCaseCount = int.Parse(Console.ReadLine());
            for (var i = 0; i < testCaseCount; i++)
            {
                int goodsCount = int.Parse(Console.ReadLine());
                List<int> collection = Console.ReadLine().Split(' ').Select(it => int.Parse(it)).ToList();
                Dictionary<int, int> uniqueGoodsCouter = new(); //value = count

                foreach (var item in collection)
                {
                    if (uniqueGoodsCouter.ContainsKey(item))
                    {
                        uniqueGoodsCouter[item]++;
                    }
                    else
                    {
                        uniqueGoodsCouter.Add(item, 1);
                    }
                }

                int totalCost = 0;
                foreach (var item in uniqueGoodsCouter)
                {
                    if (item.Value > 2)
                    {
                        int remainder = item.Value % 3;
                        int countBySale = (item.Value / 3) * 2;
                        int totalCount = remainder + countBySale;
                        totalCost += totalCount * item.Key;
                    }
                    else
                    {
                        totalCost += item.Key * item.Value;
                    }
                }
                Console.WriteLine(totalCost);
            }
          
            //////////////////////////////////////////////////////////////////
            return Console.OutString;
        }
    }


    public class MockConsole
    {
        public string InputString { get; set; }
        public string OutString { 
            get {
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

        public void WriteLine(int total)
        {
            stringBuilder.AppendLine(total.ToString());
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