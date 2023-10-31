# Statistics-Helper-Library
this library contains some lightweight math functions for my projects.
The main purpose is to obtain fast, reliable and well performing statistics on live fed data.
This is particularly useful on Sensor Data, Spot Data or to get statistics on data where the dataset is simply too large to fit into memory.

The focus is on simplicity, stability and the utmost performance which I'm personally able to archieve through my skills.

It is not about errorhandling. Make sure, you feed quality data. Garbage in, Garbage out.
While I try to add unit tests to my projects, do a quick test yourself and check if the data pushed out by this library actually makes sense to you.

# Current implementations
### Average
- Moving average
```
using QuickStatistics.Net.Average_NS;

// Create a new Moving_Average_Double object with a total tracking time of 2 hours and a value resolution of 1 minute
Moving_Average_Double ma = new Moving_Average_Double(TimeSpan.FromHours(2), TimeSpan.FromMinutes(1));

// Add some values to the moving average
ma.AddValue(5);
ma.AddValue(6);
ma.AddValue(7);

// Get the current value of the moving average
double currentValue = ma.Value;

// Set the resolution to track the past 3 hours with a value resolution of 15 minutes
ma.SetResolution(TimeSpan.FromHours(3), TimeSpan.FromMinutes(15));

// Add a value with a timestamp of 1 hour ago
ma.AddValue(8, DateTime.Now - TimeSpan.FromHours(1));

// Get the current value of the moving average
currentValue = ma.Value;

// Clear all values in the moving average
ma.Clear();
```
### Averaging
- progressing_Average:
a straight forward, very fast, precise implementation for calculating the average value. Limit is int.max (count of values) so it is not suited for an infinite stream of data such as for sensors
- Simple_Exponential_Average:
Moving average approximation, very fast and lightweight
- Simple_Moving_Average:
Average on the fly (omitting memory restraints and overflow)
Supports: AbsoluteRateOfChange, Trend, Momentum, Deviation
- Volumetric average for 2 values (averaging two values by their weight)
- Moving_Average:
Time based moving average (good for historic data and data with gaps, also good for targeting a certain time duration or when the input data stream is not consistent in speed)
Maps the incoming data to a consistient timeline with variable resolution
Supports: AbsoluteRateOfChange, Trend, Momentum, Deviation

### Median
Median can be calculated:
- On an array / List
- Procedural upon adding values
- On a rolling time window

### Minimum / Maximum
- Calculate Maximum or Minimum for a given amount of Datapoints (useful for sensor Data)
- Calculate Maximum or Minimum for a sliding time window (useful for historical or inconsistent Data)

### Statistics
- obtain standard deviation on the fly. (warning: will overflow if ran indefinitely)
- obtain exponential standard deviation on the fly for infinite amount of data points (less precision)
- AbsoluteRateOfChange, Trend, Momentum and Deviation on sliding windows (Supported by simple moving average & moving average)
