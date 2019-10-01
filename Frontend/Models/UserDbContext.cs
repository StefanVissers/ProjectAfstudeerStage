using Microsoft.Extensions.Options;
using MyCouch;
using MyCouch.Requests;
using MyCouch.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Models
{
    public class UserDbContext : IDbContext
    {
        private readonly CouchDbSettings _couchDbSettings;

        public UserDbContext(IOptions<CouchDbSettings> options)
        {
            _couchDbSettings = options.Value;
        }

        public async Task<ViewQueryResponse<UserModel>> GetAll()
        {
            using (var client = new MyCouchClient(_couchDbSettings.BaseURL, _couchDbSettings.UserDatabaseName))
            {
                var query = new QueryViewRequest("userDesignDoc", "user-list");
                var response = await client.Views.QueryAsync<UserModel>(query);
                
                return response;
            }
        }

        public async Task<EntityResponse<UserModel>> Post(UserModel userModel)
        {
            using (var client = new MyCouchClient(_couchDbSettings.BaseURL, _couchDbSettings.UserDatabaseName))
            {
                var response = await client.Entities.PostAsync<UserModel>(userModel);

                return response;
            }
        }
    }
}
