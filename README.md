# Statistics-Helper-Library
this library contains some lightweight math functions for my projects.
The main purpose is to seve fast, reliable, fast performing statistics on live fed data.
This is particularly useful on Sensor Data, Spot Data or to get statistics on data where the dataset is simply too large to get statistics in a feasible timed manner.

The focus is on simplicity, stability and the utmost performance which I'm personally able to archieve through my skills.

It is not about errorhandling. Make sure, you feed quality data. Garbage in, Garbage out.
While I try to add unit tests to my projects, do a quick test yourself and check if the data pushed out by this library actually makes sense to you.

# Current implementations
### average
- Moving average
- Quick average on the fly (omitting memory restraints and overflow)
- Volumetric average for 2 values (averaging two values by teir weight)

### Minimum Maximum
- Calculate Maximum for a given amount of Datapoints (useful for sensor Data)
