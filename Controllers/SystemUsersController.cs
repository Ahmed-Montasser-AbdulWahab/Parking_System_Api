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
using Microsoft.AspNetCore.Routing;
using Parking_System_API.Data.Repositories.VehicleR;
using Parking_System_API.Data.Repositories.ParticipantR;
using System.Linq;
using System.Web.Security;
using System.Security.Cryptography;
using Parking_System_API.Hashing;

namespace Parking_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemUsersController : ControllerBase
    {
        private readonly ISystemUserRepository systemUserRepository;
        private readonly JwtAuthenticationManager jwtAuthenticationManager;
        private readonly IMapper mapper;
        private readonly LinkGenerator linkGenerator;
        private readonly VehicleRepository vehicleRepository;
        private readonly ParticipantRepository participantRepository;
        private static long ForeignMemberId = 10000000000000 ;

        public SystemUsersController(ISystemUserRepository systemUserRepository, JwtAuthenticationManager jwtAuthenticationManager, IMapper mapper, LinkGenerator linkGenerator, VehicleRepository vehicleRepository, ParticipantRepository participantRepository)
        {
            this.systemUserRepository = systemUserRepository;
            this.jwtAuthenticationManager = jwtAuthenticationManager;
            this.mapper = mapper;
            this.linkGenerator = linkGenerator;
            this.vehicleRepository = vehicleRepository;
            this.participantRepository = participantRepository;
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
            catch (Exception ) {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }

        }

        [HttpPost("signup"), Authorize(Roles = "admin,operator")]
        public async Task<IActionResult> Signup([FromBody] SystemUser systemUser)
        {
            try
            {
                var location = linkGenerator.GetPathByAction("GetSystemUser", "SystemUsers", new { email = systemUser.Email });
                if (String.IsNullOrEmpty(location))
                {
                    BadRequest("Try Again");
                }
                var salt = Hashing.HashingClass.GenerateSalt();
                var hashed = Hashing.HashingClass.GenerateHashedPassword(systemUser.Password, salt);
                systemUser.Password = hashed;
                systemUser.Salt = salt;
                systemUserRepository.Add(systemUser);

                if (await systemUserRepository.SaveChangesAsync())
                {
                    return Created(location, mapper.Map<SystemUserModel>(systemUser));
                }
                return BadRequest("Check Provided Data, i.e:Data may be duplicated");
            }
            catch (Exception)
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
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }
        [HttpGet("{email}"), Authorize(Roles ="admin")]
        public async Task<ActionResult<SystemUserModel>> GetSystemUser(string email)
        {
            try
            {
                var systemUser = await systemUserRepository.GetSystemUserAsyncByEmail(email);
                SystemUserModel model = mapper.Map<SystemUserModel>(systemUser);
                return model;
            }
            catch (Exception)
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
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }

        [HttpPost("newParticipant"), Authorize(Roles = "operator, admin")]
        public async Task<ActionResult<Participant>> AddParticipant([FromForm] ParticipantAdminModel model)
        {
            var participant = new Participant() { Status = true };

            //Adding ParticipantId

            if(model.isEgyptian)
            {
                if (model.Id == null) {
                    return BadRequest("Please provide National Id"); }
                else
                {
                    participant.ParticipantId = model.Id.Value ;
                }

            }
            
            else if (!model.isEgyptian)
            {
                participant.ParticipantId = ForeignMemberId;
                ForeignMemberId++;
            }

            //Adding Email

            participant.Email = model.Email;

            //Adding Password and sending it Via Email
            var password = GenerateToken(8);
            /*Send Emai*/


            //end
            participant.Salt = HashingClass.GenerateSalt();

            participant.Password = HashingClass.GenerateHashedPassword(password,participant.Salt);

            //Checking Photo and detection
            if (model.ProfileImage == null)
            {
                participant.DoProvidePhoto = false;
                participant.Status = false;
            }
            else
            {

            }

            //checking name
            if (model.Name == null)
            {
                participant.Status = false ;
            }
            else
            {
                participant.Name = model.Name;
            }

            //Adding Vehicles
            if (!model.PlateNumberIds.Any())
            {
                participant.Status = false;
            }
            else {
                foreach (var v in model.PlateNumberIds)
                {
                    var Vehicle = await vehicleRepository.GetVehicleAsyncByPlateNumber(v);
                    if (Vehicle == null)
                    {
                        return BadRequest($"No Vehicle saved with the provided License Plate {v}");
                    }
                    else
                    {
                        participant.Vehicles.Add(Vehicle);

                    }
                }
            }

            //adding and saving
            participantRepository.Add(participant);
            if (!await participantRepository.SaveChangesAsync())
            {
                return BadRequest("Participant Not Saved");
            }
            else
            {
                return Created("",participant);
            }


            
        }

        public string GenerateToken(int length)
        {
            using (RNGCryptoServiceProvider cryptRNG = new RNGCryptoServiceProvider())
            {
                byte[] tokenBuffer = new byte[length];
                cryptRNG.GetBytes(tokenBuffer);
                return Convert.ToBase64String(tokenBuffer);
            }
        }


    }
}
