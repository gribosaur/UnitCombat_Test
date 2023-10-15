
public static class MathOperators 
{
    public static int Calculate(char operation, int x, int y)
    {
        switch (operation)
        {
            case '+': return x + y;
            case '-': return x - y;
            case '/': return x / y;
            case '*': return x * y;
            default: return 0;
        }
    }
}
