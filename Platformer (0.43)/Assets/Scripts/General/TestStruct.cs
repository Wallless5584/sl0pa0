public struct TestStruct
{
    public float A { get; private set; }
    public float B { get; private set; }

    public TestStruct(float a, float b)
    {
        A = a;
        B = b;
    }
}