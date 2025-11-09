using GhostCore.Data.Evaluation;
using GhostCore.Data.Evaluation.SourceGen;
using System.Numerics;

namespace GhostCore.Sandbox;

internal class Program
{
    static void Main(string[] args)
    {
        var s = new Scalar { X = 10 };
        
        var v1 = new V3 { X = 0, Y = 1, Z = 2 };
        var v2 = new V3 { X = 2, Y = 1, Z = 0 };
        var v3 = new V3 { X = 3, Y = 2, Z = 1 };

        var math1 = new VectorMath();
        math1.ConnectToPort("A", v1);
        math1.ConnectToPort("B", v2);

        var math2 = new VectorMath();
        math2.ConnectToPort("A", math1, "Sum");
        math2.ConnectToPort("B", v3);

        var math3 = new VectorMath();
        math3.ConnectToPort("A", math2, "Sum");
        math3.ConnectToPort("B", s, FloatToVector3Converter.Instance);

        var swizler = new VectorSwizzler();
        swizler.ConnectToPort("Input", math3, "Mul");

        var result = swizler.GetOutputValue(swizler.GetOutputPort("ZYX"), EvaluationContext.Default);
        if (result is not Vector3 vecResult)
            return;

        Console.WriteLine("I expected [30, 40, 50] and i got...");
        Console.WriteLine(vecResult);
    }
}

public class FloatToVector3Converter : IConverter
{
    public static readonly FloatToVector3Converter Instance = new();

    public object? Convert(object? data, Type? sourceType, Type targetType)
    {
        if (data is not float f)
            throw new InvalidOperationException("Invalid conversion. source not float");

        return new Vector3(f, f, f);
    }
}

[Evaluatable]
public partial class Scalar
{
    public float X { get; set; }

    [Output]
    public float Output => X;
}

[Evaluatable]
public partial class V3
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }

    [Output]
    public Vector3 Output => new(X, Y, Z);
}

[Evaluatable]
public partial class VectorMath
{
    [Input]
    public Vector3 A { get; set; }

    [Input]
    public Vector3 B { get; set; }

    [Output("A+B")]
    public Vector3 Sum => A + B;

    [Output("A-B")]
    public Vector3 Diff => A - B;

    [Output("A*B")]
    public Vector3 Mul => A * B;

    [Output("A/B")]
    public Vector3 Div => A / B;

    [Output("B-A")]
    public Vector3 DiffB => B - A;

    [Output("Dot Product")]
    public float Dot => Vector3.Dot(A, B);
}

[Evaluatable]
public partial class VectorSwizzler
{
    [Input]
    public Vector3 Input { get; set; }

    [Output]
    public Vector3 ZYX => new(Input.Z, Input.Y, Input.X);

    [Output]
    public Vector3 XYY => new(Input.X, Input.Y, Input.Y);

    [Output]
    public Vector3 XXX => new(Input.X, Input.X, Input.X);
}