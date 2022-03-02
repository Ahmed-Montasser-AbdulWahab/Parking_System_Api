﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking_System_API.Data.Entities;
using Parking_System_API.Data.Models;
using Parking_System_API.Data.Repositories.SystemUserR;
using Parking_System_API.Model;
using System;
using System.Threading.Tasks;

namespace Parking_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemUsersController : ControllerBase
    {
        private readonly ISystemUserRepository systemUserRepository;
        private readonly JwtAuthenticationManager jwtAuthenticationManager;
        private readonly IMapper mapper;

        public SystemUsersController(ISystemUserRepository systemUserRepository, JwtAuthenticationManager jwtAuthenticationManager, IMapper mapper)
        {
            this.systemUserRepository = systemUserRepository;
            this.jwtAuthenticationManager = jwtAuthenticationManager;
            this.mapper = mapper;
        }
        [HttpPost("login"), AllowAnonymous]
        public async Task<IActionResult> login(AuthenticationRequest authenticationRequest)
        {
            try {
                var authResult = await jwtAuthenticationManager.AuthenticateAdminAndOpertor(authenticationRequest.Email, authenticationRequest.Password);
                if (authResult == null)
                    return Unauthorized();
                else
                    return Ok(authResult);
            }
            catch (Exception ex) {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }

        }

        [HttpPost("signup"), Authorize(Roles = "admin,operator")]
        public async Task<IActionResult> Signup([FromBody] SystemUser systemUser)
        {
            try
            {
                var salt = Hashing.HashingClass.GenerateSalt();
                var hashed = Hashing.HashingClass.GenerateHashedPassword(systemUser.Password, salt);
                systemUser.Password = hashed;
                systemUser.Salt = salt;
                systemUserRepository.Add(systemUser);

                if (await systemUserRepository.SaveChangesAsync())
                {
                    return Created("", $"{systemUser.Email} is created");
                }
                return BadRequest("Check Provided Data, i.e:Data may be duplicated");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }


        [HttpGet, Authorize(Roles = "admin")]
        public async Task<ActionResult<SystemUserModel[]>> GetAllSystemUsers()
        {
            try
            {
                var systemUsers = await systemUserRepository.GetAllSystemUsersAsync();
                SystemUserModel[] models = mapper.Map<SystemUserModel[]>(systemUsers);
                return models;
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }
        [HttpGet("{email}"), Authorize(Roles ="admin")]
        public async Task<ActionResult<SystemUserModel>> GetSystemUser(string email)
        {
            try
            {
                var systemUser = await systemUserRepository.GetSystemUserAsync(email);
                SystemUserModel model = mapper.Map<SystemUserModel>(systemUser);
                return model;
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }

        [HttpGet(), Authorize(Roles = "admin")]
        public async Task<ActionResult<SystemUserModel[]>> GetAllSystemUsersByName(string name)
        {
            try
            {
                var systemUsers = await systemUserRepository.GetSystemUsersAsyncByName(name);
                SystemUserModel[] models = mapper.Map<SystemUserModel[]>(systemUsers);
                return models;
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }
    }
}
