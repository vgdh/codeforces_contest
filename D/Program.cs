using System.Text;
using static MyApp.TestSamples;

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

                var trueOrNot = sample.Answer == answer;
                var que = sample.Question.Split("\r\n");
                var ansR = sample.Answer.Split("\r\n");
                var ansB = answer.Split("\r\n");

                for (int i = 0; i < ansR.Length; i++)
                {
                    if (ansR[i] != ansB[i])
                    {
                        System.Console.WriteLine($"{ansR[i]} != {ansB[i]} {que[i+2]}");
                        throw new Exception();
                    }
                }
              
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

            var myRegex = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z0-9_-]{2,24}$");
            //var myRegex = new System.Text.RegularExpressions.Regex(@"^(?=.{2,24}$)(?![-.])[a-zA-Z0-9_-]+$");

            for (var i = 0; i < testCaseCount; i++)
            {

                var registrationTryCount = int.Parse(Console.ReadLine());

                HashSet<string> logins = new HashSet<string>();

                for (int r = 0; r < registrationTryCount; r++)
                {
                    var login = Console.ReadLine();
                    login = login.ToLower();

                    if (logins.Contains(login) | login[0] == '-')
                    {
                        Console.WriteLine("NO");
                        continue;
                    }

                    var validTest = myRegex.IsMatch(login);
                    if (validTest)
                    {
                        logins.Add(login);
                        Console.WriteLine("YES");
                    }
                    else
                    {
                        Console.WriteLine("NO");
                    }
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