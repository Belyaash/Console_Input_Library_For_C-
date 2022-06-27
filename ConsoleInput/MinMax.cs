namespace ConsoleInput;

public class MinMax<T> where T : struct, IComparable<T>
{
    public T Min { get; }
    public T Max { get; }

    private MinMax(T min, T max)
    {
        this.Min = min;
        this.Max = max;
    }

    public static MinMax<T> Range(T min, T max)
    {
        if (min.CompareTo(max) == 1)
        {
            (min, max) = (max, min);
        }

        return new MinMax<T>(min, max);
    }

    public static MinMax<T> HigherThan(T min)
    {
        T max = GenericMethods.ReadStaticField<T>("MaxValue", default(T));
        return new MinMax<T>(min,max);
    }

    public static MinMax<T> LowerThan(T max)
    {
        T min = GenericMethods.ReadStaticField<T>("MinValue", default(T));
        return new MinMax<T>(min, max);
    }

    public static MinMax<T> TypeRange()
    {
        T min = GenericMethods.ReadStaticField<T>("MinValue", default(T));
        T max = GenericMethods.ReadStaticField<T>("MaxValue", default(T));
        return new MinMax<T>(min, max);
    }
}