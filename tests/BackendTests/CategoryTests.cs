namespace BackendTests;
using Backend;
using NUnit.Framework.Internal;

public class CategoryTests
{
    [SetUp]
    public void Setup()
    {
        Category.DefinedCategories.Clear();
    }

    [TestFixture]
    class CategoryConstructorTests
    {
        [TestCase("ONqGYtCt09BxfVJmZqO8ptBEKvUrTn0mSs0s0vXz7hkNYPqsrR")]    // 50 characters case
        [TestCase("R")]    // 1 character case
        [TestCase("Productivity")]    // General case
        public void ValidCategory(string category)
        {
            Category categoryInstance = new Category(category);
            Assert.True(categoryInstance.Name == category.ToLower());
        }

        [TestCase("")]    // 0 character case
        [TestCase("ONqGYtCt09BxfVJmZqO8ptBEKvUrTn0mSs0s0vXz7hkNYPqsrR1")]    // 51 characters case
        public void InvalidCategory(string category)
        {
            try
            {
                Category categoryInstance = new Category(category);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                Assert.Pass();    // pass
            }
        }
    }

    [TestFixture]
    class IsValidCategoryTests : CategoryTests
    {
        [TestCase("ONqGYtCt09BxfVJmZqO8ptBEKvUrTn0mSs0s0vXz7hkNYPqsrR")]    // 50 characters case
        [TestCase("R")]    // 1 character case
        [TestCase("Productivity")]    // General case 
        public void ValidCategory(string category)
        {
            Assert.True(Category.IsValidName(category));
        }

        [TestCase("")]    // 0 character case
        [TestCase("ONqGYtCt09BxfVJmZqO8ptBEKvUrTn0mSs0s0vXz7hkNYPqsrR1")]    // 51 characters case
        public void InvalidCategory(string category)
        {
            Assert.False(Category.IsValidName(category));
        }
    }

    [TestFixture]
    class GetInstanceTests : CategoryTests
    {
        [TestCase("Education")]
        [TestCase("education")]
        public void InstanceWithSameNameExists(string category)
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
        public void InstanceWithSameNameDoesNotExist(string category)
        {
            Category.GetInstance(category);
            Assert.True(Category.DefinedCategories.Count == 1);
        }

        [TestCase("")]    // 0 character case
        [TestCase("ONqGYtCt09BxfVJmZqO8ptBEKvUrTn0mSs0s0vXz7hkNYPqsrR1")]    // 51 characters case
        public void InvalidName(string category)
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
    }

    [TestFixture]
    class AddCategoryTests : CategoryTests
    {
        [TestCase("Productivity")]
        public void AddSingleValidCategory(string category)
        {
            Category.AddCategory(category);

            Assert.True(Category.DefinedCategories.Count == 1);

            Assert.True(Category.DefinedCategories.ContainsKey(category.ToLower()));
        }

        [TestCase("Productivity")]
        [TestCase("productivity")]
        public void AddCategoryThatAlreadyExists(string category)
        {
            Category.AddCategory("Productivity");
            Category.AddCategory(category);

            Assert.True(Category.DefinedCategories.Count == 1);
            Assert.True(Category.DefinedCategories.ContainsKey(category.ToLower()));
        }

        [Test]
        public void AddEmptyStringFail()
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
    }

    [TestFixture]
    class ModifyCategoryTests : CategoryTests
    {
        [TestCase("Productivity")]
        public void ModifyExistingCategory(string modifiedCategory)
        {
            string oldCategory = "Gaming";
            Category.AddCategory(oldCategory);
            Category.ModifyCategory(oldCategory, modifiedCategory);

            Assert.True(Category.DefinedCategories.Count == 1);
            Assert.True(Category.DefinedCategories.ContainsKey(modifiedCategory.ToLower()));
        }

        [TestCase("")]
        [TestCase("")]    // 0 character case
        [TestCase("ONqGYtCt09BxfVJmZqO8ptBEKvUrTn0mSs0s0vXz7hkNYPqsrR1")]    // 51 characters case
        public void ModifyInvalidOrCategoryThatDoesNotExist(string modifiedCategory)
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

    [TestFixture]
    class RemoveCategoryTests : CategoryTests
    {
        [TestCase("Entertainment")]
        public void RemoveExistingCategory(string category)
        {
            Category.AddCategory(category);
            Assert.True(Category.DefinedCategories.Count == 1);
            Category.RemoveCategory(category);
            Assert.True(Category.DefinedCategories.Count == 0);
        }

        [TestCase("Entertainment")]
        [TestCase("")]    // 0 character case
        [TestCase("ONqGYtCt09BxfVJmZqO8ptBEKvUrTn0mSs0s0vXz7hkNYPqsrR1")]    // 51 characters case
        public void RemoveInvalidOrCategoryThatDoesNotExist(string category)
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
}