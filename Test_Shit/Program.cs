using System.Text;

namespace MyApp
{
    internal class Program
    {

        static void Main(string[] args)
        {
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
}