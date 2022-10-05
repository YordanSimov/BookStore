using AutoMapper;
using ProjectDK.BL.Interfaces;
using ProjectDK.DL.Interfaces;
using ProjectDK.DL.Repositories.MsSQL;
using ProjectDK.Models.Models;
using ProjectDK.Models.Requests;
using ProjectDK.Models.Responses;
using System;
using System.Net;

namespace ProjectDK.BL.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository personRepository;
        private readonly IMapper mapper;

        public PersonService(IPersonRepository personInMemoryRepository, IMapper mapper)
        {
            this.personRepository = personInMemoryRepository;
            this.mapper = mapper;
        }
        public async Task<PersonResponse> Add(PersonRequest personRequest)
        {
            var personCheck = await personRepository.GetById(personRequest.Id);
            if (personCheck != null) return new PersonResponse()
            {
                HttpStatusCode = HttpStatusCode.BadRequest,
                Message = "Person already exists",
            };

            var person = mapper.Map<Person>(personRequest);
            var result = await personRepository.Add(person);

            return new PersonResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Person = result
            };
        }

        public async Task<Person?> Delete(int id)
        {
            return await personRepository.Delete(id);
        }

        public async Task<IEnumerable<Person>> GetAll()
        {
            return await personRepository.GetAll();
        }

        public async Task<Person?> GetById(int id)
        {
            return await personRepository.GetById(id);
        }

        public async Task<PersonResponse> Update(PersonRequest personRequest)
        {
            var personCheck = await personRepository.GetById(personRequest.Id);
            if (personCheck == null) return new PersonResponse()
            {
                HttpStatusCode = HttpStatusCode.NotFound,
                Message = "Person does not exist.",
            };
            var person = mapper.Map<Person>(personRequest);
            await personRepository.Update(person);

            return new PersonResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Person = person,
            };
        }
    }
}