using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssistantEmployee.Tools
{
    sealed class AmountInWords
    {
        public enum Style { Empty, Minimum, Middle, Full }

        //Массив слов
        string[][] worlds;

        public AmountInWords()
        {
            worlds = new string[][]
            {
                new string[] {"", "сто ", "двести ", "триста ", "четыреста ", "пятьсот ", "шестьсот ", "семьсот ", "восемьсот ", "девятьсот "},
                new string[] {"", "", "двадцать ", "тридцать ", "сорок ", "пятьдесят ", "шестьдесят ", "семьдесят ", "восемьдесят ", "девяносто "},
                new string[] {"", "одна ", "две ", "три ", "четыре ", "пять ", "шесть ", "семь ", "восемь ", "девять ", "десять ", "одиннадцать ", "двенадцать ", "тринадцать ", "четырнадцать ", "пятнадцать ", "шестнадцать ", "семнадцать ", "восемнадцать ", "девятнадцать "},
                new string[] {"", "один ", "два ", "три ", "четыре ", "пять ", "шесть ", "семь ", "восемь ", "девять ", "десять ", "одиннадцать ", "двенадцать ", "тринадцать ", "четырнадцать ", "пятнадцать ", "шестнадцать ", "семнадцать ", "восемнадцать", "девятнадцать"},
                new string[] {"октиллион ", "октиллиона ", "октиллионов "},
                new string[] {"септиллион ", "септиллиона ", "септиллионов "},
                new string[] {"секстиллион ", "секстиллиона ", "секстиллионов "},
                new string[] {"квинтиллион ", "квинтиллиона ", "квинтиллионов "},
                new string[] {"квадриллион ", "квадриллиона ", "квадриллионов "},
                new string[] {"триллион ", "триллиона ", "триллионов "},
                new string[] {"миллиард ", "миллиарда ", "миллиардов "},
                new string[] {"миллион ", "миллиона ", "миллионов "},
                new string[] {"тысяча ", "тысячи ", "тысяч "},
                new string[] {" рубль ", " рубля ", " рублей "},
                new string[] {" копейка", " копейки", " копеек"},
            };
        }

        //Прочитать число
        public string Read(decimal number, Style style = Style.Full, bool register = true)
        {
            var a = IntegerLastTwoRub(number);
            var b = FractionalPartFirstTwoKop(number, a.integer);
            var c = NumberClasses(a.integer);
            StringBuilder result = new StringBuilder();
            int i = 0;
            bool isNull = true;
            foreach (decimal @class in c)
            {
                if (@class > 0m)
                {
                    if (@class > 0m && isNull) isNull = false;
                    result.Append(Convert(@class, worlds[0], worlds[1], worlds[i == 8 ? 2 : 3], i == 9 ? null : worlds[i + 4]));
                }
                i++;
            }
            if (isNull) result.Append("ноль ");
            result.Remove(result.Length - 1, 1);
            //В заглавную букву
            if (register) result[0] = char.ToUpper(result[0]);
            //Преобразование в финальную строку в зависимости от style
            switch (style)
            {
                default: return result.ToString();
                case Style.Minimum: return result.Append(" руб. ").AppendFormat("{0:00}", b.kopecks).Append(" коп.").ToString();
                case Style.Middle:
                    Add(a.ruble, result, worlds[13]);
                    result.AppendFormat("{0:00}", b.kopecks);
                    Add(b.kopecks, result, worlds[14]);
                    return result.ToString();
                case Style.Full:
                    result.Insert(0, " (").Insert(0, a.integer).Append(")");
                    Add(a.ruble, result, worlds[13]);
                    result.AppendFormat("{0:00}", b.kopecks);
                    Add(b.kopecks, result, worlds[14]);
                    return result.ToString();
            }
        }

        //Вычисление целой части числа и последних два рубля
        (decimal integer, decimal ruble) IntegerLastTwoRub(decimal number)
        {
            decimal integer = Math.Truncate(number);
            return (integer, integer % 100m);
        }

        //Вычисление дробной части числа и первые две копейки
        (decimal fractional, decimal kopecks) FractionalPartFirstTwoKop(decimal number, decimal integer)
        {
            decimal discharge = 1m;
            decimal calc = number;
            while (calc % 1 != 0)
            {
                calc *= 10m;
                discharge *= 10;
            }
            decimal fractional = Math.Truncate((number - integer) * discharge);
            decimal kopecks = discharge > 9m ? Math.Truncate(fractional / discharge * 100m) : fractional;
            return (fractional, kopecks);
        }

        //Вычисление классов числа
        decimal[] NumberClasses(decimal integer)
        {
            decimal[] result = { 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m };
            int classes = result.Length - 1;
            decimal calc = integer;
            int count = 1;
            while ((calc /= 10) > 1) count++;
            count = count % 3 > 0 ? count / 3 + 1 : count / 3;
            calc = integer;
            for (int i = 0; i < count; i++)
            {
                calc /= 1000m;
                result[classes--] = Math.Truncate((calc - Math.Truncate(calc)) * 1000m);
            }
            return result;
        }

        //Преобразование числа в сумму
        StringBuilder Convert(decimal number, string[] hundreds, string[] tens, string[] units, string[] name)
        {
            decimal hundred = Math.Truncate(number / 100m);
            decimal ten = Math.Truncate((number - hundred * 100m) / 10m);
            decimal unit = number - hundred * 100m - ten * 10m;
            decimal exc = number - hundred * 100m;
            StringBuilder result = new StringBuilder();
            if (1m <= hundred & hundred <= 9m) result.Append(hundreds[(int)hundred]);
            if (exc > 19m)
            {
                if (2m <= ten & ten <= 9m) result.Append(tens[(int)ten]);
                if (1m <= unit & unit <= 9m) result.Append(units[(int)unit]);
            }
            else if (1m <= exc & exc <= 19m) result.Append(units[(int)exc]);
            if (name != null) Add(exc, result, name);
            return result;
        }

        //Добавление окончаний классов числа
        void Add(decimal unit, StringBuilder result, string[] name)
        {
            if (11m <= unit & unit <= 19m) result.Append(name[2]);
            else
            {
                unit = unit - Math.Truncate((unit / 10m)) * 10m;
                if (unit == 1m) result.Append(name[0]);
                else if (2m <= unit & unit <= 4m) result.Append(name[1]);
                else result.Append(name[2]);
            }
        }
    }
}