namespace Functions.Interfaces
{
    public interface IMetric<in TSpace, out TMetric>
    {
        TMetric GetMetric(TSpace point1, TSpace point2);
    }
}
