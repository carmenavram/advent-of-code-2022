namespace AdventOfCode2022;

internal class Packet : IComparable<Packet>
{
    public string Value { get; init; }

    public Packet(string packet)
    {
        this.Value = packet;
    }

    public int CompareTo(Packet? other) => ComparePackets(other.Value, this.Value, false);

    public static bool InRightOrder(string packet1, string packet2, bool print) => ComparePackets(packet1, packet2, print) == 1 ? true : false;

    public override string ToString()
    {
        return this.Value;
    }

    private const int EmptyCell = -1;
    private const int NestedList = -2;

    static int ComparePackets(string packet1, string packet2, bool print)
    {
        var list1 = GetList(packet1);
        var list2 = GetList(packet2);

        if (print)
        {
            Console.WriteLine(packet1);
            Console.WriteLine(packet2);
            Console.WriteLine("----------------------------");
        }
      
        return CompareLists(list1, list2, print);
    }

    static int CompareLists(Dictionary<int, Queue<List<int>>> list1, Dictionary<int, Queue<List<int>>> list2, bool print)
    {
        while (list1[0].Any())
        {
            var item1 = list1[0].Dequeue();
            if (list2[0].Any())
            {
                var item2 = list2[0].Dequeue();
                if (print)
                {
                    PrintLists(item1, item2);
                }
                var comparison = CompareItems(item1, item2, 1, 1, print);

                if (comparison != 0)
                {
                    return comparison;
                }
            }
            else
            {
                return -1;
            }
        }

        if (list2[0].Count > 0)
        {
            // left list ran out of items 
            return 1;
        }

        return 0;

        int CompareItems(List<int> item1, List<int> item2, int level1, int level2, bool print)
        {
            for (int i = 0; i < item1.Count; i++)
            {
                if (item2.Count <= i)
                {
                    // right list ran out of items
                    return -1;
                }

                if (item1[i] != NestedList && item2[i] != NestedList)
                {
                    if (item1[i] > item2[i])
                    {
                        return -1;
                    }
                    else if (item1[i] < item2[i])
                    {
                        return 1;
                    }
                }
                else if (item1[i] == NestedList && item2[i] == NestedList)
                {
                    var nestedList1 = list1[level1].Dequeue();
                    var nestedList2 = list2[level2].Dequeue();

                    if (print)
                    {
                        PrintLists(nestedList1, nestedList2);
                    }

                    var comparison = CompareItems(nestedList1, nestedList2, level1 + 1, level2 + 1, print);
                    if (comparison != 0)
                    {
                        return comparison;
                    }
                }
                else if (item1[i] == NestedList && item2[i] != NestedList)
                {
                    var nestedList1 = list1[level1].Dequeue();
                    var nestedList2 = new List<int> { item2[i] };

                    if (print)
                    {
                        PrintLists(nestedList1, nestedList2);
                    }

                    var comparison = CompareItems(nestedList1, nestedList2, level1 + 1, level2, print);
                    if (comparison != 0)
                    {
                        return comparison;
                    }
                }
                else
                {
                    var nestedList1 = new List<int> { item1[i] };
                    var nestedList2 = list2[level2].Dequeue();
                    if (print)
                    {
                        PrintLists(nestedList1, nestedList2);
                    }

                    var comparison = CompareItems(nestedList1, nestedList2, level1, level2 + 1, print);
                    if (comparison != 0)
                    {
                        return comparison;
                    }
                }
            }

            if (item1.Count < item2.Count)
            {
                // left list ran out of items 
                return 1;
            }

            return 0;
        }
    }

    static void PrintLists(List<int> item1, List<int> item2)
    {
        PrintList(item1);
        PrintList(item2);
        Console.WriteLine();
    }

    static void PrintList(List<int> list) => Console.WriteLine($"[{string.Join(",", list.Select(i => i.ToString()))}]");

    static Dictionary<int, Queue<List<int>>> GetList(string packet)
    {
        Stack<int> leftBrackets = new();
        Dictionary<int, Queue<List<int>>> lists = new();

        for (int i = 0; i < packet.Length; i++)
        {
            if (packet[i] == '[')
            {
                leftBrackets.Push(i);
            }
            else if (packet[i] == ']')
            {
                var leftBracket = leftBrackets.Pop();
                var level = leftBrackets.Count;
                var insideBrackets = packet.Substring(leftBracket + 1, i - leftBracket - 1);
                var insideList = new List<int>();
                if (!string.IsNullOrEmpty(insideBrackets))
                {
                    var list = insideBrackets.Split(',');
                    insideList = list.Select(p => GetValue(p)).ToList();
                }

                if (!lists.ContainsKey(level))
                {
                    lists[level] = new Queue<List<int>>();
                }

                lists[level].Enqueue(insideList);
                packet = ReplaceAt(packet, leftBracket, i, 'x');
            }
        }

        return lists;
    }

    static int GetValue(string part)
    {
        if (part.Contains('x'))
        {
            return NestedList;
        }
        else if (string.IsNullOrWhiteSpace(part))
        {
            return EmptyCell;
        }
        else
        {
            return Convert.ToInt32(part);
        }
    }

    static string ReplaceAt(string packet, int start, int end, char x)
    {
        var packetAsChars = packet.ToCharArray();
        for (int i = start; i <= end; i++)
        {
            packetAsChars[i] = x;
        }

        return new string(packetAsChars);
    }
}
