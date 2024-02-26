namespace BackendTests;
using NUnit.Framework.Internal;

[TestFixture]
public class CategoryTests
{
    // [SetUp]
    // public void Setup()
    // {
    // }

    [TestCase("ONqGYtCt09BxfVJmZqO8ptBEKvUrTn0mSs0s0vXz7hkNYPqsrR")]    // 50 characters case
    [TestCase("R")]    // 1 character case
    [TestCase("Productivity")]    // General case 
    public void IsValidCatgory_ValidCategories(string category)
    {
        Assert.True(Category.IsValidName(category));
    }

    [TestCase("")]    // 0 character case
    [TestCase("ONqGYtCt09BxfVJmZqO8ptBEKvUrTn0mSs0s0vXz7hkNYPqsrR1")]    // 51 characters case
    public void IsValidCatgory_InvalidCategories(string category)
    {
        Assert.False(Category.IsValidName(category));
    }

    [TestCase("Productivity")]
    public void AddCategory_AddNewCategory_AddSuccess(string category)
    {
        Category.AddCategory(category);

        Assert.True(Category.DefinedCategories.Count == 1);

        Assert.True(Category.DefinedCategories.ContainsKey(category.ToLower()));
    }

    [TestCase("Productivity")]
    [TestCase("productivity")]
    public void AddCategory_AddCategoryThatAlreadyExists_NoDuplicates(string category)
    {
        Category.AddCategory("Productivity");
        Category.AddCategory(category);

        Assert.True(Category.DefinedCategories.Count == 1);
        Assert.True(Category.DefinedCategories.ContainsKey(category.ToLower()));
    }

    [Test]
    public void AddCategory_AddEmptyStringCategory()
    {
        try
        {
            Category.AddCategory("");
            Assert.Fail();
        }
        catch (ArgumentException)    // Pass
        {
            Assert.True(Category.DefinedCategories.Count == 0);

            Assert.True(!Category.DefinedCategories.ContainsKey(""));
        }
    }

    [TestCase("Productivity")]
    public void ModifyCategory_CategoryIsDefined_CategoryChangeSuccess(string modifiedCategory)
    {
        string oldCategory = "Gaming";
        Category.AddCategory(oldCategory);
        Category.ModifyCategory(oldCategory, modifiedCategory);

        Assert.True(Category.DefinedCategories.Count == 1);
        Assert.True(Category.DefinedCategories.ContainsKey(modifiedCategory.ToLower()));
    }

    [TestCase("")]
    public void ModifyCategory_CategoryIsDefined_CategoryChangeFail(string modifiedCategory)
    {
        string oldCategory = "Gaming";
        Category.AddCategory(oldCategory);
        try
        {
            Category.ModifyCategory(oldCategory, modifiedCategory);
            Assert.Fail();
            
        }
        catch (ArgumentException)    // Pass
        {
            Assert.True(Category.DefinedCategories.Count == 1);

            Assert.True(Category.DefinedCategories.ContainsKey(oldCategory.ToLower()));
            Assert.True(Category.DefinedCategories[oldCategory.ToLower()].Name == oldCategory.ToLower());

            Assert.False(Category.DefinedCategories.ContainsKey(modifiedCategory.ToLower()));
        }
    }
}