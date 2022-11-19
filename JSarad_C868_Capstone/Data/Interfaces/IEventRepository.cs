using JSarad_C868_Capstone.Models;

namespace JSarad_C868_Capstone.Data.Interfaces
{
    public interface IEventRepository : IDisposable 
    {
        IEnumerable<Event> GetAll();
        Event GetById(int id);
        void Add(Event eventToAdd);
        Event Update(int id, Event enentToUpdate);
        void Delete(int id);
    }
}
