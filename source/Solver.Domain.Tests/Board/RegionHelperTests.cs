using Solver.Domain.Board;

namespace Solver.Domain.Tests.Board;

public class RegionHelperTests
{
    [Test]
    public void GetRegionIndex_ZeroIsZero()
    {
        var actual = RegionHelper.GetRegionIndex(0, 0);
        
        Assert.AreEqual(0,actual);
    }
    [Test]
    public void GetRegionIndex_Tr3c0Is3()
    {
        var actual = RegionHelper.GetRegionIndex(3, 0);
        
        Assert.AreEqual(3,actual);
    }
    [Test]
    public void GetRegionIndex_Tr3c3Is4()
    {
        var actual = RegionHelper.GetRegionIndex(3, 3);
        
        Assert.AreEqual(4,actual);
    }
    
    [Test]
    public void GetRegionIndex_Tr5c5Is4()
    {
        var actual = RegionHelper.GetRegionIndex(5, 5);
        
        Assert.AreEqual(4,actual);
    }
    [Test]
    public void GetIndexWithinRegion_ZeroIsZero()
    {
        var actual = RegionHelper.GetIndexWithinRegion(0, 0);
        
        Assert.AreEqual(0,actual.column);
        Assert.AreEqual(0,actual.row);
    }
    [Test]
    public void GetRegionIndex_Tr5c5Isr2c5()
    {
        var actual = RegionHelper.GetIndexWithinRegion(5, 5);
        
        Assert.AreEqual(2,actual.column);
        Assert.AreEqual(2,actual.row);
    }
}