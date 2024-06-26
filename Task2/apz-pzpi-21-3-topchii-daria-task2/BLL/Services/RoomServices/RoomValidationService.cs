﻿using BLL.Abstractions.Interfaces.RoomInterfaces;
using Core.DataClasses;
using Core.Models.RoomModels;

namespace BLL.Services.RoomServices
{
    internal class RoomValidationService : IRoomValidationService
    {
        private const int MinNameLength = 3;

        private const int MaxNameLength = 32;

        public ExceptionalResult ValidateCreateModel(RoomCreateModel createModel)
        {
            var results = new ExceptionalResult[]
            {
                this.ValidateName(createModel.Name),
            };

            var incorrectResults = results.Where(r => !r.IsSuccess).ToList();

            return incorrectResults.Any() ? incorrectResults.First() : new ExceptionalResult();
        }

        public ExceptionalResult ValidateUpdateModel(RoomUpdateModel updateModel)
        {
            var results = new ExceptionalResult[]
            {
                this.ValidateName(updateModel.Name),
            };

            var incorrectResults = results.Where(r => !r.IsSuccess).ToList();

            return incorrectResults.Any() ? incorrectResults.First() : new ExceptionalResult();
        }

        private ExceptionalResult ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < MinNameLength)
            {
                return new ExceptionalResult(false, $"Room name can't be less then {MinNameLength} symbols");
            }

            if (name.Length > MaxNameLength)
            {
                return new ExceptionalResult(false, $"Room name can't be longer then {MaxNameLength} symbols");
            }

            return new ExceptionalResult();
        }
    }
}
