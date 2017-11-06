using System;

namespace Functions.Interfaces
{
    public interface IFunction<TSpace, TValue> where TSpace : IComparable<TSpace>
    {
        TValue Value(TSpace point);
        IInterval<TSpace> Interval { get; }
        bool IsDefinedOn(TSpace point);
        bool TryUnion(IFunction<TSpace, TValue> function);
        /// <summary>
        /// Пытается объединить две функции в одну, результат объединения помещает в resultFunction и возвращает true, если объединение прошло успешно, иначе false.
        /// </summary>
        /// <param name="function">Функция, с которой требуется объединить данную.</param>
        /// <param name="resultFunction">Результирующая функция. Если функции объеденить невозможно, то возвращает null.</param>
        /// <returns>Возвращает true, если функции были успешн объеденины, иначе false.</returns>
        bool TryUnion(IFunction<TSpace, TValue> function, out IFunction<TSpace, TValue> resultFunction);
        IFunction<TSpace, TValue> Union(IFunction<TSpace, TValue> function);
        IFunction<TSpace, TValue> ShortenIntervalTo(IInterval<TSpace> interval);
    }
}
