namespace BackendTests;
using System.Diagnostics;
using NUnit.Framework.Internal;

[TestFixture]
public class ApplicationTests
{
    private Application _Application;

    [SetUp]
    public void Setup()
    {
        _Application = new Application(new Process());
    }

    [Test]
    public void AddCategory_AddNewCategory_AddSuccess()
    {
        Application.AddCategory("Productivity");

        Assert.True(Application.DefinedCategories.Count == 0);

        Assert.True(Application.DefinedCategories.Contains("Productivity"));
    }
}