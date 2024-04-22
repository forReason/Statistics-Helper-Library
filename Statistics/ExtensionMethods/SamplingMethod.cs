namespace QuickStatistics.Net.ExtensionMethods;

/// <summary>
/// provides different methods for down-sampling arrays and Lists
/// </summary>
public enum DownSamplingMethod
{
    /// <summary>
    /// builds an average of selected data points to down-sample
    /// </summary>
    Average,
    /// <summary>
    /// builds a median of selected data points to down-sample
    /// </summary>
    Median,
    /// <summary>
    /// selects a random subset where each subset has the same probability
    /// </summary>
    RandomReservoir,
    /// <summary>
    /// takes every x-th entry, based on the target resolution and chooses the nearest neighbor to reduce aliasing
    /// </summary>
    /// <remarks>
    /// very similar to Decimate
    /// </remarks>
    NearestNeighbor,
    /// <summary>
    /// chooses the Maximum value out of the subset
    /// </summary>
    MaxPooling,
    /// <summary>
    /// chooses the minimum value for each Point
    /// </summary>
    MinPooling
}

/// <summary>
/// provides different methods for up-sampling arrays and Lists
/// </summary>
public enum UpSamplingMethod
{
    /// <summary>
    /// assumes a linear progression between values
    /// </summary>
    LinearInterpolation,
    /// <summary>
    /// assumes a smoothed out progression between values
    /// </summary>
    SplineInterpolation,
    /// <summary>
    /// chooses the closest point
    /// </summary>
    NearestNeighbor,
    /// <summary>
    /// repeats values
    /// </summary>
    Repetition
}