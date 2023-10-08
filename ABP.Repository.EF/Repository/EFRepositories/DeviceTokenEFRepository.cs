using ABP.AppCore;
using ABP.Application.Models;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABP.Repository.ContractImplementation.EFImplementations;
using Microsoft.EntityFrameworkCore;
using ABP.AppCore.Entities;
using ABP.Application.Models.Requests;

namespace ABP.Repository.Repository.EFRepositories
{
    public class DeviceTokenEFRepository : IDeviceTokenEFRepository
    {
        private readonly ABPContext _dbContext;

        private readonly IMapper _mapper;

        public DeviceTokenEFRepository(ABPContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DeviceToken>> GetDeviceTokenListAsync()
        {
            var deviceTokenList = await _dbContext.DeviceTokens.ToListAsync();

            if (deviceTokenList != null)
            {
                var result = new List<DeviceToken>();

                foreach (var token in deviceTokenList)
                {
                    result.Add(_mapper.Map<DeviceToken>(token));
                }

                return result;
            }

            return null;
        }

        public async Task<DeviceToken> GetDeviceTokenByIdAsync(int id)
        {
            var token = await _dbContext.DeviceTokens.FirstOrDefaultAsync(x => x.Id == id);

            if (token != null)
            {
                var result = _mapper.Map<DeviceToken>(token);

                return result;
            }

            return null;
        }

        public async Task<List<DeviceToken>> GetDeviceTokenByDeviceIdAsync(int deviceId)
        {
            var list = await _dbContext.DeviceTokens.ToListAsync();

            var entityList = list.Where(x => x.DeviceId == deviceId);

            if (entityList != null)
            {
                var result = new List<DeviceToken>();

                foreach (var token in entityList)
                {
                    result.Add(_mapper.Map<DeviceToken>(token));
                }

                return result;
            }

            return null;
        }

        public async Task<DeviceToken> GetDeviceTokenByRequestAsync(DeviceTokenRequest request)
        {
            var token = await _dbContext.DeviceTokens.FirstOrDefaultAsync(x => x.DeviceId == request.DeviceId && x.Name == request.Name && x.Value == request.Value);

            if (token != null)
            {
                var result = _mapper.Map<DeviceToken>(token);

                return result;
            }

            return null;
        }

        public async Task<bool> Create(DeviceToken token)
        {
            if (token != null)
            {
                var list = await GetDeviceTokenListAsync();

                var checkCopy = list.Any(d => d.Name == token.Name && d.DeviceId == token.DeviceId);

                if (!checkCopy)
                {
                    var result = _mapper.Map<DeviceTokenEntity>(token);

                    await _dbContext.DeviceTokens.AddAsync(result);
                    await _dbContext.SaveChangesAsync();

                    return true;
                }
            }

            return false;
        }

        public async Task UpdateAsync(DeviceToken token)
        {
            var original = await _dbContext.DeviceTokens.FindAsync(token.Id);

            if (original != null)
            {
                _dbContext.Entry(original).CurrentValues.SetValues(token);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var token = await _dbContext.DeviceTokens.FindAsync(id);

            if (token != null)
                await Task.Run(() => _dbContext.DeviceTokens.Remove(token));

            await _dbContext.SaveChangesAsync();
        }
    }
}
