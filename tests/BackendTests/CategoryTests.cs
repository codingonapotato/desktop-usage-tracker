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

    [TestCase("Education")]
    [TestCase("education")]
    public void GetInstance_InstanceWithSameNameExists(string category)
    {
        // Create instance and add to defined instances dictionary
        Category categoryInstance = new Category(category);
        Category.DefinedCategories.Add(category.ToLower(), categoryInstance);

        Category returnedInstance = Category.GetInstance(category);

        // Ensure that get instance does not instantiate a new category instance of the same name
        Assert.True(categoryInstance == returnedInstance);
        Assert.True(Category.DefinedCategories.Count == 1);
    }

    [TestCase("Education")]
    public void GetInstance_InstanceWithSameNameDoesNotExists(string category)
    {
        Category.GetInstance(category);
        Assert.True(Category.DefinedCategories.Count == 1);
    }

    [TestCase("")]    // 0 character case
    [TestCase("ONqGYtCt09BxfVJmZqO8ptBEKvUrTn0mSs0s0vXz7hkNYPqsrR1")]    // 51 characters case
    public void GetInstance_InvalidName(string category)
    {
        try
        {
            Category.GetInstance(category);
            Assert.Fail();
        }
        catch (ArgumentException)
        {
            Assert.True(Category.DefinedCategories.Count == 0);
        }
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

    [TestCase("Entertainment")]
    public void RemoveCategory_CategoryExists(string category)
    {
        Category.AddCategory(category);
        Assert.True(Category.DefinedCategories.Count == 1);
        Category.RemoveCategory(category);
        Assert.True(Category.DefinedCategories.Count == 0);
    }

    [TestCase("Entertainment")]
    [TestCase("")]    // 0 character case
    [TestCase("ONqGYtCt09BxfVJmZqO8ptBEKvUrTn0mSs0s0vXz7hkNYPqsrR1")]    // 51 characters case
    public void RemoveCategory_CategoryDoesNotExistsOrIsInvalid(string category)
    {
        try
        {
            Category.RemoveCategory(category);
            Assert.Fail();
        }
        catch (ArgumentException)
        {
            Assert.True(Category.DefinedCategories.Count == 0);
        }
    }
}