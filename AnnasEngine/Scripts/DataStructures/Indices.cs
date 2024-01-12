namespace AnnasEngine.Scripts.DataStructures;

public class TriangleIndices
{
    public uint FirstIndex { get; set; }
    public uint SecondIndex { get; set; }
    public uint ThirdIndex { get; set; }

    public TriangleIndices(uint firstIndex, uint secondIndex, uint thirdIndex)
    {
        FirstIndex = firstIndex;
        SecondIndex = secondIndex;
        ThirdIndex = thirdIndex;
    }

    public List<float> Build()
    {
        return new List<float>() { FirstIndex, SecondIndex, ThirdIndex };
    }

    public override string ToString()
    {
        return $"TriangleIndices: [{FirstIndex}, {SecondIndex}, {ThirdIndex}]";
    }

    public static TriangleIndices operator +(TriangleIndices indicesA, TriangleIndices indicesB)
    {
        return new TriangleIndices(indicesA.FirstIndex + indicesB.FirstIndex,
                                   indicesA.SecondIndex + indicesB.SecondIndex,
                                   indicesA.ThirdIndex + indicesB.ThirdIndex);
    }

    public static TriangleIndices operator -(TriangleIndices indicesA, TriangleIndices indicesB)
    {
        return new TriangleIndices(indicesA.FirstIndex - indicesB.FirstIndex,
                                   indicesA.SecondIndex - indicesB.SecondIndex,
                                   indicesA.ThirdIndex - indicesB.ThirdIndex);
    }

    public static TriangleIndices operator *(TriangleIndices indicesA, TriangleIndices indicesB)
    {
        return new TriangleIndices(indicesA.FirstIndex * indicesB.FirstIndex,
                                   indicesA.SecondIndex * indicesB.SecondIndex,
                                   indicesA.ThirdIndex * indicesB.ThirdIndex);
    }

    public static TriangleIndices operator /(TriangleIndices indicesA, TriangleIndices indicesB)
    {
        return new TriangleIndices(indicesA.FirstIndex / indicesB.FirstIndex,
                                   indicesA.SecondIndex / indicesB.SecondIndex,
                                   indicesA.ThirdIndex / indicesB.ThirdIndex);
    }

    public static TriangleIndices operator %(TriangleIndices indicesA, TriangleIndices indicesB)
    {
        return new TriangleIndices(indicesA.FirstIndex % indicesB.FirstIndex,
                                   indicesA.SecondIndex % indicesB.SecondIndex,
                                   indicesA.ThirdIndex % indicesB.ThirdIndex);
    }
}
