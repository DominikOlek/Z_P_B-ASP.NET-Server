using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiP.Services;

namespace ApiP.Data
{
    public class CzasowoOdebrane
    {
        public int ID { get; set; }
        public int KierowcaID { get; set; }
        public virtual Kierowcy Kierowca { get; set; }
        public DateTime DataWystawienia { get; set; }
        public DateTime DataOdebrania { get; set; }
        public DateTime DataOddania { get; set; }

    }
}

