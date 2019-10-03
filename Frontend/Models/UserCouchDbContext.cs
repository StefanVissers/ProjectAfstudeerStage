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
    public class UserCouchDbContext : IDbContext<UserModel>
    {
        private readonly CouchDbSettings _couchDbSettings;

        public UserCouchDbContext(CouchDbSettings options)
        {
            _couchDbSettings = options;
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

        public async Task<ViewQueryResponse<UserModel>> Login(UserModel userModel)
        {
            using (var client = new MyCouchClient(_couchDbSettings.BaseURL, _couchDbSettings.UserDatabaseName))
            {
                //var z = new FindRequest();
                var query = new QueryViewRequest("userDesignDoc", "user-list");
                var response = await client.Views.QueryAsync<UserModel>(query);
                
                //var x = await client.Queries.FindAsync<UserModel>(z);

                return response;
            }
        }

        UserModel IDbContext<UserModel>.Post(UserModel model)
        {
            throw new NotImplementedException();
        }

        public UserModel Get(string id)
        {
            throw new NotImplementedException();
        }
    }
}
