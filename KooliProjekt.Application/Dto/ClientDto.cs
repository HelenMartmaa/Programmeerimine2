using System;
using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Dto
{
    public class ClientDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PersonalCode { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }
    }
}