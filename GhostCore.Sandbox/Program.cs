using GhostCore.Data.Evaluation;
using GhostCore.Data.Evaluation.SourceGen;
using System.Numerics;

namespace GhostCore.Sandbox;

internal class Program
{
    static void Main(string[] args)
    {
        var v1 = new Vector3Maker { X = 0, Y = 1, Z = 2 };
        var v2 = new Vector3Maker { X = 2, Y = 1, Z = 0 };
        var v3 = new Vector3Maker { X = 3, Y = 2, Z = 1 };

        var math1 = new VectorMath();
        math1.ConnectToPort("A", v1);
        math1.ConnectToPort("B", v2);

        var math2 = new VectorMath();
        math2.ConnectToPort("A", math1, "Sum");
        math2.ConnectToPort("B", v3);

        var swizler = new VectorSwizzler();
        swizler.ConnectToPort(swizler.GetInputPort("Input"), math2, math2.GetOutputPort("Sum"));

        var result = swizler.GetOutputValue(swizler.GetOutputPort("ZYX"), EvaluationContext.Default);
        if (result is not Vector3 vecResult)
            return;

        Console.WriteLine("I expected [3, 4, 5] and i got...");
        Console.WriteLine(vecResult);
    }
}

[Evaluatable]
public partial class Vector3Maker
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

    [Output]
    public Vector3 Sum => A + B;

    [Output]
    public Vector3 Diff => A - B;

    [Output]
    public Vector3 Mul => A * B;

    [Output]
    public Vector3 Div => A / B;

    [Output]
    public Vector3 DiffB => B - A;

    [Output]
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