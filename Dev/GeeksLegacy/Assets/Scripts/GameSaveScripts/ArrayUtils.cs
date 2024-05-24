// ArrayUtils 
// Permet de transformer notre matrice multidimensionnelle (carte du monde) en string et vice-versa, pour pouvoir la stocker dans le fichier JSON. Généré à l'aide de ChatGPT
// Auteur : Amine Fanid
public static class ArrayUtils
{
    public static string ConvertToString(int[,] array)
    {
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);

        string[] elements = new string[rows * cols];
        int index = 0;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                elements[index++] = array[i, j].ToString();
            }
        }
        return string.Join(",", elements);
    }
    public static int[,] ConvertToIntArray(string str, int rows, int cols)
    {
        string[] elements = str.Split(',');
        int[,] array = new int[rows, cols];

        int index = 0;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                array[i, j] = int.Parse(elements[index++]);
            }
        }
        return array;
    }
}
