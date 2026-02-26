using System;
using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Dto
{
    public class DoctorDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Specialization { get; set; }
        public string DocLicenseNum { get; set; }
    }
}