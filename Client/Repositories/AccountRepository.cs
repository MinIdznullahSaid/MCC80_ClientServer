using API.DTOs.AccountDtos;
using API.DTOs.EmployeeDtos;
using API.Models;
using API.Utilities.Handlers;
using Client.Contracts;
using Newtonsoft.Json;
using System.Text;

namespace Client.Repositories;

public class AccountRepository : GeneralRepository<LoginDto, Guid>, IAccountRepository
{
    private readonly string request;
        private readonly HttpClient httpClient;
        public AccountRepository(string request="Accounts/") : base(request)
        {
            this.request = request;
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7149/api/")
            };
        }

        public async Task<ResponseHandler<TokenDto>> Login(LoginDto entity)
        {
            ResponseHandler<TokenDto> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            using (var response = httpClient.PostAsync(request + "Login", content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseHandler<TokenDto>>(apiResponse);
            }
            return entityVM;
        }
}
