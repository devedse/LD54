public static class MathHelpertjes
{
    public static float AtLeast(this float value, float min)
    {
        return value < min ? min : value;
    }

    public static float AtMost(this float value, float max)
    {
        return value > max ? max : value;
    }
}
