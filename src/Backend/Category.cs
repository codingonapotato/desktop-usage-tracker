
public class Category
{
    public string name;
    private const string _DEFAULT_NAME = "Uncategorized";
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
            this.name = name;
        }
        else
        {
            this.name = _DEFAULT_NAME;
            throw new ArgumentException(_INVALID_ARGUMENT_MESSAGE);
        }
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
    /// <param name="category"></param>
    /// <returns> bool </returns>
    public static bool IsValidName(string category)
    {
        return 50 >= category.Length && category.Length > 0 &&
        category.Replace(" ", "").Length != 0;
    }
}