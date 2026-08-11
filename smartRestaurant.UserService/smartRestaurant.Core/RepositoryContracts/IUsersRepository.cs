using smartRestaurant.Core.Entities;

namespace smartRestaurant.Core.RepositoryContracts;

/// <summary>
/// Contract to be implemented by UsersRepository that contains data access logic of Users data store 
/// </summary>

public interface IUsersRepository
{
    /// <summary>
    /// Method to add user to the data store and return the add user 
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task AddAsync(ApplicationUser user);

    /// <summary>
    /// Method to retrive existing user by their email 
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<ApplicationUser?> GetUserByEmailAsync(string email);
    /// <summary>
    /// Returns the users data based on the given user ID
    /// </summary>
    /// <param name="userID">User ID to search </param>
    /// <returns>ApplicatioUser object that macthes with given UserID</returns>
    Task<ApplicationUser?> GetByIdAsync(Guid id);

    /// <summary>
    /// Method to check if the given email already exists in the data store
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<bool> EmailExistsAsync(string email);

    /// <summary>
    /// Method to update the existing user in the data store
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task UpdateAsync(ApplicationUser user);
    /// <summary>
    /// Method to get all users from the data store
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<ApplicationUser>> GetAllAsync();

    // Autenticação
    /// <summary>
    /// Method to get the user for login based on the given email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<ApplicationUser?> GetForLoginAsync(string email);

    // Gestão de conta
    /// <summary>
    /// Method to confirm the email of the user based on the given user ID
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task ConfirmEmailAsync(Guid userId);
    /// <summary>
    /// Method to deactivate the user account based on the given user ID
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task DeactivateAsync(Guid userId);

    Task<bool> ExistsUserNameAsync(string userName);
    Task<ApplicationUser?> GetByRefreshTokenAsync(string refreshToken);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);



}

