﻿using EpicMedia.Models.Dto;
using EpicMedia.Web.ViewModels;
using Microsoft.AspNetCore.Components.Forms;

namespace EpicMedia.Web.Services.Interface
{
    public interface IPostService
    {
        Task<ApiModel> PostAsync(PostDto post,IBrowserFile file);
    }
}