namespace PasswordManager.Services.RequestProvider;

public interface IRequestProvider
{
    Task<TResult> GetAsync<TResult>(string uri, string token = "");

    Task<TResult> PostAsync<TResult>(string uri, TResult data, string token = "", string header = "");

    Task<TResult> PostAsync<TResult>(string uri, string data, string clientId, string clientSecret);
    
    Task<TResult> PostAsync<TResult, TRequest>(string uri, TRequest data, string token = "", string header = "");

    Task<TResult> PutAsync<TResult>(string uri, TResult data, string token = "", string header = "");
    
    Task<TResult> PutAsync<TResult, TRequest>(string uri, TRequest data, string token = "", string header = "");

    Task DeleteAsync(string uri, string token = "");
}
