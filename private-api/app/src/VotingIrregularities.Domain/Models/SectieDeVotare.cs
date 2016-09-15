﻿using System;
using System.Collections.Generic;

namespace VotingIrregularities.Domain.Models
{
    public partial class SectieDeVotare
    {
        public SectieDeVotare()
        {
            Raspuns = new HashSet<Raspuns>();
            RaspunsFormular = new HashSet<RaspunsFormular>();
        }

        public int IdSectieDeVotarre { get; set; }
        public int NumarSectie { get; set; }
        public int? IdOras { get; set; }
        public int IdJudet { get; set; }
        public string AdresaSectie { get; set; }
        public string Coordonate { get; set; }

        public virtual ICollection<Raspuns> Raspuns { get; set; }
        public virtual ICollection<RaspunsFormular> RaspunsFormular { get; set; }
        public virtual Judet IdJudetNavigation { get; set; }
        public virtual Oras IdOrasNavigation { get; set; }
    }
}
