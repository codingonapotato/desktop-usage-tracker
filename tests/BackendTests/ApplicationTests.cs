namespace BackendTests;
using System.Diagnostics;
using System.Linq.Expressions;
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

    [TestCase("ONqGYtCt09BxfVJmZqO8ptBEKvUrTn0mSs0s0vXz7hkNYPqsrR")]
    [TestCase("R")]
    [TestCase("Productivity")]
    public void IsValidCatgory_ValidCategories(string category)
    {
        Assert.True(Application.IsValidCategory(category));
    }

    [TestCase("")]
    [TestCase("ONqGYtCt09BxfVJmZqO8ptBEKvUrTn0mSs0s0vXz7hkNYPqsrR1")]
    [TestCase("")]
    public void IsValidCatgory_InvalidCategories(string category)
    {
        Assert.True(Application.IsValidCategory(category));
    }

    [TestCase("Productivity")]
    public void AddCategory_AddNewCategory_AddSuccess(string category)
    {
        Application.AddCategory(category);

        Assert.True(Application.DefinedCategories.Count == 1);

        Assert.True(Application.DefinedCategories.Contains(category.ToLower()));
    }

    [TestCase("Productivity")]
    [TestCase("productivity")]
    public void AddCategory_AddCategoryThatAlreadyExists_NoDuplicates(string category)
    {
        Application.AddCategory("Productivity");
        Application.AddCategory(category);

        Assert.True(Application.DefinedCategories.Count == 1);
        Assert.True(Application.DefinedCategories.Contains(category.ToLower()));
    }

    [Test]
    public void AddCategory_AddEmptyStringCategory()
    {
        try
        {
            Application.AddCategory("");
        }
        catch (ArgumentException)    // Pass
        {
            Assert.True(Application.DefinedCategories.Count == 0);

            Assert.False(Application.DefinedCategories.Contains(""));
        }
    }

    [TestCase("Productivity")]
    public void ModifyCategory_CategoryIsDefined_CategoryChangeSuccess(string modifiedCategory)
    {
        string oldCategory = "Gaming";
        Application.AddCategory(oldCategory);
        Application.ModifyCategory(oldCategory, modifiedCategory);

        Assert.True(Application.DefinedCategories.Count == 1);
        Assert.True(Application.DefinedCategories.Contains(modifiedCategory.ToLower()));
    }

    [TestCase("")]
    public void ModifyCategory_CategoryIsDefined_CategoryChangeFail(string modifiedCategory)
    {
        string oldCategory = "Gaming";
        Application.AddCategory(oldCategory);
        try
        {
            Application.ModifyCategory(oldCategory, modifiedCategory);
        }
        catch (ArgumentException)
        {
            Assert.True(Application.DefinedCategories.Count == 1);
            Assert.True(Application.DefinedCategories.Contains(oldCategory.ToLower()));
        }
    }
}