using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    // 28.11 tunni põhjal - base klass kõigile entity klassidele
    public abstract class Entity
    {
        public int Id { get; set; }
    }
}
