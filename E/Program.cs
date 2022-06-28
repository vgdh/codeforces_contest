using System.Text;
using static MyApp.TestSamples;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {

        class Person
        {
            public Person(string name, string phone)
            {
                Name = name;
                PhoneNumbers.Add(phone, 0);
                LatestNumofNumber = 0;
            }
            public bool AddPhone(string phone)
            {
                LatestNumofNumber++;

                if (PhoneNumbers.ContainsKey(phone))
                {
                    PhoneNumbers[phone] = LatestNumofNumber;
                    return false;
                }
                else
                {
                    PhoneNumbers.Add(phone, LatestNumofNumber);
                    return true;
                }
            }
            public int LatestNumofNumber { get; set; }
            public string Name { get; set; } = string.Empty;
            public Dictionary<string, int> PhoneNumbers { get; set; } = new();
        }

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
                        System.Console.WriteLine($"{right} != {wrong} || Question: {que[i + 2]}");
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
                var recordCount = int.Parse(Console.ReadLine());

                HashSet<string> logins = new HashSet<string>();

                Dictionary<string, Person> persons = new();
                for (int r = 0; r < recordCount; r++)
                {
                    var record = Console.ReadLine().Split(' ').ToArray();
                    if (persons.ContainsKey(record[0]))
                    {
                        persons[record[0]].AddPhone(record[1]);
                    }
                    else
                    {
                        persons.Add(record[0],new Person(record[0], record[1]));
                    }
                }

                foreach (var person in persons)
                {
                    List<string> uniquePhones = new();
                   
                    if (person.Value.PhoneNumbers.Count > 5)
                    {
                        var numList =  person.Value.PhoneNumbers.ToList().OrderBy(x => x.Value).ToList();
                        numList.RemoveRange(0, person.Value.PhoneNumbers.Count - 5);
                        person.Value.PhoneNumbers = new Dictionary<string, int>(numList);
                    }

                    //person.Value.PhoneNumbers.Reverse();
                }

                foreach (var person in persons.OrderBy(x => x.Key))
                {
                    string strLine = $"{person.Value.Name}: {person.Value.PhoneNumbers.Count}";

                    foreach (var phone in person.Value.PhoneNumbers.OrderByDescending(x=> x.Value))
                    {
                        strLine = $"{strLine} {phone.Key}";
                    }
                    Console.WriteLine(strLine);
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