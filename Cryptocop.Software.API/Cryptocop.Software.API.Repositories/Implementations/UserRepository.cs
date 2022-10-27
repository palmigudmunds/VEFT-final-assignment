﻿using System.Linq;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.Entities;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Repositories.Helpers;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly CryptocopDbContext _dbContext;
        private readonly ITokenRepository _tokenRepository;

        public UserRepository(CryptocopDbContext dbContext, ITokenRepository tokenRepository)
        {
            _dbContext = dbContext;
            _tokenRepository = tokenRepository;
        }
        public UserDto CreateUser(RegisterInputModel inputModel)
        {
            var entity = new User
            {
                FullName = inputModel.FullName,
                Email = inputModel.Email,
                HashedPassword = HashingHelper.HashPassword(inputModel.Password)
            };

            _dbContext.Users.Add(entity);
            _dbContext.SaveChanges();

            var token = _tokenRepository.CreateNewToken();

            return new UserDto
            {
                Id = entity.Id,
                FullName = entity.FullName,
                Email = entity.Email,
                TokenId = token.Id
            };
        }

        public UserDto AuthenticateUser(LoginInputModel loginInputModel)
        {
            var user = _dbContext.Users.FirstOrDefault(u => 
                u.Email == loginInputModel.Email &&
                u.HashedPassword == HashingHelper.HashPassword(loginInputModel.Password));

            if (user == null) { return null; }

            var token = new JwtToken();
            _dbContext.JwtTokens.Add(token);
            _dbContext.SaveChanges();

            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                TokenId = token.Id
            };
        }
    }
}