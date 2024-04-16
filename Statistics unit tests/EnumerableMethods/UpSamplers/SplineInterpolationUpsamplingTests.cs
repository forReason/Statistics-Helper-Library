using System;
using System.Text;
using QuickStatistics.Net.EnumerableMethods.UpSamplers;
using Xunit;

namespace Statistics_unit_tests.EnumerableMethods.UpSamplers;

public class SplineInterpolationUpsamplingTests
{
    [Fact]
    public void UpSampleSplineInterpolation_ReturnsCorrectLength()
    {
        // Arrange
        double[] source = { 1, 2, 3 };
        int targetLength = 6;

        // Act
        double[] result = UpSampler.UpSampleSplineInterpolation(source, targetLength);

        // Assert
        Assert.Equal(targetLength, result.Length);
    }

    [Fact]
    public void UpSampleSplineInterpolation_ThrowsException_IfTargetLengthIsInvalid()
    {
        // Arrange
        double[] source = { 1, 2, 3 };
        int targetLength = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => UpSampler.UpSampleSplineInterpolation(source, targetLength));
    }

    [Fact]
    public void UpSampleSplineInterpolation_ReturnsExpectedValues()
    {
        // Arrange
        double[] source = { 5,4,2,1,-8,6,8,3,10};
        int targetLength = 50;
        double[] expected =
        {
            5,4.960147850491247,4.8508911789995235,4.687679651076626,4.485962932274357,4.261190688144515,
            4.028812584238899,3.651241750645135,3.1807135872611565,2.7039241192358325,2.2944202921019996,
            2.025749051392493,1.9714573426401498,2.2727140569190913,2.5267049382344986,2.6109285588540367,
            2.476863162685651,2.0759869936372857,1.3597782956168867,-0.19092602166467643,-2.3690577577487595,
            -4.594505296332711,-6.5378360053082645,-7.869617252567172,-8.260416406001172,-6.844783635444093,
            -4.2206969181474,-1.4092125035165273,1.3127004641536775,3.66807284056838,5.379935481432739,
            6.007073851801327,6.332235510934392,6.933666697039999,7.597571727143085,8.11015491826859,
            8.257620587441457,7.773810319758592,6.692729456561906,5.47363470564592,4.308661453477775,
            3.389945086524632,2.909620991253645,3.1343595864541567,4.232604558195623,5.3643676690822035,
            6.518318843909911,7.6831280074747665,8.84746508457279,9.999999999999993,
        };

        // Act
        double[] result = UpSampler.UpSampleSplineInterpolation(source, targetLength);

        StringBuilder SplineString = new();
        foreach (double value in result)
        {
            SplineString.Append(value.ToString());
            SplineString.Append(',');
        }
        // Assert
        Assert.Equal(expected, result);
    }
}