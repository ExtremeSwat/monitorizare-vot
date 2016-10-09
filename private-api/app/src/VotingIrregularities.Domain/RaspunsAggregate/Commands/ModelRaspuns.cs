﻿using System.Collections.Generic;

namespace VotingIrregularities.Domain.RaspunsAggregate.Commands
{
    public class ModelRaspuns
    {
        public int IdIntrebare { get; set; }
        public int IdSectie { get; set; }
        public string CodFormular { get; set; }
        public List<ModelOptiuniSelectate> Optiuni { get; set; }
    }
}
