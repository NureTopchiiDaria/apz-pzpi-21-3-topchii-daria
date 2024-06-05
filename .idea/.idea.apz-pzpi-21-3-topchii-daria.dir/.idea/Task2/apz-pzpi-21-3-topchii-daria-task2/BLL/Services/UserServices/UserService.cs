using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Abstractions.Interfaces.UserInterfaces;
using Core.DataClasses;
using Core.DTOs;
using Core.Models.UserModels;
using DAL.Abstractions.Interfaces;

namespace BLL.Services.UserServices
{
    internal class UserService : IUserService
    {
        private readonly IGenericStorageWorker<UserModel> storage;
        private readonly IHashingService hashingService;

        public UserService(IGenericStorageWorker<UserModel> storage, IHashingService hashingService)
        {
            this.storage = storage;
            this.hashingService = hashingService;
        }

        public async Task<IEnumerable<UserModel>> GetByConditions(params Expression<Func<UserModel, bool>>[] conditions)
        {
            return await this.storage.GetByConditions(conditions);
        }

        public async Task<IEnumerable<UserModel>> GetActiveUsers(params Expression<Func<UserModel, bool>>[] additionalConditions)
        {
            additionalConditions = additionalConditions.Append(x => x.IsActive).ToArray();
            return await this.GetByConditions(additionalConditions);
        }

        public async Task<UserModel> GetUserById(Guid id)
        {
            return (await this.GetByConditions(u => u.Id == id)).FirstOrDefault();
        }

        public async Task<OptionalResult<UserModel>> CreateNonActiveUser(UserCreateModel user)
        {
            if ((await this.GetByConditions(x => x.Email == user.Email)).Any())
            {
                return new OptionalResult<UserModel>(false, $"User with email {user.Email} already exists");
            }

            var userModel = this.MapUserCreateModel(user);
            await this.storage.Create(userModel);

            return new OptionalResult<UserModel>(userModel);
        }

        public async Task<ExceptionalResult> Delete(Guid id)
        {
            var user = await this.GetUserById(id);
            if (user is null)
            {
                return new ExceptionalResult(false, $"User with id {id} does not exist");
            }

            await this.storage.Delete(user);

            return new ExceptionalResult();
        }

        public async Task<OptionalResult<UserModel>> ActivateUser(Guid id)
        {
            var userData = new UserUpdateModel()
            {
                Id = id,
                IsActive = true,
            };

            return await this.Update(userData, null);
        }

        public async Task<OptionalResult<UserModel>> Update(UserUpdateModel user, UserHeightDTO userHeightDTO)
        {
            if (await this.GetUserById(user.Id) is null)
            {
                return new OptionalResult<UserModel>(false, $"User with id {user.Id} does not exist");
            }

            // Проверяем, указан ли рост в футах
            if (userHeightDTO != null && userHeightDTO.IsInFeet)
            {
                float heightInMeters = this.ConvertFeetToMeters(userHeightDTO.Height);
                user.Height = heightInMeters;
            }

            var userModel = await this.MapUserUpdateModel(user);
            await this.storage.Update(userModel);

            return new OptionalResult<UserModel>(userModel);
        }

        public async Task<OptionalResult<UserModel>> UpdateProfilePicture(Guid id, byte[] profilePicture)
        {
            var user = await this.GetUserById(id);
            if (user == null)
            {
                return new OptionalResult<UserModel>(false, $"User with id {id} does not exist");
            }

            user.ProfilePicture = profilePicture;
            await this.storage.Update(user);

            return new OptionalResult<UserModel>(user);
        }

        private async Task<UserModel> MapUserUpdateModel(UserUpdateModel user)
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserUpdateModel, UserModel>(MemberList.Source).ForAllMembers(opt => opt.AllowNull());
            });
            var mapper = new Mapper(mapperConfig);
            var userObject = mapper.Map<UserModel>(user);
            var changingUser = await this.GetUserById(user.Id);

            foreach (var field in userObject.GetType().GetProperties())
            {
                if (field.GetValue(userObject) is not null)
                {
                    field.SetValue(changingUser, field.GetValue(userObject));
                }
            }

            if (user.Password is not null)
            {
                changingUser.HashedPassword = this.hashingService.Hash(user.Password);
            }

            return changingUser;
        }

        private float ConvertFeetToMeters(float feet)
        {
            // Конвертируем рост из футов в метры
            const float feetToMetersConversionFactor = 0.3048f;
            return feet * feetToMetersConversionFactor;
        }

        private UserModel MapUserCreateModel(UserCreateModel user)
        {
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<UserCreateModel, UserModel>());
            var mapper = new Mapper(mapperConfiguration);
            var userObject = mapper.Map<UserModel>(user);
            userObject.HashedPassword = this.hashingService.Hash(user.Password);
            userObject.IsActive = false;

            return userObject;
        }
    }
}
