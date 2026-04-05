using System.Text;

namespace LeetCode;

internal class Program
{
    static void Main(string[] args)
    {
        //var l1 = new ListNode(2, new ListNode(4, new ListNode(3)));
        //var l2 = new ListNode(5, new ListNode(6, new ListNode(4)));
        //var l1 = new ListNode(0);
        //var l2 = new ListNode(0);
        //var result = AddTwoNumbers(l1, l2);
        //Merge(new int[6] { 1, 2, 3, 0, 0, 0 }, 3, new int[3]{ 2, 5, 6 }, 3);
        //Merge(new int[0], 0, new int[1] { 1 }, 1);
        //NumberToWords(12345);
        //IsMatch("aaba", ".*ba");
        MyPow(1.0000000001d, -2147483648);
    }

    // Fibonacci solution, dynamic programing, memoization, top-down approach, stackalloc for memory optimization
    public int FibTopDown(int n)
    {
        if (n == 0) return 0;
        Span<int> memo = stackalloc int[n + 1];
        memo[0] = 0;
        memo[1] = 1;
        return RecTopDown(n, memo);
    }
    private int RecTopDown(int n, Span<int> memo)
    {
        if (n == 0) return 0;
        if (memo[n] != 0) return memo[n];
        memo[n] = RecTopDown(n - 1, memo) + RecTopDown(n - 2, memo);
        return memo[n];
    }
    // -End of Dynamic programing top-down approach Fibonacci solution

    // Fibonacci solution, dynamic programming, bottom-up approach, stackalloc for memory optimization
    public static int FibBottomUp(int n)
    {
        if (n == 0) return 0;
        Span<int> memo = stackalloc int[n + 1];
        memo[0] = 0;
        memo[1] = 1;

        for (int i = 2; i <= n; i++)
        {
            memo[i] = memo[i - 1] + memo[i - 2];
        }
        return memo[n];
    }
    // -End of Dynamic programing bottom-up approach Fibonacci solution

    public static double MyPow(double x, long n)
    {
        if (n == 0 || x == 1) return 1;
        double result = 1.00000d, number = x;
        long power = Math.Abs(n);
        while (power > 0)
        {
            if ((power & 1) == 1)
            {
                result *= number;
            }
            number *= number;
            power >>= 1;
        }
        if (n < 0) result = 1 / result;

        return result;
    }

