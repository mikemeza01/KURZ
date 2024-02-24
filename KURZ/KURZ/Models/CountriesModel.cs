using KURZ.Entities;
using KURZ.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KURZ.Models
{
    public class CountriesModel : ICountriesModel
    {
        private readonly KurzContext _context;


        public CountriesModel(KurzContext context)
        {
            _context = context;
        }

        public List<Countries>? CountriesList()
        {
            try
            {
                return _context.Countries.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de paises: " + ex.Message);
            }
        }
    }
}
