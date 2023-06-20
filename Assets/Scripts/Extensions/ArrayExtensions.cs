using System.Text;

namespace Extensions
{
    public static class ArrayExtensions
    {
        public static string ToString<T>(this T[] array)
        {
            StringBuilder resultBuilder = new StringBuilder();

            foreach (var item in array)
            {
                resultBuilder.Append(item);
                resultBuilder.Append(", ");
            }

            if (array.Length > 0)
            {
                resultBuilder.Length -= 2;
            }

            return resultBuilder.ToString();
        }
    }
}
