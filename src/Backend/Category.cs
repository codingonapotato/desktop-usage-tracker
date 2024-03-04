using System.Reflection.Metadata.Ecma335;

public class Category
{
    public string Name;
    private static Dictionary<string, Category> _DefinedCategories = new Dictionary<string, Category>();    // strictly lower-case alphanumerical strings as keys only
    public static Dictionary<string, Category> DefinedCategories
    {
        get { return _DefinedCategories; }
        set { _DefinedCategories = value; }
    }
    public const string DEFAULT_NAME = "uncategorized";
    private const string _INVALID_ARGUMENT_MESSAGE = "Category must be between 1 - 50 characters and cannot be all white-space characters";

    /// <summary>
    /// Instantiates a new category instance with the specified name
    /// </summary>
    /// <remarks>
    /// Requires that the specified name is a valid name. Otherwise will set to "Uncategorized" and throw an ArgumentException.
    /// If a category instance with the same name already exists, the constructor will return the existing object instance
    /// </remarks>
    /// <param name="name"></param>
    /// <exception cref="ArgumentException"></exception>
    public Category(string name)
    {
        if (IsValidName(name))
        {
            Name = name.ToLower();
        }
        else
        {
            throw new ArgumentException(_INVALID_ARGUMENT_MESSAGE);
        }
    }

    /// <summary>
    /// Returns a reference the Category instance with the specified <param name="name">name</param>.
    /// Otherwise, returns a new Category instance
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Category GetInstance(string name)
    {
        Category? categoryInstance;
        name = name.ToLower();

        if (!_DefinedCategories.TryGetValue(name, out categoryInstance))
        {
            categoryInstance = new Category(name);
            _DefinedCategories.Add(name, categoryInstance);
        }
        return categoryInstance;
    }

    /// <summary>
    /// Determines whether the input category name is valid
    /// </summary>
    /// <remarks>
    /// Valid inputs must:
    /// <list type="number">
    /// <item>
    /// Be between 1 and 50 characters (inclusive) in length
    /// </item>
    /// <item>
    /// Not a string with only whitespace characters
    /// </item>
    /// </list>
    /// </remarks>
    /// <param name="name"></param>
    /// <returns> bool </returns>
    public static bool IsValidName(string name)
    {
        return 50 >= name.Length && name.Length > 0 &&
        name.Replace(" ", "").Length != 0;
    }

    /// <summary>
    /// Adds a new category if one with the same name does not already exist. Otherwise this function does nothing
    /// </summary>
    /// <param name="name"> Name of the category to add </param>
    public static void AddCategory(string name)
    {
        name = name.ToLower();
        {
            if (!_DefinedCategories.ContainsKey(name))
            {
                Category categoryInstance = new Category(name);
                _DefinedCategories.Add(name, categoryInstance);
            }
        }

    }

    /// <summary>
    /// Modifies an existing category instance with the specified name. Otherwise, throws an ArgumentException
    /// </summary>
    /// <param name="oldCategory">Name referencing the category to change</param>
    /// <param name="newCategory">New name to update the category to</param>
    /// <exception cref="ArgumentException"></exception>
    public static void ModifyCategory(string oldCategory, string newCategory)
    {
        oldCategory = oldCategory.ToLower();
        newCategory = newCategory.ToLower();

        if (!_DefinedCategories.ContainsKey(oldCategory))
        {
            throw new ArgumentException("Category to modify does not exist");
        }
        else if (!IsValidName(newCategory))
        {
            throw new ArgumentException(_INVALID_ARGUMENT_MESSAGE);
        }
        else
        {
            Category categoryInstance = _DefinedCategories[oldCategory];
            categoryInstance.Name = newCategory;
            _DefinedCategories.Remove(oldCategory);

            _DefinedCategories.Add(newCategory, categoryInstance);
        }
    }

    /// <summary>
    /// Removes the Category reference with the specified name if it is defined. Otherwise, throws an ArgumentException
    /// </summary>
    /// <param name="name"></param>
    /// <exception cref="ArgumentException"></exception>
    public static void RemoveCategory(string name)
    {
        name = name.ToLower();

        if (!_DefinedCategories.ContainsKey(name))
        {
            throw new ArgumentException("Could not find the category requested to be removed");
        }
        else
        {
            _DefinedCategories.Remove(name);
        }
    }
}