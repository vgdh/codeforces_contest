using System.Text;

namespace MyApp
{
    internal class Program
    {
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

        static void Main(string[] args)
        {
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

        }
    }
}