    public static bool IsMatch(string s, string p)
    {
        int trueCounter = 0;
        int i = 0;
        char currentChar;
        while(i <= s.Length)
        {
            if(i < p.Length) { currentChar = p[i]; }
            else { break; }

            if (currentChar == '.')
            {
                i++;
                trueCounter++;
                continue;
            }
            else if(currentChar == '*')
            {
                i = s.Length;
            }
            else
            {
                _ = s[i] == currentChar ? trueCounter++ : trueCounter;
            }
            i++;
        }

        if(trueCounter == s.Length)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static string NumberToWords(int num)
    {
        var dictionary = new Dictionary<long, string>
        {
            // Одиниці
            { 0,"Zero"},{ 1, "One" }, { 2, "Two" }, { 3, "Three" }, { 4, "Four" }, { 5, "Five" },
            { 6, "Six" }, { 7, "Seven" }, { 8, "Eight" }, { 9, "Nine" },

            // Винятки (10-19)
            { 10, "Ten" }, { 11, "Eleven" }, { 12, "Twelve" }, { 13, "Thirteen" },
            { 14, "Fourteen" }, { 15, "Fifteen" }, { 16, "Sixteen" },
            { 17, "Seventeen" }, { 18, "Eighteen" }, { 19, "Nineteen" },

            // Десятки
            { 20, "Twenty" }, { 30, "Thirty" }, { 40, "Forty" }, { 50, "Fifty" },
            { 60, "Sixty" }, { 70, "Seventy" }, { 80, "Eighty" }, { 90, "Ninety" },

            // Великі розряди (Масштаби)
            { 100, "Hundred" },
            { 1000, "Thousand" },
            { 1000000, "Million" },
            { 1000000000, "Billion" }
        };

        LinkedList<string> resultList = new();
        string stringNum = num.ToString();
        int globalBigNumberCounter = 10;
        int localBigNumberCounter = 1;
        var internalMultiplier = 0;
        int internalCounter = 0;
        bool skipNext = false;
        int groupValue = 0;
        for (int i = stringNum.Length - 1; i >= 0; i--)
        {
            int temp = num % 10;
            num = num / 10;
            if (internalCounter % 3 == 0 && i != stringNum.Length - 1 && stringNum.Length > 3)
            {
                internalMultiplier += 3;
                localBigNumberCounter = 1;
                globalBigNumberCounter = (int)Math.Pow(10, internalMultiplier);
            }

            if (globalBigNumberCounter > 10 && localBigNumberCounter == 1)
            {
                groupValue = int.Parse(stringNum.Substring
                (
                    Math.Max(0, i - 2),
                    Math.Min(3, i + 1)
                ));
                if (groupValue > 0)
                    resultList.AddFirst(dictionary[globalBigNumberCounter]);
            }

            if (skipNext)
            {
                skipNext = false;
                localBigNumberCounter *= 10;
                internalCounter++;
                //num = num / 10;
                continue;
            }

            if (num % 10 == 1 && internalCounter % 3 == 0 && internalCounter % 3 == 0)
            //цей піздец для 11 і 12. FYI
            {
                resultList.AddFirst(dictionary[10 + temp]);
                skipNext = true;
            }
            else if (localBigNumberCounter == 1)
            {
                if (temp == 0 && i != 0)
                {

                }
                else
                {
                    resultList.AddFirst(dictionary[temp]);
                }
            }
            else if (localBigNumberCounter == 10)
            {
                if (temp == 0 && i != 0)
                {

                }
                else
                {
                    resultList.AddFirst(dictionary[temp * localBigNumberCounter]);
                }
            }
            else // last case, when localBigNumberCounter == 100
            {
                if (temp == 0 && i != 0)
                {

                }
                else
                {
                    resultList.AddFirst(dictionary[localBigNumberCounter]);
                    resultList.AddFirst(dictionary[temp]);
                }
            }

            localBigNumberCounter *= 10;
            internalCounter++;
        }

        return string.Join(" ", resultList);
    }

    public static string IntToRoman(int num)
    {
        int[] values = { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
        string[] romanNumerals = { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };
        var result = new StringBuilder();
        for (int i = 0; i < values.Length && num != 0; i++)
        {
            while (num >= values[i])
            {
                num -= values[i];
                result.Append(romanNumerals[i]);
            }
        }
        return result.ToString();
    }

    public static void Merge(int[] nums1, int m, int[] nums2, int n)
    {
        if (n == 0) return;
        if (m == 0) nums1 = nums2;

        var endIndex = nums1.Length - 1;

        while (m > 0 && n > 0)
        {
            if (nums2[n - 1] >= nums1[m - 1])
            {
                nums1[endIndex] = nums2[n - 1];
                n--;
            }
            else
            {
                nums1[endIndex] = nums1[m - 1];
                m--;
            }
            endIndex--;
        }

        while (n > 0)
        {
            nums1[endIndex] = nums2[n - 1];
            n--;
            endIndex--;
        }
    }

    public static int RomanToInt(string s)
    {
        var dictionary = new Dictionary<string, int>()
        {
            { "I", 1 },
            { "V", 5 },
            { "X", 10 },
            { "L", 50 },
            { "C", 100 },
            { "D", 500 },
            { "M", 1000 },

            { "IV", 4 },
            { "IX", 9 },
            { "XL", 40 },
            { "XC", 90 },
            { "CD", 400 },
            { "CM", 900 }
        };

        char current;
        char? next = null;
        int result = 0;
        bool needToSkip = false;
        for (var i = s.Length - 1; i != -1; i--)
        {
            if (needToSkip)
            {
                needToSkip = false;
                continue;
            }
            current = s[i];
            if (i - 1 != -1)
            {
                next = s[i - 1];
            }
            if (dictionary.TryGetValue(String.Concat(next, current), out var resultNumber))
            {
                result += resultNumber;
                needToSkip = true;
                i -= 1;
            }
            else if (dictionary.TryGetValue(current.ToString(), out var resultNumber1))
            {
                result += resultNumber1;
                needToSkip = false;
            }
        }
        return result;
    }

    public static ListNode AddTwoNumbers(ListNode l1, ListNode l2)
    {
        int l1CurrentNumber = l1.val;
        int l2CurrentNumber = l2.val;
        int sum = l1CurrentNumber + l2CurrentNumber;
        ListNode resultListNode = new()
        {
            val = sum % 10
        };
        int remainder = sum / 10;
        var result = resultListNode;

        while (l1.next != null || l2.next != null)
        {
            l1CurrentNumber = l1.next == null ? 0 : l1.next.val;
            l2CurrentNumber = l2.next == null ? 0 : l2.next.val;

            sum = l1CurrentNumber + l2CurrentNumber + remainder;
            resultListNode.next = new ListNode();
            resultListNode = resultListNode.next;
            resultListNode.val = sum % 10;
            remainder = sum / 10;
            l1 = l1.next ?? l1;
            l2 = l2.next ?? l2;
        }
        if(remainder == 1)
        {
            resultListNode.next = new ListNode();
            resultListNode = resultListNode.next;
            resultListNode.val = 1;
        }
        return result;
    }
}
