namespace GhostCore.Animations.Core
{
    public enum GhostGeometryCombine
    {
        //
        // Summary:
        //     The result geometry contains the set of all areas from either of the source geometries.
        Union = 0,
        //
        // Summary:
        //     The result geometry contains just the areas where the source geometries overlap.
        Intersect = 1,
        //
        // Summary:
        //     The result geometry contains the areas from both the source geometries, except
        //     for any parts where they overlap.
        Xor = 2,
        //
        // Summary:
        //     The result geometry contains any area that is in the first source geometry- but
        //     excludes any area belonging to the second geometry.
        Exclude = 3
    }
}
