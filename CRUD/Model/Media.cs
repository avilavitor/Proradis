using System;
using System.Collections.Generic;
using System.Text;

namespace CRUD.Model
{
  public class Media
    {
        public string Cidade { get; set; }
        public double Idade { get; set; }

    }

    public class Medias
    {
        public Media[] medias { get; set; }
    }
}
