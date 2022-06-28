using System.Collections.Specialized;
using System.Text;
using static MyApp.TestSamples;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {

            List<Sample> samples = TestSamples.GetSamples("..\\..\\..\\tests");

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
                        System.Console.WriteLine($"NUM:{i} - {right} != {wrong}");
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
        class Soft
        {
            public Soft(string name, string[] dependencies)
            {
                Name = name;
                dependency.AddRange(dependencies);

            }
            public Soft(string name)
            {
                Name = name;
            }


            public bool IsAlreadyBuilded { get; set; }
            public string Name { get; set; }
            public List<string> dependency { get; set; } = new();

            public List<string> Build(SoftRepo softRepo)
            {
                if (IsAlreadyBuilded)
                    return new();

                List<string> dependencyList = new();

                if (dependency.Count > 0)
                {

                    for (int i = dependency.Count - 1; i >= 0; i--)
                    {
                        var found = softRepo.GetSoft(dependency[i]);
                        if (found.IsAlreadyBuilded is false)
                        {
                            var result = found.Build(softRepo);
                            dependencyList.AddRange(result);
                        }

                    }
                }

                if (IsAlreadyBuilded)
                {
                    return dependencyList;
                }
                else
                {
                    IsAlreadyBuilded = true;
                    dependencyList.Add(Name);
                    return dependencyList;
                }

            }
        }

        class SoftRepo
        {
            private List<Soft> soft = new();
            private Dictionary<string, Soft> softHash = new();

            public bool AddNewSoft(Soft soft)
            {
                if (this.softHash.ContainsKey(soft.Name) is false)
                {
                    this.soft.Add(soft);
                    this.softHash.Add(soft.Name, soft);
                    return true;
                }
                else
                {
                    throw new Exception();
                }
            }

            public List<Soft> GetAllSoftToList()
            {
                return new List<Soft>(soft);
            }

            public Soft GetSoft(string name)
            {
                return softHash[name];
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
                var dependenciesCounter = int.Parse(Console.ReadLine());

                SoftRepo softRepo = new();
                for (int r = 0; r < dependenciesCounter; r++)
                {

                    var dependAndList = Console.ReadLine().Split(":").ToArray();
                    var name = dependAndList[0];

                    if (string.IsNullOrEmpty(dependAndList[1]) is false)
                    {
                        var str = dependAndList[1].Trim(' ');
                        var dependencies = str.Split(' ').ToArray();
                        softRepo.AddNewSoft(new Soft(name, dependencies));
                    }
                    else
                    {
                        softRepo.AddNewSoft(new Soft(name));
                    }

                }

                var compileRequest = int.Parse(Console.ReadLine());
                List<string> softForCompile = new();
                for (int r = 0; r < compileRequest; r++)
                {
                    var softName = Console.ReadLine();
                    softForCompile.Add(softName);
                }


                foreach (var item in softForCompile)
                {
                    var soft = softRepo.GetSoft(item);
                    if (soft.IsAlreadyBuilded)
                    {
                        Console.WriteLine("0");
                        continue;
                    }
                    var result = soft.Build(softRepo);
                    result.Insert(0, result.Count.ToString());

                    string resultString = String.Join(' ', result);
                    Console.WriteLine(resultString);
                    //System.Console.WriteLine(resultString);
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