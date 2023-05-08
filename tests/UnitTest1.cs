using System.Xml.Linq;
using System.IO;
using life;

namespace tests
{
[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestMethod1()
    {
        int result = life.Program.checkinElementCount("gen-239.txt");
        int expected = 22;
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TestMethod2()
    {
        int result = life.Program.checkinSymmetricFigureCount("gen-239.txt");
        int expected = 4;
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TestMethod3()
    {
        int[] numberOfFigures = life.Program.checkinDetectionFigure("gen-239.txt");
        int result = numberOfFigures[4];//под номером 4 ульи
        int expected = 3;
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TestMethod4()
    {
        int[] numberOfFigures = life.Program.checkinDetectionFigure("gen-580.txt");
        int result = numberOfFigures[0];//под номером 0 блоки
        int expected = 1;
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TestMethod5()
    {
        int result = life.Program.checkinElementCount("gen-502.txt");
        int expected = 20;
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TestMethod6()
    {
        int result = life.Program.checkinSymmetricFigureCount("gen-502.txt");
        int expected = 4;
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TestMethod7()
    {
        int[] numberOfFigures = life.Program.checkinDetectionFigure("gen-502.txt");
        int result = numberOfFigures[4];//под номером 4 ульи
        int expected = 2;
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TestMethod8()
    {
        int[] numberOfFigures = life.Program.checkinDetectionFigure("gen-502.txt");
        int result = numberOfFigures[0];//под номером 0 блоки
        int expected = 2;
        Assert.AreEqual(expected, result);
    }
}
}
