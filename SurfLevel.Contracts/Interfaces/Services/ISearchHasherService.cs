﻿using SurfLevel.Contracts.Models.ViewModels;

namespace SurfLevel.Contracts.Interfaces.Services
{
    public interface ISearchHasherService
    {
        string Create(Request request);

        TRequest Read<TRequest>(string hash) where TRequest : Request, new();
    }
}